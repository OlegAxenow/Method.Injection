## Method.Injection

The goal of this project is to provide a way to inject a dependency into a virtual method with simple lightweight library (it will be used in other projects).
For .NET Framework many dependency injection frameworks exist, but Method.Injection is not a framework.
Method.Injection requires to create *method builders* for each method signature (not a simplest thing because of Reflection.Emit).

## What does that mean?

Usually, you should perform the following steps:

* Choose method to add injection.
* Declare interface for injection (inherited from *IMethodInjection*).
* Implement method builder which uses interface above.
* Register method builder with *MethodBuilderRegistry*.
* Creates *InjectedAssemblyBuilder* with a *InjectionSet* and appends necessary types to support.
* Implement factory to create types with injections.

## Getting started

You can build it from source or install as NuGet package:

	PM> Install-Package Method.Inject

You can see *InjectedTypeBuilderSpec* to see how it works. Here is the excerpt:

	public interface IProtectedWorkInjection : IMethodInjection
	{
		void ProtectedWork(BaseType instance, string parameter);
	}
	
	public class ProtectedWorkInjection : MethodInjection, IProtectedWorkInjection
	{
		public virtual void ProtectedWork(BaseType instance, string parameter)
		{
			instance.CallsLog.Add(GetType().Name + ".ProtectedWork(" + parameter + ")");
		}
	}
	
	// ...
	// arrange
	var injections = new InjectionSet(new DoWorkInjection(), new ProtectedWorkInjection());
	var assemblyBuilder = new InjectedAssemblyBuilder(injections, tb => new InjectedTypeBuilder(tb));
	var type = assemblyBuilder.Append(typeof(BaseType));
	var newType = BaseTypeHelper.Create(type, injections);

	// act
	newType.DoWork("1");

	// assert
	Assert.That(string.Join(",", newType.CallsLog), 
		Is.EqualTo("BaseType(),BaseType.DoWork(1),DoWorkInjection.DoWork(1)"));


If you are not familiar with Reflection.Emit, I recommend you to write some class with overridden methods 
and run [Ildasm.exe](http://msdn.microsoft.com/en-us/library/f7dy01k1(v=vs.110).aspx) to see appropriate IL code.

## How to install from NuGet

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install Method.Inject from the package manager console:

	PM> Install-Package Method.Inject

## Requirements and dependencies

License: [MIT](http://opensource.org/licenses/MIT).

This project supports Visual Studio 2010 and 2012 and both .NET 4.0 and .NET 4.5. For .NET 4.0 and Visual Studio 2010 you should use Method.Injection.net40.sln.

The source code depends on following NuGet packages:

- NUnit (only for Method.Inject.Spec)

## Performance

Injection implemented with the help of Reflection.Emit. It allow to do injection fast enough.