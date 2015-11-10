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
    public class UserService : IServiceAsync<User>
    {
        private static List<User> users = new List<User>();

        public User Get(int id)
        {
            for (int i = 0; i < users.Count; i++)
                if (users[i].Id == id) return (User)users[i].Clone();
            return null;
        }

        public List<User> Get()
        {
            return users.Select(user => (User)user.Clone()).ToList();
        }

        public User Add(User newUser)
        {
            if (users.Count != 0)
                newUser.Id = users.Max(user => user.Id) + 1;
            else newUser.Id = 1;
            users.Add(newUser);
            return (User)newUser.Clone();
        }

        public void Delete(int id)
        {
            User user = users.SingleOrDefault(item => item.Id == id);
            if (user == null) throw new NullReferenceException();
            users.Remove(user);
        }

        public User Update(User newUser)
        {
            User oldUser = users.SingleOrDefault(item => item.Id == newUser.Id);
            if (oldUser == null) throw new NullReferenceException();
            oldUser.Email = newUser.Email;
            oldUser.Login = newUser.Login;
            oldUser.Password = newUser.Password;
            oldUser.PhoneNumber = newUser.PhoneNumber;
            return (User)oldUser.Clone();
        }

        public async Task<List<User>> GetAsync()
        {
            return await Task.Factory.StartNew(() =>
             {
                 Thread.Sleep(5000);
                 return Get();
             });
        }

        public async Task<User> GetAsync(int id)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get(id);
            });
        }

        public async Task<User> AddAsync(User newUser)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Add(newUser);
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

        public async Task<User> UpdateAsync(User newUser)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Update(newUser);
            });
        }
    }
}