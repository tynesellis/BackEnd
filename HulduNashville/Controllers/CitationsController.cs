using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HulduNashville.Data;
using HulduNashville.Models;
using Microsoft.AspNetCore.Authorization;

namespace HulduNashville.Controllers
{
    [Authorize(Roles = "Administrator, Contributor")]

    public class CitationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Citations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Citation.ToListAsync());
        }

        // GET: Citations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citation = await _context.Citation
                .SingleOrDefaultAsync(m => m.Id == id);
            if (citation == null)
            {
                return NotFound();
            }

            return View(citation);
        }

        // GET: Citations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Citations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Source")] Citation citation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(citation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(citation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HiddenCreate([Bind("Id,Source")] Citation citation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(citation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Markers");
            }
            return View(citation);
        }

        // GET: Citations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citation = await _context.Citation.SingleOrDefaultAsync(m => m.Id == id);
            if (citation == null)
            {
                return NotFound();
            }
            return View(citation);
        }

        // POST: Citations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Source")] Citation citation)
        {
            if (id != citation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(citation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitationExists(citation.Id))
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
            return View(citation);
        }

        // GET: Citations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citation = await _context.Citation
                .SingleOrDefaultAsync(m => m.Id == id);
            if (citation == null)
            {
                return NotFound();
            }

            return View(citation);
        }

        // POST: Citations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var citation = await _context.Citation.SingleOrDefaultAsync(m => m.Id == id);
            _context.Citation.Remove(citation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitationExists(int id)
        {
            return _context.Citation.Any(e => e.Id == id);
        }
    }
}
