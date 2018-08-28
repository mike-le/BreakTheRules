using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BTR.DataAccess;
using BTR.DataAccess.Entities;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTR.Controllers
{
    [Produces("application/json")]
    [Route("Ideas")]
    public class IdeasController : Controller
    {
        private BTRContext _context;
        /// <summary>
        /// Constructs the ideas controller with the BTR database context
        /// </summary>
        /// <param name="context">A <see cref="BTRContext"/></param>
        public IdeasController(BTRContext context)
        {
            _context = context;
        }

        /// <summary>
        /// An <see cref="IdeaEntity"/> is added with default values and content from HttpRequest body
        /// </summary>
        /// <param name="idea">A <see cref="IdeaEntity"/></param>
        [HttpPost]
        public async Task<ActionResult> PostIdeaAsync([FromBody]IdeaEntity idea)
        {
            ThemeEntity parentTheme = await _context.Themes.Where(t => t.ThemeId == idea.ThemeId).SingleAsync();

            if (parentTheme != null && (parentTheme.CloseDt - DateTime.Now < TimeSpan.Zero))
                return Forbid();

            idea.SubmitDt = DateTime.Now;
            idea.ModifiedDt = idea.SubmitDt;
            idea.Owner = "Michael";
            
            await _context.Ideas.AddAsync(idea);

            await _context.SaveChangesAsync(true, "testSame");

            Status ideaStatus = new Status()
            {
                StatusCode = StatusType.Submitted,
                Response = "",
                SubmitDt = idea.SubmitDt,
                IdeaId = idea.PostId,
                Idea = idea
            };

            await _context.Statuses.AddAsync(ideaStatus);

            await _context.SaveChangesAsync(true, "testSam");

            return Ok(idea);
        }

        /// <summary>
        /// An <see cref="IdeaEntity"/> will be patched with a new valid message
        /// </summary>
        /// <param name="id">A postId for <see cref="IdeaEntity"/></param>
        /// <param name="patch"> A JSONPatchDocument containing the new message </param>
        [HttpPatch("edit/{id}")]
        public async Task<ActionResult> PatchIdeaAsync(int id, [FromBody] JsonPatchDocument<IdeaEntity> patch)
        {
            var idea = await _context.Ideas
                .Where(i => i.PostId == id)
                .SingleOrDefaultAsync();

            //if (_userPrincipal.DisplayName != idea.Owner)
            //    return Forbid();

            patch.ApplyTo(idea, ModelState);

            _context.Entry(idea).Property(i => i.Message).IsModified = true;
            idea.ModifiedDt = DateTime.Now;

            await _context.SaveChangesAsync(true, "testSAM");
            return Ok(idea);
        }
    

        /// <summary>
        /// An <see cref="IdeaEntity"/> is removed along with its respective comment tree. The comment tree is removed first from bottom of tree to top to avoid reference issues.
        /// </summary>
        /// <param name="id">An id for the <see cref="IdeaEntity"/> to be removed. </param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIdeaAsync(int id)
        {
            var idea = await _context.Ideas
                .Where(i => i.PostId == id)
                .SingleOrDefaultAsync();

            //if (!_userPrincipal.IsAppAdmin && _userPrincipal.DisplayName != idea.Owner)
            //    return Forbid();

            await RemoveChildren(idea.PostId);
            _context.Ideas.Remove(idea);

            await _context.SaveChangesAsync(true, "testSAM");
            return Ok();
        }

        async Task RemoveChildren(int id)
        {
            var subComments = await _context.Comments
                .Where(c => c.ParentCommentId == id || c.ParentIdeaId == id)
                .ToListAsync();

            if (subComments.Count > 0) {
                foreach (var comment in subComments) {
                    await RemoveChildren(comment.CommentId);
                    _context.Comments.Remove(comment);
                }
            }
        }
        /// <summary>
        /// Upvote or downvote an idea.
        /// </summary>
        /// <param name="id">Id of idea to upvote/downvote.</param>
        /// <param name="direction">The direction of which to vote. Accepts 1 or -1.</param>
        /// <returns></returns>
        [HttpPost("vote/{id}")]
        public async Task<ActionResult> VoteAsync(int id, int direction)
        {
            if (Math.Abs(direction) != 1)
                return BadRequest("Invalid vote direction");

            var ideaQuery = _context.Ideas
                .Where(i => i.PostId == id)
                .Include(i => i.Votes);
            
            IdeaEntity idea;
            try
            {
                idea = await ideaQuery.SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            Vote MostRecentVoteByUser = null;
            try
            {
                MostRecentVoteByUser = idea.Votes
                    .OrderByDescending(v => v.SubmitDt)
                    //.Where(v => v.Owner == _userPrincipal.SAMName)
                    .First();
            }
            catch
            { }

            if (MostRecentVoteByUser == null)
            {
                Vote userVote = new Vote()
                {
                    SubmitDt = DateTime.Now,
                    Direction = direction,
                    Owner = "testSAM",
                    IdeaId = id
                };
                _context.Votes.Add(userVote);
                await _context.SaveChangesAsync(true, "testSAM");
            }
            else
            {
                if (MostRecentVoteByUser.Direction == direction)
                    MostRecentVoteByUser.Direction = 0;
                else
                    MostRecentVoteByUser.Direction = direction;
                await _context.SaveChangesAsync(true, "testSAM");
            }
            
            return Ok(idea.Score);
        }
    }
}