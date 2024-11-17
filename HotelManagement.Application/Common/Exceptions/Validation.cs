using FluentValidation.Results;

namespace HotelManagement.Application.Common.Exceptions
{
	public class Validation : Exception
	{
		public IDictionary<string, string[]> Errors { get; }
		public Validation() : base("One or more validation errors have occured. See the list below.") 
		{
			Errors = new Dictionary<string, string[]>();
		}

		public Validation(IEnumerable<ValidationFailure> failures) : this()
		{
			Errors = failures
				.GroupBy(f => f.PropertyName, e => e.ErrorMessage)
				.ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
		}
	}
}
