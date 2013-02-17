namespace Method.Inject.Spec.Injections
{
	public class InvalidDoWorkInjection : DoWorkInjection
	{
		public override string UniqueKey
		{
			get { return "test invalid key"; }
		}
	}
}