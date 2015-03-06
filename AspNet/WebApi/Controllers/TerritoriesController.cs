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
    public class TerritoriesController : ApiController
    {
        private NorthwindDbContext db = new NorthwindDbContext();

        public TerritoriesController() {
            Mapper.CreateMap<Region, RegionDTO>();
            Mapper.CreateMap<Territory, TerritoryDTO>()
                .ForMember(d => d.RegionDescription, o => o.MapFrom(s => s.Region != null ? s.Region.RegionDescription.Trim() : ""));

            Mapper.AssertConfigurationIsValid();
        }

        // GET: api/Territories
        public IEnumerable<TerritoryDTO> GetTerritories(int RegionID = 0) {
            var query = db.Territories
                            .Include("Region")
                            .AsQueryable();

            if (RegionID != 0) {
                query = query.Where(i => i.RegionID == RegionID);
            }

            query = query.OrderBy(i => i.TerritoryID);
            var list = Mapper.Map<List<Territory>, List<TerritoryDTO>>(query.ToList());
            return list;
        }

        // GET: api/Territories/5
        [ResponseType(typeof(TerritoryDTO))]
        public async Task<IHttpActionResult> GetTerritory(string id) {
            Territory territory = await db.Territories
                                    .Include("Region")
                                    .SingleOrDefaultAsync(i => i.TerritoryID == id);
            if (territory == null) {
                return NotFound();
            }

            var dto = Mapper.Map<Territory, TerritoryDTO>(territory);
            return Ok(dto);
        }


        // PUT: api/Territories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTerritory(string id, Territory territory) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != territory.TerritoryID) {
                return BadRequest();
            }

            db.Entry(territory).State = EntityState.Modified;

            try {
                await db.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!TerritoryExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Territories
        [ResponseType(typeof(Territory))]
        public async Task<IHttpActionResult> PostTerritory(Territory territory) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            db.Territories.Add(territory);

            try {
                await db.SaveChangesAsync();
            } catch (DbUpdateException) {
                if (TerritoryExists(territory.TerritoryID)) {
                    return Conflict();
                } else {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = territory.TerritoryID }, territory);
        }

        // DELETE: api/Territories/5
        [ResponseType(typeof(Territory))]
        public async Task<IHttpActionResult> DeleteTerritory(string id) {
            Territory territory = await db.Territories.FindAsync(id);
            if (territory == null) {
                return NotFound();
            }

            db.Territories.Remove(territory);
            await db.SaveChangesAsync();

            return Ok(territory);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TerritoryExists(string id) {
            return db.Territories.Any(e => e.TerritoryID == id);
        }
    }
}