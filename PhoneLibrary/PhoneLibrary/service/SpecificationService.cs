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
    public class SpecificationService : IServiceAsync<Specification>
    {
        public delegate void EntityChanged<TEntity>(EntityChangedEventArgs<TEntity> entity);

        public event EntityChanged<Specification> OnAdded;
        public event EntityChanged<Specification> OnGot;
        public event EntityChanged<List<Specification>> OnAllGot;
        public event EntityChanged<Specification> OnDeleted;
        public event EntityChanged<Specification> OnUpdated;

        private static List<Specification> specifications = new List<Specification>();

        public Specification Get(int id)
        {
            for (int i = 0; i < specifications.Count; i++)
                if (specifications[i].Id == id)
                {
                    Specification found = (Specification)specifications[i].Clone();
                    OnGot(new EntityChangedEventArgs<Specification>(found));
                    return found;
                }
            OnGot(new EntityChangedEventArgs<Specification>(null));
            return null;
        }

        public List<Specification> Get()
        {
            List<Specification> copiedSpecifications = specifications
                .Select(specification => (Specification)specification.Clone()).ToList();
            OnAllGot(new EntityChangedEventArgs<List<Specification>>(copiedSpecifications));
            return copiedSpecifications;
        }

        public Specification Add(Specification newSpecification)
        {
            if (specifications.Count != 0)
                newSpecification.Id = specifications.Max(specification => specification.Id) + 1;
            else newSpecification.Id = 1;
            specifications.Add(newSpecification);
            OnAdded(new EntityChangedEventArgs<Specification>(newSpecification));
            return (Specification)newSpecification.Clone();
        }

        public void Delete(int id)
        {
            Specification specification = specifications.SingleOrDefault(item => item.Id == id);
            OnDeleted(new EntityChangedEventArgs<Specification>(specification));
            if (specification == null) throw new NullReferenceException();
            specifications.Remove(specification);
        }

        public Specification Update(Specification newSpecification)
        {
            Specification oldSpecification = specifications.SingleOrDefault(item => item.Id == newSpecification.Id);
            OnUpdated(new EntityChangedEventArgs<Specification>(oldSpecification));
            if (oldSpecification == null) throw new NullReferenceException();
            oldSpecification.Category = newSpecification.Category;
            oldSpecification.Name = newSpecification.Name;
            return (Specification)oldSpecification.Clone();
        }

        public async Task<List<Specification>> GetAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get();
            });
        }

        public async Task<Specification> GetAsync(int id)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get(id);
            });
        }

        public async Task<Specification> AddAsync(Specification newSpecification)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Add(newSpecification);
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

        public async Task<Specification> UpdateAsync(Specification newSpecification)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Update(newSpecification);
            });
        }
    }
}