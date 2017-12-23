using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NSTester {
    [XmlRoot("package")]
    [XmlType("package")]
    public class NugetPackage {
        public NugetMetadata metadata { get; set; }
        public List<NugetFile> files { get; set; }

        public void resetValues() {
            metadata.resetValues();
            // https://docs.microsoft.com/en-us/nuget/guides/create-net-standard-packages-vs2015
            files.Add(new NugetFile("bin\\debug\\test.dll", "lib\\net4.5\\test.dll"));
            files.Add(new NugetFile("bin\\debug\\test.pdb", "lib\\net4.5\\test.pdb"));
        }
    }


}