using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Mango.Services.ShoppingCartAPI.Utility
{
	public class BackendApiAuthenticationHttpClientHandler : DelegatingHandler
	{
		private readonly IHttpContextAccessor _accesor;

		public BackendApiAuthenticationHttpClientHandler(IHttpContextAccessor accesor)
		{
			_accesor = accesor;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await _accesor.HttpContext.GetTokenAsync("access_token");

			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
