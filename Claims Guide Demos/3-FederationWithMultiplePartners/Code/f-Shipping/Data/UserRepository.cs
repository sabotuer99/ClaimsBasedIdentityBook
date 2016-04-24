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
    using System.Globalization;
    using System.Linq;

    public class UserRepository
    {
        private static readonly ICollection<User> UsersStore = InitializeUserStoreAndAddData();
        private static User billAtContoso;

        private static User johnDoe;

        private static User mary;
        private static User peterPorter;

        private static User rickRico;

        public static User BillAtContoso
        {
            get { return billAtContoso; }
        }

        public static User JohnDoe
        {
            get { return johnDoe; }
        }

        public static User Mary
        {
            get { return mary; }
        }

        public static User PeterPorter
        {
            get { return peterPorter; }
        }

        public static User RickRico
        {
            get { return rickRico; }
        }

        public User GetUser(string userName)
        {
            var user = UsersStore.SingleOrDefault(u => string.Compare(u.UserName, userName, CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase) == 0);
            return user;
        }

        public void UpdatePreferredShippingServiceType(User updatedUser)
        {
            var user = UsersStore.SingleOrDefault(u => string.Compare(u.UserName, updatedUser.UserName, CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase) == 0);
            user.PreferredShippingServiceType = updatedUser.PreferredShippingServiceType;
        }

        private static ICollection<User> InitializeUserStoreAndAddData()
        {
            johnDoe = new User
                          {
                              UserName = "johndoe", 
                              PreferredShippingServiceType = ShipmentServiceType.TwoDays
                          };

            peterPorter = new User
                              {
                                  UserName = "peter", 
                                  PreferredShippingServiceType = ShipmentServiceType.Overnight
                              };

            rickRico = new User
                           {
                               UserName = "rick", 
                               PreferredShippingServiceType = ShipmentServiceType.Overnight
                           };

            billAtContoso = new User
                                {
                                    UserName = "bill@contoso.com", 
                                    PreferredShippingServiceType = ShipmentServiceType.FiveDays
                                };

            mary = new User
                       {
                           UserName = "mary", 
                           PreferredShippingServiceType = ShipmentServiceType.Overnight
                       };

            var userStoreList = new List<User> { JohnDoe, PeterPorter, RickRico, BillAtContoso, Mary };

            return userStoreList;
        }
    }
}