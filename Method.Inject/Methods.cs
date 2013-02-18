using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Method.Inject
{
	/// <summary>
	/// Provides base and defines new method.
	/// </summary>
	public class Methods
	{
		/// <summary>
		/// Initializes properties for the specified parameters.
		/// </summary>
		/// <param name="typeBuilder">Properly initialized <see cref="TypeBuilder"/> with <see cref="TypeBuilder.BaseType"/>.</param>
		/// <param name="methodName">Name of the method to override.</param>
		/// <param name="parameterTypes">Parameters of the base method.</param>
		/// <param name="returnType">Optional return type of the base method.</param>
		public Methods(TypeBuilder typeBuilder, string methodName, Type[] parameterTypes, Type returnType = null)
		{
			if (typeBuilder == null) throw new ArgumentNullException("typeBuilder");
			if (parameterTypes == null) throw new ArgumentNullException("parameterTypes");
			if (methodName == null) throw new ArgumentNullException("methodName");

			Debug.Assert(typeBuilder.BaseType != null, "typeBuilder.BaseType != null");
			BaseMethod = typeBuilder.BaseType.GetMethod(methodName,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, parameterTypes, null);
			
			if (!BaseMethod.IsVirtual)
				throw new ArgumentException(string.Format("Method {0}.{1} is not virtual.", typeBuilder.BaseType.Name, methodName));

			OverriddenMethod = typeBuilder.DefineMethod(methodName, 
				(BaseMethod.IsPublic ? MethodAttributes.Public : MethodAttributes.Family) | MethodAttributes.HideBySig |
					MethodAttributes.Virtual, returnType, parameterTypes);
		}

		/// <summary>
		/// Gets <see cref="ILGenerator"/> from <see cref="OverriddenMethod"/> and calls <see cref="EmitHelper.DeclareLocalsForInjection"/>.
		/// </summary>
		public ILGenerator GetILGenerator(Type injectionType)
		{
			var il = OverriddenMethod.GetILGenerator();
			EmitHelper.DeclareLocalsForInjection(injectionType, il);
			return il;
		}

		public MethodInfo BaseMethod { get; private set; }

		public MethodBuilder OverriddenMethod { get; private set; }


	}
}