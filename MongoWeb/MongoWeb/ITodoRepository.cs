﻿using MongoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoWeb
{
    public interface ITodoRepository
    {
        void Add(Products products);
        List<Products> GetAll();
    }
}
