using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace Method.Inject
{
	/// <summary>
	/// Uses Reflection.Emit to create new dynamic assembly with injected contexts. Each context should be added with <see cref="Append"/>.
	/// </summary>
	public class InjectedAssemblyBuilder
	{
		private const char NameSeparator = '_';
		private readonly InjectionSet _injectionSet;
		private readonly Func<TypeBuilder, InjectedTypeBuilder> _injectedTypeBuilderFactory;
		private readonly AssemblyBuilder _assemblyBuilder;
		private readonly ModuleBuilder _moduleBuilder;

		public InjectedAssemblyBuilder(InjectionSet injectionSet, Func<TypeBuilder, InjectedTypeBuilder> injectedTypeBuilderFactory,
			bool saveAssemblyToDisk = false)
		{
			if (injectionSet == null) throw new ArgumentNullException("injectionSet");
			if (injectedTypeBuilderFactory == null) throw new ArgumentNullException("injectedTypeBuilderFactory");

			_injectionSet = injectionSet;
			_injectedTypeBuilderFactory = injectedTypeBuilderFactory;

			var assemblyName = new AssemblyName { Name = injectionSet.UniqueKey + Guid.NewGuid() };

			_assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName,
				saveAssemblyToDisk ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run);

			_moduleBuilder = saveAssemblyToDisk ? _assemblyBuilder.DefineDynamicModule(assemblyName.Name, assemblyName.Name + ".mod")
				: _assemblyBuilder.DefineDynamicModule(assemblyName.Name, false);
		}

		/// <summary>
		/// Appends new injected type based on specified type.
		/// </summary>
		public Type Append(Type injectedType)
		{
			if (injectedType == null) throw new ArgumentNullException("injectedType");
			var typeBuilder = CreateTypeBuilder(injectedType);
			var builder = _injectedTypeBuilderFactory(typeBuilder);
			builder.BuildConstructor();

			foreach (var type in _injectionSet.GetInjectionTypes())
			{
				builder.BuildMethod(MethodBuilderRegistry.Get(type), type);
			}

			return typeBuilder.CreateType();
		}

		/// <summary>
		/// Saves assembly to disk for debugging purposes.
		/// </summary>
		public void Save()
		{
			_assemblyBuilder.Save(_assemblyBuilder.GetName().Name + ".dll");
		}

		private TypeBuilder CreateTypeBuilder(Type contextType)
		{
			return _moduleBuilder.DefineType(GetTypeName(contextType) + NameSeparator + _injectionSet.UniqueKey,
				TypeAttributes.Public |
					TypeAttributes.Class |
					TypeAttributes.AutoClass |
					TypeAttributes.AnsiClass |
					TypeAttributes.BeforeFieldInit |
					TypeAttributes.AutoLayout,
				contextType);
		}

		private string GetTypeName(Type contextType)
		{
			if (!contextType.IsGenericType)
				return contextType.Name;

			var builder = new StringBuilder();
			var endIndex = contextType.Name.IndexOf('`');
			builder.Append(contextType.Name.Substring(0, endIndex)).Append(NameSeparator);
			foreach (var argument in contextType.GetGenericArguments())
			{
				builder.Append(GetTypeName(argument)).Append(NameSeparator);
			}
			
			return builder.ToString();
		}
	}
}