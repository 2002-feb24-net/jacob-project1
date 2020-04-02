using System;
using Xunit;
using Project1.Domain.Model;

namespace Project1.Tests
{
    public class ProductTest
    {
        private readonly Product _product = new Product();

        [Fact]
        public void Product_DefaultValue_NotNull()
        {
            Assert.NotNull(_product);
        }

        [Fact]
        public void ProductEntry()
        {
            var order = _product;
            Assert.Equal(_product, order);
        }
    }
}
