using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneLibrary.model;
using PhoneLibrary.service.async;
using System.Threading;

namespace PhoneLibrary.service
{
    public class CharacteristicService : IServiceAsync<Characteristic>
    {
        public delegate void EntityChanged<TEntity>(EntityChangedEventArgs<TEntity> entity);

        public event EntityChanged<Characteristic> OnAdded;
        public event EntityChanged<Characteristic> OnGot;
        public event EntityChanged<List<Characteristic>> OnAllGot;
        public event EntityChanged<List<Characteristic>> OnGotByPhoneId;
        public event EntityChanged<Characteristic> OnDeleted;
        public event EntityChanged<Characteristic> OnUpdated;

        private static List<Characteristic> characteristics = new List<Characteristic>();

        public Characteristic Get(int id)
        {
            for (int i = 0; i < characteristics.Count; i++)
                if (characteristics[i].Id == id)
                {
                    Characteristic found = (Characteristic)characteristics[i].Clone();
                    OnGot(new EntityChangedEventArgs<Characteristic>(found));
                    return found;
                }
            OnGot(new EntityChangedEventArgs<Characteristic>(null));
            return null;
        }

        public List<Characteristic> Get()
        {
            List<Characteristic> copiedCharacteristics = characteristics
                .Select(characteristic => (Characteristic)characteristic.Clone()).ToList();
            OnAllGot(new EntityChangedEventArgs<List<Characteristic>>(copiedCharacteristics));
            return copiedCharacteristics;
        }

        public List<Characteristic> GetByPhoneId(int phoneId)
        {
            List<Characteristic> copiedCharacteristics = characteristics
                .Where(characteristic => characteristic.PhoneId == phoneId)
                .Select(characteristic => (Characteristic)characteristic.Clone()).ToList();
            OnGotByPhoneId(new EntityChangedEventArgs<List<Characteristic>>(copiedCharacteristics));
            return copiedCharacteristics;
        }

        public Characteristic Add(Characteristic newCharacteristic)
        {
            if (characteristics.Count != 0)
                newCharacteristic.Id = characteristics.Max(characteristic => characteristic.Id) + 1;
            else newCharacteristic.Id = 1;
            characteristics.Add(newCharacteristic);
            OnAdded(new EntityChangedEventArgs<Characteristic>(newCharacteristic));
            return (Characteristic)newCharacteristic.Clone();
        }

        public void Delete(int id)
        {
            Characteristic characteristic = characteristics.SingleOrDefault(item => item.Id == id);
            OnDeleted(new EntityChangedEventArgs<Characteristic>(characteristic));
            if (characteristic == null) throw new NullReferenceException();
            characteristics.Remove(characteristic);
        }

        public Characteristic Update(Characteristic newCharacteristic)
        {
            Characteristic oldCharacteristic = characteristics.SingleOrDefault(item => item.Id == newCharacteristic.Id);
            OnUpdated(new EntityChangedEventArgs<Characteristic>(oldCharacteristic));
            if (oldCharacteristic == null) throw new NullReferenceException();
            oldCharacteristic.Specification = newCharacteristic.Specification;
            oldCharacteristic.Value = newCharacteristic.Value;
            return (Characteristic)oldCharacteristic.Clone();
        }

        public async Task<List<Characteristic>> GetAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get();
            });
        }

        public async Task<Characteristic> GetAsync(int id)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get(id);
            });
        }

        public async Task<Characteristic> AddAsync(Characteristic newT)
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

        public async Task<Characteristic> UpdateAsync(Characteristic newT)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Update(newT);
            });
        }

        public async Task<List<Characteristic>> GetByPhoneIdAsync(int phoneId)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return GetByPhoneId(phoneId);
            });
        }
    }
}