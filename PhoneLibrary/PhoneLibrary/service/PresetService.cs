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
        public delegate void EntityChanged<TEntity>(EntityChangedEventArgs<TEntity> entity);

        public event EntityChanged<Preset> OnAdded;
        public event EntityChanged<Preset> OnGot;
        public event EntityChanged<List<Preset>> OnAllGot;
        public event EntityChanged<List<Preset>> OnGotBySpecificationId;
        public event EntityChanged<Preset> OnDeleted;
        public event EntityChanged<Preset> OnUpdated;

        private static List<Preset> presets = new List<Preset>();

        public Preset Get(int id)
        {
            for (int i = 0; i < presets.Count; i++)
                if (presets[i].Id == id)
                {
                    Preset found = (Preset)presets[i].Clone();
                    OnGot(new EntityChangedEventArgs<Preset>(found));
                    return found;
                }
            OnGot(new EntityChangedEventArgs<Preset>(null));
            return null;
        }

        public List<Preset> Get()
        {
            List<Preset> copiedPresets = presets.Select(preset => (Preset)preset.Clone()).ToList();
            OnAllGot(new EntityChangedEventArgs<List<Preset>>(copiedPresets));
            return copiedPresets;
        }

        public List<Preset> GetBySpecificationId(int specificationId)
        {
            List<Preset> copiedPresets = presets.Where(preset => preset.SpecificationId == specificationId)
                .Select(preset => (Preset)preset.Clone()).ToList();
            OnGotBySpecificationId(new EntityChangedEventArgs<List<Preset>>(copiedPresets));
            return copiedPresets;
        }
        public Preset Add(Preset newPreset)
        {
            if (presets.Count != 0)
                newPreset.Id = presets.Max(preset => preset.Id) + 1;
            else newPreset.Id = 1;
            presets.Add(newPreset);
            OnAdded(new EntityChangedEventArgs<Preset>(newPreset));
            return (Preset)newPreset.Clone();
        }

        public void Delete(int id)
        {
            Preset preset = presets.SingleOrDefault(item => item.Id == id);
            OnDeleted(new EntityChangedEventArgs<Preset>(preset));
            if (preset == null) throw new NullReferenceException();
            presets.Remove(preset);
        }

        public Preset Update(Preset newPreset)
        {
            Preset oldPreset = presets.SingleOrDefault(item => item.Id == newPreset.Id);
            OnUpdated(new EntityChangedEventArgs<Preset>(oldPreset));
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