using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBDemo.Models
{ 
    public class MovieModel
    {
        [BsonId]
        public ObjectId _id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Director { get; set; }
        
        [BsonDateTimeOptions(DateOnly=true,Kind = DateTimeKind.Local, Representation = BsonType.DateTime)]
        [Required]
        public DateTime Date { get; set; }

        public List<Artist> Artists { get; set; }

        private string id;
        [BsonIgnore]
        public string ID { get { return id; } set { id = value; } }

        public MongoDBRef Studio { get; set; }
    }

    public class Artist
    {
        public string Name { get; set; }
    }
}
