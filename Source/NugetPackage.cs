using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("package")]
[XmlType("package")]
public class NugetPackage {
    public NugetMetadata metadata { get; set; }
    public List<NugetFile> files { get; set; }
}