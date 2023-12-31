using SomiodWebService.Models;
using System.Linq;

namespace SomiodWebService.Validations.Validators
{
	public class DataValidator : IValidator<Data>
	{
		private readonly string[] _validContentValues = new string[]
		{
			"on",
			"off"
		};

		public ValidationState Validate(Data obj)
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

			if (string.IsNullOrEmpty(obj.Content))
			{
				state.ErrorMessages.Add(new ValidationErrorMessage("Content is required"));
			}

			if (!_validContentValues.Contains(obj.Content.ToLower()))
			{
				state.ErrorMessages.Add(new ValidationErrorMessage("Content must be either 'on' or 'off'"));
			}

			return state;
		}
	}
}
