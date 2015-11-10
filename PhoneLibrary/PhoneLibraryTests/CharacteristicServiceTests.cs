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
            service.Add(new Characteristic { Specification = new Specification(), Value = "first" });
            service.Add(new Characteristic { Specification = new Specification(), Value = "second" });
        }

        [TestMethod]
        public void AddTest()
        {
            string value = Guid.NewGuid().ToString();
            Characteristic newCharacteristic = new Characteristic { Value = value };
            Characteristic addedCharacteristic = service.Add(newCharacteristic);
            Assert.IsNotNull(addedCharacteristic);
            Assert.IsTrue(addedCharacteristic.Id > 0);
            Assert.AreEqual(addedCharacteristic.Value, value);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            Characteristic characteristic = service.Get(1);
            Assert.IsNotNull(characteristic);
            Assert.AreEqual(characteristic.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            Characteristic characteristic = service.Get(1);
            string value = characteristic.Value;
            characteristic.Value = Guid.NewGuid().ToString();
            Characteristic newCharacteristic = service.Get(1);
            Assert.AreEqual(newCharacteristic.Value, value);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            Characteristic characteristic = service.Get(int.MaxValue);
            Assert.IsNull(characteristic);
        }

        [TestMethod]
        public void GetAllTest()
        {
            List<Characteristic> characteristics = service.Get();
            Assert.IsNotNull(characteristics);
            Assert.IsTrue(characteristics.Count > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            Characteristic characteristic = service.Get().First();
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
            service.Update(new Characteristic { Id = int.MaxValue });
        }

        [TestMethod]
        public void DeleteTest()
        {
            Characteristic characteristic = service.Get().Last();
            service.Delete(characteristic.Id);
            Characteristic deletedCharacteristic = service.Get(characteristic.Id);
            Assert.IsNull(deletedCharacteristic);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DeleteNotFoundTest()
        {
            service.Delete(int.MaxValue);
        }
    }
}