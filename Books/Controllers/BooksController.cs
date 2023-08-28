using Books.Models;
using Books.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

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
            var books = _dbcontext.Books.Include(m => m.Category).ToList();
            return View(books);
        }

        public ActionResult Detalis(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var book = _dbcontext.Books.Include(c => c.Category).SingleOrDefault(b => b.Id == id);
            if (book == null)
                return HttpNotFound();
            return View(book);
        }

        public ActionResult Create()
        {
            var viewModel = new BookFormViewModel
            {
                Categories = _dbcontext.Categories.Where(m => m.IsActive).ToList()
            };
            return View("BookForm",viewModel);
        }

        public ActionResult Edit(int? id )
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var book = _dbcontext.Books.Find(id);
            if (book == null)
                return HttpNotFound();
            var viewModel = new BookFormViewModel 
            {
                Id = book.Id,
                Title = book.Title,
                Discraption=book.Discraption,
                Author =book.Author,
                CategoryId= book.CategoryId,
                Categories = _dbcontext.Categories.Where(m => m.IsActive).ToList()
            };
            return View("BookForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _dbcontext.Categories.Where(m => m.IsActive).ToList();
                return View("BookForm", model);
            }

            if (model.Id == 0)
            {
                var book = new Book
                {
                    Title = model.Title,
                    Author = model.Author,
                    CategoryId = model.CategoryId,
                    Discraption = model.Discraption
                };
                _dbcontext.Books.Add(book);
            }
            else
            {
                var book = _dbcontext.Books.Find(model.Id);
                if (book == null)
                    return HttpNotFound();

                book.Title = model.Title;
                book.Author = model.Author;
                book.CategoryId = model.CategoryId;
                book.Discraption = model.Discraption;
            }
            _dbcontext.SaveChanges();
            TempData["Massage"] = "Saved Successfully";
            return RedirectToAction("Index");
        }
    }
}