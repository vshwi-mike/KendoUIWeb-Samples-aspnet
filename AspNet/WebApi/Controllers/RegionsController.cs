﻿using System;
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
using WebApi.Models;
using WebApi.DTO;

namespace WebApi
{
    public class RegionsController : ApiController
    {
        private NorthwindDbContext db = new NorthwindDbContext();

        public RegionsController() {
            Mapper.CreateMap<Region, RegionDTO>()
                .ForMember( m => m.RegionDescription, opt => opt.MapFrom( src => src.RegionDescription.TrimEnd()));
            Mapper.AssertConfigurationIsValid();
        }

        // GET: api/Regions
        public IEnumerable<RegionDTO> GetRegions(){
            var query = db.Regions
                            .OrderBy(i => i.RegionID);
            var list = Mapper.Map<List<Region>, List<RegionDTO>>(query.ToList());
            return list;
        }

        // GET: api/Regions/5
        [ResponseType(typeof(RegionDTO))]
        public async Task<IHttpActionResult> GetRegion(int id){
            Region region = await db.Regions.FindAsync(id);
            if (region == null) {
                return NotFound();
            }

            var dto = Mapper.Map<Region, RegionDTO>(region);
            return Ok(dto);
        }

        // PUT: api/Regions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRegion(int id, Region region){
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            if (id != region.RegionID){
                return BadRequest();
            }

            try {
                if (!RegionExists(id)) {
                    return NotFound();
                }
                db.Entry(region).State = EntityState.Modified;
                await db.SaveChangesAsync();

            } catch (Exception ex) {
                throw ex;
            }

            return Ok();
        }

        // POST: api/Regions
        [ResponseType(typeof(Region))]
        public async Task<IHttpActionResult> PostRegion(Region region) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (region.RegionID == 0) {
                //既存IDの最大値+1を取得。
                region.RegionID = db.Regions.Max(i => i.RegionID) + 1;
            }

            db.Regions.Add(region);

            try {
                await db.SaveChangesAsync();
            } catch (DbUpdateException) {
                if (RegionExists(region.RegionID)) {
                    return Conflict();
                } else {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = region.RegionID }, region);
        }

        // DELETE: api/Regions/5
        [HttpPost]
        [Route("api/Regions/DeleteRegion/{id}")]
        public async Task<IHttpActionResult> DeleteRegion(int id) {
            Region region = await db.Regions.FindAsync(id);
            if (region == null) {
                return NotFound();
            }
            db.Regions.Remove(region);
            await db.SaveChangesAsync();
            return Ok();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RegionExists(int id) {
            return db.Regions.Any(e => e.RegionID == id);
        }
    }
}