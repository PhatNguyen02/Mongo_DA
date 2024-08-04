using MongoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
namespace MongoWeb.Services
{
    public class GetAll
    {
        public ITodoRepository todoRepository;

        public GetAll(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }
        public List<Products> Excute()
        {
            return todoRepository.GetAll();
        }
    }
}