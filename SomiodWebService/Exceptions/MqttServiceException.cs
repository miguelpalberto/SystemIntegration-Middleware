using System;

namespace SomiodWebService.Exceptions
{
	public class MqttServiceException : Exception
	{
		public MqttServiceException(string message) : base(message)
		{
		}
	}
}
