using System;
using Xunit;
using Project1.Domain.Model;

namespace Project0.Tests
{
    public class CustomerTest
    {
        private readonly Customer _customer = new Customer();
        [Fact]
        public void Customer_FirstName_Entry()
        {
            string fName = "FirstName";
            _customer.FirstName = fName;
            Assert.Equal(fName, _customer.FirstName);
        }
        [Fact]
        public void Customer_LastName_Entry()
        {
            string fName = "LastName";
            _customer.LastName = fName;
            Assert.Equal(fName, _customer.LastName);
        }

        [Fact]
        public void Orders_DefaultValue_Empty()
        {
            Assert.NotNull(_customer.Orders);
            Assert.Empty(_customer.Orders);
        }

        [Fact]
        public void EmptyCustomer_NameNull()
        {
            Assert.Null(_customer.FirstName);
        }

        [Theory]
        [InlineData(1, 5, 10)]
        [InlineData(10, 15, 20)]
        public void InsertOrder_CustomerContainsOrder(params int[] productParams)
        {
            var order = new Orders { ProductId = productParams[1], CustomerId = productParams[0], Quantity = productParams[2], StoreLocationId = productParams[0] };
            _customer.Orders.Add(order);
            Assert.True(_customer.Orders.Contains(order));
        }
    }
}
