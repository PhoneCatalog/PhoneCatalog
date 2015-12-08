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
        public delegate void EntityChanged<TEntity>(EntityChangedEventArgs<TEntity> entity);

        public event EntityChanged<Category> OnAdded;
        public event EntityChanged<Category> OnGot;
        public event EntityChanged<List<Category>> OnAllGot;
        public event EntityChanged<Category> OnDeleted;
        public event EntityChanged<Category> OnUpdated;

        private static List<Category> categories = new List<Category>();

        public Category Get(int id)
        {
            for (int i = 0; i < categories.Count; i++)
                if (categories[i].Id == id)
                {
                    Category found = (Category)categories[i].Clone();
                    OnGot(new EntityChangedEventArgs<Category>(found));
                    return found;
                }
            OnGot(new EntityChangedEventArgs<Category>(null));
            return null;
        }

        public List<Category> Get()
        {
            List<Category> copiedCategories = categories.Select(category => (Category)category.Clone()).ToList();
            OnAllGot(new EntityChangedEventArgs<List<Category>>(copiedCategories));
            return copiedCategories;
        }

        public Category Add(Category newCategory)
        {
            if (categories.Count != 0)
                newCategory.Id = categories.Max(category => category.Id) + 1;
            else newCategory.Id = 1;
            categories.Add(newCategory);
            OnAdded(new EntityChangedEventArgs<Category>(newCategory));
            return (Category)newCategory.Clone();
        }

        public void Delete(int id)
        {
            Category category = categories.SingleOrDefault(item => item.Id == id);
            OnDeleted(new EntityChangedEventArgs<Category>(category));
            if (category == null) throw new NullReferenceException();
            categories.Remove(category);
        }

        public Category Update(Category newCategory)
        {
            Category oldCategory = categories.SingleOrDefault(item => item.Id == newCategory.Id);
            OnUpdated(new EntityChangedEventArgs<Category>(oldCategory));
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