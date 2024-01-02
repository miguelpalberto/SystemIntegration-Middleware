using SomiodWebService.Exceptions;
using SomiodWebService.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace SomiodWebService.Services
{
	public static class MqttService
	{
		private static Guid _clientGuid = Guid.NewGuid();

		public static void FireNotification(string endpoint, string topic, Notification notification)
		{
			var client = new MqttClient(endpoint);

			_ = client.Connect(_clientGuid.ToString());

			if (!client.IsConnected)
			{
				throw new MqttServiceException("Could not connect to MQTT broker");
			}

			_ = client.Publish(topic, Encoding.UTF8.GetBytes(XMLConversionService.ConvertToXMLDocument(notification).OuterXml));

			if (client.IsConnected)
			{
				// Wait for the message to be sent before disconnecting
				Task.Delay(1000).Wait();
				client.Disconnect();
			}
		}
	}
}
