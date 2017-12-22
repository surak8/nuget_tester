using System.Xml.Serialization;

namespace NSTester {
    [XmlType("frameworkAssembly")]
    public class NugetFWAssembly {
        [XmlAttribute]
        public string assemblyName { get; set; }
        [XmlAttribute]
        public string targetFramework { get; set; }
    }
}