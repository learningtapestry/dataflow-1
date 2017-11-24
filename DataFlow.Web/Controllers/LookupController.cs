﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataFlow.Common.DAL;
using DataFlow.Web.Helpers;
using DataFlow.Web.Services;

namespace DataFlow.Web.Controllers
{
    public class LookupController : BaseController
    {
        private readonly DataFlowDbContext dataFlowDbContext;
        private readonly EdFiService edFiService;

        public LookupController(DataFlowDbContext dataFlowDbContext, EdFiService edFiService)
        {
            this.dataFlowDbContext = dataFlowDbContext;
            this.edFiService = edFiService;
        }

        public ActionResult Index()
        {
            var vm = dataFlowDbContext.Lookups
                .OrderBy(x => x.GroupSet)
                .ThenBy(x => x.Key)
                .ToList();

            return View(vm);
        }

        public ActionResult Add()
        {
            var lookup = new DataFlow.Models.Lookup();

            return View(lookup);
        }

        public ActionResult Edit(int id)
        {
            var lookup = dataFlowDbContext.Lookups.FirstOrDefault(x => x.Id == id);

            return View(lookup);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var lookup = dataFlowDbContext.Lookups.FirstOrDefault(x => x.Id == id);
            if (lookup != null)
            {
                dataFlowDbContext.Lookups.Remove(lookup);
                await dataFlowDbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DataFlow.Models.Lookup vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            SaveLookup(vm);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DataFlow.Models.Lookup vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            SaveLookup(vm);

            return RedirectToAction("Index");
        }

        private void SaveLookup(DataFlow.Models.Lookup vm)
        {
            var isUpdate = vm.Id > 0;

            var lookup = new DataFlow.Models.Lookup();

            if (isUpdate)
            {
                lookup = dataFlowDbContext.Lookups.FirstOrDefault(x => x.Id == vm.Id);
                lookup.Id = vm.Id;
            }

            lookup.GroupSet = vm.GroupSet;
            lookup.Key = vm.Key;
            lookup.Value = vm.Value;

            dataFlowDbContext.Lookups.AddOrUpdate(lookup);
            dataFlowDbContext.SaveChanges();
        }
    }
}