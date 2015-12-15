using MongoDB.Driver;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using NancyDemo.Security;

namespace NancyDemo
{
    public class CustomBootstrapper: DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts"));
        }         
        
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var connString = "mongodb://localhost:27017";
            var databaseName = "NancyDemo";

            var client = new MongoClient(connString);
            var database = client.GetDatabase(databaseName);

            var collection = database.GetCollection<Task>("Tasks");
            var userCollection = database.GetCollection<User>("Users");        

            container.Register<IMongoClient>(client);
            container.Register(database);
            container.Register(collection);
            container.Register(userCollection);          
     
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    DisableRedirect = true,
                    UserMapper = container.Resolve<IUserMapper>()
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "http://locahost:49544");
                ctx.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
            };
        }
    }
}