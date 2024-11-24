using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using Titanium.Web.Proxy.EventArguments;

namespace Plugin.ProxyServer.Plugins.Compiler
{
	[DefaultProperty("StatusCode")]
	[DebuggerDisplay("{" + nameof(StatusCode) + "} {" + nameof(StatusDescription) + "}")]
	public class ProxyResponseDto : ProxyMessageDtoBase
	{
		/// <summary>Response Status Code</summary>
		[Description("Response Status Code")]
		public HttpStatusCode StatusCode { get; private set; }

		/// <summary>Response Status description</summary>
		[Description("Response Status description")]
		public String StatusDescription { get; private set; }

		/// <summary>Keep the connection alive?</summary>
		[Description("Keep the connection alive?")]
		public Boolean KeepAlive { get; private set; }

		internal ProxyResponseDto(SessionEventArgs e)
			: base(e, e.HttpClient.Response)
		{
			var response = e.HttpClient.Response;
			this.StatusCode = (HttpStatusCode)response.StatusCode;
			this.StatusDescription = response.StatusDescription;
			this.KeepAlive = response.KeepAlive;
		}

		public override String ToString()
			=> String.Join(" ", (Int32)this.StatusCode, this.StatusDescription);
	}
}