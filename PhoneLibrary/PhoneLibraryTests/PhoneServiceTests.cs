using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneLibrary.service;
using PhoneLibrary.model;

namespace PhoneLibraryTests
{
    [TestClass]
    public class PhoneServiceTests
    {
        private readonly PhoneService service;

        public PhoneServiceTests()
        {
            this.service = new PhoneService();
            service.OnAdded += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.Add(new Phone { Description = "firstPhone", Model = "1", Type = "monoblock" });
            service.Add(new Phone { Description = "secondPhone", Model = "2", Type = "slider" });
        }

        [TestMethod]
        public void AddTest()
        {
            string description = Guid.NewGuid().ToString();
            string model = Guid.NewGuid().ToString();
            string type = Guid.NewGuid().ToString();
            service.OnAdded += (args) =>
            {
                Assert.AreEqual(args.Entity.Description, description);
                Assert.AreEqual(args.Entity.Model, model);
                Assert.AreEqual(args.Entity.Type, type);
            };
            Phone newPhone = new Phone { Description = description, Type = type, Model = model };
            Phone addedPhone = service.Add(newPhone);
            Assert.IsNotNull(addedPhone);
            Assert.IsTrue(addedPhone.Id > 0);
            Assert.AreEqual(addedPhone.Description, description);
            Assert.AreEqual(addedPhone.Model, model);
            Assert.AreEqual(addedPhone.Type, type);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Phone phone = service.Get(1);
            Assert.IsNotNull(phone);
            Assert.AreEqual(phone.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Phone phone = service.Get(1);
            string description = phone.Description;
            string model = phone.Model;
            string type = phone.Type;
            phone.Description = Guid.NewGuid().ToString();
            phone.Model = Guid.NewGuid().ToString();
            phone.Type = Guid.NewGuid().ToString();
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Type, type);
            };
            Phone newPhone = service.Get(1);
            Assert.AreEqual(newPhone.Description, description);
            Assert.AreEqual(newPhone.Model, model);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            service.OnGot += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            Phone phone = service.Get(int.MaxValue);
            Assert.IsNull(phone);
        }

        [TestMethod]
        public void GetAllTest()
        {
            service.OnAllGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
                Assert.IsTrue(args.Entity.Count > 0);
            };
            List<Phone> phones = service.Get();
            Assert.IsNotNull(phones);
            Assert.IsTrue(phones.Count > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            service.OnAllGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.OnGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            Phone phone = service.Get().First();
            string oldType = phone.Type;
            service.OnUpdated += (args) =>
            {
                Assert.AreEqual(args.Entity.Type, oldType);
            };
            phone.Description += "upd";
            phone.Model += "upd";
            phone.Type += "upd";
            service.Update(phone);
            Phone updatedPhone = service.Get(phone.Id);
            Assert.IsNotNull(updatedPhone);
            Assert.AreEqual(updatedPhone.Description, phone.Description);
            Assert.AreEqual(updatedPhone.Model, phone.Model);
            Assert.AreEqual(updatedPhone.Type, phone.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNotFoundTest()
        {
            service.OnUpdated += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            service.Update(new Phone { Id = int.MaxValue });
        }

        [TestMethod]
        public void DeleteTest()
        {
            service.OnAllGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.OnGot += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            service.OnDeleted += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            Phone phone = service.Get().Last();
            service.Delete(phone.Id);
            Phone deletedPhone = service.Get(phone.Id);
            Assert.IsNull(deletedPhone);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DeleteNotFoundTest()
        {
            service.OnDeleted += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            service.Delete(int.MaxValue);
        }
    }
}