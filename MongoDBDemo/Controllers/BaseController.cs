using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;

namespace MongoDBDemo.Controllers
{
    public class BaseController : Controller
    {
        MongoDatabase movieDB;
        public BaseController()
        {
            MongoClient c = new MongoClient(new MongoClientSettings
            {
                //Credentials = new[] { MongoCredential.CreateMongoCRCredential("admin", "admin", "admin") },
                Server = new MongoServerAddress("localhost", 27019)
            });
            MongoServer server = c.GetServer();
            movieDB = server.GetDatabase("MovieDB");
        }

        protected MongoDatabase CurrentDB
        {
            get { return movieDB; }
        }

    }
}
