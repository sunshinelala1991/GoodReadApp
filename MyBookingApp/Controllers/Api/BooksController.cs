using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MyBookingApp.Models;
using AutoMapper;

namespace MyBookingApp.Controllers.Api
{
    public class BooksController : ApiController
    {
        private MyBookingAppContext db = new MyBookingAppContext();

        // GET: api/Books  
        //[httpget] is just for reference, reminding myself how the annotation is used
        [HttpGet]
        public IEnumerable<BookDto> GetBooks()
        {
            return db.Books.Include(b => b.Author).Select(Mapper.Map<Book,BookDto>);
        }

        // GET: api/Books/5
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> GetBook(int id)
        {
            Book book = await db.Books.Include(b => b.Author).SingleOrDefaultAsync(b=>b.Id==id);
            if (book == null)
            {
                return NotFound();
            }

            BookDetailDto bd = Mapper.Map<Book, BookDetailDto>(book);

            return Ok(bd);
        }

        // PUT: api/Books/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBook(int id, BookDetailDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookDto.Id)
            {
                return BadRequest();
            }



            if (bookDto.AuthorId == 0)
            {
                Author newAu = new Author
                {
                    Name = bookDto.AuthorName
                };
                IHttpActionResult result = await new AuthorsController().PostAuthor(newAu);
                bookDto.AuthorId = newAu.Id;
            }

            Book book = Mapper.Map<BookDetailDto, Book>(bookDto);
            book.Id = bookDto.Id;
            db.Entry(book).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Books
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> PostBook(BookDetailDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (bookDto.AuthorId == 0)
            {
                Author newAu = new Author
                {
                    Name = bookDto.AuthorName
                };
                IHttpActionResult result=await new AuthorsController().PostAuthor(newAu);
                bookDto.AuthorId = newAu.Id;
            }

            Book book= Mapper.Map<BookDetailDto, Book>(bookDto);
            db.Books.Add(book);
            await db.SaveChangesAsync();
            db.Entry(book).Reference(x => x.Author).Load();
            BookDto bd = Mapper.Map<Book, BookDto>(book);


            return CreatedAtRoute("DefaultApi", new { id = book.Id },bd);
        }

        // DELETE: api/Books/5
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> DeleteBook(int id)
        {
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            await db.SaveChangesAsync();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.Id == id) > 0;
        }
    }
}