using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz.DTO;
using Quiz.Models;
using Quiz.Repository;
using System.Security.Claims;

namespace Quiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly ExamRepository examRepository;

        public ExamController(ExamRepository examRepository)
        {
            this.examRepository = examRepository;
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var exams = examRepository.GetAll();
            return Ok(exams);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            Exam? exam = examRepository.GetById(id);

            if (exam == null)
            {
                return NotFound();
            }
            return Ok(exam);
        }


        [HttpPost]
        public IActionResult Post([FromBody] ExamDTO examFromRequest)
        {
            
            if (ModelState.IsValid)
            {
                Exam exam = new Exam
                {
                    Title = examFromRequest.Title,
                    Duration = examFromRequest.Duration,
                    Date = examFromRequest.Date,
                    Description = examFromRequest.Description,
                    CreatedBy = examFromRequest.CreatedBy // User.FindFirstValue(ClaimTypes.NameIdentifier)
                };
                examRepository.Add(exam);
                examRepository.Save();
                return CreatedAtAction(nameof(Get), new { id = exam.Id }, exam);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] ExamDTO examFromRequest)
        {
            Exam? exam = examRepository.GetById(id);
            if (exam == null)
            {
                return NotFound();
            }
            exam.Title = examFromRequest.Title;
            exam.Duration = examFromRequest.Duration;
            exam.Date = examFromRequest.Date;
            exam.Description = examFromRequest.Description;
            if (ModelState.IsValid)
            {
                examRepository.Update(id, exam);
                examRepository.Save();
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var exam = examRepository.GetById(id);
            if (exam == null)
            {
                return NotFound();
            }
            examRepository.Remove(id);
            examRepository.Save();
            return NoContent();
        }
    }
}
