﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevOpsMetrics.Core.DataAccess;
using DevOpsMetrics.Core.DataAccess.TableStorage;
using DevOpsMetrics.Core.Models.AzureDevOps;
using DevOpsMetrics.Core.Models.Common;
using DevOpsMetrics.Core.Models.GitHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DevOpsMetrics.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadTimeForChangesController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly IAzureTableStorageDA AzureTableStorageDA;

        public LeadTimeForChangesController(IConfiguration configuration, IAzureTableStorageDA azureTableStorageDA)
        {
            Configuration = configuration;
            AzureTableStorageDA = azureTableStorageDA;
        }

        // Get lead time for changes from Azure DevOps API
        [HttpGet("GetAzureDevOpsLeadTimeForChanges")]
        public async Task<LeadTimeForChangesModel> GetAzureDevOpsLeadTimeForChanges(bool getSampleData, 
            string organization, string project, string repository, string branch, string buildName, 
            int numberOfDays, int maxNumberOfItems, bool useCache)
        {
            LeadTimeForChangesModel model = new LeadTimeForChangesModel();
            try
            {
                TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);

                //Get the PAT token from the settings
                List<AzureDevOpsSettings> settings = AzureTableStorageDA.GetAzureDevOpsSettingsFromStorage(tableStorageConfig, "DevOpsAzureDevOpsSettings", PartitionKeys.CreateAzureDevOpsSettingsPartitionKey(organization, project, repository));
                string patToken = null;
                if (settings.Count > 0)
                {
                    patToken = settings[0].PatToken;
                }

                LeadTimeForChangesDA da = new LeadTimeForChangesDA();
                model = await da.GetAzureDevOpsLeadTimesForChanges(getSampleData,  patToken, tableStorageConfig,
                        organization, project, repository, branch, buildName, numberOfDays, maxNumberOfItems, useCache);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Response status code does not indicate success: 403 (rate limit exceeded).")
                {
                    model.ProjectName = project;
                    model.RateLimitHit = true;
                }
                else
                {
                    throw;
                }
            }
            return model;
        }

        // Get lead time for changes from GitHub API
        [HttpGet("GetGitHubLeadTimeForChanges")]
        public async Task<LeadTimeForChangesModel> GetGitHubLeadTimeForChanges(bool getSampleData, 
            string owner, string repo, string branch, string workflowName, string workflowId,
            int numberOfDays, int maxNumberOfItems, bool useCache)
        {
            LeadTimeForChangesModel model = new LeadTimeForChangesModel();
            try
            {
                TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);

                //Get the client id and secret from the settings
                List<GitHubSettings> settings = AzureTableStorageDA.GetGitHubSettingsFromStorage(tableStorageConfig, "DevOpsGitHubSettings", PartitionKeys.CreateGitHubSettingsPartitionKey(owner, repo));
                string clientId = null;
                string clientSecret = null;
                if (settings.Count > 0)
                {
                    clientId = settings[0].ClientId;
                    clientSecret = settings[0].ClientSecret;
                }

                LeadTimeForChangesDA da = new LeadTimeForChangesDA();
                model = await da.GetGitHubLeadTimesForChanges(getSampleData, clientId, clientSecret, tableStorageConfig,
                        owner, repo, branch, workflowName, workflowId, numberOfDays, maxNumberOfItems, useCache);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Response status code does not indicate success: 403 (rate limit exceeded).")
                {
                    model.ProjectName = repo;
                    model.RateLimitHit = true;
                }
                else
                {
                    throw;
                }
            }
            return model;

        }
    }
}