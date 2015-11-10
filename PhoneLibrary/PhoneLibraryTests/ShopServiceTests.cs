﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneLibrary.service;
using PhoneLibrary.model;

namespace PhoneLibraryTests
{
    [TestClass]
    public class ShopServiceTests
    {
        private readonly ShopService service;

        public ShopServiceTests()
        {
            this.service = new ShopService();
            service.Add(new Shop { Name = "shop_1", Address = "address1", PhoneNumber = "111-00-11", Website = "shop_1.com", Price = 10 });
            service.Add(new Shop { Name = "shop_2", Address = "address2", PhoneNumber = "222-00-22", Website = "shop_2.com", Price = 20 });
        }

        [TestMethod]
        public void AddTest()
        {
            string name = Guid.NewGuid().ToString();
            string address = Guid.NewGuid().ToString();
            string phoneNumber = Guid.NewGuid().ToString();
            string website = Guid.NewGuid().ToString();
            Shop newShop = new Shop { Name = name, Address = address, PhoneNumber = phoneNumber, Website = website };
            Shop addedShop = service.Add(newShop);
            Assert.IsNotNull(addedShop);
            Assert.IsTrue(addedShop.Id > 0);
            Assert.AreEqual(addedShop.Name, name);
            Assert.AreEqual(addedShop.Address, address);
            Assert.AreEqual(addedShop.PhoneNumber, phoneNumber);
            Assert.AreEqual(addedShop.Website, website);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            Shop shop = service.Get(1);
            Assert.IsNotNull(shop);
            Assert.AreEqual(shop.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            Shop shop = service.Get(1);
            string name = shop.Name;
            string address = shop.Address;
            string phoneNumber = shop.PhoneNumber;
            string website = shop.Website;
            shop.Name = Guid.NewGuid().ToString();
            shop.Address = Guid.NewGuid().ToString();
            shop.PhoneNumber = Guid.NewGuid().ToString();
            shop.Website = Guid.NewGuid().ToString();
            Shop newShop = service.Get(1);
            Assert.AreEqual(newShop.Name, name);
            Assert.AreEqual(newShop.Address, address);
            Assert.AreEqual(newShop.PhoneNumber, phoneNumber);
            Assert.AreEqual(newShop.Website, website);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            Shop shop = service.Get(int.MaxValue);
            Assert.IsNull(shop);
        }

        [TestMethod]
        public void GetAllTest()
        {
            List<Shop> shops = service.Get();
            Assert.IsNotNull(shops);
            Assert.IsTrue(shops.Count > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            Shop shop = service.Get().First();
            shop.Name += "upd";
            shop.Address += "upd";
            shop.PhoneNumber += "upd";
            shop.Website += "upd";
            service.Update(shop);
            Shop updatedShop = service.Get(shop.Id);
            Assert.IsNotNull(updatedShop);
            Assert.AreEqual(updatedShop.Name, shop.Name);
            Assert.AreEqual(updatedShop.Address, shop.Address);
            Assert.AreEqual(updatedShop.PhoneNumber, shop.PhoneNumber);
            Assert.AreEqual(updatedShop.Website, shop.Website);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNotFoundTest()
        {
            service.Update(new Shop { Id = int.MaxValue });
        }

        [TestMethod]
        public void DeleteTest()
        {
            Shop shop = service.Get().Last();
            service.Delete(shop.Id);
            Shop deletedShop = service.Get(shop.Id);
            Assert.IsNull(deletedShop);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DeleteNotFoundTest()
        {
            service.Delete(int.MaxValue);
        }
    }
}