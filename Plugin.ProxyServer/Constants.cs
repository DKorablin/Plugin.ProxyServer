using System;

namespace Plugin.ProxyServer
{
	internal static class Constants
	{
		/// <summary>Наименование метода перехвата запроса к прокси</summary>
		public const String InterceptRequest = "InterceptRequest";
		/// <summary>Наименование метода перехвата ответа от сервера клиенту</summary>
		public const String InterceptResponse = "InterceptResponse";
	}
}