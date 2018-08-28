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
    [Route("Comments")]
    public class CommentsController : Controller
    {
        private BTRContext _context;
        /// <summary>
        /// Constructs the comments controller with the BTR database context
        /// </summary>
        /// <param name="context">A <see cref="BTRContext"/></param>
        public CommentsController(BTRContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Queries a list of <see cref="CommentEntity"/> based on parent id
        /// </summary>
        /// <param name="id">The Id of the parent comment used to query the list of subcomments</param>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCommentByIdAsync(int id)
        {
            if (!(_context.Comments.Any(c => c.ParentCommentId == id)))
                return NotFound();

            var commentQuery = _context.Comments
                        .Where(c => c.ParentCommentId == id)
                        .Include(c => c.Votes);

            List<CommentEntity> loadedComments = await commentQuery.ToListAsync<CommentEntity>();
            //foreach (CommentEntity comment in loadedComments)
            //{
            //    Vote tempVote = comment.Votes.Where(v => v.Owner == _userPrincipal.SAMName)
            //        .OrderByDescending(v => v.SubmitDt).FirstOrDefault();

            //    if (tempVote != null)
            //        comment.UserVoteDirection = tempVote.Direction;
            //}

            loadedComments = loadedComments.OrderByDescending(c => c.Score).ThenBy(c => c.SubmitDt).ToList();
            return Ok(loadedComments);
        }

        /// <summary>
        /// A <see cref="CommentEntity"/> is added with default values and content from HttpRequest body
        /// </summary>
        /// <param name="comment">A <see cref="CommentEntity"/></param>
        [HttpPost]
        public async Task<ActionResult> PostCommentAsync([FromBody]CommentEntity comment)
        {
            comment.SubmitDt = DateTime.Now;
            comment.ModifiedDt = comment.SubmitDt;
            comment.Owner = "TestOwner";
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync(true, "testSAM");

            return Ok(comment);
        }

        /// <summary>
        /// An <see cref="CommentEntity"/> will be patched with a new valid message
        /// </summary>
        /// <param name="id">A commentId for <see cref="CommentEntity"/></param>
        /// <param name="patch"> A JSONPatchDocument containing the new message </param>
        [HttpPatch("edit/{id}")]
        public async Task<ActionResult> PatchCommentAsync(int id, [FromBody] JsonPatchDocument<CommentEntity> patch)
        {
            var comment = await _context.Comments
                .Where(i => i.CommentId == id)
                .SingleOrDefaultAsync();

            //if (_userPrincipal.DisplayName != comment.Owner)
            //   return Forbid();

            patch.ApplyTo(comment, ModelState);
            comment.ModifiedDt = DateTime.Now;

            _context.Entry(comment).Property(i => i.Message).IsModified = true;

            await _context.SaveChangesAsync(true, "testSam");
            return Ok(comment);
        }

        /// <summary>
        /// A <see cref="CommentEntity"/> is removed along with its respective comment tree 
        /// </summary>
        /// <param name="id">An id for the <see cref="CommentEntity"/> to be removed. </param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments
                .Where(i => i.CommentId == id)
                .SingleOrDefaultAsync();

            //if (!_userPrincipal.IsAppAdmin && _userPrincipal.DisplayName != comment.Owner)
            //    return Forbid();

            await RemoveChildren(comment.CommentId);

            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync(true, "testSam");
            return Ok();
        }

        async Task RemoveChildren(int id)
        {
            var subComments = await _context.Comments
                .Where(c => c.ParentCommentId == id)
                .ToListAsync();

            if (subComments.Count > 0)
            {
                foreach (var comment in subComments)
                {
                    await RemoveChildren(comment.CommentId);
                    _context.Comments.Remove(comment);
                }
            }
        }


        [HttpPost("vote/{id}")]
        public async Task<ActionResult> VoteAsync(int id, int direction)
        {
            // only accept votes of direction 1 or -1
            if (Math.Abs(direction) != 1)
                return BadRequest("Invalid vote direction");

            var commentQuery = _context.Comments
                .Where(i => i.CommentId == id)
                .Include(i => i.Votes);

            CommentEntity comment;
            try
            {
                comment = await commentQuery.SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            Vote MostRecentVoteByUser = null;
            try
            {
                MostRecentVoteByUser = comment.Votes
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
                    //Owner = _userPrincipal.SAMName,
                    CommentId = id
                };
                _context.Votes.Add(userVote);
                await _context.SaveChangesAsync(true, "testSam");
            }
            else
            {
                if (MostRecentVoteByUser.Direction == direction)
                    MostRecentVoteByUser.Direction = 0;
                else
                    MostRecentVoteByUser.Direction = direction;
                await _context.SaveChangesAsync(true, "testSame");
            }
            return Ok(comment.Score);
        }
    }
}