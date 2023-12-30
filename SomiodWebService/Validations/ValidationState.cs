using System.Collections.Generic;

namespace SomiodWebService.Validations
{
	public class ValidationState
	{
		public bool IsValid => ErrorMessages.Count == 0;

		public List<ValidationErrorMessage> ErrorMessages { get; set; } = new List<ValidationErrorMessage>();
	}
}
