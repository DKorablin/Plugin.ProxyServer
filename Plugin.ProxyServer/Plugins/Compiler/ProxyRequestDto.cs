using System;
using System.ComponentModel;
using System.Diagnostics;
using Titanium.Web.Proxy.EventArguments;

namespace Plugin.ProxyServer.Plugins.Compiler
{
	[DefaultProperty("Url")]
	[DebuggerDisplay("{" + nameof(Method) + "} {" + nameof(Host) + "}")]
	public class ProxyRequestDto : ProxyMessageDtoBase
	{
		/// <summary>Request Url</summary>
		[Description("Request Url")]
		public String Url { get; private set; }

		/// <summary>The original request Url</summary>
		[Description("The original request Url")]
		public String OriginalUrl { get; private set; }

		/// <summary>Http hostname header value if exists. Note: Changing this does NOT change host in RequestUri. Users can set new RequestUri separately</summary>
		[Description("Http hostname header value if exists. Note: Changing this does NOT change host in RequestUri. Users can set new RequestUri separately")]
		public String Host { get; private set; }

		/// <summary>Request Method</summary>
		[Description("HTTP request method")]
		public String Method { get; private set; }

		/// <summary>Is Https?</summary>
		[Description("Is Https? Interfierence with SSL dialogue will cause inject fake certificate")]
		public Boolean IsHttps { get; private set; }

		/// <summary>Is client is waiting for next payload?</summary>
		[Description("Is client is waiting for next payload?")]
		public Boolean Is100Continue { get; private set; }

		/// <summary>Does this request contain multipart/form-data?</summary>
		[Description("Does this request contain multipart/form-data?")]
		public Boolean IsMultipartFormData { get; private set; }

		/// <summary>Did server respond negatively for 100 continue request?</summary>
		[Description("Did server respond negatively for 100 continue request?")]
		public Boolean ExpectationFailed { get; private set; }

		/// <summary>Does this request has a 100-continue header?</summary>
		[Description("Does this request has a 100-continue header?")]
		public Boolean ExpectContinue { get; private set; }

		/// <summary>Does this request has an upgrade to websocket header?</summary>
		[Description("Does this request has an upgrade to websocket header?")]
		public Boolean UpgradeToWebSocket { get; private set; }

		internal ProxyRequestDto(SessionEventArgs e)
			: base(e, e.HttpClient.Request)
		{
			var request = e.HttpClient.Request;
			this.Url = request.Url;
			this.OriginalUrl = request.OriginalUrl;
			this.Host = request.Host;
			this.Method = request.Method;
			this.IsHttps = request.IsHttps;
			this.Is100Continue = request.Is100Continue;
			this.IsMultipartFormData = request.IsMultipartFormData;
			this.ExpectationFailed = request.ExpectationFailed;
			this.ExpectContinue = request.ExpectContinue;
			this.UpgradeToWebSocket = request.UpgradeToWebSocket;
		}

		public override String ToString()
			=> String.Join(" ", this.Method, this.Url);
	}
}