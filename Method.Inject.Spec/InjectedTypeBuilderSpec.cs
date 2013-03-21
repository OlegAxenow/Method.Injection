using Method.Inject.Spec.Injections;
using Method.Inject.Spec.Types;
using NUnit.Framework;

namespace Method.Inject.Spec
{
	[TestFixture]
	public class InjectedTypeBuilderSpec
	{
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			MethodBuilderRegistry.Register<IDoWorkInjection>(new DoWorkMethodBuilder("DoWork"));
			MethodBuilderRegistry.Register<IProtectedWorkInjection>(new DoWorkMethodBuilder("ProtectedWork"));
			MethodBuilderRegistry.Register<NonVirtualWorkInjection>(new DoWorkMethodBuilder("NonVirtualWork"));
		}

		[Test]
		public void Base_methods_and_injection_should_be_called()
		{
			// arrange
			var injections = new InjectionSet(new DoWorkInjection(), new ProtectedWorkInjection());
			var assemblyBuilder = new InjectedAssemblyBuilder(injections, tb => new InjectedTypeBuilder(tb));
			var type = assemblyBuilder.Append(typeof(BaseType));
			var newType = BaseTypeHelper.Create(type, injections);

			// act
			newType.DoWork("1");

			// assert
			Assert.That(string.Join(",", newType.CallsLog), Is.EqualTo("BaseType(),BaseType.DoWork(1),DoWorkInjection.DoWork(1)"));

			// act
			newType.CallsLog.Clear();
			newType.CallProtectedWork("2");

			// assert
			Assert.That(string.Join(",", newType.CallsLog), Is.EqualTo("BaseType.ProtectedWork(2),ProtectedWorkInjection.ProtectedWork(2)"));
		}

		[Test]
		public void Base_methods_and_generic_injection_should_be_called()
		{
			// arrange
			var injections = new InjectionSet(new DoWorkInjection(), new ProtectedWorkInjection());
			var assemblyBuilder = new InjectedAssemblyBuilder(injections, tb => new InjectedTypeBuilder(tb));
			var type = assemblyBuilder.Append(typeof(GenericBaseType<int, string>));
			var newType = BaseTypeHelper.Create(type, injections);

			// act
			newType.DoWork("1");

			// assert
			Assert.That(string.Join(",", newType.CallsLog), Is.EqualTo("BaseType(),BaseType.DoWork(1),DoWorkInjection.DoWork(1)"));

			// act
			newType.CallsLog.Clear();
			newType.CallProtectedWork("2");

			// assert
			Assert.That(string.Join(",", newType.CallsLog), Is.EqualTo("BaseType.ProtectedWork(2),ProtectedWorkInjection.ProtectedWork(2)"));
		}

		[Test]
		public void Non_virtual_method_should_be_rejected()
		{
			// arrange
			var injections = new InjectionSet(new NonVirtualWorkInjection());
			var assemblyBuilder = new InjectedAssemblyBuilder(injections, tb => new InjectedTypeBuilder(tb));
			
			// act + assert
			Assert.That(() => assemblyBuilder.Append(typeof(BaseType)), Throws.ArgumentException);
		}
	}
}