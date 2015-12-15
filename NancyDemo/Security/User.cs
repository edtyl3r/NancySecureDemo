using MongoDB.Bson.Serialization.Attributes;

namespace NancyDemo.Security
{
    public class User
    {
        [BsonId]       
        public string Id { get; set; }       
        public string UserName { get; set; }
        public string Password { get; set; }     

    }
}