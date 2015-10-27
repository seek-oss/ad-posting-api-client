using System;
using FluentAssertions;
using NUnit.Framework;
using SEEK.AdPostingApi.SampleConsumer.Tests;

[SetUpFixture]
public class SetUpFixture
{
    [SetUp]
    public static void Setup()
    {
        // See https://github.com/dennisdoomen/fluentassertions/issues/305 - ShouldBeEquivalentTo fails with objects from the System namespace.
        // Due to this, we need to change the IsValueType predicate so that it does not assume System.Exception and derivatives of it in the System namespace are value types.
        AssertionOptions.IsValueType = type => (type.Namespace == typeof (int).Namespace) && !(type == typeof (Exception) || type.IsSubclassOf(typeof (Exception)));
    }

    [TearDown]
    public static void TearDown()
    {
        PactProvider.AssemblyCleanup();
    }
}