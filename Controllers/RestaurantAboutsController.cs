using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using YourReservation.Models;

namespace YourReservation.Controllers
{
    public class RestaurantAboutsController : ApiController
    {
        private YourReservationEntities1 db = new YourReservationEntities1();

        // GET: api/RestaurantAbouts
        public IQueryable<RestaurantAbout> GetRestaurantAbouts()
        {
            return db.RestaurantAbouts;
        }

        // GET: api/RestaurantAbouts/5
        [ResponseType(typeof(RestaurantAbout))]
        public IHttpActionResult GetRestaurantAbout(int id)
        {
            RestaurantAbout restaurantAbout = db.RestaurantAbouts.Find(id);
            if (restaurantAbout == null)
            {
                return NotFound();
            }

            return Ok(restaurantAbout);
        }

        // PUT: api/RestaurantAbouts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurantAbout(int id, RestaurantAbout restaurantAbout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurantAbout.RestaurantAboutID)
            {
                return BadRequest();
            }

            db.Entry(restaurantAbout).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantAboutExists(id))
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

        // POST: api/RestaurantAbouts
        [ResponseType(typeof(RestaurantAbout))]
        public IHttpActionResult PostRestaurantAbout(RestaurantAbout restaurantAbout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RestaurantAbouts.Add(restaurantAbout);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurantAbout.RestaurantAboutID }, restaurantAbout);
        }

        // DELETE: api/RestaurantAbouts/5
        [ResponseType(typeof(RestaurantAbout))]
        public IHttpActionResult DeleteRestaurantAbout(int id)
        {
            RestaurantAbout restaurantAbout = db.RestaurantAbouts.Find(id);
            if (restaurantAbout == null)
            {
                return NotFound();
            }

            db.RestaurantAbouts.Remove(restaurantAbout);
            db.SaveChanges();

            return Ok(restaurantAbout);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RestaurantAboutExists(int id)
        {
            return db.RestaurantAbouts.Count(e => e.RestaurantAboutID == id) > 0;
        }
    }
}