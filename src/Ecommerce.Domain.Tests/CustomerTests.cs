using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Xunit;

namespace Ecommerce.Domain.Tests;

public class CustomerTests
{
    [Fact]
    public void Customer_Should_Have_A_Valid_Id()
    {
        var name = "João da Silva";
        var customer = new Customer( name);
        Assert.IsType<Guid>(customer.CustomerId);
    }

    [Fact]
    public void Customer_Should_Have_A_Valid_Name()
    {
        var name = "João da Silva";
        var customer = new Customer(name);
        Assert.Equal(name, customer.Name);
    }

    [Fact]
    public void Customer_Name_Should_Not_Be_Null()
    {
        string name = null;
        Assert.Throws<DomainException>(() => new Customer(name));
    }
    
    [Fact]
    public void Customer_Name_Should_Not_Be_White_Space()
    {
        string name = "   ";
        Assert.Throws<DomainException>(() => new Customer(name));
    }

    [Fact]
    public void Customer_Name_Should_have_Minimum_Length()
    {
        string name = "Jo";
        Assert.Throws<DomainException>(() => new Customer(name));
    }

    [Fact]
    public void Customer_Name_Should_have_Maximum_Length()
    {
        string name = new string('A', 101);
        Assert.Throws<DomainException>(() => new Customer(name));
    }

    [Fact]
    public void Customer_Update_Name()
    {
        var name = "João da Silva";
        var customer = new Customer(name);
        var newName = "Maria da Silva";
        customer.UpdateName(newName);
        Assert.Equal(newName, customer.Name);
    }
}
