using System;

namespace Plugin.ProxyServer.UI
{
	/// <summary>Тип группировки данных при логировании запросов</summary>
	public enum MessageGroupType
	{
		/// <summary>Группировка не используется</summary>
		None,
		/// <summary>Группировка по свистку прокси</summary>
		ProxyEndpoint,
		/// <summary>Группировать по клиенту, который запрашивает прокси</summary>
		ClientEndpoint,
		/// <summary>Группировка по удалённому серверу</summary>
		Host,
		/// <summary>HTTP method</summary>
		Method,
	}
}