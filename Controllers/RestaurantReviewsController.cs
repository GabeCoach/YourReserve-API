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
    public class RestaurantReviewsController : ApiController
    {
        private YourReservationEntities1 db = new YourReservationEntities1();

        // GET: api/RestaurantReviews
        public IQueryable<RestaurantReview> GetRestaurantReviews()
        {
            return db.RestaurantReviews;
        }

        // GET: api/RestaurantReviews/5
        [ResponseType(typeof(RestaurantReview))]
        public IHttpActionResult GetRestaurantReview(int id)
        {
            RestaurantReview restaurantReview = db.RestaurantReviews.Find(id);
            if (restaurantReview == null)
            {
                return NotFound();
            }

            return Ok(restaurantReview);
        }

        // PUT: api/RestaurantReviews/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurantReview(int id, RestaurantReview restaurantReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurantReview.RestaurantReviewID)
            {
                return BadRequest();
            }

            db.Entry(restaurantReview).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantReviewExists(id))
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

        // POST: api/RestaurantReviews
        [ResponseType(typeof(RestaurantReview))]
        public IHttpActionResult PostRestaurantReview(RestaurantReview restaurantReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RestaurantReviews.Add(restaurantReview);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurantReview.RestaurantReviewID }, restaurantReview);
        }

        // DELETE: api/RestaurantReviews/5
        [ResponseType(typeof(RestaurantReview))]
        public IHttpActionResult DeleteRestaurantReview(int id)
        {
            RestaurantReview restaurantReview = db.RestaurantReviews.Find(id);
            if (restaurantReview == null)
            {
                return NotFound();
            }

            db.RestaurantReviews.Remove(restaurantReview);
            db.SaveChanges();

            return Ok(restaurantReview);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RestaurantReviewExists(int id)
        {
            return db.RestaurantReviews.Count(e => e.RestaurantReviewID == id) > 0;
        }

        [HttpGet]
        [Route("api/RestaurantReviews/getReviewByRestaurant/{ID}")]
        public IOrderedQueryable<RestaurantReview> getReviewByRestaurant(int ID)
        {
            var query = from r in db.RestaurantReviews
                        where r.RestaurantID == ID
                        orderby r.ReviewDateTime descending
                        select r;

            return (IOrderedQueryable<RestaurantReview>)query;
        }

        [HttpGet]
        [Route("api/RestaurantReviews/getTop5Restaurants")]
        public IOrderedQueryable getTop5Restaurants()
        {
            var query = (from Rest in db.Restaurants
                         join Loc in db.RestaurantLocations on Rest.RestaurantID equals Loc.RestaurantID
                         join Rev in db.RestaurantReviews on Rest.RestaurantID equals Rev.RestaurantID
                         group Rev.Rating by Rest.RestaurantName into RatingByRestaurant
                         orderby RatingByRestaurant.Average() descending
                         select new { Restaurant = RatingByRestaurant.Key, AverageRating = RatingByRestaurant.Average() }).Take(5);
            return (IOrderedQueryable)query;
        }
    }
}