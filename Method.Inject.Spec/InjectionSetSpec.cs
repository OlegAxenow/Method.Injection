using Method.Inject.Spec.Injections;
using NUnit.Framework;

namespace Method.Inject.Spec
{
	[TestFixture]
	public class InjectionSetSpec
	{
		[Test]
		public void GetInjectionTypes_should_return_interfaces_instead_of_indirect_types()
		{
			// arrange
			var set = new InjectionSet(new DoWorkInjection(), new DoWorkInjection2());
			
			// act
			var injectionTypes = set.GetInjectionTypes();

			// assert
			Assert.That(injectionTypes, Is.EquivalentTo(new[] { typeof(IDoWorkInjection) }));
		}

		[Test]
		public void GetInjectionTypes_should_return_direct_types_or_interfaces()
		{
			// arrange
			var set = new InjectionSet(new DoWorkInjection(), new DoWorkInjectionDirect());

			// act
			var injectionTypes = set.GetInjectionTypes();

			// assert
			Assert.That(injectionTypes, Is.EquivalentTo(new[] { typeof(IDoWorkInjection), typeof(DoWorkInjectionDirect) }));
		}

		[Test]
		public void GetInjections_should_return_direct_types_or_interfaces()
		{
			// arrange
			var set = new InjectionSet(new DoWorkInjection(), new DoWorkInjectionDirect());

			// act
			var injection1 = set.GetInjections<IDoWorkInjection>();
			var injection2 = set.GetInjections<DoWorkInjection>();
			var injection3 = set.GetInjections<DoWorkInjectionDirect>();

			// assert
			Assert.That(injection1, Is.Not.Empty);
			Assert.That(injection2, Is.Empty);
			Assert.That(injection3, Is.Not.Empty);
		}

		[Test]
		public void Invalid_unique_key_should_be_rejected()
		{
			// arrange + act + assert
			Assert.That(() => new InjectionSet(new InvalidDoWorkInjection()), Throws.ArgumentException);
		}
	}
}