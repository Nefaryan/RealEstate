using RealEstate.Models.Util;
using RealEstate.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace RealEstate.Services.IServices
{
    public interface IIssueService
    {
        public Task CreateIssue(Issue issue);

        public Task<string>  Delete(int id);

        public Task<List<Issue>> GetAllIssues();
        public Task< Issue >GetSingleIssue(int id);
        public Task UpdateIssue(int id, Issue issue);
    }
}
