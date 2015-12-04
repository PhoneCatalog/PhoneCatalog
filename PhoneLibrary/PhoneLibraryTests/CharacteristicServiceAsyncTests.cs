using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneLibrary.service;
using PhoneLibrary.model;
using System.Collections.Generic;
using System.Linq;

namespace PhoneLibraryTests
{
    [TestClass]
    public class CharacteristicServiceAsyncTests
    {
        private readonly CharacteristicService service;

        public CharacteristicServiceAsyncTests()
        {
            this.service = new CharacteristicService();
            service.AddAsync(new Characteristic { Specification = new Specification(), Value = "first" }).Wait();
            service.AddAsync(new Characteristic { Specification = new Specification(), Value = "second" }).Wait();
        }

        [TestMethod]
        public void AddAsyncTest()
        {
            string value = Guid.NewGuid().ToString();
            Characteristic newCharacteristic = new Characteristic { Value = value };
            Characteristic addedCharacteristic = service.AddAsync(newCharacteristic).Result;
            Assert.IsNotNull(addedCharacteristic);
            Assert.IsTrue(addedCharacteristic.Id > 0);
            Assert.AreEqual(addedCharacteristic.Value, value);
        }

        [TestMethod]
        public void GetAsyncByIdTest()
        {
            Characteristic characteristic = service.GetAsync(1).Result;
            Assert.IsNotNull(characteristic);
            Assert.AreEqual(characteristic.Id, 1);
        }

        [TestMethod]
        public void GetAsyncByIdEditTest()
        {
            Characteristic characteristic = service.GetAsync(1).Result;
            string value = characteristic.Value;
            characteristic.Value = Guid.NewGuid().ToString();
            Characteristic newCharacteristic = service.GetAsync(1).Result;
            Assert.AreEqual(newCharacteristic.Value, value);
        }

        [TestMethod]
        public void GetAsyncByIdNotFoundTest()
        {
            Characteristic characteristic = service.GetAsync(int.MaxValue).Result;
            Assert.IsNull(characteristic);
        }

        [TestMethod]
        public void GetAsyncAllTest()
        {
            List<Characteristic> characteristics = service.GetAsync().Result;
            Assert.IsNotNull(characteristics);
            Assert.IsTrue(characteristics.Count > 0);
        }

        [TestMethod]
        public void UpdateAsyncTest()
        {
            Characteristic characteristic = service.GetAsync().Result.First();
            characteristic.Value += "upd";
            service.UpdateAsync(characteristic).Wait();
            Characteristic updatedCharacteristic = service.GetAsync(characteristic.Id).Result;
            Assert.IsNotNull(updatedCharacteristic);
            Assert.AreEqual(updatedCharacteristic.Value, characteristic.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void UpdateAsyncNotFoundTest()
        {
            service.UpdateAsync(new Characteristic { Id = int.MaxValue }).Wait();
        }

        [TestMethod]
        public void DeleteAsyncTest()
        {
            Characteristic characteristic = service.GetAsync().Result.Last();
            service.DeleteAsync(characteristic.Id).Wait();
            Characteristic deletedCharacteristic = service.GetAsync(characteristic.Id).Result;
            Assert.IsNull(deletedCharacteristic);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void DeleteAsyncNotFoundTest()
        {
            service.DeleteAsync(int.MaxValue).Wait();
        }
    }
}