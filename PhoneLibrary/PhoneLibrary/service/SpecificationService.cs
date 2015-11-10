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
        private static List<Specification> specifications = new List<Specification>();

        public Specification Get(int id)
        {
            for (int i = 0; i < specifications.Count; i++)
                if (specifications[i].Id == id) return (Specification)specifications[i].Clone();
            return null;
        }

        public List<Specification> Get()
        {
            return specifications.Select(specification => (Specification)specification.Clone()).ToList();
        }

        public Specification Add(Specification newSpecification)
        {
            if (specifications.Count != 0)
                newSpecification.Id = specifications.Max(specification => specification.Id) + 1;
            else newSpecification.Id = 1;
            specifications.Add(newSpecification);
            return (Specification)newSpecification.Clone();
        }

        public void Delete(int id)
        {
            Specification specification = specifications.SingleOrDefault(item => item.Id == id);
            if (specification == null) throw new NullReferenceException();
            specifications.Remove(specification);
        }

        public Specification Update(Specification newSpecification)
        {
            Specification oldSpecification = specifications.SingleOrDefault(item => item.Id == newSpecification.Id);
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