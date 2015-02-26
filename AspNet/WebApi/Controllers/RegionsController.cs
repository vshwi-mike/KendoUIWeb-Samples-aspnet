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
using NorthwindWeb.Models;
using NorthwindWeb.DTO;

namespace NorthwindWeb.Api
{
    public class RegionsController : ApiController
    {
        private NorthwindDbContext db = new NorthwindDbContext();

        public RegionsController() {
            Mapper.CreateMap<Region, RegionDTO>();
            Mapper.AssertConfigurationIsValid();
        }

        // GET: api/Regions
        public IEnumerable<RegionDTO> GetRegions()
        {
            var query = db.Regions
                            .OrderBy(i => i.RegionID);
            var list = Mapper.Map<List<Region>, List<RegionDTO>>(query.ToList());
            return list;
        }

        // GET: api/Regions/5
        [ResponseType(typeof(RegionDTO))]
        public async Task<IHttpActionResult> GetRegion(int id)
        {
            Region region = await db.Regions.FindAsync(id);
            if (region == null) {
                return NotFound();
            }

            var dto = Mapper.Map<Region, RegionDTO>(region);
            return Ok(dto);
        }

        // PUT: api/Regions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRegion(int id, Region region)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != region.RegionID)
            {
                return BadRequest();
            }

            db.Entry(region).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Regions
        [ResponseType(typeof(Region))]
        public async Task<IHttpActionResult> PostRegion(Region region)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Regions.Add(region);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RegionExists(region.RegionID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = region.RegionID }, region);
        }

        // DELETE: api/Regions/5
        [ResponseType(typeof(Region))]
        public async Task<IHttpActionResult> DeleteRegion(int id)
        {
            Region region = await db.Regions.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            db.Regions.Remove(region);
            await db.SaveChangesAsync();

            return Ok(region);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RegionExists(int id)
        {
            return db.Regions.Count(e => e.RegionID == id) > 0;
        }
    }
}