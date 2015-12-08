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
    public class SpecificationServiceTests
    {
        private readonly SpecificationService service;

        public SpecificationServiceTests()
        {
            this.service = new SpecificationService();
            service.OnAdded += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.Add(new Specification { Category = new Category(), Name = "first" });
            service.Add(new Specification { Category = new Category(), Name = "Second" });
        }

        [TestMethod]
        public void AddTest()
        {
            string name = Guid.NewGuid().ToString();
            service.OnAdded += (args) =>
            {
                Assert.AreEqual(args.Entity.Name, name);
            };
            Specification newSpecification = new Specification { Name = name };
            Specification addedSpecification = service.Add(newSpecification);
            Assert.IsNotNull(addedSpecification);
            Assert.IsTrue(addedSpecification.Id > 0);
            Assert.AreEqual(addedSpecification.Name, name);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Specification specification = service.Get(1);
            Assert.IsNotNull(specification);
            Assert.AreEqual(specification.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Specification specification = service.Get(1);
            string name = specification.Name;
            specification.Name = Guid.NewGuid().ToString();
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Name, name);
            };
            Specification newSpecification = service.Get(1);
            Assert.AreEqual(newSpecification.Name, name);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            service.OnGot += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            Specification specification = service.Get(int.MaxValue);
            Assert.IsNull(specification);
        }

        [TestMethod]
        public void GetAllTest()
        {
            service.OnAllGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
                Assert.IsTrue(args.Entity.Count > 0);
            };
            List<Specification> specifications = service.Get();
            Assert.IsNotNull(specifications);
            Assert.IsTrue(specifications.Count > 0);
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
            Specification specification = service.Get().First();
            string oldName = specification.Name;
            service.OnUpdated += (args) =>
            {
                Assert.AreEqual(args.Entity.Name, oldName);
            };
            specification.Name += "upd";
            service.Update(specification);
            Specification updatedSpecification = service.Get(specification.Id);
            Assert.IsNotNull(updatedSpecification);
            Assert.AreEqual(updatedSpecification.Name, specification.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNotFoundTest()
        {
            service.OnUpdated += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            service.Update(new Specification { Id = int.MaxValue });
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
            Specification specification = service.Get().Last();
            service.Delete(specification.Id);
            Specification deletedSpecification = service.Get(specification.Id);
            Assert.IsNull(deletedSpecification);
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