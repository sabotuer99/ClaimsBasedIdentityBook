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
            using (var stream = File.OpenRead(this.organizationsFilePath))
            {
                return xml.Deserialize(stream) as List<Organization>;
            }
        }

        public void AddOrganization(Organization organization)
        {
            var organizations = this.GetOrganizations();
            organizations.Add(organization);
            XmlSerializer xml = new XmlSerializer(organizations.GetType());
            using (var stream = File.OpenWrite(this.organizationsFilePath))
            {
                xml.Serialize(stream, organizations);
            }
        }

        private readonly string organizationsFilePath;

        public OrganizationRepository()
        {
            var appPath = HttpContext.Current.Server.MapPath("~/");
            this.organizationsFilePath = appPath + "..\\SharedData\\organizations.txt";
        }

        public bool IsNameAvailable(string organizationName)
        {
            var organization = this.GetOrganization(organizationName);
            return organization == null;
        }
    }
}