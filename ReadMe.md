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

You can see *InjectedTypeBuilderSpec* to see how it works.
If you are not familiar with Reflection.Emit, I recommend you to write some class with overridden methods 
and run [Ildasm.exe](http://msdn.microsoft.com/en-us/library/f7dy01k1(v=vs.110).aspx) to see appropriate IL code.

## Requirements and dependencies

License: [MIT](http://opensource.org/licenses/MIT).

This project supports Visual Studio 2010 and 2012 and both .NET 4.0 and .NET 4.5. For .NET 4.0 and Visual Studio 2010 you should use Method.Injection.net40.sln.

The source code depends on following NuGet packages:

- NUnit (only for Method.Inject.Spec)

## Performance

Injection implemented with the help of Reflection.Emit. It allow to do injection fast enough.