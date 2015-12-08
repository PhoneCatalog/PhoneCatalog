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
        public delegate void EntityChanged<TEntity>(EntityChangedEventArgs<TEntity> entity);

        public event EntityChanged<Phone> OnAdded;
        public event EntityChanged<Phone> OnGot;
        public event EntityChanged<List<Phone>> OnAllGot;
        public event EntityChanged<Phone> OnDeleted;
        public event EntityChanged<Phone> OnUpdated;

        private static List<Phone> phones = new List<Phone>();

        public Phone Get(int id)
        {
            for (int i = 0; i < phones.Count; i++)
                if (phones[i].Id == id)
                {
                    Phone found = (Phone)phones[i].Clone();
                    OnGot(new EntityChangedEventArgs<Phone>(found));
                    return found;
                }
            OnGot(new EntityChangedEventArgs<Phone>(null));
            return null;
        }

        public List<Phone> Get()
        {
            List<Phone> copiedPhones = phones.Select(phone => (Phone)phone.Clone()).ToList();
            OnAllGot(new EntityChangedEventArgs<List<Phone>>(copiedPhones));
            return copiedPhones;
        }

        public Phone Add(Phone newPhone)
        {
            if (phones.Count != 0)
                newPhone.Id = phones.Max(Phone => Phone.Id) + 1;
            else newPhone.Id = 1;
            phones.Add(newPhone);
            OnAdded(new EntityChangedEventArgs<Phone>(newPhone));
            return (Phone)newPhone.Clone();
        }

        public void Delete(int id)
        {
            Phone phone = phones.SingleOrDefault(item => item.Id == id);
            OnDeleted(new EntityChangedEventArgs<Phone>(phone));
            if (phone == null) throw new NullReferenceException();
            phones.Remove(phone);
        }

        public Phone Update(Phone newPhone)
        {
            Phone oldPhone = phones.SingleOrDefault(item => item.Id == newPhone.Id);
            OnUpdated(new EntityChangedEventArgs<Phone>(oldPhone));
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