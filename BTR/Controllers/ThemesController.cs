using BTR.DataAccess;
using BTR.DataAccess.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;


namespace BTR.Controllers
{
    [Produces("application/json")]
    [Route("Themes")]
    public class ThemesController : Controller
    {
        private BTRContext _context;
        //private UserPrincipalService _userPrincipal;
        

        public ThemesController(BTRContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Sends reminder for users to post an idea on this topic. 
        /// </summary>
        /// <param name="id">Id of theme</param>
        /// <returns></returns>
        [HttpPost("send/{id}")]
        public ActionResult Send(int id)
        {
            ThemeEntity theme = (from t in _context.Themes
                                 where t.ThemeId == id
                                 select t).First();
            if (theme == null || theme.CloseDt < DateTime.Now)
                return BadRequest();

            _context.SendEmail(theme, true);
            return Ok();
        }

        /// <summary>
        /// Retrieve all themes
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<ThemeEntity>> GetThemesAsync()
        { 
            return await _context.Themes
                    .ToListAsync();
        }

        /// <summary>
        /// Retrieve a Theme by Id. 
        /// </summary>
        /// <param name="id">A Theme id used to query all ideas for that theme/></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetThemeByIdAsync(int id) 
        {

            if (!(_context.Themes.Any(t => t.ThemeId == id)))
                return NotFound();
            var themeQuery = _context.Themes
                .Where(a => a.ThemeId == id)
                .Include(a => a.Ideas)
                .Include(a => a.Ideas)
                    .ThenInclude(i => i.Votes)
                .Include(a => a.Ideas)
                    .ThenInclude(i => i.Comments)
                        .ThenInclude(i => i.Votes)
                .Include(a => a.Ideas)
                    .ThenInclude(i => i.Comments)
                        .ThenInclude(c => c.Comments)
                            .ThenInclude(i => i.Votes)
                .FirstOrDefaultAsync();

            ThemeEntity loadedTheme = await themeQuery;

            loadedTheme.Ideas = loadedTheme.Ideas.OrderByDescending(o => o.Score).ToList<IdeaEntity>();

            //foreach (IdeaEntity idea in loadedTheme.Ideas)
            //{
            //    idea.Comments = idea.Comments.OrderByDescending(c => c.Score).ToList();

            //    Vote tempVote = idea.Votes.Where(v => v.Owner == _userPrincipal.SAMName)
            //        .OrderByDescending(v => v.SubmitDt).FirstOrDefault();

            //    if (tempVote != null)
            //        idea.UserVoteDirection = tempVote.Direction;

            //    foreach (CommentEntity comment in idea.Comments)
            //    {
            //        comment.Comments = comment.Comments.OrderByDescending(c => c.Score).ToList();

            //        Vote subtempVote = comment.Votes.Where(v => v.Owner == _userPrincipal.SAMName)
            //            .OrderByDescending(v => v.SubmitDt).FirstOrDefault();

            //        if (subtempVote != null)
            //            comment.UserVoteDirection = subtempVote.Direction;

            //        foreach (CommentEntity subComment in comment.Comments)
            //        {
            //            Vote subsubtempVote = subComment.Votes.Where(v => v.Owner == _userPrincipal.SAMName)
            //                .OrderByDescending(v => v.SubmitDt).FirstOrDefault();

            //            if (subsubtempVote != null)
            //                subComment.UserVoteDirection = subsubtempVote.Direction;
            //        }
            //    }
            //}
            return Ok(loadedTheme);
        }

        /// <summary>
        /// A <see cref="ThemeEntity"/> is added with content from HttpRequest body and authorized by admin role
        /// </summary>
        /// <param name="theme">A <see cref="ThemeEntity"/></param>
        [HttpPost]
        public async Task<IActionResult> PostThemeAsync([FromBody]ThemeEntity theme)
        {
            theme.OpenDt = DateTime.Now;
            
            await _context.Themes.AddAsync(theme);
            await _context.SaveChangesAsync();

            return Ok(theme.ThemeId);
        }

        /// <summary>
        /// A <see cref="ThemeEntity"/> edited by admin
        /// </summary>
        /// <param name="theme">A <see cref="ThemeEntity"/></param>
        [HttpPatch("edit/{id}")]
        public async Task<IActionResult> EditThemeAsync(int id, [FromBody]JsonPatchDocument<ThemeEntity> patch)
        {
            var theme = await _context.Themes
                .Where(i => i.ThemeId == id)
                .FirstOrDefaultAsync();

            patch.ApplyTo(theme, ModelState);

            _context.Database.ExecuteSqlCommand("EXEC usp_update_notifications");

            await _context.SaveChangesAsync(true, "/michael");
            return Ok(theme);
        }
    }
}