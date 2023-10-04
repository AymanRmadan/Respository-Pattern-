using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Consts;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.EF.DTOs;

namespace RespositoryPatternWithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetById/{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Books.GetById(id));
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.Books.GetAll());
        }

        [HttpGet("GetByName/{bookName}")]
        public IActionResult GetByName(string bookName)
        {
            return Ok(_unitOfWork.Books.Find(b => b.Title == bookName, new[] { "Author" }));
        }
        [HttpGet("GetAllWithAuthors/{bookName}")]
        public IActionResult GetAllWithAuthors(string bookName)
        {
            return Ok(_unitOfWork.Books.FindAll(b => b.Title.Contains(bookName), new[] { "Author" }));
        }

        [HttpGet("GetOrdered")]
        public IActionResult GetOrdered(string bookName)
        {
            return Ok(_unitOfWork.Books.FindAll(b => b.Title.Contains(bookName), null, null, b => b.Id, OrderBy.Descending));
        }

        [HttpPost("AddOne")]
        public IActionResult AddOne(AddBookDtos book)
        {
            // var book = _unitOfWork.Books.Add(new Book { Title = "Test 4", AuthorId = 1 });
            var newBook = new Book()
            {
                Title = book.Title,
                AuthorId = book.AuthorId
            };
            _unitOfWork.Books.Add(newBook);
            _unitOfWork.Complete();
            return Ok(newBook);
        }
    }
}