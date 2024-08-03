using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoWeb.Models;

namespace MongoWeb.Repositores
{
    public class TodoRepository : ITodoRepository
    {
        public readonly IMongoCollection<Products> collection;

        public TodoRepository(IMongoCollection<Products> database)
        {
            collection = database;
        }
        public void Add(Products products)
        {
            collection.InsertOne(products);
        }
        public List<Products> GetAll()
        {
            return collection.Find(_ => true).ToList();
        }
    }
}