using NUnit.Framework;

namespace Method.Inject.Spec
{
	[TestFixture]
	public class StringHelperSpec
	{
		[TestCase("test")]
		[TestCase("aaa_aaa")]
		public void Normal_name_should_be_accepted(string name)
		{
			Assert.That(StringHelper.IsInvalidName(name), Is.False);
		}

		[Test]
		public void Invalid_name_should_not_be_accepted()
		{
			foreach (var invalidNameCharacter in StringHelper.InvalidNameCharacters)
			{
				Assert.That(StringHelper.IsInvalidName("abc" + invalidNameCharacter + "123"), Is.True);
			}	
		}
	}
}