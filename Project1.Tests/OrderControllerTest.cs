using System;
using Xunit;
using Project1.WebUI.Controllers;
using System.Linq;
using Project1.Domain.Model;
using Project1.Domain.Interfaces;

namespace Project0.Tests
{
    public class OrderControllerTest
    {
        private static IProject1Repository Repo { get; }
        private readonly OrderController _orderController = new OrderController(Repo);
        [Fact]
        public void Repo_Available()
        {
            Assert.NotNull(Repo);
        }
        [Fact]
        public void Customer_Repo_Available()
        {
            Assert.NotNull(Repo.GetCustomers());
        }
        [Fact]
        public void Order_Repo_Available()
        {
            Assert.NotNull(Repo.GetOrders());
        }
        [Fact]
        public void Product_Repo_Available()
        {
            Assert.NotNull(Repo.GetProducts());
        }
        [Fact]
        public void Locations_Repo_Available()
        {
            Assert.NotNull(Repo.GetLocations());
        }
    }
}
