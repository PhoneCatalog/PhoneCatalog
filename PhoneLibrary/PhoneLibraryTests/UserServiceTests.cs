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
    public class UserServiceTests
    {
        private readonly UserService service;

        public UserServiceTests()
        {
            this.service = new UserService();
            service.OnAdded += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.Add(new User { Login = "user_1", Password = "p1", Email = "u_1.@mail.com", PhoneNumber = "111-00-11" });
            service.Add(new User { Login = "user_2", Password = "p2", Email = "u_2.@mail.com", PhoneNumber = "222-00-22" });
        }

        [TestMethod]
        public void AddTest()
        {
            string login = Guid.NewGuid().ToString();
            service.OnAdded += (args) =>
            {
                Assert.AreEqual(login, args.Entity.Login);
            };
            string password = Guid.NewGuid().ToString();
            string email = Guid.NewGuid().ToString();
            string phoneNumber = Guid.NewGuid().ToString();
            User newUser = new User { Email = email, Login = login, Password = password, PhoneNumber = phoneNumber };
            User addedUser = service.Add(newUser);
            Assert.IsNotNull(addedUser);
            Assert.IsTrue(addedUser.Id > 0);
            Assert.AreEqual(addedUser.Email, email);
            Assert.AreEqual(addedUser.Login, login);
            Assert.AreEqual(addedUser.Password, password);
            Assert.AreEqual(addedUser.PhoneNumber, phoneNumber);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            service.OnGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
                Assert.AreEqual(args.Entity.Id, 1);
            };
            User user = service.Get(1);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            User user = service.Get(1);
            string login = user.Login;
            string password = user.Password;
            string email = user.Email;
            string phoneNumber = user.PhoneNumber;
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Login, login);
            };
            user.Login = Guid.NewGuid().ToString();
            user.Password = Guid.NewGuid().ToString();
            user.Email = Guid.NewGuid().ToString();
            user.PhoneNumber = Guid.NewGuid().ToString();
            User newUser = service.Get(1);
            Assert.AreEqual(newUser.Login, login);
            Assert.AreEqual(newUser.Password, password);
            Assert.AreEqual(newUser.Email, email);
            Assert.AreEqual(newUser.PhoneNumber, phoneNumber);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            service.OnGot += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            User user = service.Get(int.MaxValue);
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetAllTest()
        {
            service.OnAllGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
                Assert.IsTrue(args.Entity.Count > 0);
            };
            List<User> users = service.Get();
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count > 0);
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
            User user = service.Get().First();
            string oldLogin = user.Login;
            service.OnUpdated += (args) =>
            {
                Assert.AreEqual(args.Entity.Login, oldLogin);
            };
            user.Email += "upd";
            user.Login += "upd";
            user.Password += "upd";
            user.PhoneNumber += "upd";
            service.Update(user);
            User updatedUser = service.Get(user.Id);
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(updatedUser.Email, user.Email);
            Assert.AreEqual(updatedUser.Login, user.Login);
            Assert.AreEqual(updatedUser.Password, user.Password);
            Assert.AreEqual(updatedUser.PhoneNumber, user.PhoneNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNotFoundTest()
        {
            service.OnUpdated += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            service.Update(new User { Id = int.MaxValue });
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
            User user = service.Get().Last();
            service.OnDeleted += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.Delete(user.Id);
            User deletedUser = service.Get(user.Id);
            Assert.IsNull(deletedUser);
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