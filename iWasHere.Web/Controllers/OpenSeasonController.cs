﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using iWasHere.Web.Models;
using iWasHere.Domain.Model;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using iWasHere.Domain.Service;
using iWasHere.Domain.DTOs;

namespace iWasHere.Web.Controllers
{
    public class OpenSeasonController : Controller
    {
        private readonly DictionaryService dictionaryService;

        public OpenSeasonController(DictionaryService dictionaryService)
        {
            this.dictionaryService = dictionaryService;
        }
        public IActionResult Index()
        {
            //List<DictionaryOpenSeasonModel> dictionaryOpenSeasonModel = dictionaryService.GetDictionaryOpenSeasonModels();

            return View();
        }


        public IActionResult GetOpenSeason([DataSourceRequest]DataSourceRequest request, string textBox)
        {
            //List<DictionaryOpenSeasonModel> dictionaryOpenSeasonModels = dictionaryService.GetDictionaryOpenSeasonModels();
            int totalRows = 0;
            int PageSize = request.PageSize;
            int Page = request.Page;
            DataSourceResult result = new DataSourceResult();
            List<DictionaryOpenSeasonModel> dictionaryOpenSeasonModels = dictionaryService.GetDictionaryOpenSeasonModels(PageSize, Page, out totalRows, textBox);
            result.Total = totalRows;
            result.Data = dictionaryOpenSeasonModels;
            return Json(result);

            //return Json(dictionaryService.GetDictionaryOpenSeasonModels().ToDataSourceResult(request));
        }
        public ActionResult DeleteOpenSeason([DataSourceRequest] DataSourceRequest request, DictionaryOpenSeasonModel openSeason)
        {
            int openSeasonId = openSeason.Id;
            if (openSeason != null)
            {
                dictionaryService.DeleteOpenSeason(openSeasonId);
            }

            return Json(new[] { openSeason }.ToDataSourceResult(request, ModelState));

        }


        public IActionResult InsertOpenSeason(string id)
        {
            DictionaryOpenSeasonModel model = new DictionaryOpenSeasonModel();
            model.Id = Convert.ToInt32(id);
            if (Convert.ToInt32(id) == 0)
            {
                return View();
            }
            else
            {
               model= dictionaryService.GetOpenSeason(model.Id);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertOpenSeason(DictionaryOpenSeasonModel model, string submitButton, int id)
        {
            if (model != null)
            {
                string errorMessage;
                if (id == 0)
                {
                    errorMessage = dictionaryService.InsertOpenSeason(model);
                    if (String.IsNullOrWhiteSpace(errorMessage))
                    {
                        //dictionaryService.InsertOpenSeason(model);
                        if (submitButton == "SaveAndNew")
                        {
                            ModelState.Clear();
                            return View();
                        }
                        else
                        {
                            return View("Index");
                        }
                    }
                    else
                    {
                        //dictionaryService.UpdateOpenSeason(model);
                        ModelState.AddModelError("error", errorMessage);
                        return View();
                    }
                }
                else
                {
                    model.Id = id;
                    errorMessage = dictionaryService.UpdateOpenSeason(model);
                    if (String.IsNullOrWhiteSpace(errorMessage))
                    {
                        return View("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("error", errorMessage);
                        return View();
                    }
                }
            }
            else
            {
                return Json(ModelState.ToDataSourceResult());
            }
        }
    }
}
