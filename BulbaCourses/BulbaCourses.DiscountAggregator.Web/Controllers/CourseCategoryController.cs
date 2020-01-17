﻿using BulbaCourses.DiscountAggregator.Logic.Models;
using BulbaCourses.DiscountAggregator.Logic.Services;
using FluentValidation.WebApi;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BulbaCourses.DiscountAggregator.Web.Controllers
{
    [RoutePrefix("api/categories")]
    public class CourseCategoryController : ApiController
    {
        private readonly ICourseCategoryServices _courseCategoryService;

        public CourseCategoryController(ICourseCategoryServices courseCategoryService)
        {
            this._courseCategoryService = courseCategoryService;
        }

        [HttpGet, Route("")]
        [Description("Get all categories")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Invalid paramater format")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Categories doesn't exists")]
        [SwaggerResponse(HttpStatusCode.OK, "Categories found", typeof(IEnumerable<CourseCategory>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Something wrong")]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var result = await _courseCategoryService.GetAllAsync();
            return result == null ? NotFound() : (IHttpActionResult)Ok(result);
        }

        [HttpGet, Route("{id}")]
        [Description("Get category by Id")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Invalid paramater format")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Category doesn't exists")]
        [SwaggerResponse(HttpStatusCode.OK, "Category found", typeof(CourseCategory))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Something wrong")]
        public async Task<IHttpActionResult> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            try
            {
                var result = await _courseCategoryService.GetByIdAsync(id);
                return result == null ? NotFound() : (IHttpActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create([FromBody, CustomizeValidator(RuleSet = "AddCategory,default")]CourseCategory courseCategory)
        {
            if (courseCategory == null)
            {
                return BadRequest();
            }

            courseCategory.Id = Guid.NewGuid().ToString();
            var result = await _courseCategoryService.AddAsync(courseCategory);
            return result.IsError ? BadRequest(result.Message) : (IHttpActionResult)Ok(result.Data);
        }

        [HttpDelete, Route("{id}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Ivalid paramater format")]
        [SwaggerResponse(HttpStatusCode.OK, "Category deleted", typeof(CourseCategory))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Something wrong")]
        public IHttpActionResult DeleteById(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }
            try
            {
                _courseCategoryService.DeleteByIdAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut, Route("id")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Ivalid paramater format")]
        [SwaggerResponse(HttpStatusCode.OK, "Category updated", typeof(CourseCategory))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Something wrong")]
        public IHttpActionResult Update(string id, [FromBody, CustomizeValidator(RuleSet = "default")]CourseCategory courseCategory)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }

            try
            {
                _courseCategoryService.UpdateAsync(courseCategory);
                return Ok();
            }

            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}