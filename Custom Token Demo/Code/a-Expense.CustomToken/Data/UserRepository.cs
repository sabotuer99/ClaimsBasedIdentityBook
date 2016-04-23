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
    using Samples.Web.ClaimsUtilities;

    public class UserRepository
    {
        private static readonly ICollection<User> UsersStore = InitializeUserStoreAndAddData();

        private static Role employeeRole;

        private static Role accountantRole;

        private static User johnDoe;

        private static User maryMay;

        public static Role EmployeeRole
        {
            get
            {
                return employeeRole;
            }
        }

        public static Role AccountantRole
        {
            get
            {
                return accountantRole;
            }
        }

        public static User JohnDoe
        {
            get
            {
                return johnDoe;
            }
        }

        public static User MaryMay
        {
            get
            {
                return maryMay;
            }
        }

        public void SaveUser(User user)
        {
            UsersStore.Add(user);
        }

        public User GetUser(string userName)
        {
            var user = UsersStore.SingleOrDefault(u => u.FederatedUserName == userName);
            return user;
        }

        public void UpdateUserPreferredReimbursementMethod(User updatedUser)
        {
            var user = UsersStore.SingleOrDefault(u => u.UserName == updatedUser.UserName);
            user.PreferredReimbursementMethod = updatedUser.PreferredReimbursementMethod;
        }
      
        private static ICollection<User> InitializeUserStoreAndAddData()
        {
            johnDoe = new User
            {
                Id = new Guid("{3824B8C2-B263-4fb9-A6AE-9284012C1004}"),
                UserName = "johndoe",
                FederatedUserName = @"adatum\johndoe",
                PreferredReimbursementMethod = ReimbursementMethod.Check
            };

            maryMay = new User
            {
                Id = new Guid("{9B9175AE-1A39-43ce-8D63-B9B1BDC8E2FE}"),
                UserName = "mary",
                FederatedUserName = @"adatum\mary",
                PreferredReimbursementMethod = ReimbursementMethod.Cash
            };

            var peter = new User
            {
                Id = new Guid("{372A59F1-1FC2-4404-882C-3B8EF2B14C27}"),
                UserName = "peter",
                FederatedUserName = @"adatum\peter",
                PreferredReimbursementMethod = ReimbursementMethod.Cash
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

            peter.Roles.Add(EmployeeRole);
            userStoreList.Add(peter);

            return userStoreList;
        }
    }
}

