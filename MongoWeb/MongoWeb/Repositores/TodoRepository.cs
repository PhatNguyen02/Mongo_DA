using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoWeb.Models;
using MongoDB.Bson;

namespace MongoWeb.Repositores
{
    public class TodoRepository : ITodoRepository
    {
        public readonly IMongoCollection<Products> collection;
        public readonly IMongoCollection<Users> collectionUser;

        public TodoRepository(IMongoCollection<Products> database, IMongoCollection<Users> userCollection)
        {
            collection = database;
            collectionUser = userCollection;
        }
        public void Add(Products products)
        {
            collection.InsertOne(products);
        }
        public List<Products> GetAll()
        {
            return collection.Find(_ => true).ToList();
        }
        // User Methods
        public void AddUser(Users user)
        {
            collectionUser.InsertOne(user);
        }

        public List<Users> GetAllUsers()
        {
            return collectionUser.Find(_ => true).ToList();
        }

        public Users GetUserById(string id)
        {
            return collectionUser.Find(user => user.Id == new ObjectId(id)).FirstOrDefault();
        }

        public void UpdateUser(Users user)
        {
            collectionUser.ReplaceOne(u => u.Id == user.Id, user);
        }

        public void DeleteUser(string id)
        {
            collectionUser.DeleteOne(user => user.Id == new ObjectId(id));
        }
    }
}