﻿using System;
using DevOpsMetrics.Core.DataAccess;
using DevOpsMetrics.Core.Models.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevOpsMetrics.Tests.Service
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestCategory("IntegrationTest")]
    [TestClass]
    public class MeanTimeToRestoreDATests
    {
        private IConfigurationRoot _configuration;

        [TestInitialize]
        public void TestStartUp()
        {
            IConfigurationBuilder config = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json");
            config.AddUserSecrets<DeploymentFrequencyDATests>();
            _configuration = config.Build();
        }

        [TestMethod]
        public void MeanTimeToRestoreDAIntegrationTest()
        {
            //Arrange
            bool getSampleData = true;
            TableStorageConfiguration tableStorageConfig = Common.GenerateTableAuthorization(_configuration);
            string resourceGroup = "DevOpsMetricsTestRG";
            DevOpsPlatform targetDevOpsPlatform = DevOpsPlatform.AzureDevOps;
            int numberOfDays = 30;
            int maxNumberOfItems = 20;

            //Act
            MeanTimeToRestoreDA da = new MeanTimeToRestoreDA();
            MeanTimeToRestoreModel model = da.GetAzureMeanTimeToRestore(getSampleData, tableStorageConfig, targetDevOpsPlatform, resourceGroup, numberOfDays, maxNumberOfItems);

            //Assert
            Assert.IsTrue(model != null);
            Assert.IsTrue(model.TargetDevOpsPlatform == targetDevOpsPlatform);
            Assert.AreEqual(resourceGroup, model.ResourceGroup);
            Assert.IsTrue(model.MeanTimeToRestoreEvents.Count > 0);
            Assert.IsTrue(model.MTTRAverageDurationInHours > 0);
            Assert.AreEqual(numberOfDays, model.NumberOfDays);
            Assert.IsTrue(model.MaxNumberOfItems > 0);
            Assert.IsTrue(model.TotalItems > 0);
        }

        [TestMethod]
        public void MeanTimeToRestoreDASamLearnsAzureProdIntegrationTest()
        {
            //Arrange
            bool getSampleData = false;
            TableStorageConfiguration tableStorageConfig = Common.GenerateTableAuthorization(_configuration);
            string resourceGroup = "SamLearnsAzureProd";
            DevOpsPlatform targetDevOpsPlatform = DevOpsPlatform.AzureDevOps;
            int numberOfDays = 30;
            int maxNumberOfItems = 20;

            //Act
            MeanTimeToRestoreDA da = new MeanTimeToRestoreDA();
            MeanTimeToRestoreModel model = da.GetAzureMeanTimeToRestore(getSampleData, tableStorageConfig, targetDevOpsPlatform, resourceGroup, numberOfDays, maxNumberOfItems);

            //Assert
            Assert.IsTrue(model != null);
            Assert.IsTrue(model.TargetDevOpsPlatform == targetDevOpsPlatform);
            Assert.AreEqual(resourceGroup, model.ResourceGroup);
            Assert.IsTrue(model.MeanTimeToRestoreEvents.Count >= 0);
            Assert.IsTrue(model.MTTRAverageDurationInHours >= 0);
            Assert.AreEqual(numberOfDays, model.NumberOfDays);
            Assert.IsTrue(model.MaxNumberOfItems >= 0);
            Assert.IsTrue(model.TotalItems >= 0);
        }


        [TestMethod]
        public void TimeToRestoreServiceDAIntegrationTest()
        {
            //Arrange
            bool getSampleData = false;
            TableStorageConfiguration tableStorageConfig = Common.GenerateTableAuthorization(_configuration);
            string resourceGroup = "SamLearnsAzureProd";
            DevOpsPlatform targetDevOpsPlatform = DevOpsPlatform.AzureDevOps;
            int numberOfDays = 60;
            int maxNumberOfItems = 20;

            //Act
            MeanTimeToRestoreDA da = new MeanTimeToRestoreDA();
            MeanTimeToRestoreModel model = da.GetAzureMeanTimeToRestore(getSampleData, tableStorageConfig, targetDevOpsPlatform, resourceGroup, numberOfDays, maxNumberOfItems);

            //Assert
            Assert.IsTrue(model != null);
            Assert.IsTrue(model.TargetDevOpsPlatform == targetDevOpsPlatform);
            Assert.AreEqual(resourceGroup, model.ResourceGroup);
            Assert.IsTrue(model.MeanTimeToRestoreEvents.Count >= 0);
            Assert.IsTrue(model.MTTRAverageDurationInHours >= 0);
            Assert.IsTrue(model.MTTRAverageDurationDescription != "");
            Assert.AreEqual(numberOfDays, model.NumberOfDays);
            Assert.IsTrue(model.MaxNumberOfItems >= 0);
            Assert.IsTrue(model.TotalItems >= 0);
        }

        [TestMethod]
        public void TimeToRestoreServiceDALiveIntegrationTest()
        {
            //Arrange
            bool getSampleData = false;
            TableStorageConfiguration tableStorageConfig = Common.GenerateTableAuthorization(_configuration);
            string resourceGroup = "SamLearnsAzureProd";
            DevOpsPlatform targetDevOpsPlatform = DevOpsPlatform.AzureDevOps;
            int numberOfDays = 60;
            int maxNumberOfItems = 20;

            //Act
            MeanTimeToRestoreDA da = new MeanTimeToRestoreDA();
            MeanTimeToRestoreModel model = da.GetAzureMeanTimeToRestore(getSampleData, tableStorageConfig, targetDevOpsPlatform, resourceGroup, numberOfDays, maxNumberOfItems);

            //Assert
            Assert.IsTrue(model != null);
            Assert.IsTrue(model.TargetDevOpsPlatform == targetDevOpsPlatform);
            Assert.AreEqual(resourceGroup, model.ResourceGroup);
            Assert.IsTrue(model.MeanTimeToRestoreEvents.Count >= 0);
            Assert.IsTrue(model.MTTRAverageDurationInHours >= 0);
            Assert.IsTrue(model.MTTRAverageDurationDescription != "");
            Assert.AreEqual(numberOfDays, model.NumberOfDays);
            Assert.IsTrue(model.MaxNumberOfItems >= 0);
            Assert.IsTrue(model.TotalItems >= 0);
        }
    }
}
