using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HulduNashville.Data;
using HulduNashville.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace HulduNashville.Controllers
{
    [Authorize(Roles = "Administrator, Contributor")]

    public class MarkersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MarkersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Markers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Marker.Include(m => m.Category).Include(m => m.Citation).Include(m => m.Image);
            return View(await applicationDbContext.ToListAsync());
        }
        [AllowAnonymous]
        //GET: List of Markers
        public async Task<JsonResult> GetMarkers()
        {
            var applicationDbContext = _context.Marker.Include(m => m.Category).Include(m => m.Citation).Include(m => m.Image);

            return Json(await applicationDbContext.ToListAsync());
        }

        // GET: Markers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marker = await _context.Marker
                .Include(m => m.Category)
                .Include(m => m.Citation)
                .Include(m => m.Image)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (marker == null)
            {
                return NotFound();
            }

            return View(marker);
        }

        // GET: Markers/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Title");
            ViewData["CitationId"] = new SelectList(_context.Citation, "Id", "Source");
            ViewData["ImageId"] = new SelectList(_context.Image, "Id", "ImageName");
            return View();
        }

        // POST: Markers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Address,Lat,Lng,CategoryId,CitationId,ImageId")] Marker marker)
        {
            var client = new HttpClient();
            string address = marker.Address;
            string requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false$key=AIzaSyCjC9iCmI7i2do5GFSUYDnmiyqAShhyj4Y", Uri.EscapeDataString(address));


            var response = await client.GetAsync(requestUri);

            string textResult = await response.Content.ReadAsStringAsync();
            client.Dispose();


            try
            {
                ModelState.Remove("Lat");
                ModelState.Remove("Lng");
                ModelState.Remove("Address");
                var json = JObject.Parse(textResult);
                string lat = (string)json["results"][0]["geometry"]["location"]["lat"];
                string lng = (string)json["results"][0]["geometry"]["location"]["lng"];
                string newAddress = (string)json["results"][0]["formatted_address"];
                marker.Lat = lat;
                marker.Lng = lng;
                marker.Address = newAddress;


                if (ModelState.IsValid)
                {
                    _context.Add(marker);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            } catch
            {
               TempData["msg"] = "<script>alert('Address Did Not Validate.  Please Try Again');</script>";

            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Title", marker.CategoryId);
            ViewData["CitationId"] = new SelectList(_context.Citation, "Id", "Source", marker.CitationId);
            ViewData["ImageId"] = new SelectList(_context.Image, "Id", "ImageURL", marker.ImageId);
            return View(marker);
        }

        // GET: Markers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marker = await _context.Marker.SingleOrDefaultAsync(m => m.Id == id);
            if (marker == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Title", marker.CategoryId);
            ViewData["CitationId"] = new SelectList(_context.Citation, "Id", "Source", marker.CitationId);
            ViewData["ImageId"] = new SelectList(_context.Image, "Id", "ImageURL", marker.ImageId);
            return View(marker);
        }

        // POST: Markers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Address,Description,Lat,Lng,CategoryId,CitationId,ImageId")] Marker marker)
        {
            if (id != marker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarkerExists(marker.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Title", marker.CategoryId);
            ViewData["CitationId"] = new SelectList(_context.Citation, "Id", "Source", marker.CitationId);
            ViewData["ImageId"] = new SelectList(_context.Image, "Id", "ImageURL", marker.ImageId);
            return View(marker);
        }

        // GET: Markers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marker = await _context.Marker
                .Include(m => m.Category)
                .Include(m => m.Citation)
                .Include(m => m.Image)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (marker == null)
            {
                return NotFound();
            }

            return View(marker);
        }

        // POST: Markers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var marker = await _context.Marker.SingleOrDefaultAsync(m => m.Id == id);
            _context.Marker.Remove(marker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarkerExists(int id)
        {
            return _context.Marker.Any(e => e.Id == id);
        }
    }
}
