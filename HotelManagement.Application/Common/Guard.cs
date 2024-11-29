public static class Guard
{
	public static void AgainstNull(object argument, string argumentName)
	{
		if (argument == null)
		{
			throw new ArgumentNullException(argumentName, $"{argumentName} cannot be null.");
		}
	}

	public static void AgainstNullOrEmpty(Guid argument, string argumentName)
	{
		if (argument == Guid.Empty)
		{
			throw new ArgumentException($"{argumentName} cannot be empty.", argumentName);
		}
	}
}
