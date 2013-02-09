using Method.Inject.Spec.Types;

namespace Method.Inject.Spec.Injections
{
	public class DoWorkInjectionDirect : MethodInjection
	{
		public virtual void DoWork(BaseType instance, string parameter)
		{
			instance.CallsLog.Add(GetType().Name + ".DoWork(" + parameter + ")");
		}
	}
}