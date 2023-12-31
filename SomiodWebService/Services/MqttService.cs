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
			var client = new MqttClient("127.0.0.1");

			_ = client.Connect(_clientGuid.ToString());

			if (!client.IsConnected)
			{
				throw new MqttServiceException("Could not connect to MQTT broker");
			}

			_ = client.Publish(topic, Encoding.UTF8.GetBytes(XMLConversionService.ConvertToXMLDocument(notification).OuterXml));

			//dont immediately disconnect, wait for the message to be sent
			_ = Task.Delay(2000);
			client.Disconnect();
		}
	}
}
