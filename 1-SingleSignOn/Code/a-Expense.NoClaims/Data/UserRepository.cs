//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AExpense.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Security;
    using Samples.Web.ClaimsUtilities;

    public class UserRepository
    {
        private const int SaltSize = 5;
        private static readonly ICollection<User> UsersStore = InitializeUserStoreAndAddData();

        private static Role accountantRole;
        private static Role employeeRole;

        private static User johnDoe;

        private static User maryMay;

        public static Role AccountantRole
        {
            get { return accountantRole; }
        }

        public static Role EmployeeRole
        {
            get { return employeeRole; }
        }

        public static User JohnDoe
        {
            get { return johnDoe; }
        }

        public static User MaryMay
        {
            get { return maryMay; }
        }

        public User GetUser(string userName)
        {
            return UsersStore.SingleOrDefault(u => u.UserName == userName);
        }

        public void SaveUser(User user)
        {
            UsersStore.Add(user);
        }

        public void UpdateUserPreferredReimbursementMethod(User updatedUser)
        {
            var user = UsersStore.SingleOrDefault(u => u.UserName == updatedUser.UserName);
            user.PreferredReimbursementMethod = updatedUser.PreferredReimbursementMethod;
        }

        public bool ValidateUser(string userName, string password)
        {
            var user = this.GetUser(userName);

            if (user == null)
            {
                return false;
            }

            var salt = user.GetSalt(SaltSize);
            var hashedAndSaltedPassword = CreatePasswordHash(password, salt);

            return string.Compare(hashedAndSaltedPassword, user.PasswordHashAndSalt, StringComparison.Ordinal) == 0;
        }

        private static string CreatePasswordHash(string password, string salt)
        {
            var saltAndPassword = String.Concat(password, salt);
            var hashedPassword =
                FormsAuthentication.HashPasswordForStoringInConfigFile(
                    saltAndPassword, "SHA1");
            hashedPassword = String.Concat(hashedPassword, salt);

            return hashedPassword;
        }

        private static ICollection<User> InitializeUserStoreAndAddData()
        {
            johnDoe = new User
                          {
                              Id = new Guid("{3824B8C2-B263-4fb9-A6AE-9284012C1004}"), 
                              UserName = "johndoe", 
                              FullName = "John Doe", 
                              CostCenter = Adatum.CostCenters.CustomerService, 
                              PreferredReimbursementMethod = ReimbursementMethod.Check, 
                              PasswordHashAndSalt = "79373A35560863479ADBFC5725F9D52EA7CC88A5NOtw="
                          };

            maryMay = new User
                          {
                              Id = new Guid("{9B9175AE-1A39-43ce-8D63-B9B1BDC8E2FE}"), 
                              UserName = "mary", 
                              FullName = "Mary May", 
                              CostCenter = Adatum.CostCenters.Accountancy, 
                              PreferredReimbursementMethod = ReimbursementMethod.Cash, 
                              PasswordHashAndSalt = "1A6997BA0F23035644E9B37E92F925034D75E2E9hL3w="
                          };

            employeeRole = new Role
                               {
                                   Id = new Guid("{9A7254DD-864A-4784-9F80-A9D5EB647741}"), 
                                   Name = Adatum.Roles.Employee
                               };

            accountantRole = new Role
                                 {
                                     Id = new Guid("{49A1E6FC-A7C2-434e-BE1E-00AFD3F25A7C}"), 
                                     Name = Adatum.Roles.Accountant
                                 };

            var userStoreList = new List<User>();
            JohnDoe.Roles.Add(EmployeeRole);
            userStoreList.Add(JohnDoe);

            MaryMay.Roles.Add(EmployeeRole);
            MaryMay.Roles.Add(AccountantRole);
            userStoreList.Add(MaryMay);

            return userStoreList;
        }
    }
}