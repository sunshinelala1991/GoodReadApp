using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBookingApp.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
namespace MyBookingApp.Controllers
{
    public class BooksController : Controller
    {

        private MyBookingAppContext _context;

        public BooksController()
        {

            _context = new MyBookingAppContext();

        }
        // GET: Books
        public ActionResult Index()
        {
            return View();
        }

        // GET: Books
        [HttpGet]
        public ActionResult Detail(int id)
        {

            Book book = _context.Books.Include(b=>b.Author).SingleOrDefault(b => b.Id == id);

            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            var book=_context.Books.Include(b => b.Author).SingleOrDefault(b => b.Id == id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);

        }

    }
}