using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoneLibrary.model;
using PhoneLibrary.service.async;
using System.Threading.Tasks;
using System.Threading;

namespace PhoneLibrary.service
{
    public class CommentService : IServiceAsync<Comment>
    {
        private static List<Comment> comments = new List<Comment>();

        public Comment Get(int id)
        {
            for (int i = 0; i < comments.Count; i++)
                if (comments[i].Id == id) return (Comment)comments[i].Clone();
            return null;
        }

        public List<Comment> Get()
        {
            return comments.Select(comment => (Comment)comment.Clone()).ToList();
        }

        public List<Comment> GetByPhoneId(int phoneId)
        {
            return comments
                .Where(comment => comment.PhoneId == phoneId)
                .Select(comment => (Comment)comment.Clone()).ToList();
        }

        public Comment Add(Comment newComment)
        {
            if (comments.Count != 0)
                newComment.Id = comments.Max(comment => comment.Id) + 1;
            else newComment.Id = 1;
            comments.Add(newComment);
            return (Comment)newComment.Clone();
        }

        public void Delete(int id)
        {
            Comment comment = comments.SingleOrDefault(item => item.Id == id);
            if (comment == null) throw new NullReferenceException();
            comments.Remove(comment);
        }

        public Comment Update(Comment newComment)
        {
            Comment oldComment = comments.SingleOrDefault(item => item.Id == newComment.Id);
            if (oldComment == null) throw new NullReferenceException();
            oldComment.Author = newComment.Author;
            oldComment.Date = newComment.Date;
            oldComment.Mark = newComment.Mark;
            oldComment.Text = newComment.Text;
            return (Comment)oldComment.Clone();
        }

        public async Task<List<Comment>> GetAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get();
            });
        }

        public async Task<Comment> GetAsync(int id)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get(id);
            });
        }

        public async Task<Comment> AddAsync(Comment newT)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Add(newT);
            });
        }

        public async Task DeleteAsync(int id)
        {
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                Delete(id);
            });
        }

        public async Task<Comment> UpdateAsync(Comment newT)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Update(newT);
            });
        }

        public async Task<List<Comment>> GetByPhoneIdAsync(int phoneId)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return GetByPhoneId(phoneId);
            });
        }
    }
}