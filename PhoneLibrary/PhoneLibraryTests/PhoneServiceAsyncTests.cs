using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneLibrary.service;
using PhoneLibrary.model;
using System.Collections.Generic;
using System.Linq;

namespace PhoneLibraryTests
{
    [TestClass]
    public class PhoneServiceAsyncTests
    {
        private readonly PhoneService service;

        public PhoneServiceAsyncTests()
        {
            this.service = new PhoneService();
            service.AddAsync(new Phone { Description = "firstPhone", Model = "1", Type = "monoblock" }).Wait();
            service.AddAsync(new Phone { Description = "secondPhone", Model = "2", Type = "slider" }).Wait();
        }

        [TestMethod]
        public void AddAsyncTest()
        {
            string description = Guid.NewGuid().ToString();
            string model = Guid.NewGuid().ToString();
            string type = Guid.NewGuid().ToString();
            Phone newPhone = new Phone { Description = description, Type = type, Model = model };
            Phone addedPhone = service.AddAsync(newPhone).Result;
            Assert.IsNotNull(addedPhone);
            Assert.IsTrue(addedPhone.Id > 0);
            Assert.AreEqual(addedPhone.Description, description);
            Assert.AreEqual(addedPhone.Model, model);
            Assert.AreEqual(addedPhone.Type, type);
        }

        [TestMethod]
        public void GetAsyncByIdTest()
        {
            Phone phone = service.GetAsync(1).Result;
            Assert.IsNotNull(phone);
            Assert.AreEqual(phone.Id, 1);
        }

        [TestMethod]
        public void GetAsyncByIdEditTest()
        {
            Phone phone = service.GetAsync(1).Result;
            string description = phone.Description;
            string model = phone.Model;
            string type = phone.Type;
            phone.Description = Guid.NewGuid().ToString();
            phone.Model = Guid.NewGuid().ToString();
            phone.Type = Guid.NewGuid().ToString();
            Phone newPhone = service.GetAsync(1).Result;
            Assert.AreEqual(newPhone.Description, description);
            Assert.AreEqual(newPhone.Model, model);
        }

        [TestMethod]
        public void GetAsyncByIdNotFoundTest()
        {
            Phone phone = service.GetAsync(int.MaxValue).Result;
            Assert.IsNull(phone);
        }

        [TestMethod]
        public void GetAsyncAllTest()
        {
            List<Phone> phones = service.GetAsync().Result;
            Assert.IsNotNull(phones);
            Assert.IsTrue(phones.Count > 0);
        }

        [TestMethod]
        public void UpdateAsyncTest()
        {
            Phone phone = service.GetAsync().Result.First();
            phone.Description += "upd";
            phone.Model += "upd";
            phone.Type += "upd";
            service.UpdateAsync(phone).Wait();
            Phone updatedPhone = service.GetAsync(phone.Id).Result;
            Assert.IsNotNull(updatedPhone);
            Assert.AreEqual(updatedPhone.Description, phone.Description);
            Assert.AreEqual(updatedPhone.Model, phone.Model);
            Assert.AreEqual(updatedPhone.Type, phone.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void UpdateAsyncNotFoundTest()
        {
            service.UpdateAsync(new Phone { Id = int.MaxValue }).Wait();
        }

        [TestMethod]
        public void DeleteAsyncTest()
        {
            Phone phone = service.GetAsync().Result.Last();
            service.DeleteAsync(phone.Id).Wait();
            Phone deletedPhone = service.GetAsync(phone.Id).Result;
            Assert.IsNull(deletedPhone);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void DeleteAsyncNotFoundTest()
        {
            service.DeleteAsync(int.MaxValue).Wait();
        }
    }
}