using Nancy.Authentication.Forms;
using System;
using Nancy;
using Nancy.Security;
using MongoDB.Driver;

namespace NancyDemo.Security
{
    public class UserMapper : IUserMapper
    {
        private IMongoCollection<User> _users;

        public UserMapper(IMongoCollection<User> users)
        {
            _users = users;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var user =  _users.Find(x => x.Id == identifier.ToString()).FirstOrDefaultAsync().Result;  
            return new Identity { UserName = user.UserName, UserId = new Guid(user.Id) };
        }
    }
}