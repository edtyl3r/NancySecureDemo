using System;
using System.Collections.Generic;
using Nancy.Security;

namespace NancyDemo.Security
{
    public class Identity: IUserIdentity
    {       
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid UserId { get; set; }

        public IEnumerable<string> Claims
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}