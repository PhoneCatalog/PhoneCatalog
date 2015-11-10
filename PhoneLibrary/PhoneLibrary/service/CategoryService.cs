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
    public class CategoryService : IServiceAsync<Category>
    {
        private static List<Category> categories = new List<Category>();

        public Category Get(int id)
        {
            for (int i = 0; i < categories.Count; i++)
                if (categories[i].Id == id) return (Category)categories[i].Clone();
            return null;
        }

        public List<Category> Get()
        {
            return categories.Select(category => (Category)category.Clone()).ToList();
        }

        public Category Add(Category newCategory)
        {
            if (categories.Count != 0)
                newCategory.Id = categories.Max(category => category.Id) + 1;
            else newCategory.Id = 1;
            categories.Add(newCategory);
            return (Category)newCategory.Clone();
        }

        public void Delete(int id)
        {
            Category category = categories.SingleOrDefault(item => item.Id == id);
            if (category == null) throw new NullReferenceException();
            categories.Remove(category);
        }

        public Category Update(Category newCategory)
        {
            Category oldCategory = categories.SingleOrDefault(item => item.Id == newCategory.Id);
            if (oldCategory == null) throw new NullReferenceException();
            oldCategory.Name = newCategory.Name;
            return (Category)oldCategory.Clone();
        }

        public async Task<List<Category>> GetAsync()
        {
            return await Task.Factory.StartNew(() =>
             {
                 Thread.Sleep(5000);
                 return Get();
             });
        }

        public async Task<Category> GetAsync(int id)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get(id);
            });
        }

        public async Task<Category> AddAsync(Category newCategory)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Add(newCategory);
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

        public async Task<Category> UpdateAsync(Category newCategory)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Update(newCategory);
            });
        }
    }
}