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
    public class CategoryServiceTests
    {
        private readonly CategoryService service;

        public CategoryServiceTests()
        {
            this.service = new CategoryService();
            service.OnAdded += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.Add(new Category { Name = "Processor" });
            service.Add(new Category { Name = "Battery" });
        }

        [TestMethod]
        public void AddTest()
        {
            string name = Guid.NewGuid().ToString();
            service.OnAdded += (args) =>
            {
                Assert.AreEqual(args.Entity.Name, name);
            };
            Category newCategory = new Category { Name = name };
            Category addedCategory = service.Add(newCategory);
            Assert.IsNotNull(addedCategory);
            Assert.IsTrue(addedCategory.Id > 0);
            Assert.AreEqual(addedCategory.Name, name);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Category category = service.Get(1);
            Assert.IsNotNull(category);
            Assert.AreEqual(category.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Category category = service.Get(1);
            string name = category.Name;
            category.Name = Guid.NewGuid().ToString();
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Name, name);
            };
            Category newCategory = service.Get(1);
            Assert.AreEqual(newCategory.Name, name);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            service.OnGot += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            Category category = service.Get(int.MaxValue);
            Assert.IsNull(category);
        }

        [TestMethod]
        public void GetAllTest()
        {
            service.OnAllGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
                Assert.IsTrue(args.Entity.Count > 0);
            };
            List<Category> categories = service.Get();
            Assert.IsNotNull(categories);
            Assert.IsTrue(categories.Count > 0);
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
            Category category = service.Get().First();
            string oldName = category.Name;
            service.OnUpdated += (args) =>
            {
                Assert.AreEqual(args.Entity.Name, oldName);
            };
            category.Name += "upd";
            service.Update(category);
            Category updatedCategory = service.Get(category.Id);
            Assert.IsNotNull(updatedCategory);
            Assert.AreEqual(updatedCategory.Name, category.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNotFoundTest()
        {
            service.OnUpdated += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            service.Update(new Category { Id = int.MaxValue });
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
            Category category = service.Get().Last();
            service.Delete(category.Id);
            Category deletedCategory = service.Get(category.Id);
            Assert.IsNull(deletedCategory);
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