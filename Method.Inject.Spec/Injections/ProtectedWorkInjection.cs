using Method.Inject.Spec.Types;

namespace Method.Inject.Spec.Injections
{
	public class ProtectedWorkInjection : MethodInjection, IProtectedWorkInjection
	{
		public virtual void ProtectedWork(BaseType instance, string parameter)
		{
			instance.CallsLog.Add(GetType().Name + ".ProtectedWork(" + parameter + ")");
		}
	}
}