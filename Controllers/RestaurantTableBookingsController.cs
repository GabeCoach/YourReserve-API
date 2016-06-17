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
using System.Web.Http.Cors;

namespace YourReservation.Controllers
{
    [EnableCors(origins: "http://localhost:62012", headers: "*", methods: "*")]
    public class RestaurantTableBookingsController : ApiController
    {
        private YourReservationEntities3 db = new YourReservationEntities3();

        // GET: api/RestaurantTableBookings
        public IQueryable<RestaurantTableBooking> GetRestaurantTableBookings()
        {
            return db.RestaurantTableBookings;
        }

        // GET: api/RestaurantTableBookings/5
        [ResponseType(typeof(RestaurantTableBooking))]
        public IHttpActionResult GetRestaurantTableBooking(int id)
        {
            RestaurantTableBooking restaurantTableBooking = db.RestaurantTableBookings.Find(id);
            if (restaurantTableBooking == null)
            {
                return NotFound();
            }

            return Ok(restaurantTableBooking);
        }

        // PUT: api/RestaurantTableBookings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurantTableBooking(int id, RestaurantTableBooking restaurantTableBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurantTableBooking.TableBookingID)
            {
                return BadRequest();
            }

            db.Entry(restaurantTableBooking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantTableBookingExists(id))
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

        // POST: api/RestaurantTableBookings
        [ResponseType(typeof(RestaurantTableBooking))]
        public IHttpActionResult PostRestaurantTableBooking(RestaurantTableBooking restaurantTableBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RestaurantTableBookings.Add(restaurantTableBooking);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurantTableBooking.TableBookingID }, restaurantTableBooking);
        }

        // DELETE: api/RestaurantTableBookings/5
        [ResponseType(typeof(RestaurantTableBooking))]
        public IHttpActionResult DeleteRestaurantTableBooking(int id)
        {
            RestaurantTableBooking restaurantTableBooking = db.RestaurantTableBookings.Find(id);
            if (restaurantTableBooking == null)
            {
                return NotFound();
            }

            db.RestaurantTableBookings.Remove(restaurantTableBooking);
            db.SaveChanges();

            return Ok(restaurantTableBooking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RestaurantTableBookingExists(int id)
        {
            return db.RestaurantTableBookings.Count(e => e.TableBookingID == id) > 0;
        }

        //public IOrderedQueryable getTableAvailability(DateTime Date, TimeSpan Time)
        //{
        //    var queryOne = from r in db.RestaurantTableBookings
        //                   join Table in db.RestaurantTableConfigs on r.TableConfigID equals Table.TableConfigID into TableConfig
        //                   join Res in db.Reservations on r.ReservationID equals Res.ReservationID 
        //                   join Stat in db.RestaurantReservationStatus on r.ReservationStatusID equals Stat.ReservationStatusID into Status
        //                   where Date == Res.ReservationDate && Time == Res.ReservationTime
        //}
    }
}