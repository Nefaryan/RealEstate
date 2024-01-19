using Microsoft.Extensions.Logging;
using RealEstate.Data;
using RealEstate.Models;
using RealEstate.Models.Util;
using RealEstate.Repository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RealEstate.Services.IServices;

namespace RealEstate.Services
{
    public class IssueService : IIssueService
    {

        private readonly IGenericRepository<Issue> _issueRepository;

        public IssueService(IGenericRepository<Issue> issueRepo)
        {

            _issueRepository = issueRepo;
           
        }

       
        public async Task CreateIssue(Issue issue)
        {
            await _issueRepository.Create(issue);
        }

        public async Task<string> Delete(int id)
        {
            try
            {
                return await _issueRepository.Delete(id);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task< List<Issue>> GetAllIssues() 
        { 
            return await _issueRepository.GetAll(); 
        }


        public async Task<Issue> GetSingleIssue(int id) { return await _issueRepository.GetSingle(id);
        
        
        }

        public async Task UpdateIssue(int id, Issue updatedIssue)
        {
            try
            {
                var existingIssue = await _issueRepository.GetSingle(id);

                if (existingIssue == null)
                {
                    throw new Exception("Issue not found");
                }

                existingIssue.Name = updatedIssue.Name;
                existingIssue.Description = updatedIssue.Description;
                existingIssue.DataApertura = updatedIssue.DataApertura;
                existingIssue.DataChiusura = updatedIssue.DataChiusura;
                existingIssue.Proprietà = updatedIssue.Proprietà;
                existingIssue.Commenti = updatedIssue.Commenti;
                existingIssue.Stato = updatedIssue.Stato;

                await _issueRepository.Update(id, existingIssue);
            }
            catch (Exception ex)
            { 
              
                throw;
            }
        }


    }
}
