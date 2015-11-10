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
    public class ShopService : IServiceAsync<Shop>
    {
        private static List<Shop> shops = new List<Shop>();

        public Shop Get(int id)
        {
            for (int i = 0; i < shops.Count; i++)
                if (shops[i].Id == id) return (Shop)shops[i].Clone();
            return null;
        }

        public List<Shop> Get()
        {
            return shops.Select(shop => (Shop)shop.Clone()).ToList();
        }

        public List<Shop> GetByPhoneId(int phoneId)
        {
            return shops
                .Where(shop => shop.PhoneId == phoneId)
                .Select(shop => (Shop)shop.Clone()).ToList();
        }

        public Shop Add(Shop newShop)
        {
            if (shops.Count != 0)
                newShop.Id = shops.Max(shop => shop.Id) + 1;
            else newShop.Id = 1;
            shops.Add(newShop);
            return (Shop)newShop.Clone();
        }

        public void Delete(int id)
        {
            Shop shop = shops.SingleOrDefault(item => item.Id == id);
            if (shop == null) throw new NullReferenceException();
            shops.Remove(shop);
        }

        public Shop Update(Shop newShop)
        {
            Shop oldShop = shops.SingleOrDefault(item => item.Id == newShop.Id);
            if (oldShop == null) throw new NullReferenceException();
            oldShop.Address = newShop.Address;
            oldShop.Name = newShop.Name;
            oldShop.PhoneNumber = newShop.PhoneNumber;
            oldShop.Price = newShop.Price;
            oldShop.Website = newShop.Website;
            return (Shop)oldShop.Clone();
        }

        public async Task<List<Shop>> GetAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get();
            });
        }

        public async Task<Shop> GetAsync(int id)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get(id);
            });
        }

        public async Task<Shop> AddAsync(Shop newT)
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

        public async Task<Shop> UpdateAsync(Shop newT)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Update(newT);
            });
        }

        public async Task<List<Shop>> GetByPhoneIdAsync(int phoneId)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return GetByPhoneId(phoneId);
            });
        }
    }
}