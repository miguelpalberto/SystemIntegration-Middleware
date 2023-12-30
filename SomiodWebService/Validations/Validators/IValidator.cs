namespace SomiodWebService.Validations
{
	internal interface IValidator<T> where T : class, new()
	{
		ValidationState Validate(T obj);
	}
}
