using MongoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoWeb.Services
{
    public class AddTodo
    {
        private ITodoRepository todoRepository;

        public AddTodo(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }
        public void Excute(Products todo)
        {
            todoRepository.Add(todo);
        }
    }
}