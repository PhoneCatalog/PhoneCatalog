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
            service.Add(new Comment { Author = new User(), Date = new DateTime(), Text = "text1", Mark = 10 });
            service.Add(new Comment { Author = new User(), Date = new DateTime(), Text = "text2", Mark = 20 });
        }

        [TestMethod]
        public void AddTest()
        {
            string text = Guid.NewGuid().ToString();
            Comment newComment = new Comment { Text = text };
            Comment addedComment = service.Add(newComment);
            Assert.IsNotNull(addedComment);
            Assert.IsTrue(addedComment.Id > 0);
            Assert.AreEqual(addedComment.Text, text);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            Comment comment = service.Get(1);
            Assert.IsNotNull(comment);
            Assert.AreEqual(comment.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            Comment comment = service.Get(1);
            string text = comment.Text;
            comment.Text = Guid.NewGuid().ToString();
            Comment newComment = service.Get(1);
            Assert.AreEqual(newComment.Text, text);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            Comment comment = service.Get(int.MaxValue);
            Assert.IsNull(comment);
        }

        [TestMethod]
        public void GetAllTest()
        {
            List<Comment> comments = service.Get();
            Assert.IsNotNull(comments);
            Assert.IsTrue(comments.Count > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            Comment comment = service.Get().First();
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
            service.Update(new Comment { Id = int.MaxValue });
        }

        [TestMethod]
        public void DeleteTest()
        {
            Comment comment = service.Get().Last();
            service.Delete(comment.Id);
            Comment deletedComment = service.Get(comment.Id);
            Assert.IsNull(deletedComment);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DeleteNotFoundTest()
        {
            service.Delete(int.MaxValue);
        }
    }
}