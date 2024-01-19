using RealEstate.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.Services.IServices
{
    public interface ICommentService
    {
        public Task CreateComments(Comments comment);
        public Task UpdateComment(int id, Comments comment);
        public Task<string> DeleteComment(int id);
        public Task<Comments> GetCommentById(int id);
        public Task<List<Comments>> GetAllComments();

    }
}
