using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TopLearn.DataLayer.Context;

namespace TopLearn.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseApiController : ControllerBase
    {
        TopLearn.DataLayer.Context.TopLearnContext _context;


        public CourseApiController(TopLearnContext context)
        {
            _context = context;
        }


        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();

                var courseTitles = _context.Course.Where(c=>c.CourseTitle.Contains(term))
                    .Select(c=>c.CourseTitle).ToList();

                return Ok(courseTitles);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
