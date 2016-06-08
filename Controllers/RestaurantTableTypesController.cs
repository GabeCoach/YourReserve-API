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
    public class RestaurantTableTypesController : ApiController
    {
        private YourReservationEntities3 db = new YourReservationEntities3();

        // GET: api/RestaurantTableTypes
        public IQueryable<RestaurantTableType> GetRestaurantTableTypes()
        {
            return db.RestaurantTableTypes;
        }

        // GET: api/RestaurantTableTypes/5
        [Authorize]
        [ResponseType(typeof(RestaurantTableType))]
        public IHttpActionResult GetRestaurantTableType(int id)
        {
            RestaurantTableType restaurantTableType = db.RestaurantTableTypes.Find(id);
            if (restaurantTableType == null)
            {
                return NotFound();
            }

            return Ok(restaurantTableType);
        }

        // PUT: api/RestaurantTableTypes/5
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurantTableType(int id, RestaurantTableType restaurantTableType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurantTableType.TableTypeID)
            {
                return BadRequest();
            }

            db.Entry(restaurantTableType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantTableTypeExists(id))
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

        // POST: api/RestaurantTableTypes
        [Authorize]
        [ResponseType(typeof(RestaurantTableType))]
        public IHttpActionResult PostRestaurantTableType(RestaurantTableType restaurantTableType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RestaurantTableTypes.Add(restaurantTableType);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurantTableType.TableTypeID }, restaurantTableType);
        }

        // DELETE: api/RestaurantTableTypes/5
        [Authorize]
        [ResponseType(typeof(RestaurantTableType))]
        public IHttpActionResult DeleteRestaurantTableType(int id)
        {
            RestaurantTableType restaurantTableType = db.RestaurantTableTypes.Find(id);
            if (restaurantTableType == null)
            {
                return NotFound();
            }

            db.RestaurantTableTypes.Remove(restaurantTableType);
            db.SaveChanges();

            return Ok(restaurantTableType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RestaurantTableTypeExists(int id)
        {
            return db.RestaurantTableTypes.Count(e => e.TableTypeID == id) > 0;
        }
    }
}