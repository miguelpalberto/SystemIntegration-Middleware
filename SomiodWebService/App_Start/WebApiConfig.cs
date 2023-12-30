using System.Web.Http;

namespace SomiodWebService
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			_ = config.Routes.MapHttpRoute(
				name: "Somiod",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			//remove json formatter
			_ = GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.JsonFormatter);

			var xml = config.Formatters.XmlFormatter;
			xml.UseXmlSerializer = true;
		}
	}
}
