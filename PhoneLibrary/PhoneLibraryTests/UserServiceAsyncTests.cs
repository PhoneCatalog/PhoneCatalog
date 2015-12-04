using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneLibrary.service;
using PhoneLibrary.model;
using System.Collections.Generic;
using System.Linq;

namespace PhoneLibraryTests
{
    [TestClass]
    public class UserServiceAsyncTests
    {
        private readonly UserService service;

        public UserServiceAsyncTests()
        {
            this.service = new UserService();
            service.AddAsync(new User { Login = "user_1", Password = "p1", Email = "u_1.@mail.com", PhoneNumber = "111-00-11" }).Wait();
            service.AddAsync(new User { Login = "user_2", Password = "p2", Email = "u_2.@mail.com", PhoneNumber = "222-00-22" }).Wait();
        }

        [TestMethod]
        public void AddAsyncTest()
        {
            string login = Guid.NewGuid().ToString();
            string password = Guid.NewGuid().ToString();
            string email = Guid.NewGuid().ToString();
            string phoneNumber = Guid.NewGuid().ToString();
            User newUser = new User { Email = email, Login = login, Password = password, PhoneNumber = phoneNumber };
            User addedUser = service.AddAsync(newUser).Result;
            Assert.IsNotNull(addedUser);
            Assert.IsTrue(addedUser.Id > 0);
            Assert.AreEqual(addedUser.Email, email);
            Assert.AreEqual(addedUser.Login, login);
            Assert.AreEqual(addedUser.Password, password);
            Assert.AreEqual(addedUser.PhoneNumber, phoneNumber);
        }

        [TestMethod]
        public void GetAsyncByIdTest()
        {
            User user = service.GetAsync(1).Result;
            Assert.IsNotNull(user);
            Assert.AreEqual(user.Id, 1);
        }

        [TestMethod]
        public void GetAsyncByIdEditTest()
        {
            User user = service.GetAsync(1).Result;
            string login = user.Login;
            string password = user.Password;
            string email = user.Email;
            string phoneNumber = user.PhoneNumber;
            user.Login = Guid.NewGuid().ToString();
            user.Password = Guid.NewGuid().ToString();
            user.Email = Guid.NewGuid().ToString();
            user.PhoneNumber = Guid.NewGuid().ToString();
            User newUser = service.GetAsync(1).Result;
            Assert.AreEqual(newUser.Login, login);
            Assert.AreEqual(newUser.Password, password);
            Assert.AreEqual(newUser.Email, email);
            Assert.AreEqual(newUser.PhoneNumber, phoneNumber);
        }

        [TestMethod]
        public void GetAsyncByIdNotFoundTest()
        {
            User user = service.GetAsync(int.MaxValue).Result;
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetAsyncAllTest()
        {
            List<User> users = service.GetAsync().Result;
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]
        public void UpdateAsyncTest()
        {
            User user = service.GetAsync().Result.First();
            user.Email += "upd";
            user.Login += "upd";
            user.Password += "upd";
            user.PhoneNumber += "upd";
            service.UpdateAsync(user).Wait();
            User updatedUser = service.GetAsync(user.Id).Result;
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(updatedUser.Email, user.Email);
            Assert.AreEqual(updatedUser.Login, user.Login);
            Assert.AreEqual(updatedUser.Password, user.Password);
            Assert.AreEqual(updatedUser.PhoneNumber, user.PhoneNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void UpdateAsyncNotFoundTest()
        {
            service.UpdateAsync(new User { Id = int.MaxValue }).Wait();
        }

        [TestMethod]
        public void DeleteAsyncTest()
        {
            User user = service.GetAsync().Result.Last();
            service.DeleteAsync(user.Id).Wait();
            User deletedUser = service.GetAsync(user.Id).Result;
            Assert.IsNull(deletedUser);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void DeleteAsyncNotFoundTest()
        {
            service.DeleteAsync(int.MaxValue).Wait();
        }
    }
}
