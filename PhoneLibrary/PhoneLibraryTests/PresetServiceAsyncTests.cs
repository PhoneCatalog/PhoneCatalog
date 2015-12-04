using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneLibrary.service;
using PhoneLibrary.model;
using System.Linq;

namespace PhoneLibraryTests
{
    [TestClass]
    public class PresetServiceAsyncTests
    {
        private readonly PresetService service;

        public PresetServiceAsyncTests()
        {
            this.service = new PresetService();
            service.AddAsync(new Preset { Value = "Red" }).Wait();
            service.AddAsync(new Preset { Value = "White" }).Wait();
        }

        [TestMethod]
        public void AddAsyncTest()
        {
            string value = Guid.NewGuid().ToString();
            Preset newPreset = new Preset { Value = value };
            Preset addedPreset = service.AddAsync(newPreset).Result;
            Assert.IsNotNull(addedPreset);
            Assert.IsTrue(addedPreset.Id > 0);
            Assert.AreEqual(addedPreset.Value, value);
        }

        [TestMethod]
        public void GetAsyncByIdTest()
        {
            Preset preset = service.GetAsync(1).Result;
            Assert.IsNotNull(preset);
            Assert.AreEqual(preset.Id, 1);
        }

        [TestMethod]
        public void GetAsyncByIdEditTest()
        {
            Preset preset = service.GetAsync(1).Result;
            string value = preset.Value;
            preset.Value = Guid.NewGuid().ToString();
            Preset newPreset = service.GetAsync(1).Result;
            Assert.AreEqual(newPreset.Value, value);
        }

        [TestMethod]
        public void GetAsyncByIdNotFoundTest()
        {
            Preset preset = service.GetAsync(int.MaxValue).Result;
            Assert.IsNull(preset);
        }

        [TestMethod]
        public void GetAsyncAllTest()
        {
            List<Preset> presets = service.GetAsync().Result;
            Assert.IsNotNull(presets);
            Assert.IsTrue(presets.Count > 0);
        }

        [TestMethod]
        public void UpdateAsyncTest()
        {
            Preset preset = service.GetAsync().Result.First();
            preset.Value += "upd";
            service.UpdateAsync(preset).Wait();
            Preset updatedPreset = service.GetAsync(preset.Id).Result;
            Assert.IsNotNull(updatedPreset);
            Assert.AreEqual(updatedPreset.Value, preset.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void UpdateAsyncNotFoundTest()
        {
            service.UpdateAsync(new Preset { Id = int.MaxValue }).Wait();
        }

        [TestMethod]
        public void DeleteAsyncTest()
        {
            Preset preset = service.GetAsync().Result.Last();
            service.DeleteAsync(preset.Id).Wait();
            Preset deletedPreset = service.GetAsync(preset.Id).Result;
            Assert.IsNull(deletedPreset);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void DeleteAsyncNotFoundTest()
        {
            service.DeleteAsync(int.MaxValue).Wait();
        }
    }
}