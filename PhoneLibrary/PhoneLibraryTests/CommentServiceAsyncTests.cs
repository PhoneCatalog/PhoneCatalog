using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneLibrary.service;
using PhoneLibrary.model;
using System.Collections.Generic;
using System.Linq;

namespace PhoneLibraryTests
{
    [TestClass]
    public class CommentServiceAsyncTests
    {
        private readonly CommentService service;

        public CommentServiceAsyncTests()
        {
            this.service = new CommentService();
            service.AddAsync(new Comment { Author = new User(), Date = new DateTime(), Text = "text1", Mark = 10 }).Wait();
            service.AddAsync(new Comment { Author = new User(), Date = new DateTime(), Text = "text2", Mark = 20 }).Wait();
        }

        [TestMethod]
        public void AddAsyncTest()
        {
            string text = Guid.NewGuid().ToString();
            Comment newComment = new Comment { Text = text };
            Comment addedComment = service.AddAsync(newComment).Result;
            Assert.IsNotNull(addedComment);
            Assert.IsTrue(addedComment.Id > 0);
            Assert.AreEqual(addedComment.Text, text);
        }

        [TestMethod]
        public void GetAsyncByIdTest()
        {
            Comment comment = service.GetAsync(1).Result;
            Assert.IsNotNull(comment);
            Assert.AreEqual(comment.Id, 1);
        }

        [TestMethod]
        public void GetAsyncByIdEditTest()
        {
            Comment comment = service.GetAsync(1).Result;
            string text = comment.Text;
            comment.Text = Guid.NewGuid().ToString();
            Comment newComment = service.GetAsync(1).Result;
            Assert.AreEqual(newComment.Text, text);
        }

        [TestMethod]
        public void GetAsyncByIdNotFoundTest()
        {
            Comment comment = service.GetAsync(int.MaxValue).Result;
            Assert.IsNull(comment);
        }

        [TestMethod]
        public void GetAsyncAllTest()
        {
            List<Comment> comments = service.GetAsync().Result;
            Assert.IsNotNull(comments);
            Assert.IsTrue(comments.Count > 0);
        }

        [TestMethod]
        public void UpdateAsyncTest()
        {
            Comment comment = service.GetAsync().Result.First();
            comment.Text += "upd";
            service.UpdateAsync(comment).Wait();
            Comment updatedComment = service.GetAsync(comment.Id).Result;
            Assert.IsNotNull(updatedComment);
            Assert.AreEqual(updatedComment.Text, comment.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void UpdateAsyncNotFoundTest()
        {
            service.UpdateAsync(new Comment { Id = int.MaxValue }).Wait();
        }

        [TestMethod]
        public void DeleteAsyncTest()
        {
            Comment comment = service.GetAsync().Result.Last();
            service.DeleteAsync(comment.Id).Wait();
            Comment deletedComment = service.GetAsync(comment.Id).Result;
            Assert.IsNull(deletedComment);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void DeleteAsyncNotFoundTest()
        {
            service.DeleteAsync(int.MaxValue).Wait();
        }
    }
}