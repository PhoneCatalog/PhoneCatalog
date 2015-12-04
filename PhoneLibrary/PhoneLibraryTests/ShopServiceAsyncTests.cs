using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneLibrary.service;
using PhoneLibrary.model;
using System.Collections.Generic;
using System.Linq;

namespace PhoneLibraryTests
{
    [TestClass]
    public class ShopServiceAsyncTests
    {
        private readonly ShopService service;

        public ShopServiceAsyncTests()
        {
            this.service = new ShopService();
            service.AddAsync(new Shop { Name = "shop_1", Address = "address1", PhoneNumber = "111-00-11", Website = "shop_1.com", Price = 10 }).Wait();
            service.AddAsync(new Shop { Name = "shop_2", Address = "address2", PhoneNumber = "222-00-22", Website = "shop_2.com", Price = 20 }).Wait();
        }

        [TestMethod]
        public void AddAsyncTest()
        {
            string name = Guid.NewGuid().ToString();
            string address = Guid.NewGuid().ToString();
            string phoneNumber = Guid.NewGuid().ToString();
            string website = Guid.NewGuid().ToString();
            Shop newShop = new Shop { Name = name, Address = address, PhoneNumber = phoneNumber, Website = website };
            Shop addedShop = service.AddAsync(newShop).Result;
            Assert.IsNotNull(addedShop);
            Assert.IsTrue(addedShop.Id > 0);
            Assert.AreEqual(addedShop.Name, name);
            Assert.AreEqual(addedShop.Address, address);
            Assert.AreEqual(addedShop.PhoneNumber, phoneNumber);
            Assert.AreEqual(addedShop.Website, website);
        }

        [TestMethod]
        public void GetAsyncByIdTest()
        {
            Shop shop = service.GetAsync(1).Result;
            Assert.IsNotNull(shop);
            Assert.AreEqual(shop.Id, 1);
        }

        [TestMethod]
        public void GetAsyncByIdEditTest()
        {
            Shop shop = service.GetAsync(1).Result;
            string name = shop.Name;
            string address = shop.Address;
            string phoneNumber = shop.PhoneNumber;
            string website = shop.Website;
            shop.Name = Guid.NewGuid().ToString();
            shop.Address = Guid.NewGuid().ToString();
            shop.PhoneNumber = Guid.NewGuid().ToString();
            shop.Website = Guid.NewGuid().ToString();
            Shop newShop = service.GetAsync(1).Result;
            Assert.AreEqual(newShop.Name, name);
            Assert.AreEqual(newShop.Address, address);
            Assert.AreEqual(newShop.PhoneNumber, phoneNumber);
            Assert.AreEqual(newShop.Website, website);
        }

        [TestMethod]
        public void GetAsyncByIdNotFoundTest()
        {
            Shop shop = service.GetAsync(int.MaxValue).Result;
            Assert.IsNull(shop);
        }

        [TestMethod]
        public void GetAsyncAllTest()
        {
            List<Shop> shops = service.GetAsync().Result;
            Assert.IsNotNull(shops);
            Assert.IsTrue(shops.Count > 0);
        }

        [TestMethod]
        public void UpdateAsyncTest()
        {
            Shop shop = service.GetAsync().Result.First();
            shop.Name += "upd";
            shop.Address += "upd";
            shop.PhoneNumber += "upd";
            shop.Website += "upd";
            service.UpdateAsync(shop).Wait();
            Shop updatedShop = service.GetAsync(shop.Id).Result;
            Assert.IsNotNull(updatedShop);
            Assert.AreEqual(updatedShop.Name, shop.Name);
            Assert.AreEqual(updatedShop.Address, shop.Address);
            Assert.AreEqual(updatedShop.PhoneNumber, shop.PhoneNumber);
            Assert.AreEqual(updatedShop.Website, shop.Website);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void UpdateAsyncNotFoundTest()
        {
            service.UpdateAsync(new Shop { Id = int.MaxValue }).Wait();
        }

        [TestMethod]
        public void DeleteAsyncTest()
        {
            Shop shop = service.GetAsync().Result.Last();
            service.DeleteAsync(shop.Id).Wait();
            Shop deletedShop = service.GetAsync(shop.Id).Result;
            Assert.IsNull(deletedShop);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void DeleteAsyncNotFoundTest()
        {
            service.DeleteAsync(int.MaxValue).Wait();
        }
    }
}