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
        public delegate void EntityChanged<TEntity>(EntityChangedEventArgs<TEntity> entity);

        public event EntityChanged<Comment> OnAdded;
        public event EntityChanged<Comment> OnGot;
        public event EntityChanged<List<Comment>> OnAllGot;
        public event EntityChanged<List<Comment>> OnGotByPhoneId;
        public event EntityChanged<Comment> OnDeleted;
        public event EntityChanged<Comment> OnUpdated;

        private static List<Comment> comments = new List<Comment>();

        public Comment Get(int id)
        {
            for (int i = 0; i < comments.Count; i++)
                if (comments[i].Id == id)
                {
                    Comment found = (Comment)comments[i].Clone();
                    OnGot(new EntityChangedEventArgs<Comment>(found));
                    return found;
                }
            OnGot(new EntityChangedEventArgs<Comment>(null));
            return null;
        }

        public List<Comment> Get()
        {
            List<Comment> copiedComments = comments.Select(comment => (Comment)comment.Clone()).ToList();
            OnAllGot(new EntityChangedEventArgs<List<Comment>>(copiedComments));
            return copiedComments;
        }

        public List<Comment> GetByPhoneId(int phoneId)
        {
            List<Comment> copiedComments = comments.Where(comment => comment.PhoneId == phoneId)
                .Select(comment => (Comment)comment.Clone()).ToList();
            OnGotByPhoneId(new EntityChangedEventArgs<List<Comment>>(copiedComments));
            return copiedComments;
        }

        public Comment Add(Comment newComment)
        {
            if (comments.Count != 0)
                newComment.Id = comments.Max(comment => comment.Id) + 1;
            else newComment.Id = 1;
            comments.Add(newComment);
            OnAdded(new EntityChangedEventArgs<Comment>(newComment));
            return (Comment)newComment.Clone();
        }

        public void Delete(int id)
        {
            Comment comment = comments.SingleOrDefault(item => item.Id == id);
            OnDeleted(new EntityChangedEventArgs<Comment>(comment));
            if (comment == null) throw new NullReferenceException();
            comments.Remove(comment);
        }

        public Comment Update(Comment newComment)
        {
            Comment oldComment = comments.SingleOrDefault(item => item.Id == newComment.Id);
            OnUpdated(new EntityChangedEventArgs<Comment>(oldComment));
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