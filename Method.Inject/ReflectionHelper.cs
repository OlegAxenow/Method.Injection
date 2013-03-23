using System;
using System.Reflection;

namespace Method.Inject
{
	public static class ReflectionHelper
	{
		/// <summary>
		/// Returns closest method (if not found in specified <paramref name="type"/>, search base interfaces).
		/// </summary>
		/// <remarks>Delegates work to <see cref="Type.GetMethod(string, BindingFlags, Binder, Type[], ParameterModifier[])"/>.</remarks>
		/// <exception cref="ArgumentOutOfRangeException">If method with specified name does not found.</exception>
		public static MethodInfo GetMethod(Type type, string name, BindingFlags bindingAttr, 
			Type[] parameterTypes, Binder binder = null, ParameterModifier[] modifiers = null)
		{
			if (type == null) throw new ArgumentNullException("type");
			var result = type.GetMethod(name, bindingAttr, binder, parameterTypes, modifiers);
			if (result != null)
				return result;

			foreach (var baseInterface in type.GetInterfaces())
			{
				result = baseInterface.GetMethod(name, bindingAttr, binder, parameterTypes, modifiers);
				if (result != null)
					return result;
			}
			
			throw new ArgumentOutOfRangeException("name");
		}
	}
}