using SomiodWebService.Models;

namespace SomiodWebService.Validations.Validators
{
	public class ApplicationValidator : IValidator<Application>
	{
		public ValidationState Validate(Application obj)
		{
			var state = new ValidationState();

			if (string.IsNullOrEmpty(obj.Name))
			{
				state.ErrorMessages.Add(new ValidationErrorMessage("Name is required"));
			}

			if (obj.Name.Length < 3)
			{
				state.ErrorMessages.Add(new ValidationErrorMessage("Name must be at least 3 characters"));
			}

			if (obj.Name.Length > 255)
			{
				state.ErrorMessages.Add(new ValidationErrorMessage("Name cannot be longer than 50 characters"));
			}

			return state;
		}
	}
}
