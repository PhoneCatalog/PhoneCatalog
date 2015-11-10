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
    public class PhoneService : IServiceAsync<Phone>
    {
        private static List<Phone> phones = new List<Phone>();

        public Phone Get(int id)
        {
            for (int i = 0; i < phones.Count; i++)
                if (phones[i].Id == id) return (Phone)phones[i].Clone();
            return null;
        }

        public List<Phone> Get()
        {
            return phones.Select(phone => (Phone)phone.Clone()).ToList();
        }

        public Phone Add(Phone newPhone)
        {
            if (phones.Count != 0)
                newPhone.Id = phones.Max(Phone => Phone.Id) + 1;
            else newPhone.Id = 1;
            phones.Add(newPhone);
            return (Phone)newPhone.Clone();
        }

        public void Delete(int id)
        {
            Phone phone = phones.SingleOrDefault(item => item.Id == id);
            if (phone == null) throw new NullReferenceException();
            phones.Remove(phone);
        }

        public Phone Update(Phone newPhone)
        {
            Phone oldPhone = phones.SingleOrDefault(item => item.Id == newPhone.Id);
            if (oldPhone == null) throw new NullReferenceException();
            oldPhone.Model = newPhone.Model;
            oldPhone.PhoneProducer = newPhone.PhoneProducer;
            oldPhone.Series = newPhone.Series;
            oldPhone.Type = newPhone.Type;
            oldPhone.DateOfRelease = newPhone.DateOfRelease;
            oldPhone.Description = newPhone.Description;
            return (Phone)oldPhone.Clone();
        }

        public async Task<List<Phone>> GetAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get();
            });
        }

        public async Task<Phone> GetAsync(int id)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get(id);
            });
        }

        public async Task<Phone> AddAsync(Phone newPhone)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Add(newPhone);
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

        public async Task<Phone> UpdateAsync(Phone newPhone)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Update(newPhone);
            });
        }
    }
}