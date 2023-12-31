using SomiodWebService.Models;
using System.Linq;

namespace SomiodWebService.Validations.Validators
{
	public class SubscriptionValidator : IValidator<Subscription>
	{
		private readonly string[] _validEventValues = new string[]
		{
			"1", //CREATION
			"2" , //DELETION
			"12" //BOTH
		};

		public ValidationState Validate(Subscription obj)
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

			if (string.IsNullOrEmpty(obj.Endpoint))
			{
				state.ErrorMessages.Add(new ValidationErrorMessage("Endpoint is required"));
			}

			if (string.IsNullOrEmpty(obj.Event))
			{
				state.ErrorMessages.Add(new ValidationErrorMessage("Event is required"));
			}

			if (!_validEventValues.Contains(obj.Event))
			{
				state.ErrorMessages.Add(new ValidationErrorMessage("Event must be 1(CREATION), 2(DELETION) or 12(BOTH)"));
			}

			return state;
		}
	}
}
