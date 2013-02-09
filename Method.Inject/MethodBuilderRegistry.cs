using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Method.Inject
{
	/// <summary>
	/// Provides simple method to register and get <see cref="IMethodBuilder"/> for injection types (or interfaces).
	/// </summary>
	public static class MethodBuilderRegistry
	{
		private static readonly ConcurrentDictionary<Type, IMethodBuilder> Builders = new ConcurrentDictionary<Type, IMethodBuilder>();

		/// <summary>
		/// Registers <see cref="IMethodBuilder"/> for the specified injection type (<see cref="T"/>).
		/// </summary>
		public static void Register<T>(IMethodBuilder builder) where T : IMethodInjection
		{
			Register(typeof(T), builder);
		}

		/// <summary>
		/// Registers <see cref="IMethodBuilder"/> for the specified injection type.
		/// </summary>
		public static void Register(Type injectionType, IMethodBuilder builder)
		{
			if (injectionType == null) throw new ArgumentNullException("injectionType");
			if (builder == null) throw new ArgumentNullException("builder");

			Builders.AddOrUpdate(injectionType, builder, (t, b) => builder);
		}

		/// <summary>
		/// Gets <see cref="IMethodBuilder"/> for the specified injection type.
		/// </summary>
		/// <exception cref="KeyNotFoundException">If <see name="T"/> does not registered with <see cref="Register"/>.</exception>
		public static IMethodBuilder Get<T>()
		{
			return Get(typeof(T));
		}

		/// <summary>
		/// Gets <see cref="IMethodBuilder"/> for the specified injection type.
		/// </summary>
		/// <exception cref="KeyNotFoundException">If <paramref name="injectionType"/> does not registered with <see cref="Register"/>.</exception>
		public static IMethodBuilder Get(Type injectionType)
		{
			return Builders[injectionType];
		}

		/// <summary>
		/// Clears previously registered <see cref="IMethodBuilder"/>s.
		/// </summary>
		public static void Clear()
		{
			Builders.Clear();
		}
	}
}