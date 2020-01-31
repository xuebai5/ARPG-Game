using System;
using System.Threading;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Proto.MongoDB;
using Xunit;

namespace ServerUnitTest
{
    public class MongTest
    {

        static MongTest()
        {
            BsonClassMap.RegisterClassMap<PlayInfoEntity>(
           (cm) =>
           {
               cm.AutoMap();
               cm.MapIdMember(c => c.Uuid).SetIdGenerator(StringObjectIdGenerator.Instance);

           });
        }

        private readonly string url = @"mongodb+srv://dbuser:54249636@cluster0-us8pa.gcp.mongodb.net/test?retryWrites=true&w=majority";

        [Fact]
        public  void TestAsync()
        {
            
        }


        [Theory]
        [InlineData("admin","123456")]
        [InlineData("test", "123456")]
        public void InsertEntity(string user, string passw)
        {
            var client = new MongoClient(url);
            var db = client.GetDatabase("Game");
            var users = db.GetCollection<PlayInfoEntity>("userinfo");
            var entity = new PlayInfoEntity
            {
                CreateDateTime = DateTime.Now.Ticks,
                LastLoginDateTime = DateTime.Now.Ticks,
                LoginCount = 0,
                Password = ServerUtility.Md5Tool.GetMd5Hash(passw),
                ServerId = 1,
                Username = user
            };
            users.InsertOne(entity);
            Assert.NotEmpty(entity.Uuid);
        }
    }
}