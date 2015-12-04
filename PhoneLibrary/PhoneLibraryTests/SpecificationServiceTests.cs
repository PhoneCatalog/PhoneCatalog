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
            service.Add(new Specification { Category = new Category(), Name = "first" });
            service.Add(new Specification { Category = new Category(), Name = "Second" });
        }

        [TestMethod]
        public void AddTest()
        {
            string name = Guid.NewGuid().ToString();
            Specification newSpecification = new Specification { Name = name };
            Specification addedSpecification = service.Add(newSpecification);
            Assert.IsNotNull(addedSpecification);
            Assert.IsTrue(addedSpecification.Id > 0);
            Assert.AreEqual(addedSpecification.Name, name);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            Specification specification = service.Get(1);
            Assert.IsNotNull(specification);
            Assert.AreEqual(specification.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            Specification specification = service.Get(1);
            string name = specification.Name;
            specification.Name = Guid.NewGuid().ToString();
            Specification newSpecification = service.Get(1);
            Assert.AreEqual(newSpecification.Name, name);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            Specification specification = service.Get(int.MaxValue);
            Assert.IsNull(specification);
        }

        [TestMethod]
        public void GetAllTest()
        {
            List<Specification> specifications = service.Get();
            Assert.IsNotNull(specifications);
            Assert.IsTrue(specifications.Count > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            Specification specification = service.Get().First();
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
            service.Update(new Specification { Id = int.MaxValue });
        }

        [TestMethod]
        public void DeleteTest()
        {
            Specification specification = service.Get().Last();
            service.Delete(specification.Id);
            Specification deletedSpecification = service.Get(specification.Id);
            Assert.IsNull(deletedSpecification);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DeleteNotFoundTest()
        {
            service.Delete(int.MaxValue);
        }
    }
}