using System;

namespace Method.Inject.Spec.Types
{
	/// <summary>
	/// Helper class. Used for frequent operation
	/// </summary>
	public class BaseTypeHelper
	{
		public static BaseType Create(Type targetType, InjectionSet injectionSet)
		{
			if (!typeof(BaseType).IsAssignableFrom(targetType))
				throw new ArgumentException(string.Format("Target type {0} should be inherited from {1}.", targetType, typeof(BaseType)));

			var constructor = targetType.GetConstructor(new[] { typeof(InjectionSet) });
			if (constructor == null)
				throw new InvalidOperationException(
					string.Format("Constructor with injection set does not supported by {0}.", targetType));
			return (BaseType)constructor.Invoke(new object[] { injectionSet });
		}
	}
}