namespace Method.Inject.Spec.Types
{
	/// <summary>
	/// Base type for generic injection testing.
	/// </summary>
	public class GenericBaseType<T1, T2> : BaseType
	{
		public T1 Test1 { get; set; }
		
		public T2 Test2 { get; set; }
	}
}