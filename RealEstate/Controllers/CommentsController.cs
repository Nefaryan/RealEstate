using Microsoft.AspNetCore.Mvc;
using RealEstate.Models;
using RealEstate.Services;
using System;
using System.Threading.Tasks;

namespace RealEstate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly CommentsService _commentsService;

        public CommentsController(CommentsService commentsService)
        {
            _commentsService = commentsService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] Comments comment)
        {
            try
            {
                await _commentsService.CreateComments(comment);
                return Ok("Comment created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [HttpGet("{CommentId}")]
        public async Task<IActionResult> GetCommentById(int commentId)
        {
            try
            {
                var comment = await _commentsService.GetCommentById(commentId);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            try
            {
                var houses = await _commentsService.GetAllComments();
                return Ok(houses);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] Comments comment)
        {
            try
            {
                await _commentsService.UpdateComment(commentId, comment);
                return Ok("Comment Updated");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                await _commentsService.DeleteComment(commentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
