using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneLibrary.service;
using PhoneLibrary.model;
using System.Collections.Generic;
using System.Linq;

namespace PhoneLibraryTests
{
    [TestClass]
    public class CategoryServiceAsyncTests
    {
        private readonly CategoryService service;

        public CategoryServiceAsyncTests()
        {
            this.service = new CategoryService();
            service.AddAsync(new Category { Name = "Processor" }).Wait();
            service.AddAsync(new Category { Name = "Battery" }).Wait();
        }

        [TestMethod]
        public void AddAsyncTest()
        {
            string name = Guid.NewGuid().ToString();
            Category newCategory = new Category { Name = name };
            Category addedCategory = service.AddAsync(newCategory).Result;
            Assert.IsNotNull(addedCategory);
            Assert.IsTrue(addedCategory.Id > 0);
            Assert.AreEqual(addedCategory.Name, name);
        }

        [TestMethod]
        public void GetAsyncByIdTest()
        {
            Category category = service.GetAsync(1).Result;
            Assert.IsNotNull(category);
            Assert.AreEqual(category.Id, 1);
        }

        [TestMethod]
        public void GetAsyncByIdEditTest()
        {
            Category category = service.GetAsync(1).Result;
            string name = category.Name;
            category.Name = Guid.NewGuid().ToString();
            Category newCategory = service.GetAsync(1).Result;
            Assert.AreEqual(newCategory.Name, name);
        }

        [TestMethod]
        public void GetAsyncByIdNotFoundTest()
        {
            Category category = service.GetAsync(int.MaxValue).Result;
            Assert.IsNull(category);
        }

        [TestMethod]
        public void GetAsyncAllTest()
        {
            List<Category> categories = service.GetAsync().Result;
            Assert.IsNotNull(categories);
            Assert.IsTrue(categories.Count > 0);
        }

        [TestMethod]
        public void UpdateAsyncTest()
        {
            Category category = service.GetAsync().Result.First();
            category.Name += "upd";
            service.UpdateAsync(category).Wait();
            Category updatedCategory = service.GetAsync(category.Id).Result;
            Assert.IsNotNull(updatedCategory);
            Assert.AreEqual(updatedCategory.Name, category.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void UpdateAsyncNotFoundTest()
        {
            service.UpdateAsync(new Category { Id = int.MaxValue }).Wait();
        }

        [TestMethod]
        public void DeleteAsyncTest()
        {
            Category category = service.GetAsync().Result.Last();
            service.DeleteAsync(category.Id).Wait();
            Category deletedCategory = service.GetAsync(category.Id).Result;
            Assert.IsNull(deletedCategory);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void DeleteAsyncNotFoundTest()
        {
            service.DeleteAsync(int.MaxValue).Wait();
        }
    }
}