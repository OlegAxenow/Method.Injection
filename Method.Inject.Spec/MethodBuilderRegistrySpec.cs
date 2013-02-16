using System.Collections.Generic;
using Method.Inject.Spec.Injections;
using NUnit.Framework;

namespace Method.Inject.Spec
{
	[TestFixture]
	public class MethodBuilderRegistrySpec
	{
		[Test] 
		public void Registered_builders_should_be_returned()
		{
			// arrange
			ResetRegistry();
			
			// act
			var methodBuilder = MethodBuilderRegistry.Get<IDoWorkInjection>();

			// assert
			Assert.That(methodBuilder, Is.Not.Null);
		}

		[Test]
		public void Unregistered_builders_should_throw_exception()
		{
			// arrange
			ResetRegistry();

			// act + assert
			Assert.That(() => MethodBuilderRegistry.Get(typeof(string)), Throws.InstanceOf<KeyNotFoundException>());
		}

		[Test]
		public void Clear_should_remove_registered_builders()
		{
			// arrange
			ResetRegistry();

			// act + assert
			var methodBuilder = MethodBuilderRegistry.Get<IDoWorkInjection>();
			Assert.That(methodBuilder, Is.Not.Null);

			MethodBuilderRegistry.Clear();
			Assert.That(() => MethodBuilderRegistry.Get<IDoWorkInjection>(), Throws.InstanceOf<KeyNotFoundException>());
		}

		private static void ResetRegistry()
		{
			MethodBuilderRegistry.Clear();
			MethodBuilderRegistry.Register<IDoWorkInjection>(new DoWorkMethodBuilder());
		}
	}
}