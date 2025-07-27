using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;
using TrilhaApiDesafio.Dto;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("schedule")]
    public class ScheduleController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public ScheduleController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public IActionResult Search(int? id, string? title, DateTime? deadline, string? status)
        {
            var query = _context.Schedules.AsQueryable();

            if (id.HasValue)
                query = query.Where(c => c.Id == id.Value);

            if (!string.IsNullOrEmpty(title))
                query = query.Where(c => c.Title.Contains(title)); // ou == se quiser exato

            if (deadline.HasValue)
            {
                var date = deadline.Value.Date;
                query = query.Where(c => c.Deadline.Date == date);
            }


            if (!string.IsNullOrEmpty(status) &&
                Enum.TryParse<EnumScheduleStatus>(status, true, out var statusEnum))
            {
                query = query.Where(c => c.Status == statusEnum);
            }

            var resultado = query.ToList();

            if (!resultado.Any())
                return NotFound("Nenhuma tarefa encontrada com os critérios informados.");

            return Ok(resultado);
        }

        [HttpGet("all")]
        public IActionResult ObterTodos()
        {
            var schedulesList = _context.Schedules.ToList();
            if (schedulesList == null) return NotFound();
            return Ok(schedulesList);
        }

        [HttpPost]
        public IActionResult Criar(CreateSchedule newSchedule)
        {
            if (string.IsNullOrEmpty(newSchedule.Title))
                return BadRequest("Insira um titulo válido");

            if (string.IsNullOrEmpty(newSchedule.Description))
                return BadRequest("Insira uma descrição válida");

            if (newSchedule.Deadline == DateTime.MinValue)
                return BadRequest("Insira uma data válida");

            if (string.IsNullOrEmpty(newSchedule.Status) ||
                !Enum.TryParse<EnumScheduleStatus>(newSchedule.Status, true, out var statusEnum))
            {
                return BadRequest("Insira um status válido");
            }

            var schedule = new Schedule {
                Title = newSchedule.Title,
                Description = newSchedule.Description,
                Status = statusEnum,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Deadline = newSchedule.Deadline
            };

            _context.Schedules.Add(schedule);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Search), new { id = schedule.Id }, schedule);
        }

        [HttpPatch]
        public IActionResult Atualizar(int id, UpdateSchedule updateSchedule)
        {
            var scheduleInBank = _context.Schedules.Find(id);

            if (scheduleInBank == null)
                return NotFound();            

            scheduleInBank.Title = updateSchedule.Title ?? scheduleInBank.Title;
            scheduleInBank.Description = updateSchedule.Description ?? scheduleInBank.Description;
            scheduleInBank.UpdatedAt = DateTime.Now;
            scheduleInBank.Deadline = updateSchedule.Deadline ?? scheduleInBank.Deadline;   

            if (Enum.TryParse<EnumScheduleStatus>(updateSchedule.Status, true, out var statusEnum))
            {
                scheduleInBank.Status = statusEnum;
            }

            _context.SaveChanges();
            return Ok(scheduleInBank);
        }

        [HttpDelete]
        public IActionResult Deletar(int id)
        {
            var scheduleInBank = _context.Schedules.Find(id);

            if (scheduleInBank == null)
                return NotFound();

            _context.Schedules.Remove(scheduleInBank);

            _context.SaveChanges();
            return NoContent();
        }
    }
}
