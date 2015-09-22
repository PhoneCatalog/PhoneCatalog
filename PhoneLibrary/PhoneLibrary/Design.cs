using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary
{
    public class Design : BaseClass
    {
        public string CaseDesign { get; set; }
        public bool RemovablePanels { get; set; }
        public bool LED { get; set; }
        public bool StereoSpeakers { get; set; }
        public string CaseMaterial { get; set; }
        public string CaseColor { get; set; }
        public string Protection { get; set; }
        public string QWERTY { get; set; }
        public string SIMcardType { get; set; }
        public bool Projector { get; set; }
        public bool Flashlight { get; set; }
        public bool SOSButton { get; set; }
    }
}
