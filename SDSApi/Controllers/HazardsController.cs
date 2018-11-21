using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SDSApi.Models;

namespace SDSApi.Controllers
{
    public class HazardsController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/Hazards
        public IQueryable<Hazard> GetHazards()
        {
            return db.Hazards;
        }

        // GET: api/Hazards/5
        [ResponseType(typeof(Hazard))]
        public async Task<IHttpActionResult> GetHazard(int id)
        {
            Hazard hazard = await db.Hazards.FindAsync(id);
            if (hazard == null)
            {
                return NotFound();
            }

            return Ok(hazard);
        }

        // PUT: api/Hazards/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHazard(int id, Hazard hazard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hazard.hid)
            {
                return BadRequest();
            }

            db.Entry(hazard).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HazardExists(id))
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

        // POST: api/Hazards
        [ResponseType(typeof(Hazard))]
        public async Task<IHttpActionResult> PostHazard(Hazard hazard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Hazards.Add(hazard);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = hazard.hid }, hazard);
        }

        // DELETE: api/Hazards/5
        [ResponseType(typeof(Hazard))]
        public async Task<IHttpActionResult> DeleteHazard(int id)
        {
            Hazard hazard = await db.Hazards.FindAsync(id);
            if (hazard == null)
            {
                return NotFound();
            }

            db.Hazards.Remove(hazard);
            await db.SaveChangesAsync();

            return Ok(hazard);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HazardExists(int id)
        {
            return db.Hazards.Count(e => e.hid == id) > 0;
        }
    }
}
/**
namespace SDSApi.Controllers
{
    public class HazardsController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/Hazards
        public IQueryable<Hazard> GetHazards()
        {
            return db.Hazards;
        }

        // GET: api/Hazards/5
        [ResponseType(typeof(Hazard))]
        public async Task<IHttpActionResult> GetHazard(int id)
        {
            Hazard hazard = await db.Hazards.FindAsync(id);
            if (hazard == null)
            {
                return NotFound();
            }

            return Ok(hazard);
        }

        // PUT: api/Hazards/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHazard(int id, Hazard hazard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hazard.hid)
            {
                return BadRequest();
            }

            db.Entry(hazard).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HazardExists(id))
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

        // POST: api/Hazards
        [ResponseType(typeof(Hazard))]
        public async Task<IHttpActionResult> PostHazard(Hazard hazard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Hazards.Add(hazard);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = hazard.hid }, hazard);
        }

        // DELETE: api/Hazards/5
        [ResponseType(typeof(Hazard))]
        public async Task<IHttpActionResult> DeleteHazard(int id)
        {
            Hazard hazard = await db.Hazards.FindAsync(id);
            if (hazard == null)
            {
                return NotFound();
            }

            db.Hazards.Remove(hazard);
            await db.SaveChangesAsync();

            return Ok(hazard);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HazardExists(int id)
        {
            return db.Hazards.Count(e => e.hid == id) > 0;
        }
    }
}
    **/
