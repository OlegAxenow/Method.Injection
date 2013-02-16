using Method.Inject.Spec.Types;

namespace Method.Inject.Spec.Injections
{
	/// <summary>
	/// Test direct type injection (without interfaces).
	/// </summary>
	public class DoWorkInjectionDirect : MethodInjection
	{
		public virtual void DoWork(BaseType instance, string parameter)
		{
			instance.CallsLog.Add(GetType().Name + ".DoWorkInjectionDirect(" + parameter + ")");
		}
	}
}