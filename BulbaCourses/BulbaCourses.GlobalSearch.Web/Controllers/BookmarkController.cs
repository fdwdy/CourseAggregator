﻿using BulbaCourses.GlobalSearch.Logic.DTO;
using BulbaCourses.GlobalSearch.Logic.InterfaceServices;
using BulbaCourses.GlobalSearch.Logic.Models;
using FluentValidation.WebApi;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace BulbaCourses.GlobalSearch.Web.Controllers
{
    [RoutePrefix("api/bookmarks")]
    [Authorize]
    public class BookmarkController : ApiController
    {
        private readonly IBookmarkService _bookmarkService;
        public BookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        //Role - admin
        /// <summary>
        /// Get all bookmarks
        /// </summary>
        /// <returns>Returns all bookmarks stored</returns>
        [HttpGet, Route("")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There are no bookmarks in list")]
        [SwaggerResponse(HttpStatusCode.OK, "Bookmarks were found", typeof(IEnumerable<BookmarkDTO>))]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _bookmarkService.GetAllAsync();
            return result == null ? NotFound() : (IHttpActionResult)Ok(result);
        }

        /// <summary>
        /// Get bookmark
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Ivalid bookmark id")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Bookmark doesn't exists")]
        [SwaggerResponse(HttpStatusCode.OK, "Bookmark was found", typeof(BookmarkDTO))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Something goes wrong")]
        public async Task<IHttpActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }
            try
            {
                var result = await _bookmarkService.GetByIdAsync(id);
                return result == null ? NotFound() : (IHttpActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get user bookmarks
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("user")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Ivalid UserId")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Bookmark wasn't found")]
        [SwaggerResponse(HttpStatusCode.OK, "ID users bookmarks were found", typeof(IEnumerable<BookmarkDTO>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Something goes wrong")]
        public async Task<IHttpActionResult> GetByUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    string UserId = (User as ClaimsPrincipal).FindFirst("sub").ToString().ToString().Replace("sub: ", "");
                    var result = await _bookmarkService.GetByUserIdAsync(UserId);
                    return result == null ? NotFound() : (IHttpActionResult)Ok(result);
                }
                catch (InvalidOperationException ex)
                {
                    return InternalServerError(ex);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Add bookmark
        /// </summary>
        /// <param name="bookmark"></param>
        /// <returns></returns>
        [HttpPost, Route("")]
        [SwaggerResponse(HttpStatusCode.OK, "Bookmark added")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Ivalid bookmark data")]
        public async Task<IHttpActionResult> Create([FromBody, CustomizeValidator]BookmarkDTO bookmark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bookmark.UserId = (User as ClaimsPrincipal).FindFirst("sub").ToString().ToString().Replace("sub: ", "");
            var result = await _bookmarkService.AddAsync(bookmark);
            return result.IsError ? BadRequest(result.Message) : (IHttpActionResult)Ok(result.Data);
        }

        /// <summary>
        /// Delete bookmark
        /// </summary>
        /// <param name="id">Bookmark id</param>
        /// <returns></returns>
        [HttpDelete, Route("{id}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Ivalid ID")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Bookmark doesn't exists")]
        [SwaggerResponse(HttpStatusCode.OK, "Bookmark deleted")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Something goes wrong")]
        public async Task<IHttpActionResult> RemoveById(string id)
        {
            string UserId = (User as ClaimsPrincipal).FindFirst("sub").ToString().ToString().Replace("sub: ", "");
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _) || _bookmarkService.GetById(id) == null || _bookmarkService.GetById(id).UserId != UserId)
            {
                return BadRequest();
            }
            try
            {
                var result = await _bookmarkService.RemoveByIdAsync(id);
                return result.IsError ? BadRequest(result.Message) : (IHttpActionResult)Ok(result.IsSuccess);
            }
            catch (InvalidOperationException ex)
            {
                return InternalServerError(ex);
            }
        }

        //Role - admin
        /// <summary>
        /// Delete bookmarks
        /// </summary>
        /// <returns></returns>
        [HttpDelete, Route("")]
        [SwaggerResponse(HttpStatusCode.OK, "Bookmarks removed")]
        public IHttpActionResult ClearAll()
        {
            _bookmarkService.RemoveAll();
            return Ok();
        }
    }
}
