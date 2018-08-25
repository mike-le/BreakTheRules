using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTR.DataAccess;
using BTR.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTR.Controllers
{
    [Produces("application/json")]
    [Route("Status")]
    public class StatusController : Controller
    {
        private BTRContext _context;
        private UNOSUserPrincipalService _userPrincipal;

        /// <summary>
        /// Constructs the comments controller with the BTR database context
        /// </summary>
        /// <param name="context">A <see cref="BTRContext"/></param>
        public StatusController(BTRContext context, UNOSUserPrincipalService principalService)
        {
            _context = context;
            _userPrincipal = principalService;
        }

        /// <summary>
        /// Retrieve a status by an Idea's postId
        /// </summary>
        /// <param name="id">An Idea's id used to query the current status/></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetStatusesByIdAsync(int id)
        {
            if (!(_context.Statuses.Any(t => t.IdeaId == id)))
                return NotFound();

            var statusQuery = _context.Statuses
                .OrderByDescending(a => a.SubmitDt)
                .Where(a => a.IdeaId == id);

            List<Status> status = await statusQuery.ToListAsync<Status>();

            return Ok(status);
        }

        /// <summary>
        /// An <see cref="Status"/> is updated with a new response value and status code
        /// </summary>
        /// <param name="id">An id for the <see cref="IdeaEntity"/> that is linked to the status. </param>
        /// <param name="patch">An patch for the <see cref="Status"/> entity that contains new values for response and status code. </param>
        [HttpPost("{id}")]
        public async Task<ActionResult> PostStatus([FromBody]Status response)
        {
            if (!(_userPrincipal.IsAppAdmin))
                return Forbid();

            response.SubmitDt = DateTime.Now;

            await _context.Statuses.AddAsync(response);
            await _context.SaveChangesAsync(true, _userPrincipal.SAMName);

            // remove notifications when status has been acknowledged
            if (_context.Notifications.Any(n => n.Status.Idea.PostId == response.IdeaId))
            {
                var targetNotification = _context.Notifications.Where(n => n.Status.Idea.PostId == response.IdeaId).SingleOrDefault();
                _context.Notifications.Remove(targetNotification);
                await _context.SaveChangesAsync(true, _userPrincipal.SAMName);
            }

            return Ok(response);
        }
    }
}
