using Newtonsoft.Json;
using System;

namespace Models
{
    public class Product
    {
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "retail-price")]
		public decimal RetailPrice { get; set; }
    }
}