using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.ViewModel
{
    public class UserAccount
    {
        public string UserName { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public UserAccount(string userName, string firstName, string lastName)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}