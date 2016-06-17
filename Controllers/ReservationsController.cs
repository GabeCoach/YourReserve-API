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
using ReportingPDF;
using System.Threading.Tasks;
using System.Web.Http.Cors;


namespace YourReservation.Controllers
{
    [EnableCors(origins: "http://localhost:62012", headers: "*", methods: "*")]
    public class ReservationsController : ApiController
    {
        private YourReservationEntities3 db = new YourReservationEntities3();

        // GET: api/Reservations
        public IQueryable<Reservation> GetReservations()
        {
            return db.Reservations;
        }

        // GET: api/Reservations/5
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult GetReservation(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        [Authorize]
        // PUT: api/Reservations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReservation(int id, Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reservation.ReservationID)
            {
                return BadRequest();
            }

            db.Entry(reservation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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

        // POST: api/Reservations
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult PostReservation(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int TableAssignment = assignTable(Convert.ToInt32(reservation.PartyNumber));

            reservation.RestaurantTableConfigID = TableAssignment;

            db.Reservations.Add(reservation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = reservation.ReservationID }, reservation);
        }

        [Authorize]
        // DELETE: api/Reservations/5
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult DeleteReservation(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }

            db.Reservations.Remove(reservation);
            db.SaveChanges();

            return Ok(reservation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservationExists(int id)
        {
            return db.Reservations.Count(e => e.ReservationID == id) > 0;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getReservationsByID/{ID}")]
        public IOrderedQueryable getReservationsByID(int ID)
        {
            var query = from r in db.Reservations
                        where r.RestaurantID == ID
                        select r;

            return (IOrderedQueryable)query;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getByID/{ID}")]
        public IOrderedQueryable getByID(int ID)
        {
            var query = from r in db.Reservations
                        where r.ReservationID == ID
                        select r;

            return (IOrderedQueryable)query;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getTotalDayReservations/{ID}")]
        public int getTotalDayReservations(int ID)
        {
            DateTime today = DateTime.Today;

            var query = (from r in db.Reservations
                         where r.ReservationDate == today && r.RestaurantID == ID
                         select r).Count();

            return (int)query;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getTotalWeekReservations/{ID}")]
        public int getTotalWeekReservations(int ID)
        {
            DateTime startOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
            DateTime endOfWeek = DateTime.Today.AddDays(1 * (int)(DateTime.Today.DayOfWeek));

            var query = (from r in db.Reservations
                         where r.ReservationDate >= startOfWeek && r.ReservationDate < endOfWeek && r.RestaurantID == ID
                        select r).Count();

            return (int)query;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getTotalMonthReservations/{ID}")]
        public int getTotalMonthReservations(int ID)
        {

            int iMonthStart = Convert.ToInt32(DateTime.Now.Month.ToString());
            int iMonthEnd = Convert.ToInt32(DateTime.Now.Month.ToString()) + 1;

            int iYearStart = Convert.ToInt32(DateTime.Now.Year.ToString());
            int iYearEnd = Convert.ToInt32(DateTime.Now.Year.ToString());

            if (iMonthStart > 12)
            {
                iMonthEnd = 1;
                iYearEnd = iYearEnd + 1;
            }

            string sStartDate = iMonthStart.ToString() + "/01/" + iYearStart.ToString();
            string sEndDate = iMonthEnd.ToString() + "/01/" + iYearEnd.ToString();

            DateTime dtStartDate = Convert.ToDateTime(sStartDate);
            DateTime dtEndDate = Convert.ToDateTime(sEndDate);

            var query = (from r in db.Reservations
                         where r.ReservationDate >= dtStartDate && r.ReservationDate < dtEndDate && r.RestaurantID == ID
                         select r).Count();

            return (int)query;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getAverageReservationsPerDay/{ID}")]
        public int getAverageReservationsPerDay(int ID)
        {

            var startDate = from r in db.Restaurants
                            select r.DateRegistered;

            var itemDates = startDate.FirstOrDefault();

            int avgReservationsPerDay = 0;

            if (itemDates != null)
            {

                var endDate = DateTime.Now;

                var numDays = ((TimeSpan)(endDate - itemDates)).TotalDays;

                var totalReservations = (from r in db.Reservations
                                         where r.RestaurantID == ID
                                         select r).Count();

                avgReservationsPerDay = totalReservations / Convert.ToInt32(numDays);
            }
            return avgReservationsPerDay;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getAverageReservationsPerWeek/{ID}")]
        public int getAverageReservationsPerWeek(int ID)
        {
            var dtRegistedDate = from r in db.Restaurants
                                 where r.RestaurantID == ID
                                 select r.DateRegistered;

            var totalReservations = (from r in db.Reservations
                                     where r.RestaurantID == ID
                                     select r).Count();

            var itemDates = dtRegistedDate.FirstOrDefault();

            int avgReservationsPerWeek = 0;

            if(itemDates != null)
            {
                var dtToday = DateTime.Now;

                var NumOfDays = ((TimeSpan)(dtToday - itemDates)).TotalDays;

                double numWeeks = NumOfDays / 7.0;

                avgReservationsPerWeek = totalReservations / Convert.ToInt32(numWeeks);
            }
           

            return avgReservationsPerWeek;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getAverageReservationPerMonth/{ID}")]
        public int getAverageReservationPerMonth(int ID)
        {
            var oRegistedDate = from r in db.Restaurants
                                where r.RestaurantID == ID
                                 select r.DateRegistered;

            var totalReservations = (from r in db.Reservations
                                     where r.RestaurantID == ID
                                     select r).Count();

            var itemDates = oRegistedDate.FirstOrDefault();

            DateTime dtDateRegistered = Convert.ToDateTime(itemDates);

            var numOfMonths = 0;

            if(itemDates != null)
            {
                var thisMonth = DateTime.Now;

                var Span = thisMonth - dtDateRegistered;

                DateTime Age = DateTime.MinValue + Span;

                int monthDiff = Age.Month;

                numOfMonths = totalReservations / monthDiff;
            }

            return numOfMonths;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getTotalReservationsEachMonth/{ID}")]
        public IOrderedQueryable getTotalReservationsEachMonth(int ID)
        {
            var monthReservationCount = from r in db.Reservations
                                        where r.ReservationDate != null && r.RestaurantID == ID
                                        group r by r.ReservationDate.Value.Month into rDate
                                        orderby rDate.Key ascending
                                        select new { Month = rDate.Key, Cnt = rDate.Count() };

            return (IOrderedQueryable)monthReservationCount;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getPeekTimes/{ID}")]
        public IOrderedQueryable getPeekTimes(int ID)
        {

            var query = (from r in db.Reservations
                         where r.RestaurantID == ID
                         group r by r.ReservationTime into rTime
                         select rTime.Key).Take(5);
                         
            return (IOrderedQueryable)query;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getPartyNames/{ID}")]
        public IOrderedQueryable<Reservation>getPartyNames(int ID)
        {

            var query = (from r in db.Reservations
                         where r.ReservationDate == DateTime.Now && r.RestaurantID == ID
                         select r);

            return (IOrderedQueryable<Reservation>)query;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getTopReservationPartyName/{ID}")]
        public IOrderedQueryable getTopReservationPartyName(int ID)
        {
            var query = (from customer in db.Reservations
                         where customer.RestaurantID == ID
                         group customer by new { customer.FirstName, customer.LastName } into cust
                         select new { Name = cust.Key.FirstName + " " + cust.Key.LastName, Cnt = cust.Count() }).Take(10);

            return (IOrderedQueryable)query;
        }

        [Authorize]
        [HttpPost]
        [Route("api/Reservations/PrintReport")]
        public void PrintReport()
        {
            Task<string> data = Request.Content.ReadAsStringAsync();

            var sData = data.Result;

            DataModel oData = JsonConvert.DeserializeObject<DataModel>(sData);

            List<string> Items = new List<string>();

            Items.Add(oData.ToString());

            ReservationReport oReport = new ReservationReport();

            oReport.printPDF(oData.FirstName, oData.LastName, oData.Date, oData.Time, oData.PartyAmount, oData.Email);
        }

        [HttpPost]
        [Route("api/Reservations/getAvailableTimes")]
        public List<TimeSpan> getAvailableTimes()
        {
            Task<string> data = Request.Content.ReadAsStringAsync();
            List<TimeSpan> TimeList = new List<TimeSpan>();
            

            try
            {
                string sData = data.Result;

                DataModel oData = JsonConvert.DeserializeObject<DataModel>(sData);

                var dayOfWeek = Convert.ToDateTime(oData.Date).DayOfWeek.ToString();
                var PartyAmount = Convert.ToInt32(oData.PartyAmount);

                var query = from t in db.RestaurantTimeConfigs
                            where t.Day == dayOfWeek
                            select new { ClosingTime = t.ClosingTime, OpeningTime = t.OpeningTime };

                var FirstOrDefault = query.FirstOrDefault();

                TimeSpan OpeningTime = TimeSpan.Zero;
                TimeSpan ClosingTime = TimeSpan.Zero;

                int Count = 0;
                int Count2 = 0;


                while (query != null && Count < query.Count())
                {
                    foreach (var Time in query)
                    {
                        OpeningTime = (TimeSpan)Time.OpeningTime;
                        ClosingTime = (TimeSpan)Time.ClosingTime;

                        if(Convert.ToDateTime(oData.Date) > DateTime.Now)
                        {

                            //var TableQuery = from b in db.RestaurantTableConfigs
                            //                 where b.NumberOfSeats >= PartyAmount
                            //                 orderby b.NumberOfSeats ascending
                            //                 select b;

                            //List<RestaurantTableConfig> TableList = TableQuery.ToList();
                            //var index = 0;


                            //for(TimeSpan i = OpeningTime; i < ClosingTime; index++)
                            //{

                            //}

                            while (OpeningTime < ClosingTime)
                            {
                                var TableQuery = from b in db.RestaurantTableConfigs
                                                 where b.NumberOfSeats >= PartyAmount
                                                 orderby b.NumberOfSeats ascending
                                                 select b;

                                int TableID = 0;
                                int ResTableID = 0;
                                TimeSpan ResTime = TimeSpan.Zero;

                                foreach (var table in TableQuery)
                                {
                                    var reservation = from r in db.Reservations
                                                      where r.RestaurantTableConfigID != null
                                                      select r;

                                    TableID = table.TableConfigID;

                                    foreach (var res in reservation)
                                    {
                                        ResTableID = (int)res.RestaurantTableConfigID;
                                        break;
                                    }
                                    break;
                                }

                                if (TableID != ResTableID)
                                {
                                    TimeList.Add(OpeningTime);
                                    OpeningTime = OpeningTime + TimeSpan.FromMinutes(30);
                                }
                            }
                        }
                        else
                        {
                            while (OpeningTime <= ClosingTime && OpeningTime > DateTime.Now.TimeOfDay)
                            {
                                var TableQuery = from b in db.RestaurantTableConfigs
                                                 where b.NumberOfSeats >= PartyAmount
                                                 orderby b.NumberOfSeats ascending
                                                 select b;

                                foreach (var table in TableQuery)
                                {
                                    var reservation = from c in db.Reservations
                                                      select c;

                                    foreach (var res in reservation)
                                    {
                                        {
                                            if (table.TableConfigID != res.RestaurantTableConfigID && OpeningTime != res.ReservationTime)
                                            { 
                                                TimeList.Add(OpeningTime);                                              
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        
                    }

                    OpeningTime = OpeningTime + TimeSpan.FromMinutes(30);
                    Count = Count++;
                } 
            }
            catch(Exception ex)
            {
                //return Content(HttpStatusCode.BadRequest, ex.Message);
            }

            string sReturn = TimeList.ToString();

            return TimeList;
        }

        private int assignTable(int PartyAmount)
        {
            int ConfigNumber = 0;

            var reservation = from r in db.Reservations
                              select r;

            foreach(var res in reservation)
            {
                 var TableQuery = from b in db.RestaurantTableConfigs
                              where b.NumberOfSeats >= PartyAmount && b.TableConfigID != res.RestaurantTableConfigID
                              orderby b.NumberOfSeats ascending
                              select b.TableConfigID;

                 ConfigNumber = TableQuery.FirstOrDefault();
            }
            return ConfigNumber;
        }

        [Authorize]
        [HttpPost]
        [Route("api/Reservations/getTableAssignments/{ID}")]
        public IOrderedQueryable getTableAssignments(int ID)
        {
            Task<string> data = Request.Content.ReadAsStringAsync();
            string sData = data.Result;
            DataModel oData = JsonConvert.DeserializeObject<DataModel>(sData);

            TimeSpan tsTime = TimeSpan.Parse(oData.Time);
            DateTime dtDate = Convert.ToDateTime(oData.Date);

            var getAssignments = from r in db.Reservations
                                 where r.ReservationTime == tsTime && r.ReservationDate == dtDate && r.RestaurantID == ID
                                 join d in db.RestaurantTableConfigs on r.RestaurantTableConfigID equals d.TableConfigID
                                 select new { Name = r.FirstName + " " + r.LastName, TableNumber = d.TableNumber, Time = r.ReservationTime };

            return (IOrderedQueryable)getAssignments;
        }

        [Authorize]
        [HttpGet]
        [Route("api/Reservations/getCustomerReservations/{UserID}")]
        public IOrderedQueryable getCustomerReservations(int UserID)
        {
            var query = from r in db.Reservations
                        where r.UserID == UserID
                        join t in db.Restaurants on r.RestaurantID equals t.RestaurantID
                        select new { Restaurant = t.RestaurantName, Date = r.ReservationDate, Time = r.ReservationTime, PartyAmount = r.PartyNumber };

            return (IOrderedQueryable)query;
        }
    }
}

public class DataModel
{
    public string Date { get; set; }
    public string Time { get; set; }
    public string PartyAmount { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

