using BookStack.Models;
using BookStack.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStack.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var books = context.Books.OrderByDescending(b => b.Id).ToList();
            return View(books);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookDto bookDto)
        {
            if (bookDto.ImageFile == null) {
                ModelState.AddModelError("ImageFile", "This Image file is required");
            }
            if (!ModelState.IsValid) {
                return View(bookDto);
            }

            // save image
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(bookDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/images/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath)) {
                bookDto.ImageFile.CopyTo(stream);
            }

            // save the new book in the database
            Book book = new Book() { 
                Title = bookDto.Title,
                Category = bookDto.Category,
                Price = bookDto.Price,
                Description = bookDto.Description,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
                
            };

            context.Books.Add(book);
            context.SaveChanges();

                return RedirectToAction("Index", "Books");
        }

        public IActionResult Edit(int id) {
            var book = context.Books.Find(id);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

            var bookDto = new BookDto()
            {
                Category = book.Category,
                Price = book.Price,
                Description = book.Description,
                Title = book.Title,
            };

            ViewData["Id"] = book.Id;
            ViewData["ImageFileName"] = book.ImageFileName;
            ViewData["CreatedAt"] = book.CreatedAt.ToString("MM/dd/yyyy");

            return View(bookDto);

            
        }

        [HttpPost]
        public IActionResult Edit(int id, BookDto bookDto)
        {
            var book = context.Books.Find(id);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Id"] = book.Id;
                ViewData["ImageFileName"] = book.ImageFileName;
                ViewData["CreatedAt"] = book.CreatedAt.ToString("MM/dd/yyyy");
                return View(bookDto);
            }

            string newFileName = book.ImageFileName;
            if (bookDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(bookDto.ImageFile.FileName);

                string imageFullPath = Path.Combine(environment.WebRootPath, "images", newFileName);
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    bookDto.ImageFile.CopyTo(stream);
                }

                string oldImageFullPath = Path.Combine(environment.WebRootPath, "images", book.ImageFileName);
                if (System.IO.File.Exists(oldImageFullPath))
                {
                    System.IO.File.Delete(oldImageFullPath);
                }
            }

            book.Title = bookDto.Title;
            book.Price = bookDto.Price;
            book.Category = bookDto.Category;
            book.ImageFileName = newFileName;
            book.Description = bookDto.Description;

            context.SaveChanges();

            return RedirectToAction("Index", "Books");
        }

        public IActionResult Delete(int id) {

            var book = context.Books.Find(id);
            if (book == null){
                return RedirectToAction("Index", "Books");
            }

            string imageFullPath = environment.WebRootPath + "/images/" + book.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Books.Remove(book);
            context.SaveChanges(true);


            return RedirectToAction("Index","Books");
        }
    }
}
