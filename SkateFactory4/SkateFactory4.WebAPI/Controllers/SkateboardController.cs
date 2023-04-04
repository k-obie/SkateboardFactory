using SkateFactory4.Data;
using SkateFactory4.Models;
using SkateFactory4.Models.Enums;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SkateFactory4.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkateboardController : ControllerBase
    {
        private readonly SkateFactoryContext _context;

        public SkateboardController(SkateFactoryContext context)
        {
            _context = context;
        }

        // GET: api/Skateboards
        [HttpGet]
        public ActionResult<IEnumerable<Skateboard>> GetList(ESkateboardCriteria criteria)
        {
            if (criteria == ESkateboardCriteria.All)
                return _context.Skateboards.ToList();
            else if (criteria == ESkateboardCriteria.Electric)
                return _context.Skateboards.Where(x => x.SkateType == 1).ToList();
            else
                return _context.Skateboards.Where(x => x.SkateType == 2).ToList();
        }

        // GET: api/Skateboards/5
        [HttpGet("{id}")]
        public ActionResult<Skateboard?> SearchById(int id)
        {
            var result = _context.Skateboards.Find(id);
            return result;
        }

        [HttpPut]
        public IActionResult Update(Skateboard skateboard)
        {
            _context.Update(skateboard);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult Insert(Skateboard skateboard)
        {
            _context.Skateboards.Add(skateboard);
            _context.SaveChanges();
            return Ok();
        }

        // DELETE: api/Skateboards/5
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var skateboard = new Skateboard() { Id = id };
            _context.Skateboards.Remove(skateboard);
            _context.SaveChanges();
            return Ok();
        }
    }
}
