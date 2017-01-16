using Polly;
using Polly.Timeout;
using Polly.Wrap;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Providers
{
	public class ResiliencyPolicy
	{
		// "Polly" Resiliancy Framework (https://github.com/App-vNext/Polly)

		#region Singleton Implementation
		private static readonly ResiliencyPolicy instance = new ResiliencyPolicy();
		static ResiliencyPolicy()
		{}
		private ResiliencyPolicy()
		{
			InitPolicies();
		}
		public static ResiliencyPolicy Instance
		{
			get { return instance; }
		}
		#endregion

		#region array containing HTTP status codes to handle
		private readonly HttpStatusCode[] _httpStatusCodesToHandle =
		{
			HttpStatusCode.BadGateway,
			HttpStatusCode.BadRequest,
			HttpStatusCode.Conflict,
			HttpStatusCode.ExpectationFailed,
			HttpStatusCode.Forbidden,
			HttpStatusCode.GatewayTimeout,
			HttpStatusCode.Gone,
			HttpStatusCode.HttpVersionNotSupported,
			HttpStatusCode.InternalServerError,
			HttpStatusCode.MethodNotAllowed,
			HttpStatusCode.Moved,
			HttpStatusCode.MovedPermanently,
			HttpStatusCode.NotAcceptable,
			HttpStatusCode.NotFound,
			HttpStatusCode.NotImplemented,
			HttpStatusCode.ProxyAuthenticationRequired,
			HttpStatusCode.RequestEntityTooLarge,
			HttpStatusCode.RequestTimeout,
			HttpStatusCode.RequestUriTooLong,
			HttpStatusCode.ServiceUnavailable,
			HttpStatusCode.Unauthorized,
			HttpStatusCode.UnsupportedMediaType
		};
		#endregion

		private Policy<HttpResponseMessage> _simpleCircuitBreakerPolicy;
		private TimeoutPolicy<HttpResponseMessage> _timeoutPolicy;
		private PolicyWrap<HttpResponseMessage> _policyWrapper;

		public HttpStatusCode[] HttpErrorStatusCodes
		{
			get { return _httpStatusCodesToHandle; }
		}
		public Policy<HttpResponseMessage> SimpleCircuitBreakerPolicy
		{
			get { return _simpleCircuitBreakerPolicy; }
		}
		public TimeoutPolicy<HttpResponseMessage> TimeoiutPolicy
		{
			get { return _timeoutPolicy; }
		}
		public PolicyWrap<HttpResponseMessage> PolicyWrapper
		{
			get { return _policyWrapper; }
		}

		private void InitPolicies()
		{
			_simpleCircuitBreakerPolicy = Policy
				.HandleResult<HttpResponseMessage>(r => _httpStatusCodesToHandle.Contains(r.StatusCode))
				.CircuitBreaker(
					handledEventsAllowedBeforeBreaking: 1,
					durationOfBreak: TimeSpan.FromSeconds(5)
				);

			_timeoutPolicy = Policy.Timeout<HttpResponseMessage>(3);

			_policyWrapper = Policy.Wrap<HttpResponseMessage>(_simpleCircuitBreakerPolicy, _timeoutPolicy);
		}
	}
}