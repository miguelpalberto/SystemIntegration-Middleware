using System;

namespace SomiodWebService.Exceptions
{
	public class RestSharpServiceException : Exception
	{
		public RestSharpServiceException(string message) : base(message)
		{
		}
	}
}
