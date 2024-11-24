using System;

namespace Plugin.ProxyServer.Dto
{
	/// <summary>Тип движка для генерации подменных сертификатов</summary>
	public enum CertificateEngineType
	{
		/// <summary>Bouncy Castle</summary>
		BouncyCastle,
		/// <summary>Default windows certificate engine</summary>
		DefaultWindows,
	}
}