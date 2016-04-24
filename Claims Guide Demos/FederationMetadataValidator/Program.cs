using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FederationMetadataValidator
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var file = new FileStream(@"C:\Users\troy\projects\FederatedDemos\6-FederationAndAcs\Code\Adatum.FederationProvider.6\FederationMetadata\2007-06\FederationMetadata.xml", FileMode.Open))
            {
                using (var xmlr = new XmlTextReader(file))
                {
                    new System.IdentityModel.Metadata.MetadataSerializer().ReadMetadata(xmlr);
                }
            }
        }
    }
}
