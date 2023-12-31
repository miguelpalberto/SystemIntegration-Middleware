using RestSharp;
using SomiodWebService.Exceptions;
using SomiodWebService.Models;

namespace SomiodWebService.Services
{
	public static class RestSharpService
	{
		public static void FireNotification(string endpoint, string topic, Notification notification)
		{
			try
			{
				using (var client = new RestClient(endpoint))
				{
					var request = new RestRequest(topic, Method.Post);
					_ = request.AddHeader("Content-Type", "application/xml");
					_ = request.AddXmlBody(notification);

					_ = client.Execute(request);
				}
			}
			catch (System.Exception)
			{
				throw new RestSharpServiceException("RestSharp client failed to execute");
			}
		}
	}
}
