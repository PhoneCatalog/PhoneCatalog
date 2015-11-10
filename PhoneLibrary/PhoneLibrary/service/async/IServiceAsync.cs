using PhoneLibrary.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneLibrary.service.async
{
    interface IServiceAsync<T> where T : BaseClass
    {
        Task<List<T>> GetAsync();

        Task<T> GetAsync(int id);

        Task<T> AddAsync(T newT);

        Task DeleteAsync(int id);

        Task<T> UpdateAsync(T newT);
    }
}
