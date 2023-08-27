using Books.Models;
using Books.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Books.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDBcontext _dbcontext;

        public BooksController()
        {
            _dbcontext = new ApplicationDBcontext();
        }

        // GET: Books
        public ActionResult Index()
        {
            var books = _dbcontext.Books.Include(m=>m.Category).ToList();
            return View(books);
        }

        public ActionResult Create()
        {
            var viewModel = new BookFormViewModel
            {
                Categories = _dbcontext.Categories.Where(m => m.IsActive).ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _dbcontext.Categories.Where(m => m.IsActive).ToList();
                return View("Create" ,model);
            }
            var book = new Book
            {
                Title =model.Title,
                Author =model.Author,
                CategoryId =model.CategoryId,
                Discraption =model.Discraption
            };
            _dbcontext.Books.Add(book);
            _dbcontext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}