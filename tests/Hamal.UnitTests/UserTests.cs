using System.Reflection;
using Hamal.Domain.Entities;

namespace Hamal.UnitTests;

public class UserTests
{
    private static Type? TryGetUserType()
    {
        // Use the Hamal.Domain assembly (discovered via the Role enum) to locate the User type.
        var domainAssembly = typeof(User).Assembly;

        // Prefer an exact match on the canonical namespace first.
        var type = domainAssembly.GetType("Hamal.Domain.User");

        // If not found, fall back to any class named "User" within Hamal.Domain* namespaces.
        type ??= domainAssembly
            .GetTypes()
            .FirstOrDefault(t =>
                t.IsClass &&
                t.Name == "User" &&
                t.Namespace != null &&
                t.Namespace.StartsWith("Hamal.Domain", StringComparison.Ordinal));

        return type;
    }

    [Fact]
    public void User_TypeExists_InDomainAssembly()
    {
        var type = TryGetUserType();
        Assert.NotNull(type);
    }

    [Fact]
    public void User_IsAConcreteClass()
    {
        var type = TryGetUserType();
        Assert.NotNull(type);
        Assert.True(type!.IsClass, "User must be a class.");
        Assert.False(type.IsAbstract, "User should not be abstract.");
    }

    [Fact]
    public void User_HasAtLeastOnePublicProperty()
    {
        var type = TryGetUserType();
        Assert.NotNull(type);
        var publicProps = type!.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        Assert.True(publicProps.Length >= 1, "Expected User to expose at least one public instance property.");
    }
}