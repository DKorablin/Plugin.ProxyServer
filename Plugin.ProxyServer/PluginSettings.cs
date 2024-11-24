using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Plugin.ProxyServer.Dto;
using Plugin.ProxyServer.UI;
using SAL.Flatbed;

namespace Plugin.ProxyServer
{
	public class PluginSettings : INotifyPropertyChanged
	{
		internal static class Constants
		{
			public const String TemplateIpAddr = "{ipAddress}";
			public static String[] DefaultEndpoints = new String[] { "http://" + TemplateIpAddr + ":8080" };
			public const String RootCertificate = "RootCertificate";
		}

		private readonly PluginHost _plugin;
		private String[] _endpoints = Constants.DefaultEndpoints;
		private Boolean _isSystemHttpsProxy = false;
		private Boolean _isSystemHttpProxy = false;
		private Boolean _isStartProxy = false;
		private Boolean _enableConnectionPool = true;
		private Boolean _enable100ContinueBehaviour = true;
		private Boolean _enableWinAuth;
		private static IPAddress _HostAddress;
		private MessageGroupType _logGroup = MessageGroupType.None;
		private CertificateEngineType _certificateEngine = CertificateEngineType.BouncyCastle;
		private X509Certificate2 _rootCertificate;
		private String _rootCertificatePassword;
		private SslProtocols _supportedSslProtocols = SslProtocols.Default;

		[Category("Server")]
		[Description("Endpoints list for Web proxy service. Use " + Constants.TemplateIpAddr + " template for Dns.GetHostName()")]
		public String[] Endpoints
		{
			get => this._endpoints;
			set
			{
				String[] tmpEndpoints;
				if(value == null || value.Length == 0)
					tmpEndpoints = Constants.DefaultEndpoints;
				else
				{
					tmpEndpoints = Array.FindAll(value, url => Uri.TryCreate(FormatEndpoint(url), UriKind.Absolute, out Uri dummy1));
					if(tmpEndpoints.Length == 0)
						tmpEndpoints = Constants.DefaultEndpoints;
				}
				this.SetField(ref this._endpoints, tmpEndpoints, nameof(this.Endpoints));
			}
		}

		[DefaultValue(false)]
		[Category("Server")]
		[DisplayName("Proxy Started")]
		[Description("To run proxy on startup or not to run")]
		public Boolean IsStartProxy
		{
			get => this._isStartProxy;
			set => this.SetField(ref this._isStartProxy, value, nameof(this.IsStartProxy));
		}

		[DefaultValue(SslProtocols.Default)]
		[Category("Server")]
		[DisplayName("Supported SSL Protocols")]
		[Description("Supported by server TLS/SSL protocols (Disabled)")]
		[Editor(typeof(ColumnEditorTyped<SecurityProtocolType>), typeof(UITypeEditor))]
		public SslProtocols SupportedSslProtocols
		{
			get => this._supportedSslProtocols;
			set => this.SetField(ref this._supportedSslProtocols, value, nameof(this.SupportedSslProtocols));
		}

		[DefaultValue(true)]
		[Category("Server")]
		[DisplayName("Enable 100 Continue")]
		[Description("This means that the server has received the request headers, and that the client should proceed to send the request body")]
		public Boolean Enable100ContinueBehaviour
		{
			get => this._enable100ContinueBehaviour;
			set => this.SetField(ref this._enable100ContinueBehaviour, value, nameof(this.Enable100ContinueBehaviour));
		}

		[DefaultValue(true)]
		[Category("Server")]
		[DisplayName("Enable Connection Pool")]
		public Boolean EnableConnectionPool
		{
			get => this._enableConnectionPool;
			set => this.SetField(ref this._enableConnectionPool, value, nameof(this.EnableConnectionPool));
		}

		[DefaultValue(true)]
		[Category("Server")]
		[DisplayName("Enable WinAuth")]
		public Boolean EnableWinAuth
		{
			get => this._enableWinAuth;
			set => this.SetField(ref this._enableWinAuth, value, nameof(this.EnableWinAuth));
		}

		[DefaultValue(false)]
		[Category("System")]
		[DisplayName("Set as System HTTP Proxy")]
		[Description("Set HTTP proxy as system proxy for all applications")]
		public Boolean IsSystemHttpProxy
		{
			get => this._isSystemHttpProxy;
			set => this.SetField(ref this._isSystemHttpProxy, value, nameof(this.IsSystemHttpProxy));
		}

		[DefaultValue(false)]
		[Category("System")]
		[DisplayName("Set as System HTTPS Proxy")]
		[Description("Set HTTPS proxy as system proxy for all applciations")]
		public Boolean IsSystemHttpsProxy
		{
			get => this._isSystemHttpsProxy;
			set => this.SetField(ref this._isSystemHttpsProxy, value, nameof(this.IsSystemHttpsProxy));
		}

		[DefaultValue(CertificateEngineType.BouncyCastle)]
		[Category("Certificate")]
		[DisplayName("Certificate Engine")]
		[Description("Certificate engine generator for SSL connections")]
		public CertificateEngineType CertificateEngine
		{
			get => this._certificateEngine;
			set { this.SetField(ref this._certificateEngine, value, nameof(this.CertificateEngine)); }
		}

		[Category("Certificate")]
		[DisplayName("Root Certificate")]
		[Description("Root certificate for MitM proxy")]
		[Editor(typeof(CertificateBrowser), typeof(UITypeEditor))]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DefaultValue(null)]
		public X509Certificate2 RootCertificate
		{
			get
			{
				if(this._rootCertificate == null)
				{
					Byte[] data = this.GetRootCertificate();
					if(data != null)
						this._rootCertificate = new X509Certificate2(data, this.RootCertificatePassword, X509KeyStorageFlags.Exportable);
				}
				return this._rootCertificate;
			}
		}

		[Category("Certificate")]
		[DisplayName("Root Certificate password")]
		[Description("Specify password for proxy root certificate. Certificate will be removed if property is changed.")]
		//[PasswordPropertyText(true)]
		public String RootCertificatePassword
		{
			get => this._rootCertificatePassword;
			set
			{
				if(this.SetField(ref this._rootCertificatePassword, value, nameof(RootCertificatePassword)))
					this.DeleteRootCertificate();
			}
		}

		[Category("UI")]
		[DisplayName("Log Grouping")]
		[Description("Group logged data")]
		[DefaultValue(MessageGroupType.None)]
		public MessageGroupType LogGroup
		{
			get => this._logGroup;
			set => this.SetField(ref this._logGroup, value, nameof(this.LogGroup));
		}

		/// <summary>Хост адрес текущей машины</summary>
		private static IPAddress HostAddress
		{
			get
			{
				if(_HostAddress == null)
				{
					IPHostEntry ip = Dns.GetHostEntry(Dns.GetHostName());
					_HostAddress = Array.Find(ip.AddressList, addr => addr.AddressFamily == AddressFamily.InterNetwork);
				}
				return _HostAddress;
			}
		}

		internal PluginSettings(PluginHost plugin)
			=> this._plugin = plugin;

		#region Endpoints
		/// <summary>Получить список хостов для прокси с заменой шаблонных параметров</summary>
		/// <returns>IP адрес и порт</returns>
		internal String[] GetEndpoints()
			=> FormatEndpoints(this.Endpoints);

		private static String[] FormatEndpoints(String[] endpoints)
			=> Array.ConvertAll(endpoints, item => FormatEndpoint(item));

		private static String FormatEndpoint(String endpoint)
			=> endpoint.Replace(Constants.TemplateIpAddr, PluginSettings.HostAddress.ToString());
		#endregion Endpoints

		#region RootCertificate
		/// <summary>Получить пароль для сертификата. Если пароль не указан, то пароль будет сгенерён автоматически</summary>
		/// <returns>Сгенерированный пароль</returns>
		internal String GetRootCertificatePassword()
		{
			String result = this.RootCertificatePassword;
			if(String.IsNullOrEmpty(result))
				this.RootCertificatePassword = result = Utils.GeneratePassword();
			return result;
		}
		internal Byte[] GetRootCertificate()
		{
			using(Stream stream = this._plugin.Host.Plugins.Settings(this._plugin).LoadAssemblyBlob(Constants.RootCertificate))
				if(stream != null)
					return Utils.ConvertStreamToArray(stream);
			return null;
		}

		internal void DeleteRootCertificate()
			=> this.SetRootCertificate(null, null);

		internal void SetRootCertificate(String filePath)
		{
			if(String.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
				throw new ArgumentNullException(nameof(filePath));

			X509Certificate2 certificate = new X509Certificate2(filePath, this.RootCertificatePassword, X509KeyStorageFlags.Exportable);
			using(FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				this.SetRootCertificate(certificate, stream);
		}

		internal void SetRootCertificate(Byte[] certData)
		{
			if(certData == null || certData.Length == 0)
				throw new ArgumentNullException(nameof(certData));

			X509Certificate2 certificate = new X509Certificate2(certData, this.RootCertificatePassword, X509KeyStorageFlags.Exportable);
			using(MemoryStream stream = new MemoryStream(certData))
				this.SetRootCertificate(certificate, stream);
		}

		private void SetRootCertificate(X509Certificate2 certificate, Stream stream)
		{
			ISettingsProvider provider = this._plugin.Host.Plugins.Settings(this._plugin);
			if(certificate == null || stream == null)
				provider.RemoveAssemblyBlob(Constants.RootCertificate);
			else
				provider.SaveAssemblyBlob(Constants.RootCertificate, stream);

			this.SetField(ref this._rootCertificate, certificate, Constants.RootCertificate);
		}
		#endregion RootCertificate

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private Boolean SetField<T>(ref T field, T value, String propertyName)
		{
			if(EqualityComparer<T>.Default.Equals(field, value))
				return false;

			field = value;
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}
		#endregion INotifyPropertyChanged
	}
}