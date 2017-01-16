using Newtonsoft.Json;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ProductApi.Controllers
{
    public class ProductController : ApiController
    {
		private List<Product> _products;
		public ProductController()
		{
			LoadJson();
		}
		public void LoadJson()
		{
			using (StreamReader r = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/products.json")))
			{
				string json = r.ReadToEnd();
				_products = JsonConvert.DeserializeObject<List<Product>>(json);
			}
		}
		[HttpGet]
		public Product Get(int id)
		{
			Product product = null;
			switch (id)
			{
				case 99:
					throw new HttpResponseException(HttpStatusCode.InternalServerError);
				default:
					product = _products.FirstOrDefault(p => p.Id == id);
					if (product == null)
					{
						//throw new Exception("Something went sideways.");
						throw new HttpResponseException(
								Request.CreateErrorResponse(
									HttpStatusCode.NotFound,
									new Exception(string.Format("A Product with ID {0} was not found.", id))
								)
							);
					}
					break;
			}
			return product;
		}
    }
}