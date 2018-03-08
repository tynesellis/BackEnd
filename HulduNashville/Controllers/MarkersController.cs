using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HulduNashville.Data;
using HulduNashville.Models;

namespace HulduNashville.Controllers
{
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
            ViewData["ImageId"] = new SelectList(_context.Image, "Id", "ImageURL");
            return View();
        }

        // POST: Markers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,LatLong,CategoryId,CitationId,ImageId")] Marker marker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(marker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,LatLong,CategoryId,CitationId,ImageId")] Marker marker)
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
