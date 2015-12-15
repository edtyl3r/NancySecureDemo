using MongoDB.Driver;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Nancy.Security;
using NancyDemo.Security;
using System.Linq;
using System;

namespace NancyDemo
{
    public class AuthModule : NancyModule
    {
        public AuthModule(IMongoCollection<User> users) : base("/api")
        {
            Get["/logout"] = parameters => {
                this.RequiresAuthentication();

                var response = this.LogoutWithoutRedirect();
                return Response.AsText("logout").WithCookie(response.Cookies.First());
            };

            Post["/login"] = parameters => {

                var model = this.Bind<User>();

                var user = ValidateUser(model.UserName, model.Password, users);

                if (user == null)
                {
                    return HttpStatusCode.Unauthorized;
                }

                var authResult = this.LoginAndRedirect(user.UserId);
                return Response.AsJson(new
                {
                    username = user.UserName,
                    userId = user.UserId
                }).WithCookie(authResult.Cookies.First());
            };

        }

        private Identity ValidateUser(string userName, string password, IMongoCollection<User> users)
        {

            var User = users.Find(x => x.UserName == userName).FirstOrDefaultAsync().Result;

            if (User != null && User.Password == password)
            {
                return new Identity { UserName = userName, UserId = new Guid(User.Id) };
            }

            return null;
        }
    }
}