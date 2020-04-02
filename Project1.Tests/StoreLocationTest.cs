using System;
using Xunit;
using Project1.Domain.Model;

namespace Project1.Tests
{
    public class StoreLocationTest
    {
        private readonly StoreLocation _location = new StoreLocation();
        [Fact]
        public void Location_Name_Stores()
        {
            string locationNames = "LocationName";
            _location.LocationName = locationNames;
            Assert.Equal(locationNames, _location.LocationName);
        }


        [Fact]
        public void Products_DefaultValue_Empty()
        {
            Assert.NotNull(_location.Product);
            Assert.Empty(_location.Product);
        }

        [Fact]
        public void EmptyLocation_NameNull()
        {
            Assert.Null(_location.LocationName);
        }

        [Theory]
        [InlineData(1, 5, 10)]
        [InlineData(10, 15, 20)]
        public void InsertProduct_LocationContainsProduct(params int[] productParams)
        {
            var product = new Product { Name = "Name", Price = productParams[1], Stock = productParams[2], StoreLocationId = productParams[0] };
            _location.Product.Add(product);
            Assert.True(_location.Product.Contains(product));
        }
    }
}
