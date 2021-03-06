﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DataFlow.EdFi.Api.Resources;
using DataFlow.EdFi.Sdk;
using DataFlow.Web.Helpers;
using DataFlow.Web.Services;
using RestSharp;

namespace DataFlow.Web.Controllers
{
    public class ConfigurationController : BaseController
    {
        public ConfigurationController(IBaseServices baseService) : base(baseService)
        {
            
        }

        public ActionResult Index()
        {
            var vm = ConfigurationService.GetConfiguration();

            return View(vm);
        }

        [HttpPost]
        public bool ConfigurationIsOk(string apiServerUrl, string apiServerKey, string apiServerSecret)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderLocal, certificate, chain, sslPolicyErrors) => true;

                var oAuthUrl = Common.Helpers.UrlUtility.GetUntilOrEmpty(apiServerUrl.Trim(), "/api/");

                var client = new RestClient(apiServerUrl);
                var tokenRetriever = new TokenRetriever(oAuthUrl, apiServerKey, apiServerSecret);
                client.Authenticator = new BearerTokenAuthenticator(tokenRetriever);

                var api = new SchoolsApi(client);
                var apiCall = api.GetSchoolsAll(0, 1);

                return apiCall.IsSuccessful;
            }
            catch (Exception e)
            {
                LogService.Error("Configuration for API failed.", e);
                return false;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DataFlow.Web.Models.ApiConfigurationValues vm)
        {
            if (!string.IsNullOrWhiteSpace(vm.INSTANCE_ORGANIZATION_LOGO) && !vm.INSTANCE_ORGANIZATION_LOGO.HasImageExtension())
            {
                ModelState.AddModelError("INSTANCE_ORGANIZATION_LOGO", "Organization logo must end with the following file extensions: jpg, gif, png, or svg.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var apiServerUrl = ConfigurationService.GetConfigurationByKey(Constants.API_SERVER_URL);
            var apiServerKey = ConfigurationService.GetConfigurationByKey(Constants.API_SERVER_KEY);
            var apiServerSecret = ConfigurationService.GetConfigurationByKey(Constants.API_SERVER_SECRET);
            var instanceOrganizationLogo = ConfigurationService.GetConfigurationByKey(Constants.INSTANCE_ORGANIZATION_LOGO);
            var instanceOrganizationUrl = ConfigurationService.GetConfigurationByKey(Constants.INSTANCE_ORGANIZATION_URL);
            var instanceEduUseText = ConfigurationService.GetConfigurationByKey(Constants.INSTANCE_EDU_USE_TEXT);
            var instanceAllowRegistrations = ConfigurationService.GetConfigurationByKey(Constants.INSTANCE_ALLOW_USER_REGISTRATION);

            apiServerUrl.Value = vm.API_SERVER_URL;
            apiServerKey.Value = vm.API_SERVER_KEY;
            apiServerSecret.Value = vm.API_SERVER_SECRET;
            instanceOrganizationLogo.Value = vm.INSTANCE_ORGANIZATION_LOGO;
            instanceOrganizationUrl.Value = vm.INSTANCE_ORGANIZATION_URL;
            instanceEduUseText.Value = vm.INSTANCE_EDU_USE_TEXT;
            instanceAllowRegistrations.Value = Convert.ToString(vm.INSTANCE_ALLOW_USER_REGISTRATION);

            var confs = new List<DataFlow.Models.Configuration>
            {
                apiServerUrl,
                apiServerKey,
                apiServerSecret,
                instanceOrganizationLogo,
                instanceOrganizationUrl,
                instanceEduUseText,
                instanceAllowRegistrations
            };

            ConfigurationService.SaveConfiguration(confs);
            ConfigurationService.FillSwaggerMetadata(apiServerUrl.Value);

            vm.FormResult.ShowInfoMessage = true;
            vm.FormResult.IsSuccess = true;
            vm.FormResult.InfoMessage = "Application configuration saved!";

            LogService.Info("Configuration was modified.");

            return View(vm);
        }
    }
}