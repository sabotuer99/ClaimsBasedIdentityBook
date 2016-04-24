Steps to publish the package to Windows Azure (Waz):
	1. Create a project in Waz to host the service
	
	2. Upload localhost.pfx certificate to the Waz project where the application will be hosted
		a. The certificate can be found at:
		   [samples-installation-directory]\Setup\DependencyChecker\certs\localhost.pfx 
		   (password is: xyz)
		   
	3. Modify the web.config file of a-Expense.ClaimsAware application:
		a. Replace the <microsoft.identityModel> section with this one:
		   [!IMPORTANT: Replace {service-url} with the service host url you have chosen in Waz.]
			<microsoft.identityModel>
				<service>     
					<audienceUris>       
						<add value="https://{service-url}.cloudapp.net/" />     
					</audienceUris>     
					<federatedAuthentication>       
						<wsFederation passiveRedirectEnabled="true" 
									  issuer="https://localhost/Adatum.SimulatedIssuer.5/" 
									  realm="https://{service-url}.cloudapp.net/" requireHttps="true" />       
						<cookieHandler requireSsl="true" />     
					</federatedAuthentication>          
					<issuerNameRegistry type="Microsoft.IdentityModel.Tokens.ConfigurationBasedIssuerNameRegistry, Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35">       
						<trustedIssuers>         
							<!--Adatum's identity provider -->         
							<add thumbprint="f260042d59e14817984c6183fbc6bfc71baf5462" name="adatum" />       
						</trustedIssuers>     
					</issuerNameRegistry>     
					<certificateValidation certificateValidationMode="None" />   
				</service> 
			</microsoft.identityModel>
			
	4. Right-click a-expense.cloud project and select 'Publish...'

	5. In azure portal, upload the azure package and the configuration file that were created by the previous step

	6. In azure portal, start the application