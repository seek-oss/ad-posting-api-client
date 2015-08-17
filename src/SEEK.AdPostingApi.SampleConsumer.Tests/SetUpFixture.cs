using NUnit.Framework;
using SEEK.AdPostingApi.SampleConsumer.Tests;

[SetUpFixture]
public class SetUpFixture
{
    [TearDown]
    public static void TearDown()
    {
        PactProvider.AssemblyCleanup();
    }
}
