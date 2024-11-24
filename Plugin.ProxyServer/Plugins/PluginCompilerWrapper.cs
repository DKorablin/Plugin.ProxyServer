using System;
using System.Diagnostics;
using System.Reflection;
using Plugin.ProxyServer.Plugins.Compiler;
using SAL.Flatbed;
using SAL.Windows;
using Titanium.Web.Proxy.EventArguments;

namespace Plugin.ProxyServer.Plugins
{
	/// <summary>Обёртка плагина .NET компиляции</summary>
	internal class PluginCompilerWrapper
	{
		#region Fields
		private readonly PluginHost _plugin;
		private IPluginDescription _pluginCompiler;
		#endregion Fields

		/// <summary>Плагин компиляции</summary>
		private static class CompilerPlugin
		{
			/// <summary>ID плагина с рантайм компиляцией</summary>
			public const String Name = "425f8b0c-f049-44ee-8375-4cc874d6bf94";

			/// <summary>Публичные методы плагина</summary>
			public static class Methods
			{
				/// <summary>Проверка на существование кода</summary>
				public const String IsMethodExists = "IsMethodExists";

				/// <summary>Выполнить динамический код</summary>
				public const String InvokeDynamicMethod = "InvokeDynamicMethod";
			}

			/// <summary>Окна плагина</summary>
			public static class Windows
			{
				/// <summary>Окно редактирования исходного кода на .NET</summary>
				public const String DocumentCompiler = "Plugin.Compiler.DocumentCompiler";

				/// <summary>Событие сохранения данных в окне <c>DocumentCompiler</c></summary>
				public const String SaveEventName = "SaveEvent";
			}
		}

		/// <summary>Ссылка на описание собственного плагина</summary>
		private IPluginDescription PluginSelf
		{
			get
			{
				foreach(IPluginDescription plugin in this._plugin.Host.Plugins)
					if(plugin.Instance == this._plugin)
						return plugin;
				throw new InvalidOperationException();
			}
		}

		/// <summary>ССылка на описание плагина компиляции</summary>
		public IPluginDescription PluginCompiler
			=>  this._pluginCompiler ?? (this._pluginCompiler = this._plugin.Host.Plugins[CompilerPlugin.Name]);

		/// <summary>Проверка на существование перехватчика запросов от клиента к прокси</summary>
		public Boolean IsInterceptRequestExists
			=>  this.PluginCompiler != null && this.IsMethodExists(this.PluginSelf, Constants.InterceptRequest);

		/// <summary>Проверка на существовние перехватчика ответов от сервера к прокси</summary>
		public Boolean IsInterceptResponseExists
			=> this.PluginCompiler != null && this.IsMethodExists(this.PluginSelf, Constants.InterceptResponse);

		/// <summary>Плагин компиляции перехватывает запросы и ответы через прокси</summary>
		public Boolean IsCompilerAttached { get; private set; }

		public static String Name => CompilerPlugin.Name;

		public PluginCompilerWrapper(PluginHost plugin)
			=> this._plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));

		/// <summary>Запустить перехватчик HTTP транспорта</summary>
		public void AttachInterceptor()
		{
			if(this._plugin.IsStarted)
			{
				if(this.IsInterceptRequestExists)
					this._plugin.ProxyWrapper.Request += ProxyWrapper_Request;
				if(this.IsInterceptResponseExists)
					this._plugin.ProxyWrapper.Response += ProxyWrapper_Response;
				this.IsCompilerAttached = true;
			}
		}

		/// <summary>Остановить перехватчик HTTP транспорта</summary>
		public void DetachInterceptor()
		{
			if(!this._plugin.IsStarted)
			{
				this._plugin.ProxyWrapper.Request -= ProxyWrapper_Request;
				this._plugin.ProxyWrapper.Response -= ProxyWrapper_Response;
				this.IsCompilerAttached = false;
			}
		}

		/// <summary>Обработка события HTTP запроса к прокси</summary>
		/// <param name="sender">Инстанс прокси сервера</param>
		/// <param name="e">Аргументы события</param>
		private void ProxyWrapper_Request(Object sender, SessionEventArgs e)
		{
			try
			{
				CompilerProxyDto args = new CompilerProxyDto(e, false);
				Object result = this.InvokeDynamicMethod(Constants.InterceptRequest, args);
			} catch(TargetInvocationException exc)
			{
				this._plugin.Trace.TraceData(TraceEventType.Error, 10, exc.InnerException == null ? exc : exc.InnerException);
			}
		}

		/// <summary>Обработка события HTTP ответа от сервера к прокси</summary>
		/// <param name="sender">Инстанс прокси сервера</param>
		/// <param name="e">Аргументы события</param>
		private void ProxyWrapper_Response(Object sender, SessionEventArgs e)
		{
			try
			{
				CompilerProxyDto args = new CompilerProxyDto(e, false);
				Object result = this.InvokeDynamicMethod(Constants.InterceptResponse, args);
			} catch(TargetInvocationException exc)
			{
				this._plugin.Trace.TraceData(TraceEventType.Error, 10, exc.InnerException == null ? exc : exc.InnerException);
			}
		}

		/// <summary>Создать окно с редактированием кода компиляции</summary>
		/// <param name="className">Наименование класса, используемом в коде компиляции</param>
		/// <param name="onSave">Событие, вызываемое при сохранении кода в окне</param>
		public void CreateCompilerWindow(String className, EventHandler<DataEventArgs> onSave)
		{
			IPluginDescription self = this.PluginSelf;

			this.CreateCompilerWindow(self.ID, className, onSave);
		}

		/// <summary>Создать окно с редактированием кода компиляции</summary>
		/// <param name="callerPluginId">Идентификатор вызываемого плагина</param>
		/// <param name="methodName">Наименование класса, используемом в коде компиляции</param>
		/// <param name="onSave">Событие, вызываемое при сохранении кода в окне</param>
		public void CreateCompilerWindow(String callerPluginId, String methodName, EventHandler<DataEventArgs> onSave)
		{
			_ = this.PluginCompiler
				?? throw new ArgumentNullException(nameof(this.PluginCompiler), $"Plugin ID={CompilerPlugin.Name} not installed");

			IHostWindows hostWindows = (IHostWindows)this._plugin.Host;

			IWindow wnd = hostWindows.Windows.CreateWindow(CompilerPlugin.Name,
				CompilerPlugin.Windows.DocumentCompiler,
				false,
				new
				{
					CallerPluginId = callerPluginId,
					ArgumentsType = new Type[] { typeof(CompilerProxyDto), },
					ReturnType = typeof(void),
					ClassName = methodName
				});

			if(wnd != null && onSave != null)
				wnd.AddEventHandler(CompilerPlugin.Windows.SaveEventName, onSave);
		}

		/// <summary>Проверка на существование динамического метода в плагине компиляции</summary>
		/// <param name="plugin">Плагин источник запроса динамического метода</param>
		/// <param name="methodName">Наименование метода в плагине компиляции</param>
		/// <returns>Данный метод существует в плагине компиляции для такого плагина</returns>
		public Boolean IsMethodExists(IPluginDescription plugin, String methodName)
		{
			IPluginDescription pluginCompiler = this.PluginCompiler;
			if(pluginCompiler == null)
				return false;
			else
				return (Boolean)pluginCompiler.Type
					.GetMember<IPluginMethodInfo>(CompilerPlugin.Methods.IsMethodExists)
					.Invoke(new Object[] { plugin, methodName, });
		}

		/// <summary>Вызвать динамический код, написанный заранее через плагин</summary>
		/// <param name="className">Наименование класса, испольуемом в коде компиляции</param>
		/// <param name="compilerArgs">Аргументы, передаваемые в скомпилированный класс</param>
		/// <returns>Результат выполнения метода</returns>
		public Object InvokeDynamicMethod(String className, params Object[] compilerArgs)
		{
			IPluginDescription self = this.PluginSelf;

			return this.InvokeDynamicMethod(self, className, compilerArgs);
		}

		/// <summary>Вызвать динамический код, написанный заранее через плагин</summary>
		/// <param name="plugin">Описатель вызываемого плагина</param>
		/// <param name="methodName">Наименование класса, испольуемом в коде компиляции</param>
		/// <param name="compilerArgs">Аргументы, передаваемые в скомпилированный класс</param>
		/// <returns>Результат выполнения метода</returns>
		public Object InvokeDynamicMethod(IPluginDescription plugin, String methodName, params Object[] compilerArgs)
		{
			IPluginDescription pluginCompiler = this.PluginCompiler
				?? throw new ArgumentNullException(nameof(this.PluginCompiler), $"Plugin ID={CompilerPlugin.Name} not installed");

			Object[] args = new Object[] { plugin, methodName, compilerArgs };

			return pluginCompiler.Type
				.GetMember<IPluginMethodInfo>(CompilerPlugin.Methods.InvokeDynamicMethod)
				.Invoke(args);
		}
	}
}