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
    public class RestaurantContactsController : ApiController
    {
        private YourReservationEntities1 db = new YourReservationEntities1();

        // GET: api/RestaurantContacts
        public IQueryable<RestaurantContact> GetRestaurantContacts()
        {
            return db.RestaurantContacts;
        }

        // GET: api/RestaurantContacts/5
        [ResponseType(typeof(RestaurantContact))]
        public IHttpActionResult GetRestaurantContact(int id)
        {
            RestaurantContact restaurantContact = db.RestaurantContacts.Find(id);
            if (restaurantContact == null)
            {
                return NotFound();
            }

            return Ok(restaurantContact);
        }

        // PUT: api/RestaurantContacts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurantContact(int id, RestaurantContact restaurantContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurantContact.RestaurantContactID)
            {
                return BadRequest();
            }

            db.Entry(restaurantContact).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantContactExists(id))
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

        // POST: api/RestaurantContacts
        [ResponseType(typeof(RestaurantContact))]
        public IHttpActionResult PostRestaurantContact(RestaurantContact restaurantContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RestaurantContacts.Add(restaurantContact);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurantContact.RestaurantContactID }, restaurantContact);
        }

        // DELETE: api/RestaurantContacts/5
        [ResponseType(typeof(RestaurantContact))]
        public IHttpActionResult DeleteRestaurantContact(int id)
        {
            RestaurantContact restaurantContact = db.RestaurantContacts.Find(id);
            if (restaurantContact == null)
            {
                return NotFound();
            }

            db.RestaurantContacts.Remove(restaurantContact);
            db.SaveChanges();

            return Ok(restaurantContact);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RestaurantContactExists(int id)
        {
            return db.RestaurantContacts.Count(e => e.RestaurantContactID == id) > 0;
        }
    }
}