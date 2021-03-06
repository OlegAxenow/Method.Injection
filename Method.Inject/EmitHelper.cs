﻿using System;
using System.Reflection.Emit;

namespace Method.Inject
{
	/// <summary>
	/// Helper class. Used to simplify frequent operations inside a <see cref="IMethodBuilder"/> implementation.
	/// </summary>
	public static class EmitHelper
	{
		private const string GetInjectionsName = "GetInjections";
		private static readonly Type[] EmptyParameters = new Type[0];

		/// <summary>
		/// First local variable should be injectionType.MakeArrayType() type.
		/// </summary>
		/// <remarks> Parameters not checked.</remarks>
		public static void EmitGetInjections(this ILGenerator il, FieldBuilder injectionSetField, Type injectionType)
		{
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, injectionSetField);
			il.Emit(OpCodes.Callvirt, typeof(InjectionSet).GetMethod(GetInjectionsName, EmptyParameters).MakeGenericMethod(new[] { injectionType }));

			il.Emit(OpCodes.Stloc_0);
		}

		/// <summary>
		/// Emits "for" loop with calls of <paramref name="callInsideLoop"/>.
		/// </summary>
		/// <remarks> Parameters not checked.</remarks>
		public static void EmitInjectionLoop(this ILGenerator il, Action<ILGenerator> callInsideLoop)
		{
			var endOfFor = il.DefineLabel();
			var beginOfFor = il.DefineLabel();

			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc_1);
			il.Emit(OpCodes.Br, endOfFor);

			il.MarkLabel(beginOfFor);
			
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Ldloc_1);
			il.Emit(OpCodes.Ldelem_Ref);

			callInsideLoop(il);

			il.Emit(OpCodes.Ldloc_1);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Add);
			il.Emit(OpCodes.Stloc_1);
				
			il.MarkLabel(endOfFor);

			il.Emit(OpCodes.Ldloc_1);
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Ldlen);

			il.Emit(OpCodes.Conv_I4);
			il.Emit(OpCodes.Clt);
			il.Emit(OpCodes.Stloc_2);
			il.Emit(OpCodes.Ldloc_2);

			il.Emit(OpCodes.Brtrue, beginOfFor);
		}

		/// <summary>
		/// Declares three local variables for injection array and loop.
		/// </summary>
		/// <remarks> Parameters not checked.</remarks>
		public static void DeclareLocalsForInjection(Type injectionType, ILGenerator il)
		{
			il.DeclareLocal(injectionType.MakeArrayType());		// 0
			il.DeclareLocal(typeof(int));						// 1
			il.DeclareLocal(typeof(bool));						// 2
		}
	}
}