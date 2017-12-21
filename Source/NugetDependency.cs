using System.Xml.Serialization;

[XmlType("dependency")]
public class NugetDependency {

    [XmlAttribute]
    public string id { get; set; }

    [XmlAttribute]
    public string version { get; set; }
}