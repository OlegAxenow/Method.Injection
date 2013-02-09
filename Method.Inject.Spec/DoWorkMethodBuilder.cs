using System;
using System.Diagnostics;
using System.Reflection;
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
		private const string MethodName = "DoWork";

		public void Build(TypeBuilder typeBuilder, FieldBuilder injectionSetField, Type injectionType)
		{
			var parameterTypes = new[] { typeof(string) };

			var method = typeBuilder.DefineMethod(MethodName,
				MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual, null, parameterTypes);

			Debug.Assert(typeBuilder.BaseType != null, "_typeBuilder.BaseType != null");
			var baseMethod = typeBuilder.BaseType.GetMethod(MethodName, parameterTypes);
			var injectionMethod = injectionType.GetMethod(MethodName, new[] { typeof(BaseType), typeof(string) });

			var il = method.GetILGenerator();

			EmitHelper.DeclareLocalsForInjection(injectionType, il);

			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldarg_1);
			il.Emit(OpCodes.Call, baseMethod);

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