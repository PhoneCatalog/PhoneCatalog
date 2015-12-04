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
            service.Add(new Category { Name = "Processor" });
            service.Add(new Category { Name = "Battery" });
        }

        [TestMethod]
        public void AddTest()
        {
            string name = Guid.NewGuid().ToString();
            Category newCategory = new Category { Name = name };
            Category addedCategory = service.Add(newCategory);
            Assert.IsNotNull(addedCategory);
            Assert.IsTrue(addedCategory.Id > 0);
            Assert.AreEqual(addedCategory.Name, name);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            Category category = service.Get(1);
            Assert.IsNotNull(category);
            Assert.AreEqual(category.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            Category category = service.Get(1);
            string name = category.Name;
            category.Name = Guid.NewGuid().ToString();
            Category newCategory = service.Get(1);
            Assert.AreEqual(newCategory.Name, name);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            Category category = service.Get(int.MaxValue);
            Assert.IsNull(category);
        }

        [TestMethod]
        public void GetAllTest()
        {
            List<Category> categories = service.Get();
            Assert.IsNotNull(categories);
            Assert.IsTrue(categories.Count > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            Category category = service.Get().First();
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
            service.Update(new Category { Id = int.MaxValue });
        }

        [TestMethod]
        public void DeleteTest()
        {
            Category category = service.Get().Last();
            service.Delete(category.Id);
            Category deletedCategory = service.Get(category.Id);
            Assert.IsNull(deletedCategory);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DeleteNotFoundTest()
        {
            service.Delete(int.MaxValue);
        }
    }
}