using Method.Inject.Spec.Types;

namespace Method.Inject.Spec.Injections
{
	public interface IProtectedWorkInjection : IMethodInjection
	{
		void ProtectedWork(BaseType instance, string parameter);
	}
}