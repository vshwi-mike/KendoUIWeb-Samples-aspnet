using System;
using System.Collections.Generic;
//using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using AutoMapper.Mappers;
using NorthwindWeb.Models;
using NorthwindWeb.DTO;

namespace NorthwindWeb.Api
{
    public class CategoriesController : ApiController
    {
        private NorthwindDbContext db = new NorthwindDbContext();

        public CategoriesController() {
            Mapper.CreateMap<Category, CategoryDTO>();
            Mapper.AssertConfigurationIsValid();
        }


        // GET: api/Categories
        public IEnumerable<CategoryDTO> GetCategories()
        {
            var query = db.Categories
                            .OrderBy(i => i.CategoryID);
            var list = Mapper.Map<List<Category>, List<CategoryDTO>>(query.ToList());
            return list;
        }

        // GET: api/Categories/5
        [ResponseType(typeof(CategoryDTO))]
        public async Task<IHttpActionResult> GetCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null) {
                return NotFound();
            }

            var dto = Mapper.Map<Category, CategoryDTO>(category);
            return Ok(dto);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            if (id != category.CategoryID) {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!CategoryExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Categories
        [ResponseType(typeof(CategoryDTO))]
        public async Task<IHttpActionResult> PostCategory(Category category)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryID }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(CategoryDTO))]
        public async Task<IHttpActionResult> DeleteCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null) {
                return NotFound();
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            var dto = Mapper.Map<Category, CategoryDTO>(category);
            return Ok(dto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryID == id) > 0;
        }
    }
}