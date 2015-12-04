using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneLibrary.service;
using PhoneLibrary.model;
using System.Collections.Generic;
using System.Linq;

namespace PhoneLibraryTests
{
    [TestClass]
    public class SpecificationServiceAsyncTests
    {
        private readonly SpecificationService service;

        public SpecificationServiceAsyncTests()
        {
            this.service = new SpecificationService();
            service.AddAsync(new Specification { Category = new Category(), Name = "first" }).Wait();
            service.AddAsync(new Specification { Category = new Category(), Name = "Second" }).Wait();
        }

        [TestMethod]
        public void AddAsyncTest()
        {
            string name = Guid.NewGuid().ToString();
            Specification newSpecification = new Specification { Name = name };
            Specification addedSpecification = service.AddAsync(newSpecification).Result;
            Assert.IsNotNull(addedSpecification);
            Assert.IsTrue(addedSpecification.Id > 0);
            Assert.AreEqual(addedSpecification.Name, name);
        }

        [TestMethod]
        public void GetAsyncByIdTest()
        {
            Specification specification = service.GetAsync(1).Result;
            Assert.IsNotNull(specification);
            Assert.AreEqual(specification.Id, 1);
        }

        [TestMethod]
        public void GetAsyncByIdEditTest()
        {
            Specification specification = service.GetAsync(1).Result;
            string name = specification.Name;
            specification.Name = Guid.NewGuid().ToString();
            Specification newSpecification = service.GetAsync(1).Result;
            Assert.AreEqual(newSpecification.Name, name);
        }

        [TestMethod]
        public void GetAsyncByIdNotFoundTest()
        {
            Specification specification = service.GetAsync(int.MaxValue).Result;
            Assert.IsNull(specification);
        }

        [TestMethod]
        public void GetAsyncAllTest()
        {
            List<Specification> specifications = service.GetAsync().Result;
            Assert.IsNotNull(specifications);
            Assert.IsTrue(specifications.Count > 0);
        }

        [TestMethod]
        public void UpdateAsyncTest()
        {
            Specification specification = service.GetAsync().Result.First();
            specification.Name += "upd";
            service.UpdateAsync(specification).Wait();
            Specification updatedSpecification = service.GetAsync(specification.Id).Result;
            Assert.IsNotNull(updatedSpecification);
            Assert.AreEqual(updatedSpecification.Name, specification.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void UpdateAsyncNotFoundTest()
        {
            service.UpdateAsync(new Specification { Id = int.MaxValue }).Wait();
        }

        [TestMethod]
        public void DeleteAsyncTest()
        {
            Specification specification = service.GetAsync().Result.Last();
            service.DeleteAsync(specification.Id).Wait();
            Specification deletedSpecification = service.GetAsync(specification.Id).Result;
            Assert.IsNull(deletedSpecification);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void DeleteAsyncNotFoundTest()
        {
            service.DeleteAsync(int.MaxValue).Wait();
        }
    }
}