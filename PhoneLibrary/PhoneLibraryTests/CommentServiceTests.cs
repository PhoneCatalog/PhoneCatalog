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
    public class CommentServiceTests
    {
        private readonly CommentService service;

        public CommentServiceTests()
        {
            this.service = new CommentService();
            service.OnAdded += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.Add(new Comment { Author = new User(), Date = new DateTime(), Text = "text1", Mark = 10 });
            service.Add(new Comment { Author = new User(), Date = new DateTime(), Text = "text2", Mark = 20 });
        }

        [TestMethod]
        public void AddTest()
        {
            string text = Guid.NewGuid().ToString();
            service.OnAdded += (args) =>
            {
                Assert.AreEqual(args.Entity.Text, text);
            };
            Comment newComment = new Comment { Text = text };
            Comment addedComment = service.Add(newComment);
            Assert.IsNotNull(addedComment);
            Assert.IsTrue(addedComment.Id > 0);
            Assert.AreEqual(addedComment.Text, text);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Comment comment = service.Get(1);
            Assert.IsNotNull(comment);
            Assert.AreEqual(comment.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Comment comment = service.Get(1);
            string text = comment.Text;
            comment.Text = Guid.NewGuid().ToString();
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Text, text);
            };
            Comment newComment = service.Get(1);
            Assert.AreEqual(newComment.Text, text);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            service.OnGot += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            Comment comment = service.Get(int.MaxValue);
            Assert.IsNull(comment);
        }

        [TestMethod]
        public void GetAllTest()
        {
            service.OnAllGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
                Assert.IsTrue(args.Entity.Count > 0);
            };
            List<Comment> comments = service.Get();
            Assert.IsNotNull(comments);
            Assert.IsTrue(comments.Count > 0);
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
            Comment comment = service.Get().First();
            string oldText = comment.Text;
            service.OnUpdated += (args) =>
            {
                Assert.AreEqual(args.Entity.Text, oldText);
            };
            comment.Text += "upd";
            service.Update(comment);
            Comment updatedComment = service.Get(comment.Id);
            Assert.IsNotNull(updatedComment);
            Assert.AreEqual(updatedComment.Text, comment.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNotFoundTest()
        {
            service.OnUpdated += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            service.Update(new Comment { Id = int.MaxValue });
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
            Comment comment = service.Get().Last();
            service.Delete(comment.Id);
            Comment deletedComment = service.Get(comment.Id);
            Assert.IsNull(deletedComment);
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