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

			config.Routes.MapHttpRoute(
				name: "Somiod",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			var xml = config.Formatters.XmlFormatter;
			xml.UseXmlSerializer = true;
		}
	}
}
