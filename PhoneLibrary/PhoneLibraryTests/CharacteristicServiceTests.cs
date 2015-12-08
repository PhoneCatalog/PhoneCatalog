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
    public class CharacteristicServiceTests
    {
        private readonly CharacteristicService service;

        public CharacteristicServiceTests()
        {
            this.service = new CharacteristicService();
            service.OnAdded += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.Add(new Characteristic { Specification = new Specification(), Value = "first" });
            service.Add(new Characteristic { Specification = new Specification(), Value = "second" });
        }

        [TestMethod]
        public void AddTest()
        {
            string value = Guid.NewGuid().ToString();
            service.OnAdded += (args) =>
            {
                Assert.AreEqual(args.Entity.Value, value);
            };
            Characteristic newCharacteristic = new Characteristic { Value = value };
            Characteristic addedCharacteristic = service.Add(newCharacteristic);
            Assert.IsNotNull(addedCharacteristic);
            Assert.IsTrue(addedCharacteristic.Id > 0);
            Assert.AreEqual(addedCharacteristic.Value, value);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Characteristic characteristic = service.Get(1);
            Assert.IsNotNull(characteristic);
            Assert.AreEqual(characteristic.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Characteristic characteristic = service.Get(1);
            string value = characteristic.Value;
            characteristic.Value = Guid.NewGuid().ToString();
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Value, value);
            };
            Characteristic newCharacteristic = service.Get(1);
            Assert.AreEqual(newCharacteristic.Value, value);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            service.OnGot += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            Characteristic characteristic = service.Get(int.MaxValue);
            Assert.IsNull(characteristic);
        }

        [TestMethod]
        public void GetAllTest()
        {
            service.OnAllGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
                Assert.IsTrue(args.Entity.Count > 0);
            };
            List<Characteristic> characteristics = service.Get();
            Assert.IsNotNull(characteristics);
            Assert.IsTrue(characteristics.Count > 0);
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
            Characteristic characteristic = service.Get().First();
            string oldValue = characteristic.Value;
            service.OnUpdated += (args) =>
            {
                Assert.AreEqual(args.Entity.Value, oldValue);
            };
            characteristic.Value += "upd";
            service.Update(characteristic);
            Characteristic updatedCharacteristic = service.Get(characteristic.Id);
            Assert.IsNotNull(updatedCharacteristic);
            Assert.AreEqual(updatedCharacteristic.Value, characteristic.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNotFoundTest()
        {
            service.OnUpdated += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            service.Update(new Characteristic { Id = int.MaxValue });
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
            Characteristic characteristic = service.Get().Last();
            service.Delete(characteristic.Id);
            Characteristic deletedCharacteristic = service.Get(characteristic.Id);
            Assert.IsNull(deletedCharacteristic);
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