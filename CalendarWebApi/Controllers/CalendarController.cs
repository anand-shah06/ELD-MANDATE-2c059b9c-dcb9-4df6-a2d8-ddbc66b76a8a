using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalendarWebApi.DataAccess;
using CalendarWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CalendarWebApi.DTO;

namespace CalendarWebApi.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IRepository repository; 
        public CalendarController(IRepository repositoryParam)
        {
            repository = repositoryParam;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Calendar>>> Get()
        {
            List<Calendar> calendars = await repository.GetCalendar();
            return StatusCode(200, calendars);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Calendar calendarParam)
        {
            var calendar =  await repository.AddEvent(calendarParam);
            return StatusCode(201, calendar);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            List<Calendar> calendars = await repository.GetCalendar();
            var doCalendarExist = calendars.Exists(c => c.Id == id);
            if (doCalendarExist)
            {
                var calendar = await repository.DeleteEvent(calendars[calendars.FindIndex(c => c.Id == id)]);
                return StatusCode(200);
            }
            else
            {
                return StatusCode(404);
            }
            
        }
    }
}
