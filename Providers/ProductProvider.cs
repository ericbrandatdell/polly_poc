using Models;
using System;
using System.Net.Http;

namespace Providers
{
    public class ProductProvider
    {
		public Product GetProduct(int id)
		{
			var response = ResiliencyPolicy.Instance.PolicyWrapper.Execute(() =>
				DoServiceCall(id)
			);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(string.Format("{0} ({1})", response.ReasonPhrase, response.StatusCode));
			}
			else
			{
				return response.Content.ReadAsAsync<Product>().Result;
			}
		}

		private HttpResponseMessage DoServiceCall(int id)
		{
			return new HttpClient().GetAsync(string.Format("http://localhost:61569/api/product/{0}", id)).Result;
		}
    }
}