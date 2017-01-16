using System;
using System.Web.Mvc;
using Providers;
using Models;

namespace WebApp.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult About()
		{
			return View();
		}
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult GetProduct(int id)
		{
			Product product = null;
			try
			{
				product	= new ProductProvider().GetProduct(id);
				if (product != null)
					ViewBag.Message = string.Format("'{0}' product retrieved.", product.Name);
			}
			catch(Exception exc)
			{
				ViewBag.Message = string.Format("{0}", exc.Message);
			}
			return View(product);
		}
	}
}