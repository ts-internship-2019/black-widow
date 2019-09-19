﻿using iWasHere.Domain.DTOs;
using iWasHere.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iWasHere.Domain.Service
{
    public class DictionaryService
    {
        private readonly BlackWidowContext _bwContext;
        private readonly DatabaseContext _databaseContext;

        public DictionaryService(BlackWidowContext databaseContext)
        {
            _bwContext = databaseContext;
        }

        public List<DictionaryLandmarkTypeModel> GetDictionaryLandmarkTypeModels()
        {
            List<DictionaryLandmarkTypeModel> dictionaryTicketModels = _databaseContext.DictionaryLandmarkType.Select(a => new DictionaryLandmarkTypeModel()
            {
                Id = a.DictionaryItemId,
                Name = a.DictionaryItemName
            }).ToList();

            return dictionaryTicketModels;
        }

        public List<DictionaryTicketModel> GetDictionaryTicketModels()
        {
           
            List<DictionaryTicketModel> dictionaryTicketModels = _bwContext.DictionaryTicket.Select(a => new DictionaryTicketModel()
            {
                DictionaryTicketId = a.DictionaryTicketId,
                TicketCategory = a.TicketCategory
            }).ToList();

            return dictionaryTicketModels;
        }

        public List<DictionaryTicketModel> GetDictionaryTicketPagination(int pageSize, int page, out int noRows)
        {
            noRows = _bwContext.DictionaryTicket.Count();
            int skip = (page - 1) * pageSize;
            List<DictionaryTicketModel> dictionaryTicketModels = _bwContext.DictionaryTicket.Select(a => new DictionaryTicketModel()
            {
                DictionaryTicketId = a.DictionaryTicketId,
                TicketCategory = a.TicketCategory
            }).Skip(skip).Take(pageSize).ToList();

            return dictionaryTicketModels;
        }

        public List<DictionaryAttractionCategoryModel> GetDictionaryAttractionCategoryModels(int skip, int take, out int total, string input)
        {
            List<DictionaryAttractionCategoryModel> dictionaryAttractionCategoryModels = new List<DictionaryAttractionCategoryModel>();
            int skip_amount = (skip - 1) * take;

            IQueryable<DictionaryAttractionCategory> query = _bwContext.DictionaryAttractionCategory;
            if (!String.IsNullOrWhiteSpace(input))
            {
                query = query.Where(a => a.AttractionCategoryName.Contains(input));
            }
            total = query.Count();
            dictionaryAttractionCategoryModels = query.Select(a => new DictionaryAttractionCategoryModel()
            {
                AttractionCategoryId = a.AttractionCategoryId,
                AttractionCategoryName = a.AttractionCategoryName
            }).Skip(skip_amount).Take(take).ToList();

            return dictionaryAttractionCategoryModels;
        }

        public List<DictionaryCountryModel> GetDictionaryCountryModels(int pageSize, int page, out int total)
        {
            total = _bwContext.DictionaryCountry.Count();
            int skip = (page - 1) * pageSize;
            List<DictionaryCountryModel> dictionaryCountryModels = _bwContext.DictionaryCountry.Select(a => new DictionaryCountryModel()
            {
                Id = a.CountryId,
                Name = a.CountryName
            }).Skip(skip).Take(pageSize).ToList();

            return dictionaryCountryModels;
        }
        //ale lu paulica de aici

        public List<County_DTO> GetDictionaryCounty(int PageSize, int Page, out int totalRows, string f)
        {
            //f filtru de judet
            totalRows = _bwContext.DictionaryCounty.Count(); // .where de pus conditia de where  pt a refreshui
            List<County_DTO> dictionaryCounty = new List<County_DTO>();
            int skip = (Page - 1) * PageSize;

          

            if (string.IsNullOrWhiteSpace(f))
            {
                dictionaryCounty = _bwContext.DictionaryCounty.Select(a => new County_DTO()
                {
                    CountyId = a.CountyId,
                    CountyName = a.CountyName,
                    CountryId = a.CountryId,
                    CountryName = a.Country.CountryName
                })
                .Skip(skip).Take(PageSize).ToList();

            }
            else
            {
                dictionaryCounty = _bwContext.DictionaryCounty.Where(a => a.CountyName.Contains(f)).Select(a => new County_DTO()
                {
                    CountyId = a.CountyId,
                    CountyName = a.CountyName,
                    CountryId = a.CountryId,
                    CountryName = a.Country.CountryName
                })
               .Skip(skip).Take(PageSize).ToList();

               
            }
            return dictionaryCounty;

        }
        public County_DTO GetCounty_ADD_UPDATE(int id)
        {

            County_DTO dictionaryCity = _bwContext.DictionaryCounty
                .Where(a => a.CountyId == id)
                .Select(a => new County_DTO()
                {
                    CountyId = a.CountyId,
                    CountyName = a.CountyName,
                    CountryId = a.CountryId,
                    CountryName = a.Country.CountryName


                }).First();

            return dictionaryCity;
        }
        public void Insert(County_DTO model)
        {
            int id = _bwContext.DictionaryCountry.Where(x => x.CountryName == model.CountryName).Select(x => x.CountryId).FirstOrDefault();
            _bwContext.DictionaryCounty.Add(new DictionaryCounty
            {
               
                CountyName = model.CountyName,
                CountryId = id
               
            });
            _bwContext.SaveChanges();
        }
        //pana aici
        public List<DictionaryOpenSeasonModel> GetDictionaryOpenSeasonModels(int PageSize, int Page, out int totalRows)
        {
            totalRows = _bwContext.DictionaryOpenSeason.Count();
            int skip = (Page - 1) * PageSize;
            List<DictionaryOpenSeasonModel> dictionaryOpenSeasonModels = _bwContext.DictionaryOpenSeason.Select(a => new DictionaryOpenSeasonModel()
            {
                Id = a.OpenSeasonId,
                Type = a.OpenSeasonType
            }).Skip(skip).Take(PageSize).ToList();

            return dictionaryOpenSeasonModels;
        }

        public void AddAttractionCategory(string attractionCategoryName)
        {
            _bwContext.DictionaryAttractionCategory.Add(new DictionaryAttractionCategory
            {
                AttractionCategoryName = attractionCategoryName
            });

            _bwContext.SaveChanges();
        }

        public List<DictionaryCountryModel> GetDictionaryCountryModels()
        {

            List<DictionaryCountryModel> dictionaryCountryModels = _bwContext.DictionaryCountry.Select(a => new DictionaryCountryModel()
            {
                Id = a.CountryId,
                Name = a.CountryName
            }).ToList();

            return dictionaryCountryModels;
        }
        public string Delete_Country(int id)
        {
            try
            {
                _bwContext.Remove(_bwContext.DictionaryCountry.Single(a => a.CountryId == id));
                _bwContext.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return "Acesta tara nu poate fi sters pentru ca are asociat un judet!!!";
            }
        }
    }
}
