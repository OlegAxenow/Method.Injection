namespace Method.Inject
{
	/// <summary>
	/// Base interface for supporting injections with <see cref="IMethodBuilder"/> .
	/// </summary>
	public interface IMethodInjection
	{
		/// <summary>
		/// Unique key to distinguish different injections. Used to generate class name. Don't use illegal characters (illegal for identifiers).
		/// </summary>
		string UniqueKey { get; }
	}
}