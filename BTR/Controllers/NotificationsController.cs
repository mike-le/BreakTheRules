using BTR.DataAccess;
using BTR.DataAccess.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTR.Controllers
{
    [Produces("application/json")]
    [Route("Notifications")]
    public class NotificationsController : Controller
    {
        private BTRContext _context;
        public NotificationsController(BTRContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetNotifications()
        {
            //if (!(_userprincipal.IsAppAdmin || _userprincipal.IsExecutive))
                return Ok(); // executives and admins are the only users with notifications 
            //if (_userprincipal.IsExecutive)
            //    return Ok(_context.Notifications.Where(n => n.IsExec).Include(n => n.Status).ThenInclude(s => s.Idea).ToList());
            //else
            //    return Ok(_context.Notifications.Where(n => !n.IsExec).Include(n => n.Status).ThenInclude(s => s.Idea).ToList());
        }
        [HttpGet("{id}")]
        public ActionResult GetNotificationsById(int id)
        {
            //if (!(_userprincipal.IsAppAdmin || _userprincipal.IsExecutive))
                return Ok(); // executives and admins are the only users with notifications 
            //return Ok(_context.Notifications.Where(n => n.Id == id).Include(n => n.Status).ThenInclude(s => s.Idea).ToList());

        }
    }
}