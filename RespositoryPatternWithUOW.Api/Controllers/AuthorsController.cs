using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.EF.DTOs;
using System.Threading.Tasks;

namespace RespositoryPatternWithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.Authors.GetAll());
        }
        [HttpGet("GetById/{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Authors.GetById(id));
        }

        [HttpGet("GetByIdAsync/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _unitOfWork.Authors.GetByIdAsync(id));
        }

        [HttpPost("AddAuthor")]
        public async Task<IActionResult> AddAuthor(AddAuthorDtos author)
        {
            var newAuthor = new Author()
            {
                Name = author.Name
            };
            _unitOfWork.Authors.Add(newAuthor);
            _unitOfWork.Complete();
            return Ok(newAuthor);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var author = _unitOfWork.Authors.GetById(id);
            _unitOfWork.Authors.Delete(author);
            _unitOfWork.Complete();
            return Ok(author);
        }
    }
}