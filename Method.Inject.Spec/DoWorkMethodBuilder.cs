using System;
using System.Reflection.Emit;
using Method.Inject.Spec.Injections;
using Method.Inject.Spec.Types;

namespace Method.Inject.Spec
{
	/// <summary>
	/// <see cref="IMethodBuilder"/> implementation for testing <see cref="IDoWorkInjection"/>.
	/// </summary>
	public class DoWorkMethodBuilder : IMethodBuilder
	{
		private readonly string _methodName;

		public DoWorkMethodBuilder(string methodName)
		{
			_methodName = methodName;
		}

		public void Build(TypeBuilder typeBuilder, FieldBuilder injectionSetField, Type injectionType)
		{
			var parameterTypes = new[] { typeof(string) };

			var methods = new Methods(typeBuilder, _methodName, parameterTypes);

			var injectionMethod = injectionType.GetMethod(_methodName, new[] { typeof(BaseType), typeof(string) });

			var il = methods.GetILGenerator(injectionType);
			
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldarg_1);
			il.Emit(OpCodes.Call, methods.BaseMethod);

			il.EmitGetInjections(injectionSetField, injectionType);

			il.EmitInjectionLoop(injectionMethod, x =>
			{
				x.Emit(OpCodes.Ldarg_0);
				x.Emit(OpCodes.Ldarg_1);
			});

			il.Emit(OpCodes.Ret);
		} 
	}
}