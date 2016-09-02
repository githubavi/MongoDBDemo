using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDBDemo.Models;


namespace MongoDBDemo.Controllers
{
    public class MongoDBController : BaseController
    {
        //
        // GET: /MongoDB/

        public ActionResult Index()
        {
            var movies = this.CurrentDB.GetCollection<BsonDocument>("Movies");
            
            return View(movies.AsQueryable<MovieModel>());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(MovieModel moviemodel, FormCollection fc)
        {
            var artists = fc["txtArtists"].Split(",".ToCharArray());
            moviemodel.Artists = artists.Select(p => new Artist { Name = p }).ToList();

            if (moviemodel.Date == null || moviemodel.Date == DateTime.MinValue || string.IsNullOrEmpty(moviemodel.Director) || string.IsNullOrEmpty(moviemodel.Title))
                return View(moviemodel);
            else
            {
                var movies = this.CurrentDB.GetCollection<BsonDocument>("Movies");
                moviemodel.Studio = new MongoDBRef("test", "names", new ObjectId("51c2dc1a566f427635fed1ed"));
                movies.Insert(moviemodel);

                return RedirectToAction("Index");
            }
        }


        //
        // GET: /Movies/Edit
        public ActionResult Edit(string Id)
        {
            var movies = this.CurrentDB.GetCollection("Movies");
            var id = new ObjectId(Id);
            MovieModel moviemodel = movies.AsQueryable<MovieModel>().Where(m => m._id == id).FirstOrDefault();
            if (moviemodel == null)
            {
                return HttpNotFound();
            }
            return View(moviemodel);
        }

        //
        // POST: /Movies/Edit/5

        [HttpPost]
        public ActionResult Edit(MovieModel moviemodel, FormCollection fc)
        {
            var artists = fc["txtArtists"].Split(",".ToCharArray());
            moviemodel.Artists = artists.Select(p => new Artist { Name = p }).ToList();
            moviemodel._id = new ObjectId(moviemodel.ID);

            if (moviemodel.Date == null || moviemodel.Date == DateTime.MinValue || string.IsNullOrEmpty(moviemodel.Director) || string.IsNullOrEmpty(moviemodel.Title))
                return View(moviemodel);
            else
            {
                var movies = this.CurrentDB.GetCollection("Movies");
                var query = movies.AsQueryable<MovieModel>().Where(m => m._id == moviemodel._id);
                movies.Update((query as MongoQueryable<MovieModel>).GetMongoQuery(), Update.Replace<MovieModel>(moviemodel));

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            var movies = this.CurrentDB.GetCollection("Movies");
            var id = new ObjectId(Id);
            MovieModel moviemodel = movies.AsQueryable<MovieModel>().Where(m => m._id == id).FirstOrDefault();
            if (moviemodel == null)
            {
                return HttpNotFound();
            }
            return View(moviemodel);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(MovieModel moviemodel)
        {
            moviemodel._id = new ObjectId(moviemodel.ID);
            var movies = this.CurrentDB.GetCollection<MovieModel>("Movies");
            var mq = movies.AsQueryable().Where(m => m._id == moviemodel._id);
            movies.Remove((mq as MongoQueryable<MovieModel>).GetMongoQuery());

            return RedirectToAction("Index");
        }

        public ActionResult Details(string Id)
        {
            var id = new ObjectId(Id);
            var movies = this.CurrentDB.GetCollection<MovieModel>("Movies");
            var moviemodel = movies.AsQueryable().Where(m => m._id == id).FirstOrDefault();

            var testdata = this.CurrentDB.FetchDBRef(moviemodel.Studio);

            if (moviemodel == null)
            {
                return HttpNotFound();
            }
            return View(moviemodel);
        }

    }
}
