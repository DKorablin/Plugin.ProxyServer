using System;
using System.ComponentModel;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Http;

namespace Plugin.ProxyServer.Plugins.Compiler
{
	public class ProxyMessageDtoBase
	{
		/// <summary>Gets the header text.</summary>
		[Description("Gets the header text.")]
		public String HeaderText { get; private set; }
		/// <summary>Body as byte array</summary>
		[Description("Body as byte array")]
		public Byte[] Body { get; private set; }
		/// <summary>Body as string</summary>
		/// <remarks>Use the encoding specified to decode the byte[] data to string</remarks>
		[Description("Body as string")]
		public String BodyString { get; private set; }
		/// <summary>Http Version</summary>
		[Description("Http Version")]
		public Version HttpVersion { get; private set; }
		/// <summary>Content encoding for this request/response</summary>
		[Description("Content encoding for this request/response")]
		public String ContentEncoding { get; private set; }
		/// <summary>Length of the body</summary>
		[Description("Length of the body")]
		public Int64 ContentLength { get; private set; }
		/// <summary>Content-type of the request/response.</summary>
		[Description("Content-type of the request/response")]
		public String ContentType { get; private set; }
		/// <summary>Has the request/response body?</summary>
		[Description("Has the request/response body?")]
		public Boolean HasBody { get; private set; }
		/// <summary>Is body send as chunked bytes</summary>
		[Description("Is body send as chunked bytes")]
		public Boolean IsChunked { get; private set; }

		internal ProxyMessageDtoBase(SessionEventArgs e, Request request)
			: this(request)
		{
			if(e.HttpClient.Request.HasBody)
			{
				this.Body = Utils.RunTaskSync(e.GetRequestBody());
				this.BodyString = Utils.RunTaskSync(e.GetRequestBodyAsString());
				/*e.GetRequestBody().ContinueWith((p) => { this.Body = p.Result; });
				e.GetRequestBodyAsString().ContinueWith((p) => { this.BodyString = p.Result; });*/
			}
		}

		internal ProxyMessageDtoBase(SessionEventArgs e, Response response)
			: this(response)
		{
			if(e.HttpClient.Response.HasBody)
			{//HACK: На некоторых запросах получение payload'а - зависает. Предположительно, по причине передачи Content-Length>0 и пустого payload'а
				this.Body = Utils.RunTaskSync(e.GetResponseBody());
				this.BodyString = Utils.RunTaskSync(e.GetResponseBodyAsString());
				/*e.GetResponseBody().ContinueWith((p) => { this.Body = p.Result; });
				e.GetResponseBodyAsString().ContinueWith((p) => { this.BodyString = p.Result; });*/
			}
		}
		private ProxyMessageDtoBase(RequestResponseBase message)
		{
			this.ContentEncoding = message.ContentEncoding;
			this.ContentLength = message.ContentLength;
			this.ContentType = message.ContentType;
			this.HeaderText = message.HeaderText;
			this.HttpVersion = message.HttpVersion;
			this.HasBody = message.HasBody;
			this.IsChunked = message.IsChunked;
		}
	}
}