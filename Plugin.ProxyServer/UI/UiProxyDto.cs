using System;
using System.Collections.Generic;
using System.ComponentModel;
using Plugin.ProxyServer.Plugins.Compiler;
using Titanium.Web.Proxy.EventArguments;

namespace Plugin.ProxyServer.UI
{
	internal class UiProxyDto
	{
		[Category("HTTP")]
		[DisplayName("Reques Date")]
		public DateTime RequestDate { get; private set; }

		[Category("HTTP")]
		[DisplayName("Is HTTPS")]
		[Description("Does this session uses SSL?")]
		public Boolean IsHttps { get; private set; }

		[Category("HTTP")]
		[DisplayName("Is Transparent")]
		[Description("Is this a transparent endpoint?")]
		public Boolean IsTransparent { get; private set; }

		[Category("Proxy")]
		[DisplayName("Time Line")]
		[Description("Relative milliseconds for various events.")]
		public Dictionary<String, DateTime> TimeLine { get; private set; }

		[Category("Proxy")]
		[DisplayName("Proxy endpoint")]
		public String ProxyEndpoint { get; private set; }

		[Category("HTTP")]
		[DisplayName("Client endpoint")]
		public String ClientEndpoint { get; private set; }

		[Category("Proxy")]
		[Description("Request from client to proxy")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ProxyRequestDto Request { get; private set; }

		[Category("Proxy")]
		[Description("Response from server to proxy")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ProxyResponseDto Response { get; private set; }

		#region Methods
		[Category("Method")]
		[DisplayName("GenericResponse()")]
		[Description(@"Before request is made to server respond with the specified HTML string and the specified status to client. And then ignore the request
html - The html content
statusCode - The HTTP status code
closeServerConnection - Close the server connection used by request if any?
headers - The HTTP headers")]
		public String GenericResponseString { get => "(String html, HttpStatusCode statusCode, Boolean closeServerConnection = false, params KeyValuePair<String, String>[] headers)"; }

		[Category("Method")]
		[DisplayName("GenericResponse()")]
		[Description(@"Before request is made to server respond with the specified HTML string and the specified status to client. And then ignore the request
html - The html content
statusCode - The HTTP status code
closeServerConnection - Close the server connection used by request if any?
headers - The HTTP headers")]
		public String GenericResponseByte { get => "(Byte[] result, HttpStatusCode statusCode, Boolean closeServerConnection = false, params KeyValuePair<String, String>[] headers)"; }

		[Category("Method")]
		[DisplayName("Ok()")]
		[Description(@"Before request is made to server respond with the specified HTML string to client and ignore the request
html - HTML content to sent
closeServerConnection - Close the server connection used by request if any?
headers - HTTP response headers")]
		public String OkHtml { get => "(String html, Boolean closeServerConnection = false, params KeyValuePair<String, String>[] headers)"; }

		[Category("Method")]
		[DisplayName("Ok()")]
		[Description(@"Before request is made to server respond with the specified byte[] to client and ignore the request
result - The html content bytes
closeServerConnection - Close the server connection used by request if any?
headers - The HTTP headers")]
		public String OkByte { get => "(Byte[] result, Boolean closeServerConnection = false, params KeyValuePair<String, String>[] headers)"; }

		[Category("Method")]
		[DisplayName("Redirect()")]
		[Description(@"Redirect to provided URL
url - The URL to redirect
closeServerConnection - Close the server connection used by request if any?")]
		public String Redirect { get => "(String url, Boolean closeServerConnection = false)"; }

		[Category("Method")]
		[DisplayName("SetRequestBody()")]
		[Description(@"Sets the request body
body - The request body bytes")]
		public String SetRequestBody { get => "(Byte[] body)"; }

		[Category("Method")]
		[DisplayName("SetRequestBodyString()")]
		[Description(@"Sets the body with the specified string
body - The request body string to set")]
		public String SetRequestBodyString { get => "(String body)"; }

		[Category("Method")]
		[DisplayName("SetResponseBody()")]
		[Description(@"Set the response body bytes
body - The body bytes to set")]
		public String SetResponseBody { get => "(Byte[] body)"; }

		[Category("Method")]
		[DisplayName("SetResponseBodyString()")]
		[Description(@"Replace the response body with the specified string
body - The body string to set")]
		public String SetResponseBodyString { get => "(String body)"; }

		[Category("Method")]
		[DisplayName("TerminateSession()")]
		[Description("Terminates the session abruptly by terminating client/server connections")]
		public String TerminateSession { get => "()"; }

		[Category("Method")]
		[DisplayName("TerminateServerConnection()")]
		[Description("Terminate the connection to server at the end of this HTTP request/response session")]
		public String TerminateServerConnection { get => "()"; }
		#endregion Methods

		public UiProxyDto(SessionEventArgs args)
		{
			this.RequestDate = DateTime.Now;
			this.IsHttps = args.IsHttps;
			this.Request = new ProxyRequestDto(args);
			this.ProxyEndpoint = args.LocalEndPoint.IpAddress.ToString() + ":" + args.LocalEndPoint.Port;
			this.ClientEndpoint = args.ClientEndPoint.ToString();
		}

		public void SetResponsePayload(SessionEventArgs args)
		{
			this.Response = new ProxyResponseDto(args);

			this.TimeLine = new Dictionary<String, DateTime>(args.TimeLine.Count);
			foreach(var item in args.TimeLine)
				this.TimeLine.Add(item.Key, item.Value);
		}
	}
}