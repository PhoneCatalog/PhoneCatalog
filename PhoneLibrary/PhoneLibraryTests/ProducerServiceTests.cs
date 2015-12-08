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
    public class ProducerServiceTests
    {
        private readonly ProducerService service;

        public ProducerServiceTests()
        {
            this.service = new ProducerService();
            service.OnAdded += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.Add(new Producer { Name = "Lenovo", Country = "China", Site = "lenovo.com" });
            service.Add(new Producer { Name = "Asus", Country = "China", Site = "asus.com" });
        }

        [TestMethod]
        public void AddTest()
        {
            string name = Guid.NewGuid().ToString();
            string country = Guid.NewGuid().ToString();
            string site = Guid.NewGuid().ToString();
            service.OnAdded += (args) =>
            {
                Assert.AreEqual(args.Entity.Name, name);
                Assert.AreEqual(args.Entity.Country, country);
                Assert.AreEqual(args.Entity.Site, site);
            };
            Producer newProducer = new Producer { Country = country, Name = name, Site = site };
            Producer addedProducer = service.Add(newProducer);
            Assert.IsNotNull(addedProducer);
            Assert.IsTrue(addedProducer.Id > 0);
            Assert.AreEqual(addedProducer.Name, name);
            Assert.AreEqual(addedProducer.Country, country);
            Assert.AreEqual(addedProducer.Site, site);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Producer producer = service.Get(1);
            Assert.IsNotNull(producer);
            Assert.AreEqual(producer.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Producer producer = service.Get(1);
            string name = producer.Name;
            string country = producer.Country;
            string site = producer.Site;
            producer.Name = Guid.NewGuid().ToString();
            producer.Country = Guid.NewGuid().ToString();
            producer.Site = Guid.NewGuid().ToString();
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Name, name);
            };
            Producer newProducer = service.Get(1);
            Assert.AreEqual(newProducer.Name, name);
            Assert.AreEqual(newProducer.Country, country);
            Assert.AreEqual(newProducer.Site, site);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            service.OnGot += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            Producer producer = service.Get(int.MaxValue);
            Assert.IsNull(producer);
        }

        [TestMethod]
        public void GetAllTest()
        {
            service.OnAllGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
                Assert.IsTrue(args.Entity.Count > 0);
            };
            List<Producer> producers = service.Get();
            Assert.IsNotNull(producers);
            Assert.IsTrue(producers.Count > 0);
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
            Producer producer = service.Get().First();
            string oldName = producer.Name;
            service.OnUpdated += (args) =>
            {
                Assert.AreEqual(args.Entity.Name, oldName);
            };
            producer.Name += "upd";
            producer.Country += "upd";
            producer.Site += "upd";
            service.Update(producer);
            Producer updatedProducer = service.Get(producer.Id);
            Assert.IsNotNull(updatedProducer);
            Assert.AreEqual(updatedProducer.Name, producer.Name);
            Assert.AreEqual(updatedProducer.Country, producer.Country);
            Assert.AreEqual(updatedProducer.Site, producer.Site);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNotFoundTest()
        {
            service.OnUpdated += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            service.Update(new Producer { Id = int.MaxValue, Country = "new", Name = "new", Site = "new" });
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
            Producer producer = service.Get().Last();
            service.Delete(producer.Id);
            Producer deletedProducer = service.Get(producer.Id);
            Assert.IsNull(deletedProducer);
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