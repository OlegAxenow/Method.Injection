using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Method.Inject
{
	/// <summary>
	/// Contains set of injections.
	/// </summary>
	/// <remarks> If injection implement interface derived from <see cref="IMethodInjection"/>, this interface will be used in 
	/// <see cref="GetInjectionTypes"/>. If occasionally more than one interface implemented, only one will be used.</remarks>
	public class InjectionSet
	{
		private readonly Dictionary<Type, List<IMethodInjection>> _injectionsByType;

		public InjectionSet(params IMethodInjection[] injections)
			: this((IEnumerable<IMethodInjection>)injections)
		{
		}

		public InjectionSet(IEnumerable<IMethodInjection> injections)
		{
			if (injections == null) throw new ArgumentNullException("injections");

			_injectionsByType = new Dictionary<Type, List<IMethodInjection>>();
			var builder = new StringBuilder();

			foreach (var injection in injections.OrderBy(x => x.GetType().FullName))
			{
				AddToLists(injection);

				if (StringHelper.IsInvalidName(injection.UniqueKey))
					throw new ArgumentException(string.Format("Injection {0} has illegal characters in unique key '{1}'.", 
						injection.GetType(), injection.UniqueKey));
				builder.Append("_").Append(injection.UniqueKey);
			}

			UniqueKey = builder.ToString();
		}

		private void AddToLists(IMethodInjection methodInjection)
		{
			var type = methodInjection.GetType();
			var interfaces = type.GetInterfaces();

			foreach (var @interface in interfaces
				.Where(@interface => typeof(IMethodInjection).IsAssignableFrom(@interface) && typeof(IMethodInjection) != @interface))
			{
				type = @interface;
				break;
			}

			List<IMethodInjection> list;
			if (!_injectionsByType.TryGetValue(type, out list))
			{
				list = new List<IMethodInjection>();
				_injectionsByType.Add(type, list);
			}
			
			list.Add(methodInjection);
		}

		public string UniqueKey { get; private set; }

		/// <summary>
		/// Returns distinct injection types in this <see cref="InjectionSet"/>.
		/// </summary>
		public IEnumerable<Type> GetInjectionTypes()
		{
			return _injectionsByType.Keys;
		}

		/// <summary>
		/// Returns injections by type of injection.
		/// </summary>
		public T[] GetInjections<T>() where T : IMethodInjection
		{
			List<IMethodInjection> injections;
			return _injectionsByType.TryGetValue(typeof(T), out injections) ? injections.Cast<T>().ToArray() : new T[0];
		}
	}
}