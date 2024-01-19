using Microsoft.Extensions.Logging;
using RealEstate.Models;
using RealEstate.Repository;
using RealEstate.Repository.DTO;
using RealEstate.Services.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.Services
{
    public class CommentsService : ICommentService
    {
        private readonly IGenericRepository<Comments> _commentsRepository;
        private readonly ILogger<CommentsService> _logger;

        public CommentsService(IGenericRepository<Comments> commentsRepository, ILogger<CommentsService> logger)
        {
            _commentsRepository = commentsRepository;
            _logger = logger;
        }
        public async Task CreateComments(Comments comment)
        {
            try
            {
                _logger.LogInformation("Creating Comments");
                await _commentsRepository.Create(comment);
                _logger.LogInformation("Comment created");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<Comments> GetCommentById(int id)
        {
            try
            {
                _logger.LogInformation($"Getting comment whit id: {id}");
                return await _commentsRepository.GetSingle(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<List<Comments>> GetAllComments()
        {
            try
            {
                _logger.LogInformation("Getting all Comments");
                return await _commentsRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
        public async Task UpdateComment(int commentId, Comments updatedComment)
        {
            try
            {
                _logger.LogInformation($"Updating comment whit Id: {commentId}");
                await _commentsRepository.Update(commentId, updatedComment);
                _logger.LogInformation("Comment updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<string> DeleteComment(int commentId)
        {
            try
            {
                _logger.LogInformation($"Deleting user with Id: {commentId}");
                return await _commentsRepository.Delete(commentId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }

        }
    }
}