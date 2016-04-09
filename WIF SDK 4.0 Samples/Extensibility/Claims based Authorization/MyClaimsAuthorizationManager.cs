//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using Microsoft.IdentityModel.Claims;

namespace ClaimsBasedAuthorization
{
    /// <summary>
    /// Simple ClaimsAuthorizationManager implementation that reads policy information from the .config file
    /// </summary>
    public class MyClaimsAuthorizationManager : ClaimsAuthorizationManager
    {
        static Dictionary<ResourceAction, Func<IClaimsPrincipal, bool>> _policies = new Dictionary<ResourceAction, Func<IClaimsPrincipal, bool>>();
        static PolicyReader _policyReader = new PolicyReader();
        static object _syncObject = new object();
        
        /// <summary>
        /// Creates a new instance of the MyClaimsAuthorizationManager
        /// </summary>
        /// <param name="objXmlElement">XmlElement containing the policy information read from the config file</param>
        public MyClaimsAuthorizationManager( object objXmlElement )
        {
            XmlNodeList nodes = objXmlElement as XmlNodeList;
            LoadPolicies(nodes);
        }

        /// <summary>
        /// Loads the policies from config file
        /// </summary>
        static void LoadPolicies(XmlNodeList nodes)
        {
            Expression<Func<IClaimsPrincipal, bool>> policyExpression;

            foreach( XmlNode node in nodes )
            {
                //
                // Initialize the policy cache
                //
                XmlDictionaryReader rdr = XmlDictionaryReader.CreateDictionaryReader( new XmlTextReader( new StringReader( node.OuterXml ) ) ); 
                rdr.MoveToContent();
                
                string resource = rdr.GetAttribute( "resource" );
                string action = rdr.GetAttribute( "action" );

                Console.WriteLine( "Loading policy for Resource:{0}, Action:{1}", resource, action );

                policyExpression = _policyReader.ReadPolicy( rdr );

                //
                // Compile the policy expression into a function
                //
                Func<IClaimsPrincipal, bool> policy = policyExpression.Compile();

                //
                // Insert the policy function into the policy cache
                //
                _policies[new ResourceAction( resource, action )] = policy;
            }            
        }

        #region ClaimsAuthorizationManager Members

        /// <summary>
        /// Checks if the principal specified in the authorization context is authorized to perform action specified in the authorization context 
        /// on the specified resoure
        /// </summary>
        /// <param name="pec">Authorization context</param>
        /// <returns>true if authorized, false otherwise</returns>
        public override bool CheckAccess( AuthorizationContext pec )
        {
            Console.WriteLine( "Checking access on Resource:{0}, Action:{1} for Principal:{2}",
                pec.Resource.First<Claim>().Value,
                pec.Action.First<Claim>().Value,
                pec.Principal.Identity.Name );

            //
            // Evaluate the policy against the claims of the 
            // principal to determine access
            //
            return _policies[new ResourceAction( pec.Resource.First<Claim>().Value, pec.Action.First<Claim>().Value )]( pec.Principal );
        }

        #endregion
    }
}
