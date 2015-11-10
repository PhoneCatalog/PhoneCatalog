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
    public class PresetService : IServiceAsync<Preset>
    {
        private static List<Preset> presets = new List<Preset>();

        public Preset Get(int id)
        {
            for (int i = 0; i < presets.Count; i++)
                if (presets[i].Id == id) return (Preset)presets[i].Clone();
            return null;
        }

        public List<Preset> Get()
        {
            return presets.Select(preset => (Preset)preset.Clone()).ToList();
        }

        public List<Preset> GetBySpecificationId(int specificationId)
        {
            return presets
                .Where(preset => preset.SpecificationId == specificationId)
                .Select(preset => (Preset)preset.Clone()).ToList();
        }
        public Preset Add(Preset newPreset)
        {
            if (presets.Count != 0)
                newPreset.Id = presets.Max(preset => preset.Id) + 1;
            else newPreset.Id = 1;
            presets.Add(newPreset);
            return (Preset)newPreset.Clone();
        }

        public void Delete(int id)
        {
            Preset preset = presets.SingleOrDefault(item => item.Id == id);
            if (preset == null) throw new NullReferenceException();
            presets.Remove(preset);
        }

        public Preset Update(Preset newPreset)
        {
            Preset oldPreset = presets.SingleOrDefault(item => item.Id == newPreset.Id);
            if (oldPreset == null) throw new NullReferenceException();
            oldPreset.Value = newPreset.Value;
            return (Preset)oldPreset.Clone();
        }

        public async Task<List<Preset>> GetAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get();
            });
        }

        public async Task<Preset> GetAsync(int id)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Get(id);
            });
        }

        public async Task<Preset> AddAsync(Preset newPreset)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Add(newPreset);
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

        public async Task<Preset> UpdateAsync(Preset newPreset)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return Update(newPreset);
            });
        }

        public async Task<List<Preset>> GetBySpecificationIdAsync(int specificationId)
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return GetBySpecificationId(specificationId);
            });
        }
    }
}