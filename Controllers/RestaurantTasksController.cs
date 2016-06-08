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
    [Authorize]
    public class RestaurantTasksController : ApiController
    {
        private YourReservationEntities3 db = new YourReservationEntities3();

        // GET: api/RestaurantTasks
        public IQueryable<RestaurantTask> GetRestaurantTasks()
        {
            return db.RestaurantTasks;
        }

        // GET: api/RestaurantTasks/5
        [ResponseType(typeof(RestaurantTask))]
        public IHttpActionResult GetRestaurantTask(int id)
        {
            RestaurantTask restaurantTask = db.RestaurantTasks.Find(id);
            if (restaurantTask == null)
            {
                return NotFound();
            }

            return Ok(restaurantTask);
        }

        [HttpGet]
        [Route("api/RestaurantTasks/getByRestaurantID/{ID}")]
        public IOrderedQueryable getByRestaurantID(int ID)
        {
            var query = from t in db.RestaurantTasks
                        where t.RestaurantID == ID
                        select t;

            return (IOrderedQueryable)query;
        }

        // PUT: api/RestaurantTasks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurantTask(int id, RestaurantTask restaurantTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurantTask.RestaurantTaskID)
            {
                return BadRequest();
            }

            db.Entry(restaurantTask).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantTaskExists(id))
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

        // POST: api/RestaurantTasks
        [ResponseType(typeof(RestaurantTask))]
        public IHttpActionResult PostRestaurantTask(RestaurantTask restaurantTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RestaurantTasks.Add(restaurantTask);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurantTask.RestaurantTaskID }, restaurantTask);
        }

        // DELETE: api/RestaurantTasks/5
        [ResponseType(typeof(RestaurantTask))]
        public IHttpActionResult DeleteRestaurantTask(int id)
        {
            RestaurantTask restaurantTask = db.RestaurantTasks.Find(id);
            if (restaurantTask == null)
            {
                return NotFound();
            }

            db.RestaurantTasks.Remove(restaurantTask);
            db.SaveChanges();

            return Ok(restaurantTask);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RestaurantTaskExists(int id)
        {
            return db.RestaurantTasks.Count(e => e.RestaurantTaskID == id) > 0;
        }
    }
}