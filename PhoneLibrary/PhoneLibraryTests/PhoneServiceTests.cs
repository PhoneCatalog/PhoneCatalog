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
            service.Add(new Phone { Description = "firstPhone", Model = "1", Type = "monoblock" });
            service.Add(new Phone { Description = "secondPhone", Model = "2", Type = "slider" });
        }

        [TestMethod]
        public void AddTest()
        {
            string description = Guid.NewGuid().ToString();
            string model = Guid.NewGuid().ToString();
            string type = Guid.NewGuid().ToString();
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
            Phone phone = service.Get(1);
            Assert.IsNotNull(phone);
            Assert.AreEqual(phone.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            Phone phone = service.Get(1);
            string description = phone.Description;
            string model = phone.Model;
            string type = phone.Type;
            phone.Description = Guid.NewGuid().ToString();
            phone.Model = Guid.NewGuid().ToString();
            phone.Type = Guid.NewGuid().ToString();
            Phone newPhone = service.Get(1);
            Assert.AreEqual(newPhone.Description, description);
            Assert.AreEqual(newPhone.Model, model);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            Phone phone = service.Get(int.MaxValue);
            Assert.IsNull(phone);
        }

        [TestMethod]
        public void GetAllTest()
        {
            List<Phone> phones = service.Get();
            Assert.IsNotNull(phones);
            Assert.IsTrue(phones.Count > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            Phone phone = service.Get().First();
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
        public void UpdadeNotFoundTest()
        {
            service.Update(new Phone { Id = int.MaxValue });
        }

        [TestMethod]
        public void DeleteTest()
        {
            Phone phone = service.Get().Last();
            service.Delete(phone.Id);
            Phone deletedPhone = service.Get(phone.Id);
            Assert.IsNull(deletedPhone);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DeleteNotFoundTest()
        {
            service.Delete(int.MaxValue);
        }
    }
}