namespace Method.Inject
{
	/// <summary>
	/// Base class for supporting <see cref="IMethodBuilder"/> injections.
	/// </summary>
	public class MethodInjection : IMethodInjection
	{
		/// <summary>
		/// Base implementation uses type name (without namespace) as unique key.
		/// </summary>
		public virtual string UniqueKey
		{
			get { return GetType().Name; }
		}
	}
}