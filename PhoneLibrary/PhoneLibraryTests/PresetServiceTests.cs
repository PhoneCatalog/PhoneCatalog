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
            service.OnAdded += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.Add(new Preset { Value = "Red" });
            service.Add(new Preset { Value = "White" });
        }

        [TestMethod]
        public void AddTest()
        {
            string value = Guid.NewGuid().ToString();
            service.OnAdded += (args) =>
            {
                Assert.AreEqual(args.Entity.Value, value);
            };
            Preset newPreset = new Preset { Value = value };
            Preset addedPreset = service.Add(newPreset);
            Assert.IsNotNull(addedPreset);
            Assert.IsTrue(addedPreset.Id > 0);
            Assert.AreEqual(addedPreset.Value, value);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Preset preset = service.Get(1);
            Assert.IsNotNull(preset);
            Assert.AreEqual(preset.Id, 1);
        }

        [TestMethod]
        public void GetByIdEditTest()
        {
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Id, 1);
            };
            Preset preset = service.Get(1);
            string value = preset.Value;
            preset.Value = Guid.NewGuid().ToString();
            service.OnGot += (args) =>
            {
                Assert.AreEqual(args.Entity.Value, value);
            };
            Preset newPreset = service.Get(1);
            Assert.AreEqual(newPreset.Value, value);
        }

        [TestMethod]
        public void GetByIdNotFoundTest()
        {
            service.OnGot += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            Preset preset = service.Get(int.MaxValue);
            Assert.IsNull(preset);
        }

        [TestMethod]
        public void GetAllTest()
        {
            service.OnAllGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
                Assert.IsTrue(args.Entity.Count > 0);
            };
            List<Preset> presets = service.Get();
            Assert.IsNotNull(presets);
            Assert.IsTrue(presets.Count > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            service.OnAllGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.OnGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            Preset preset = service.Get().First();
            string oldValue = preset.Value;
            service.OnUpdated += (args) =>
            {
                Assert.AreEqual(args.Entity.Value, oldValue);
            };
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
            service.OnUpdated += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            service.Update(new Preset { Id = int.MaxValue });
        }

        [TestMethod]
        public void DeleteTest()
        {
            service.OnAllGot += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            service.OnGot += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            service.OnDeleted += (args) =>
            {
                Assert.IsNotNull(args.Entity);
            };
            Preset preset = service.Get().Last();
            service.Delete(preset.Id);
            Preset deletedPreset = service.Get(preset.Id);
            Assert.IsNull(deletedPreset);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DeleteNotFoundTest()
        {
            service.OnDeleted += (args) =>
            {
                Assert.IsNull(args.Entity);
            };
            service.Delete(int.MaxValue);
        }
    }
}