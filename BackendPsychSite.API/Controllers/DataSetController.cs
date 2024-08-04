using BackendPsychSite.API.DTOs;
using BackendPsychSite.Core.Models;
using BackendPsychSite.Infrastructure.Services;
using BackendPsychSite.UseCases.Interfaces;
using BackendPsychSite.UseCases.Utils;
using Microsoft.AspNetCore.Mvc;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendPsychSite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataSetController : ControllerBase
    {
        private IDataSetService _dsService;
        public DataSetController(IDataSetService service)
        {
            _dsService = service;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(Guid id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] DataSetDto dataSetDto, IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            try
            {
                using (var stream = formFile.OpenReadStream())
                {
                    DataSetArgs args = new DataSetArgs { Name = dataSetDto.Name, ProjectName = dataSetDto.ProjectName, UserEmail = dataSetDto.UserEmail, Path = "None" };
                    await _dsService.CreateAsync(stream, args);
                }
                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
