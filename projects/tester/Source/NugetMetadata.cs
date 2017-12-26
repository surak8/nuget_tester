
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NSTester {

    [XmlType("metadata")]
    public class NugetMetadata {
        public string id { get; set; }
        public string version { get; set; }
        public string authors { get; set; }
        public string description { get; set; }
        public string projectUrl { get; set; }

        internal void resetValues() {
            iconUrl = null;
            projectUrl = null;
            tags = null;
            licenseUrl = null;
            releaseNotes = null;
            copyright = null;
            owners = null;
            //description = null;
            if (string.IsNullOrEmpty (description))
            description = "test description";
            dependencies.Clear();
            frameworkAssemblies.Clear();

        }

        public string licenseUrl { get; set; }
        public string iconUrl { get; set; }
        public string language { get; set; }
        public string owners { get; set; }
        public string releaseNotes { get; set; }
        public string copyright { get; set; }
        public string tags { get; set; }
        public bool requireLicenseAcceptance { get; set; }

        public List<NugetDependency> dependencies { get; set; }
        public List<NugetFWAssembly> frameworkAssemblies { get; set; }
    }


}