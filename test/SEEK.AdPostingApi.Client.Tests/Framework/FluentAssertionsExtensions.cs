using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public static class FluentAssertionsExtensions
    {
        public static void ShouldBeEquivalentToException<TSubject>(this TSubject actualException, TSubject expectedException, string because = "", params object[] reasonArgs)
            where TSubject : Exception
        {
            actualException.ShouldBeEquivalentTo(expectedException, options => options.ExcludingExceptionRuntimeProperties(), because, reasonArgs);
        }

        public static EquivalencyAssertionOptions<TSubject> ExcludingNonPublicProperties<TSubject>(this EquivalencyAssertionOptions<TSubject> options)
        {
            var internalPropertyNames = typeof(TSubject).GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Select(p => p.Name).ToArray();

            return options.Excluding(e => internalPropertyNames.Contains(e.SelectedMemberInfo.Name));
        }

        public static EquivalencyAssertionOptions<TSubject> ExcludingNonPublicFields<TSubject>(this EquivalencyAssertionOptions<TSubject> options)
        {
            var internalFieldNames = typeof(TSubject).GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Select(f => f.Name).ToArray();

            return options.Excluding(e => internalFieldNames.Contains(e.SelectedMemberInfo.Name));
        }

        public static EquivalencyAssertionOptions<TSubject> ExcludingExceptionRuntimeProperties<TSubject>(this EquivalencyAssertionOptions<TSubject> options)
            where TSubject : Exception
        {
            var publicPropertyNames = new[] { nameof(Exception.Source), nameof(Exception.StackTrace), nameof(Exception.TargetSite) };

            return options
                .ExcludingNonPublicProperties()
                .ExcludingNonPublicFields()
                .Excluding(e => publicPropertyNames.Contains(e.SelectedMemberInfo.Name));
        }
    }
}