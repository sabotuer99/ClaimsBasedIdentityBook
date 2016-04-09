using System;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace WFE
{
    public class TrustedIssuerNameRegistry : IssuerNameRegistry
    {
        public override string GetIssuerName(SecurityToken securityToken)
        {
            X509SecurityToken x509Token = securityToken as X509SecurityToken;
            if (x509Token != null)
            {
                // Warning: This sample does a simple compare of the Issuer Certificate
                // to a subject name. This is not appropriate for production use. 
                // Check your validation policy and authenticate issuers based off the policy.
                if (String.Equals(x509Token.Certificate.SubjectName.Name, "CN=STS"))
                {
                    return x509Token.Certificate.SubjectName.Name;
                }
            }

            throw new SecurityTokenException("Untrusted issuer.");
        }
    }
}
