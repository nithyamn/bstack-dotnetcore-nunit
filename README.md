# bstack-dotnetcore-nunit

# Add BrowserStack Credentials
- Update your BrowserStack username and access key details in the "appSettings.json" (https://github.com/nithyamn/bstack-dotnetcore-nunit/blob/main/BstackNetCoreNunit/appSettings.json) file
- Get your credentials from here https://www.browserstack.com/accounts/settings

# Commands to trigger tests
- **Build project** - `dotnet build`
- **Single test** - `dotnet test --filter SingleTestCase`. `SingleTestCase` is the name of the test method under `[Test]` annotation that needs to be executed.
- **Local test** - `dotnet test --filter LocalTestCase` 
- **Parallel test** - `dotnet test --filter ParallelTestCase -- NUnit.NumberOfTestWorkers=4`. 

`NUnit.NumberOfTestWorkers` is basically setting the no.of parallel threads to be triggered concurrently. 

Note: Parallel count can also be updated in the `AssemblyInfo.cs` file as `LevelOfParallelism`
`[assembly:LevelOfParallelism(3)]`

# Additional notes
- Use `[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]` along with `Parallelizable` attribute for running parallel tests in a isolated and thread safe manner.

# References
- https://docs.nunit.org/articles/nunit/writing-tests/attributes/parallelizable.html
- https://docs.nunit.org/articles/nunit/writing-tests/attributes/levelofparallelism.html
- https://docs.nunit.org/articles/nunit/running-tests/Console-Command-Line.html
- https://stackoverflow.com/questions/54881733/how-to-pass-workers-parameter-to-nunit-runner-when-running-dotnet-test-for-a
- https://docs.microsoft.com/en-us/dotnet/standard/assembly/
- https://docs.microsoft.com/en-us/dotnet/core/tools/
- https://docs.nunit.org/articles/nunit/writing-tests/attributes/fixturelifecycle.html