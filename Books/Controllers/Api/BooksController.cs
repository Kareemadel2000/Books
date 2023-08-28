using Books.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Books.Controllers.Api
{
    public class BooksController : ApiController
    {
        private readonly ApplicationDBcontext _dbcontext;

        public BooksController()
        {
            _dbcontext = new ApplicationDBcontext();
        }

        [HttpDelete]
        public IHttpActionResult DeleteBook(int id)
        {
            var book = _dbcontext.Books.Find(id);
            if (book == null)
                return NotFound();
            _dbcontext.Books.Remove(book);
            _dbcontext.SaveChanges();   
            return Ok();
        }
    }
}
