//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace FShipping.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml.Serialization;

    public class OrganizationRepository
    {
        public Organization GetOrganization(string organizationName)
        {
            var upperOrganizationName = organizationName.ToUpperInvariant();

            return (from o in this.GetOrganizations()
                    where o.Name.ToUpperInvariant() == upperOrganizationName
                    select o)
                .FirstOrDefault();
        }

        public IList<Organization> GetOrganizations()
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Organization>));
            var organizationsFilePath = HttpContext.Current.Server.MapPath("~/");
            organizationsFilePath = organizationsFilePath + "..\\SharedData\\organizations.txt";
            using (var stream = File.OpenRead(organizationsFilePath))
            {
                return xml.Deserialize(stream) as List<Organization>;
            }
        }
    }
}