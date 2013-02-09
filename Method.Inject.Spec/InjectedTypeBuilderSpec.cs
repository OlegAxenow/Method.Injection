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
			MethodBuilderRegistry.Register<IDoWorkInjection>(new DoWorkMethodBuilder());
		}

		[Test]
		public void Base_methods_and_injection_should_be_called()
		{
			// arrange
			var injections = new InjectionSet(new DoWorkInjection());
			var assemblyBuilder = new InjectedAssemblyBuilder(injections, tb => new InjectedTypeBuilder(tb));

			// act
			var type = assemblyBuilder.Append(typeof(BaseType));
			var newType = BaseTypeHelper.Create(type, injections);
			newType.DoWork("1");

			// assert
			Assert.That(string.Join(",", newType.CallsLog), Is.EqualTo("BaseType(),BaseType.DoWork(1),DoWorkInjection.DoWork(1)"));
		}
	}
}