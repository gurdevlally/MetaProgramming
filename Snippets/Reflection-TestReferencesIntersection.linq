<Query Kind="Program">
  <Reference Relative="..\MetaProgramming\MetaProgramming.RoslynCTP.Tests\CodeSmells.Samples\CodeSmells.FakeBusinessLogic\bin\Debug\CodeSmells.FakeBusinessLogic.dll">D:\work\Courses\MetaProgramming\MetaProgramming\MetaProgramming.RoslynCTP.Tests\CodeSmells.Samples\CodeSmells.FakeBusinessLogic\bin\Debug\CodeSmells.FakeBusinessLogic.dll</Reference>
  <Reference Relative="..\MetaProgramming\MetaProgramming.RoslynCTP.Tests\CodeSmells.Samples\CodeSmells.FakeDataAccessLibrary\bin\Debug\CodeSmells.FakeDataAccessLibrary.dll">D:\work\Courses\MetaProgramming\MetaProgramming\MetaProgramming.RoslynCTP.Tests\CodeSmells.Samples\CodeSmells.FakeDataAccessLibrary\bin\Debug\CodeSmells.FakeDataAccessLibrary.dll</Reference>
  <Reference>C:\Chocolatey\lib\NUnit.Runners.2.6.2\tools\nunit.framework.dll</Reference>
  <Reference>C:\Chocolatey\lib\NUnit.Runners.2.6.2\tools\lib\nunit-console-runner.dll</Reference>
  <Namespace>NUnit.Framework</Namespace>
</Query>

void Main()
{
	// nunit runner
	NUnit.ConsoleRunner.Runner.Main(new string[]
   	{
    	Assembly.GetExecutingAssembly().Location, 
   	});
}

public IEnumerable Assemblies 
{
	get
	{
		yield return typeof(CodeSmells.FakeDataAccessLibrary.Repository).Assembly;
		yield return typeof(CodeSmells.FakeBusinessLogic.BusinessRule).Assembly;
	}
}

[Test]
[Combinatorial]
public void TestReferencesIntersection(
    [ValueSource("Assemblies")] Assembly leftAssembly,
    [ValueSource("Assemblies")] Assembly rightAssembly)
{
    if (leftAssembly == rightAssembly)
    {
        return;
    }

    var rightReferences = FilterOutAssemblies(rightAssembly);
    var leftReferences = FilterOutAssemblies(leftAssembly);
    var intersection = leftReferences.Intersect(rightReferences).ToArray();

    Assert.That(intersection, 
				Is.Not.Null.And.Empty);
}

private static IEnumerable<string> FilterOutAssemblies(Assembly source)
{
    return new HashSet<string>(source
        .GetReferencedAssemblies()
        .Where(assembly => assembly.Name != @"mscorlib"
                            && assembly.Name != @"Microsoft.CSharp"
                            && !assembly.Name.StartsWith(@"System")
                            && !assembly.Name.StartsWith(@"AutoMapper")
                            && !assembly.Name.StartsWith(@"nCrunch.TestRuntime")
                            && !assembly.Name.StartsWith(@"Ninject")
                            && !assembly.Name.StartsWith(@"log4net"))
        .Select(assembly => assembly.Name));
}