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
        [HttpGet("{id}")]   // Finally it was checked and tested.
        public async Task<ActionResult> Get()
        {
            var calendars = await repository.GetCalendar();
            return StatusCode(200, calendars);
        }

        // GET api/values/5
        [HttpGet]
        public async Task<ActionResult> Get(int id)
        {           
            EventQueryModel eventQueryModel = new EventQueryModel();
            eventQueryModel.Id = id;
            List<Calendar> calendars = await repository.GetCalendar(eventQueryModel);
            return StatusCode(200, calendars.ToArray());
        }
                
        [HttpGet]
        [Route("query")]
        public async Task<ActionResult> Get(string location)
        {         
            EventQueryModel eventQueryModel = new EventQueryModel();
            eventQueryModel.Location = location;
          
            var calendar = await repository.GetCalendar(eventQueryModel);
            return StatusCode(200, calendar.ToArray());
          
        }

        [HttpGet]
        [Route("sort")]
        public async Task<ActionResult> Sort()
        {
            var calendars = await repository.GetEventsSorted();
            return StatusCode(200, calendars);
        }

        // PUT api/values/5
        [HttpPost]
        public async Task<ActionResult> Post(Calendar calendar)
        {
            
            var calendarReturn = await repository.AddEvent(calendar);
            return StatusCode(201, calendarReturn);
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Calendar calendar)
        {
            //var calendar = new Calendar {Name = "ChangedEvent3", Location  = "Alaska" };
            var calendarReturn = await repository.UpdateEvent(calendar);
            return StatusCode(204, calendarReturn);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool doCalendarExist = false;
            List<Calendar> calendars = await repository.GetCalendar();
            if(calendars != null)
            {
                doCalendarExist = calendars.Exists(c => c.Id == id);
            }            
            if (doCalendarExist)
            {
                var calendar = await repository.DeleteEvent(calendars[calendars.FindIndex(c => c.Id == id)]);
                return StatusCode(204);
            }
            else
            {
                return StatusCode(404);
            }            
        }
    }
}
