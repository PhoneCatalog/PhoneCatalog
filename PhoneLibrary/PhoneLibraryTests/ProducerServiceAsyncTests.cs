using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneLibrary.service;
using PhoneLibrary.model;
using System.Collections.Generic;
using System.Linq;

namespace PhoneLibraryTests
{
    [TestClass]
    public class ProducerServiceAsyncTests
    {
        private readonly ProducerService service;

        public ProducerServiceAsyncTests()
        {
            this.service = new ProducerService();
            service.AddAsync(new Producer { Name = "Lenovo", Country = "China", Site = "lenovo.com" }).Wait();
            service.AddAsync(new Producer { Name = "Asus", Country = "China", Site = "asus.com" }).Wait();
        }

        [TestMethod]
        public void AddAsyncTest()
        {
            string name = Guid.NewGuid().ToString();
            string country = Guid.NewGuid().ToString();
            string site = Guid.NewGuid().ToString();
            Producer newProducer = new Producer { Country = country, Name = name, Site = site };
            Producer addedProducer = service.AddAsync(newProducer).Result;
            Assert.IsNotNull(addedProducer);
            Assert.IsTrue(addedProducer.Id > 0);
            Assert.AreEqual(addedProducer.Name, name);
            Assert.AreEqual(addedProducer.Country, country);
            Assert.AreEqual(addedProducer.Site, site);
        }

        [TestMethod]
        public void GetAsyncByIdTest()
        {
            Producer producer = service.GetAsync(1).Result;
            Assert.IsNotNull(producer);
            Assert.AreEqual(producer.Id, 1);
        }

        [TestMethod]
        public void GetAsyncByIdEditTest()
        {
            Producer producer = service.GetAsync(1).Result;
            string name = producer.Name;
            string country = producer.Country;
            string site = producer.Site;
            producer.Name = Guid.NewGuid().ToString();
            producer.Country = Guid.NewGuid().ToString();
            producer.Site = Guid.NewGuid().ToString();
            Producer newProducer = service.GetAsync(1).Result;
            Assert.AreEqual(newProducer.Name, name);
            Assert.AreEqual(newProducer.Country, country);
            Assert.AreEqual(newProducer.Site, site);
        }

        [TestMethod]
        public void GetAsyncByIdNotFoundTest()
        {
            Producer producer = service.GetAsync(int.MaxValue).Result;
            Assert.IsNull(producer);
        }

        [TestMethod]
        public void GetAsyncAllTest()
        {
            List<Producer> producers = service.GetAsync().Result;
            Assert.IsNotNull(producers);
            Assert.IsTrue(producers.Count > 0);
        }

        [TestMethod]
        public void UpdateAsyncTest()
        {
            Producer producer = service.GetAsync().Result.First();
            producer.Name += "upd";
            producer.Country += "upd";
            producer.Site += "upd";
            service.UpdateAsync(producer).Wait();
            Producer updatedProducer = service.GetAsync(producer.Id).Result;
            Assert.IsNotNull(updatedProducer);
            Assert.AreEqual(updatedProducer.Name, producer.Name);
            Assert.AreEqual(updatedProducer.Country, producer.Country);
            Assert.AreEqual(updatedProducer.Site, producer.Site);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void UpdateAsyncNotFoundTest()
        {
            service.UpdateAsync(new Producer { Id = int.MaxValue, Country = "new", Name = "new", Site = "new" }).Wait();
        }

        [TestMethod]
        public void DeleteAsyncTest()
        {
            Producer producer = service.GetAsync().Result.Last();
            service.DeleteAsync(producer.Id).Wait();
            Producer deletedProducer = service.GetAsync(producer.Id).Result;
            Assert.IsNull(deletedProducer);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void DeleteAsyncNotFoundTest()
        {
            service.DeleteAsync(int.MaxValue).Wait();
        }
    }
}