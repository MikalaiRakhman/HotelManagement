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

	public static void AgainstUnauthorized(object argument)
	{
		if (argument == null)
		{
			throw new UnauthorizedAccessException($"Failed to login. User with this email was not found.");
		}
	}

	public static void AgainsInvalidPassword(bool isTrue)
	{
		if (!isTrue)
		{
			throw new UnauthorizedAccessException($"Failed to login. Invalid password.");
		}
	}
}
