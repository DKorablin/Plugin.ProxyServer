using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;
using Titanium.Web.Proxy.Network;
using ProxySrv = Titanium.Web.Proxy.ProxyServer;

namespace Plugin.ProxyServer
{
	/// <summary>Обёртка Прокси-сервера</summary>
	internal class ProxyServerWrapper : IDisposable
	{
		private readonly PluginHost _plugin;
		private readonly ProxySrv _proxy;
		private List<ExplicitProxyEndPoint> _explicitEndpoints;
		private EventHandler<SessionEventArgs> _request;
		private EventHandler<SessionEventArgs> _response;

		/// <summary>Событие прихода запроса от клиента</summary>
		public event EventHandler<SessionEventArgs> Request
		{
			add
			{
				if(this._request == null)
					this._proxy.BeforeRequest += OnRequest;
				this._request += value;
			}
			remove
			{
				this._request -= value;
				if(this._request == null)
					this._proxy.BeforeRequest -= OnRequest;
			}
		}

		/// <summary>Событие прихода ответа от сервера</summary>
		public event EventHandler<SessionEventArgs> Response
		{
			add
			{
				if(this._response == null)
					this._proxy.BeforeResponse += OnResponse;
				this._response += value;
			}
			remove
			{
				this._response -= value;
				if(this._response == null)
					this._proxy.BeforeResponse -= OnResponse;
			}
		}

		/// <summary>Изменилось состояние подключения прокси-сервера</summary>
		public event EventHandler<EventArgs> ProxyStateChanged;

		/// <summary>Прокси запущен и готов обрабатывать запросы клиентов</summary>
		public Boolean IsProxyRunning { get => this._proxy?.ProxyRunning == true; }

		public ProxyServerWrapper(PluginHost plugin)
		{
			this._plugin = plugin
				?? throw new ArgumentNullException(nameof(plugin));

			if(plugin.Settings.Endpoints.Length == 0)
				throw new ArgumentException("Endpoints list required", nameof(plugin.Settings.Endpoints));

			//this._proxy = new ProxySrv(userTrustRootCertificate: false);
			this._proxy = new ProxySrv();
			this._explicitEndpoints = new List<ExplicitProxyEndPoint>();
		}

		public void Start()
		{
			//optionally set the Certificate Engine
			//Under Mono only BouncyCastle will be supported
			switch(this._plugin.Settings.CertificateEngine)
			{
			case Dto.CertificateEngineType.BouncyCastle:
				this._proxy.CertificateManager.CertificateEngine = CertificateEngine.BouncyCastle;
				break;
			case Dto.CertificateEngineType.DefaultWindows:
				this._proxy.CertificateManager.CertificateEngine = CertificateEngine.DefaultWindows;
				break;
			default:
				throw new NotImplementedException($"CertificateEngine: {this._plugin.Settings.CertificateEngine} not supported");
			}

			X509Certificate2 rootCertificate = this._plugin.Settings.RootCertificate;
			if(rootCertificate != null)
			{
				this._proxy.CertificateManager.RootCertificate = rootCertificate;
				this._proxy.CertificateManager.PfxPassword = this._plugin.Settings.RootCertificatePassword;

				//Boolean userTrustRootCertificate = true;
				//Boolean machineTrustRootCertificate = false;
				//this._proxy.CertificateManager.EnsureRootCertificate(userTrustRootCertificate, machineTrustRootCertificate);
			}

			this._proxy.Enable100ContinueBehaviour = this._plugin.Settings.Enable100ContinueBehaviour;
			this._proxy.EnableConnectionPool = this._plugin.Settings.EnableConnectionPool;
			this._proxy.EnableWinAuth = this._plugin.Settings.EnableWinAuth;
			//TODO: Пока заблокировано, ибо DefaultValue - не проставляется для свойства, а берётся значение по умолчанию
			//this._proxy.SupportedSslProtocols = this._plugin.Settings.SupportedSslProtocols;

			this._proxy.ServerCertificateValidationCallback += OnCertificateValidation;
			this._proxy.ClientCertificateSelectionCallback += OnCertificateSelection;
			this._proxy.ExceptionFunc = ProxyExceptionOccured;

			foreach(String endpoint in this._plugin.Settings.GetEndpoints())
			{
				Uri url = new Uri(endpoint);
				IPAddress addr = IPAddress.Parse(url.Host);
				ExplicitProxyEndPoint objEndpoint = new ExplicitProxyEndPoint(addr, url.Port, true)
				{
					//Use self-issued generic certificate on all https requests
					//Optimizes performance by not creating a certificate for each https-enabled domain
					//Useful when certificate trust is not required by proxy clients
					//GenericCertificate = new X509Certificate2(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "genericcert.pfx"), "password")
				};

				//Fired when a CONNECT request is received
				objEndpoint.BeforeTunnelConnectRequest += OnBeforeTunnelConnectRequest;

				//An explicit endpoint is where the client knows about the existence of a proxy
				//So client sends request in a proxy friendly manner
				this._proxy.AddEndPoint(objEndpoint);

				this._explicitEndpoints.Add(objEndpoint);
			}

			this._proxy.Start();

			/*//Transparent endpoint is useful for reverse proxy (client is not aware of the existence of proxy)
			//A transparent endpoint usually requires a network router port forwarding HTTP(S) packets or DNS
			//to send data to this endPoint
			var transparentEndPoint = new TransparentProxyEndPoint(IPAddress.Any, 8001, true)
			{
				//Generic Certificate hostname to use
				//when SNI is disabled by client
				GenericCertificateName = "google.com"
			};

			this._proxy.AddEndPoint(transparentEndPoint);*/

			if(this._plugin.Settings.IsSystemHttpProxy)//Only explicit proxies can be set as system proxy!
				this._proxy.SetAsSystemHttpProxy(this._explicitEndpoints[0]);
			if(this._plugin.Settings.IsSystemHttpsProxy)
				this._proxy.SetAsSystemHttpsProxy(this._explicitEndpoints[0]);

			this.OnProxyServerStateChanged();
		}

		public void Stop()
		{
			if(this.IsProxyRunning)
			{
				this.StopI();
				this.OnProxyServerStateChanged();
			}
		}

		private void StopI()
		{
			while(this._explicitEndpoints.Count > 0)
			{
				ExplicitProxyEndPoint objEndpoint = this._explicitEndpoints[0];
				objEndpoint.BeforeTunnelConnectRequest -= OnBeforeTunnelConnectRequest;
				this._explicitEndpoints.RemoveAt(0);
			}

			this._proxy.ServerCertificateValidationCallback -= OnCertificateValidation;
			this._proxy.ClientCertificateSelectionCallback -= OnCertificateSelection;

			this._proxy.Stop();
		}

		private void OnProxyServerStateChanged()
		{
			if(this.IsProxyRunning)
				this._plugin.Trace.TraceEvent(TraceEventType.Start, 10, "Started at Url:\r\n\t{0}", String.Join("\r\n\t", this.GetHostAddress()));
			else
				this._plugin.Trace.TraceEvent(TraceEventType.Stop, 10, "Proxy stopped");

			this.ProxyStateChanged?.Invoke(this, EventArgs.Empty);
		}

		private IEnumerable<String> GetHostAddress()
		{
			foreach(ProxyEndPoint endpoint in this._proxy.ProxyEndPoints)
				yield return endpoint.IpAddress + ":" + endpoint.Port;
		}

		private void ProxyExceptionOccured(Exception exc)
			=> this._plugin.Trace.TraceData(TraceEventType.Error, 10, exc);

		private Task OnBeforeTunnelConnectRequest(Object sender, TunnelConnectSessionEventArgs e)
		{
			/*String hostname = e.WebSession.Request.RequestUri.Host;
			if(hostname.Contains("dropbox.com"))
			{
				//Exclude Https addresses you don't want to proxy
				//Useful for clients that use certificate pinning
				//for example dropbox.com
				e.DecryptSsl = false;
			}*/

			return Task.FromResult(0);
		}

		private Task OnRequest(Object sender, SessionEventArgs e)
		{
			////read request headers
			//HeaderCollection requestHeaders = e.WebSession.Request.Headers;

			this._request?.Invoke(this, e);

			return Task.FromResult(0);
		}

		//Modify response
		private Task OnResponse(Object sender, SessionEventArgs e)
		{
			//read response headers
			//HeaderCollection responseHeaders = e.WebSession.Response.Headers;

			this._response?.Invoke(this, e);

			return Task.FromResult(0);
		}

		/// Allows overriding default certificate validation logic
		private Task OnCertificateValidation(Object sender, CertificateValidationEventArgs e)
		{
			//set IsValid to true/false based on Certificate Errors
			if(e.SslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
				e.IsValid = true;

			return Task.FromResult(0);
		}

		/// Allows overriding default client certificate selection logic during mutual authentication
		private Task OnCertificateSelection(Object sender, CertificateSelectionEventArgs e)
		{
			//set e.clientCertificate to override
			return Task.FromResult(0);
		}

		public void Dispose()
		{
			if(this.IsProxyRunning)
				this.StopI();
			this._proxy.Dispose();
		}
	}
}