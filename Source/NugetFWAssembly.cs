using System.Xml.Serialization;

[XmlType("frameworkAssembly")]
public class NugetFWAssembly {
    [XmlAttribute]
    public string assemblyName { get; set; }
    [XmlAttribute]
    public string targetFramework { get; set; }
}
