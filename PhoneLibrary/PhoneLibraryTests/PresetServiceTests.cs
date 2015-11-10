using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneLibrary.service;
using PhoneLibrary.model;

namespace PhoneLibraryTests
{
    [TestClass]
    public class PresetServiceTests
    {
        private readonly PresetService service;

        public PresetServiceTests()
        {
            this.service = new PresetService();
            service.Add(new Preset { Value = "Red" });
            service.Add(new Preset { Value = "White" });
        }

        [TestMethod]
        public void AddTest()
        {
            string value = Guid.NewGuid().ToString();
            Preset newPreset = new Preset { Value = value };
            Preset addedPreset = service.Add(newPreset);
            Assert.IsNotNull(addedPreset);
            Assert.IsTrue(addedPreset.Id > 0);
            Assert.AreEqual(addedPreset.Value, value);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            Preset preset = service.Get(1);
            Assert.IsNotNull(preset);
            Assert.AreEqual(preset.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            Preset preset = service.Get(1);
            string value = preset.Value;
            preset.Value = Guid.NewGuid().ToString();
            Preset newPreset = service.Get(1);
            Assert.AreEqual(newPreset.Value, value);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            Preset preset = service.Get(int.MaxValue);
            Assert.IsNull(preset);
        }

        [TestMethod]
        public void GetAllTest()
        {
            List<Preset> presets = service.Get();
            Assert.IsNotNull(presets);
            Assert.IsTrue(presets.Count > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            Preset preset = service.Get().First();
            preset.Value += "upd";
            service.Update(preset);
            Preset updatedPreset = service.Get(preset.Id);
            Assert.IsNotNull(updatedPreset);
            Assert.AreEqual(updatedPreset.Value, preset.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdadeNotFoundTest()
        {
            service.Update(new Preset { Id = int.MaxValue });
        }

        [TestMethod]
        public void DeleteTest()
        {
            Preset preset = service.Get().Last();
            service.Delete(preset.Id);
            Preset deletedPreset = service.Get(preset.Id);
            Assert.IsNull(deletedPreset);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DeleteNotFoundTest()
        {
            service.Delete(int.MaxValue);
        }
    }
}