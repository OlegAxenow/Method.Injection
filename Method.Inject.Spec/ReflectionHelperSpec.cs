using System;
using System.Reflection;
using NUnit.Framework;

namespace Method.Inject.Spec
{
	[TestFixture]
	public class ReflectionHelperSpec
	{
		[Test]
		public void Method_from_specified_type_should_be_obtained()
		{
			// arrange + act
			var method = GetMethod(typeof(Inheritor2), "Method2");

			// assert
			Assert.That(method, Is.Not.Null);
			Assert.That(method.DeclaringType, Is.EqualTo(typeof(Inheritor2)));
		}

		[Test]
		public void Method_from_base_type_should_be_obtained()
		{
			// arrange + act
			var method = GetMethod(typeof(Inheritor3), "Method1");

			// assert
			Assert.That(method, Is.Not.Null);
			Assert.That(method.DeclaringType, Is.EqualTo(typeof(Base)));
		}

		[Test]
		public void Method_from_specified_interface_should_be_obtained()
		{
			// arrange + act
			var method = GetMethod(typeof(IMethod4), "Method4");

			// assert
			Assert.That(method, Is.Not.Null);
			Assert.That(method.DeclaringType, Is.EqualTo(typeof(IMethod4)));
		}

		[Test]
		public void Method_from_base_interface_should_be_obtained()
		{
			// arrange + act
			var method = GetMethod(typeof(IMethod4), "Method3");

			// assert
			Assert.That(method, Is.Not.Null);
			Assert.That(method.DeclaringType, Is.EqualTo(typeof(IBase)));
		}

		[Test]
		public void If_method_not_found_exception_should_be_thrown()
		{
			// arrange + act + assert
			Assert.That(() => GetMethod(typeof(Inheritor3), "Method5"), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		private static MethodInfo GetMethod(Type type, string name)
		{
			return ReflectionHelper.GetMethod(type, name, BindingFlags.Instance | BindingFlags.Public, new[] { typeof(int) });
		}

		public class Base
		{
			public void Method1(int id)
			{
			}
		}

		public class Inheritor2 : Base
		{
			public void Method2(int id)
			{
			}
		}

		public interface IBase
		{
			void Method3(int id);
		}

		public interface IMethod4 : IBase
		{
			void Method4(int id);
		}

		public class Inheritor3 : Inheritor2, IMethod4
		{
			public void Method3(int id)
			{
			}

			public void Method4(int id)
			{
			}
		}
	}
}