namespace SomiodWebService.Validations
{
	public class ValidationErrorMessage
	{
		public string Message { get; set; }

		public ValidationErrorMessage(string errorMessage)
		{
			Message = errorMessage;
		}
	}
}
