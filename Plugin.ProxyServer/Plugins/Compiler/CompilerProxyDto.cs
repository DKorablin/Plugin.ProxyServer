using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace Plugin.ProxyServer.Plugins.Compiler
{
	/// <summary>Объект переброса данных из плагина в динамический компилятор</summary>
	public class CompilerProxyDto
	{
		/// <summary>Аргументы управления запросом от прокси</summary>
		private SessionEventArgs Args { get; }

		/// <summary>Does this session uses SSL?</summary>
		public Boolean IsHttps => this.Args.IsHttps;

		/// <summary>Is this a transparent endpoint?</summary>
		public Boolean IsTransparent => this.Args.IsTransparent;

		/// <summary>HTTP proxy client</summary>
		public String ClientEndpoint => this.Args.ClientEndPoint.ToString();

		/// <summary>Should we send the request again ?</summary>
		public Boolean ReRequest
		{
			get => this.Args.ReRequest;
			set => this.Args.ReRequest = value;
		}

		public ProxyRequestDto Request { get; private set; }

		public ProxyResponseDto Response { get; private set; }

		internal CompilerProxyDto(SessionEventArgs e, Boolean isResponse)
		{
			this.Args = e;
			this.Request = new ProxyRequestDto(e);
			if(isResponse)
				this.Response = new ProxyResponseDto(e);
		}

		/// <summary>Before request is made to server respond with the specified HTML string and the specified status to client. And then ignore the request</summary>
		/// <param name="html">The html content</param>
		/// <param name="statusCode">The HTTP status code</param>
		/// <param name="closeServerConnection">Close the server connection used by request if any?</param>
		/// <param name="headers">The HTTP headers</param>
		public void GenericResponse(String html, HttpStatusCode statusCode, Boolean closeServerConnection = false, params KeyValuePair<String, String>[] headers)
		{
			Dictionary<String, HttpHeader> headers2 = headers == null ? null : headers.ToDictionary(p => p.Key, p => new HttpHeader(p.Key, p.Value));
			this.Args.GenericResponse(html, statusCode, headers2, closeServerConnection);
		}

		/// <summary>Before request is made to server respond with the specified byte[], the specified status to client. And then ignore the request</summary>
		/// <param name="result">The bytes to sent</param>
		/// <param name="statusCode">The HTTP status code</param>
		/// <param name="closeServerConnection">Close the server connection used by request if any?</param>
		/// <param name="headers">The HTTP headers</param>
		public void GenericResponse(Byte[] result, HttpStatusCode statusCode, Boolean closeServerConnection = false, params KeyValuePair<String, String>[] headers)
		{
			Dictionary<String, HttpHeader> headers2 = headers == null ? null : headers.ToDictionary(p => p.Key, p => new HttpHeader(p.Key, p.Value));
			this.Args.GenericResponse(result, statusCode, headers2, closeServerConnection);
		}

		/// <summary>Before request is made to server respond with the specified HTML string to client and ignore the request</summary>
		/// <param name="html">HTML content to sent</param>
		/// <param name="closeServerConnection">Close the server connection used by request if any?</param>
		/// <param name="headers">HTTP response headers</param>
		public void Ok(String html, Boolean closeServerConnection = false, params KeyValuePair<String, String>[] headers)
		{
			Dictionary<String, HttpHeader> headers2 = headers == null ? null : headers.ToDictionary(p => p.Key, p => new HttpHeader(p.Key, p.Value));
			this.Args.Ok(html, headers2, closeServerConnection);
		}

		/// <summary>Before request is made to server respond with the specified byte[] to client and ignore the request</summary>
		/// <param name="result">The html content bytes</param>
		/// <param name="closeServerConnection">Close the server connection used by request if any?</param>
		/// <param name="headers">The HTTP headers</param>
		public void Ok(Byte[] result, Boolean closeServerConnection = false, params KeyValuePair<String, String>[] headers)
		{
			Dictionary<String, HttpHeader> headers2 = headers == null ? null : headers.ToDictionary(p => p.Key, p => new HttpHeader(p.Key, p.Value));
			this.Args.Ok(result, headers2, closeServerConnection);
		}

		/// <summary>Redirect to provided URL</summary>
		/// <param name="url">The URL to redirect</param>
		/// <param name="closeServerConnection">Close the server connection used by request if any?</param>
		public void Redirect(String url, Boolean closeServerConnection = false)
			=> this.Args.Redirect(url, closeServerConnection);

		/// <summary>Sets the request body</summary>
		/// <param name="body">The request body bytes</param>
		public void SetRequestBody(Byte[] body)
			=> this.Args.SetRequestBody(body);

		/// <summary>Sets the body with the specified string</summary>
		/// <param name="body">The request body string to set</param>
		public void SetRequestBodyString(String body)
			=> this.Args.SetRequestBodyString(body);

		/// <summary>Set the response body bytes</summary>
		/// <param name="body">The body bytes to set</param>
		public void SetResponseBody(Byte[] body)
			=> this.Args.SetResponseBody(body);

		/// <summary>Replace the response body with the specified string</summary>
		/// <param name="body">The body string to set</param>
		public void SetResponseBodyString(String body)
			=> this.Args.SetResponseBodyString(body);

		/// <summary>Terminates the session abruptly by terminating client/server connections</summary>
		public void TerminateSession()
			=> this.Args.TerminateSession();

		/// <summary>Terminate the connection to server at the end of this HTTP request/response session</summary>
		public void TerminateServerConnection()
			=> this.Args.TerminateServerConnection();
	}
}