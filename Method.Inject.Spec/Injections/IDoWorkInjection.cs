using Method.Inject.Spec.Types;

namespace Method.Inject.Spec.Injections
{
	public interface IDoWorkInjection : IMethodInjection
	{
		void DoWork(BaseType instance, string parameter);
	}
}