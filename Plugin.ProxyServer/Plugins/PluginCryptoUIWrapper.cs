using System;
using System.Collections.Generic;
using SAL.Flatbed;

namespace Plugin.ProxyServer.Plugins
{
	internal class PluginCryptoUIWrapper
	{
		#region Fields
		private readonly PluginHost _plugin;
		private IPluginDescription _pluginCompiler;
		#endregion Fields

		/// <summary>Плагин генерации сертификатов</summary>
		private static class CryptoUIPlugin
		{
			/// <summary>ID плагина с генератором сертификатом</summary>
			public const String Name = "0d31a0ae-154e-4208-8e48-be90e8a22f69";

			/// <summary>Публичные методы плагина</summary>
			public static class Methods
			{
				/// <summary>Метод получения списка всех поддерживаемых алгоритмов</summary>
				public const String GetAlgorithmNames = "GetAlgorithmNames";

				/// <summary>Метод генерации сертификата</summary>
				public const String GenerateCertificate = "GenerateCertificate";
			}
		}

		public static String Name { get => CryptoUIPlugin.Name; }

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
		public IPluginDescription PluginCryptoUI => this._pluginCompiler ?? (this._pluginCompiler = this._plugin.Host.Plugins[CryptoUIPlugin.Name]);

		public PluginCryptoUIWrapper(PluginHost plugin)
			=> this._plugin = plugin;

		/// <summary>Получить список поддерживаемых алгоритмов</summary>
		/// <returns>Список алгоритмов для создания сертификата</returns>
		public IEnumerable<String> GetAlgorithmNames()
		{
			IPluginDescription pluginCryptoUI = this.PluginCryptoUI
				?? throw new ArgumentNullException(nameof(this.PluginCryptoUI), $"Plugin ID={CryptoUIPlugin.Name} not installed");

			return (IEnumerable<String>)pluginCryptoUI.Type
				.GetMember<IPluginMethodInfo>(CryptoUIPlugin.Methods.GetAlgorithmNames)
				.Invoke();
		}

		/// <summary>Вызвать динамический код, написанный заранее через плагин</summary>
		/// <param name="plugin">Описатель вызываемого плагина</param>
		/// <param name="methodName">Наименование класса, испольуемом в коде компиляции</param>
		/// <param name="compilerArgs">Аргументы, передаваемые в скомпилированный класс</param>
		/// <returns>Результат выполнения метода</returns>
		public Byte[] GenerateCertificate(String subject, String password, Int32 strength, String algorithm, DateTime from, DateTime to, params KeyValuePair<String, String>[] extensions)
		{
			IPluginDescription pluginCryptoUI = this.PluginCryptoUI
				?? throw new ArgumentNullException(nameof(this.PluginCryptoUI), $"Plugin ID={CryptoUIPlugin.Name} not installed");

			Object[] args = new Object[] { subject, password, strength, algorithm, from, to, extensions };

			return (Byte[])pluginCryptoUI.Type
				.GetMember<IPluginMethodInfo>(CryptoUIPlugin.Methods.GenerateCertificate)
				.Invoke(args);
		}
	}
}