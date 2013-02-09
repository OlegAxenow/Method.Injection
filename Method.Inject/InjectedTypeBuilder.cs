using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;

namespace Method.Inject
{
	public class InjectedTypeBuilder
	{
		private readonly TypeBuilder _typeBuilder;

		private readonly FieldBuilder _injectionSetField;

		public InjectedTypeBuilder(TypeBuilder typeBuilder)
		{
			if (typeBuilder == null) throw new ArgumentNullException("typeBuilder");

			_typeBuilder = typeBuilder;

			_injectionSetField = typeBuilder.DefineField("_injectionSet", typeof(InjectionSet), 
				FieldAttributes.Private | FieldAttributes.InitOnly);
		}

		public void BuildMethod(IMethodBuilder methodBuilder, Type injectionType)
		{
			if (methodBuilder == null) throw new ArgumentNullException("methodBuilder");
			methodBuilder.Build(_typeBuilder, _injectionSetField, injectionType);
		}

		/// <summary>
		/// Builds constructor if necessary (see example). Default implementation builds constructor for base constructor without parameters.
		/// </summary>
		/// <example>
		/// public BaseClass_generated(InjectionSet ps) {
		/// 	_injectionSet = ps;
		/// }
		/// </example>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
		public virtual void BuildConstructor()
		{
			var constructorBuilder =
				_typeBuilder.DefineConstructor(
					MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, 
					CallingConventions.Standard, new[] { typeof(InjectionSet) });

			Debug.Assert(_typeBuilder.BaseType != null, "_typeBuilder.BaseType != null");

			var baseConstructor = _typeBuilder.BaseType.GetConstructor(new Type[0]);
			if (baseConstructor == null)
				throw new ArgumentException(
					string.Format("Type {0} does not have a default constructor.", 
						_typeBuilder.BaseType));

			var il = constructorBuilder.GetILGenerator();
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Call, baseConstructor);

			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldarg_1);
			il.Emit(OpCodes.Stfld, _injectionSetField);

			il.Emit(OpCodes.Ret);
		}

		protected TypeBuilder TypeBuilder
		{
			get { return _typeBuilder; }
		}

		protected FieldBuilder InjectionSetField
		{
			get { return _injectionSetField; }
		}
	}
}