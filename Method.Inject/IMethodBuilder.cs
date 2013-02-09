using System;
using System.Reflection.Emit;

namespace Method.Inject
{
	public interface IMethodBuilder
	{
		void Build(TypeBuilder typeBuilder, FieldBuilder injectionSetField, Type injectionType);
	}
}