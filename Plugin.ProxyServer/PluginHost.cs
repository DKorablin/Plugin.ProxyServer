using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Plugin.ProxyServer.Plugins;
using SAL.Flatbed;
using SAL.Windows;

namespace Plugin.ProxyServer
{
	public class PluginHost : IPlugin, IPluginSettings<PluginSettings>
	{
		private PluginSettings _settings;
		private TraceSource _trace;
		private Dictionary<String, DockState> _documentTypes;
		private IMenuItem _menuNetwork;

		internal IHost Host { get; }

		internal IHostWindows HostWindows { get => this.Host as IHostWindows; }

		internal IMenuItem ProxyMenu { get; private set; }

		internal ProxyServerWrapper ProxyWrapper { get; private set; }

		internal PluginCompilerWrapper PluginCompiler { get; private set; }

		internal PluginCryptoUIWrapper PluginCryptoUI { get; private set; }

		internal TraceSource Trace { get => this._trace ?? (this._trace = PluginHost.CreateTraceSource<PluginHost>()); }

		/// <summary>Настройки для взаимодействия из хоста</summary>
		Object IPluginSettings.Settings { get => this.Settings; }

		/// <summary>Настройки для взаимодействия из плагина</summary>
		public PluginSettings Settings
		{
			get
			{
				if(this._settings == null)
				{
					this._settings = new PluginSettings(this);
					this.Host.Plugins.Settings(this).LoadAssemblyParameters(this._settings);
					this._settings.PropertyChanged += _settings_PropertyChanged;
				}
				return this._settings;
			}
		}

		/// <summary>Проверка прокси на запуск</summary>
		public Boolean IsStarted { get => this.ProxyWrapper.IsProxyRunning; }

		private Dictionary<String, DockState> DocumentTypes
		{
			get
			{
				if(this._documentTypes == null)
					this._documentTypes = new Dictionary<String, DockState>()
					{
						{typeof(PanelProxyLog).ToString(),DockState.DockLeftAutoHide },
					};
				return this._documentTypes;
			}
		}

		public PluginHost(IHost host)
			=> this.Host = host ?? throw new ArgumentNullException(nameof(host));

		public IWindow GetPluginControl(String typeName, Object args)
			=> this.CreateWindow(typeName, false, args);

		/// <summary>Запустить прокси</summary>
		public void StartProxy()
		{
			if(!this.ProxyWrapper.IsProxyRunning)
			{
				this.ProxyWrapper.Start();
				this.PluginCompiler.AttachInterceptor();
			}
		}

		/// <summary>Остановить прокси</summary>
		public void StopProxy()
		{
			if(this.ProxyWrapper.IsProxyRunning)
			{
				this.ProxyWrapper.Stop();
				this.PluginCompiler.DetachInterceptor();
			}
		}

		Boolean IPlugin.OnConnection(ConnectMode mode)
		{
			this.Host.Plugins.PluginsLoaded += new EventHandler(PluginHost_PluginsLoaded);

			IHostWindows host = this.HostWindows;
			if(host != null)
			{
				IMenuItem menuTools = host.MainMenu.FindMenuItem("Tools");
				if(menuTools == null)
				{
					this.Trace.TraceEvent(TraceEventType.Error, 10, "Menu item 'Tools' not found");
					return false;
				}

				this._menuNetwork = menuTools.FindMenuItem("Network");
				if(this._menuNetwork == null)
				{
					this._menuNetwork = menuTools.Create("Network");
					this._menuNetwork.Name = "Tools.Network";
					menuTools.Items.Add(this._menuNetwork);
				}

				this.ProxyMenu = this._menuNetwork.Create("&Proxy Server");
				this.ProxyMenu.Name = "Tools.Network.ProxyServer";
				this.ProxyMenu.Click += (sender, e)=> { this.CreateWindow(typeof(PanelProxyLog).ToString(), false); };
				this._menuNetwork.Items.Insert(0, this.ProxyMenu);
				return true;
			}
			return true;
		}

		Boolean IPlugin.OnDisconnection(DisconnectMode mode)
		{
			if(this.ProxyWrapper != null)
			{
				this.ProxyWrapper.Dispose();
				this.ProxyWrapper = null;
			}

			if(this.ProxyMenu != null)
				this._menuNetwork.Items.Remove(this.ProxyMenu);

			if(this._menuNetwork != null && this._menuNetwork.Items.Count == 0)
				this.HostWindows.MainMenu.Items.Remove(this._menuNetwork);
			return true;
		}

		private IWindow CreateWindow(String typeName, Boolean searchForOpened, Object args = null)
			=> this.DocumentTypes.TryGetValue(typeName, out DockState state)
				? this.HostWindows.Windows.CreateWindow(this, typeName, searchForOpened, state, args)
				: null;

		private static TraceSource CreateTraceSource<T>(String name = null) where T : IPlugin
		{
			TraceSource result = new TraceSource(typeof(T).Assembly.GetName().Name + name);
			result.Switch.Level = SourceLevels.All;
			result.Listeners.Remove("Default");
			result.Listeners.AddRange(System.Diagnostics.Trace.Listeners);
			return result;
		}

		private void _settings_PropertyChanged(Object sender, PropertyChangedEventArgs e)
		{
			try
			{
				switch(e.PropertyName)
				{
				case nameof(PluginSettings.IsStartProxy):
					if(this.Settings.IsStartProxy)
						this.StartProxy();
					else
						this.StopProxy();
					break;
				}
			} catch(Exception exc)
			{
				this.Trace.TraceData(TraceEventType.Error, 10, exc);
			}
		}

		private void PluginHost_PluginsLoaded(Object sender, EventArgs e)
		{
			this.Host.Plugins.PluginsLoaded -= new EventHandler(PluginHost_PluginsLoaded);

			this.PluginCompiler = new PluginCompilerWrapper(this);
			this.PluginCryptoUI = new PluginCryptoUIWrapper(this);
			this.ProxyWrapper = new ProxyServerWrapper(this);
			if(this.Settings.IsStartProxy)
				this.StartProxy();
		}
	}
}