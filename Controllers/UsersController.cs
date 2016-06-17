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
using Newtonsoft.Json;
using System.Web.Http.Cors;

namespace YourReservation.Controllers
{
    [EnableCors(origins: "http://localhost:62012", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        private YourReservationEntities1 db = new YourReservationEntities1();

        [Authorize]
        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        [Authorize]
        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var oEncrypt = new EncryptDecrypt();

            oEncrypt.HashValue = "13yH5J87m3Xx8";
            oEncrypt.SaltKey = "Gh7f8JAs308f6";
            oEncrypt.VIKey = "Hj74K45yGn28A51l";

            string sEncryptedPassword = user.Password;

            sEncryptedPassword = oEncrypt.Encrypt(user.Password);
            user.Password = sEncryptedPassword;

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }
    }
}