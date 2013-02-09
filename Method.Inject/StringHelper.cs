using System;

namespace Method.Inject
{
	public static class StringHelper
	{
		public static readonly char[] InvalidNameCharacters =
			new[] { ' ', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '?', '.', ',', '"', '`', ':', ';', '{', '}', '<', '>', '|', '\'', '/', '\\' };

		public static bool IsInvalidName(string name)
		{
			if (name == null) throw new ArgumentNullException("name");
			return name.IndexOfAny(InvalidNameCharacters) >= 0;
		}
	}
}