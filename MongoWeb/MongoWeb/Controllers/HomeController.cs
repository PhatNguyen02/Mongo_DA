﻿using MongoWeb.Models;
using MongoWeb.Repositores;
using MongoWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using X.PagedList;
using X.PagedList.Extensions;

namespace MongoWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private AddTodo addTodo;
        private GetAll getAllTodos;

        public HomeController(AddTodo addTodo, GetAll getAllTodos)
        {
            this.addTodo = addTodo;
            this.getAllTodos = getAllTodos;
        }
        public ActionResult Index(int page = 1)
        {

            int pageSize = 8;

            var allProducts = getAllTodos.Excute();
            var categories = getAllTodos.GetProductCategories();

            // Phân trang danh sách sản phẩm
            var pagedProducts = allProducts
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalItems = allProducts.Count();

            ViewBag.Categories = categories;
            ViewBag.TotalPages = Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;

            return View(pagedProducts);

        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Products todo)
        {
            if (ModelState.IsValid)
            {
                addTodo.Excute(todo);
                return RedirectToAction("Index");
            }
            return View(todo);
        }
        [HttpGet]
        public ActionResult Details(string id)
        {
            var product = getAllTodos.GetById(id); // Sử dụng phương thức GetById để lấy thông tin chi tiết sản phẩm
            if (product == null)
            {
                return HttpNotFound(); // Trả về lỗi 404 nếu không tìm thấy sản phẩm
            }
            return View(product); // Trả về view hiển thị chi tiết sản phẩm
        }
        [HttpGet]
        public ActionResult Search(string query, int page = 1)
        {
            int pageSize = 8;

            // Sử dụng TodoRepository để tìm kiếm
            var searchResults = getAllTodos.Search(query); // Sử dụng SearchProducts thay vì Search

            // Phân trang kết quả tìm kiếm
            var pagedResults = searchResults
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalItems = searchResults.Count();

            // Cung cấp dữ liệu cho view
            ViewBag.Query = query;
            ViewBag.TotalPages = Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;

            // Trả về view Index với kết quả tìm kiếm
            return View("Index", pagedResults);
        }

    }
}