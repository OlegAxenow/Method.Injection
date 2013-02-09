using System.Collections.Generic;

namespace Method.Inject.Spec.Types
{
	/// <summary>
	/// Base type for injection testing.
	/// </summary>
	public class BaseType
	{
		private readonly List<string> _callsLog = new List<string>();

		public BaseType()
		{
			_callsLog.Add("BaseType()");
		}

		public virtual void DoWork(string parameter)
		{
			_callsLog.Add(string.Format("BaseType.DoWork({0})", parameter));
		}

		public List<string> CallsLog
		{
			get { return _callsLog; }
		}
	}
}