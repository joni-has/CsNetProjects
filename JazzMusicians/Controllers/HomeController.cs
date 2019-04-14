using JazzMusicians.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JazzMusicians.Controllers
{
    public class HomeController : Controller
    {
        private JazzMusicianEntities _db = new JazzMusicianEntities();
        // GET: Home
        public ActionResult Index(string artistInstrument, string searchName)
        {
            //instrument search
            var artistList = new List<string>();
            var instrumentQuery = from artistInstrumentData in _db.Musicians
                                  orderby artistInstrumentData.Instruments
                                  select artistInstrumentData.Instruments;

            artistList.AddRange(instrumentQuery.Distinct());
            ViewBag.artistInstrument = new SelectList(artistList);


            //name search
            var artists = from a in _db.Musicians select a;

            if (!String.IsNullOrEmpty(searchName))
            {
                //if 'a' contains any letters from the searchName(Artist Name) then it will return searched artists in view.
                artists = artists.Where(a => a.Name.Contains(searchName));
            }

            if (!String.IsNullOrEmpty(artistInstrument))
            {
                artists = artists.Where(a => a.Instruments == artistInstrument);
            }
            return View(artists);
        }

        public new ActionResult Profile (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Musician musician = _db.Musicians.Find(id);
            if(musician == null)
            {
                return HttpNotFound();
            }
            return View(musician);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Id, Thumbnail, Name, Instruments, Years, Description")] Musician musician)
        {
            if(musician.Thumbnail == null)
            {
                musician.Thumbnail = "http://tinyurl.com/hzeag2q";
            }

            if(musician.Years == null)
            {
                musician.Years = "-";
            }

            if(musician.Description == null)
            {
                musician.Description = "Biography has not been added to this artist.";
            }

            if (ModelState.IsValid)
            {
                _db.Musicians.Add(musician);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(musician);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Musician musician = _db.Musicians.Find(id);
            if(musician == null)
            {
                return HttpNotFound();
            }
            return View(musician);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Name, Thumbnail, Instruments, Years, Description")] Musician musician)
        {
            if(musician.Thumbnail == null)
            {
                musician.Thumbnail = "http://tinyurl.com/hzeag2q";
            }

            if(ModelState.IsValid)
            {
                _db.Entry(musician).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(musician);
        }

        public ActionResult Delete(int? id)
        {
            Musician musician = _db.Musicians.Find(id);
            return View(musician);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            //finding the musician by ID
            Musician musician = _db.Musicians.Find(id);

            //deleting the musician from the database
            _db.Musicians.Remove(musician);

            _db.SaveChanges();

            //return to the index once process is complete
            return RedirectToAction("Index");
        }
    }
}