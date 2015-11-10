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
    public class ProducerService : IServiceAsync<Producer>
    {
        private static List<Producer> producers = new List<Producer>();

        public Producer Get(int id)
        {
            for (int i = 0; i < producers.Count; i++)
                if (producers[i].Id == id) return (Producer)producers[i].Clone();
            return null;
        }

        public List<Producer> Get()
        {
            return producers.Select(producer => (Producer)producer.Clone()).ToList();
        }

        public Producer Add(Producer newProducer)
        {
            if (producers.Count != 0)
                newProducer.Id = producers.Max(Producer => Producer.Id) + 1;
            else newProducer.Id = 1;
            producers.Add(newProducer);
            return (Producer)newProducer.Clone();
        }

        public void Delete(int id)
        {
            Producer producer = producers.SingleOrDefault(item => item.Id == id);
            if (producer == null) throw new NullReferenceException();
            producers.Remove(producer);
        }

        public Producer Update(Producer newProducer)
        {
            Producer oldProducer = producers.SingleOrDefault(item => item.Id == newProducer.Id);
            if (oldProducer == null) throw new NullReferenceException();
            oldProducer.Country = newProducer.Country;
            oldProducer.Name = newProducer.Name;
            oldProducer.Site = newProducer.Site;
            return (Producer)oldProducer.Clone();
        }

        public async Task<List<Producer>> GetAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get();
            });
        }

        public async Task<Producer> GetAsync(int id)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get(id);
            });
        }

        public async Task<Producer> AddAsync(Producer newProducer)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Add(newProducer);
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

        public async Task<Producer> UpdateAsync(Producer newProducer)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Update(newProducer);
            });
        }
    }
}