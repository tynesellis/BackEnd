using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HulduNashville.Data;
using HulduNashville.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace HulduNashville.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comment.Include(c => c.Marker);
            return View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        //GET: List of Markers
        public async Task<JsonResult> GetComments()
        {
            List<UserCommentViewModel> UserComments = new List<UserCommentViewModel>();
            var comments = await _context.Comment.ToListAsync();
            var users = await _context.Users.ToListAsync();
            comments.ForEach(c =>
            {
                var SpecificUser = users.Find(u => u.Id == c.User.Id);
                UserCommentViewModel NewModel = new UserCommentViewModel()
                {
                    CommentId = c.Id,
                    CommentString = c.CommentText,
                    MarkerId = c.MarkerId,
                    UserName = SpecificUser.UserName
                };
                UserComments.Add(NewModel);
            });
            return Json(UserComments);
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Marker)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["MarkerId"] = new SelectList(_context.Marker, "Id", "Address");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment, int mId)
        {
            ModelState.Remove("MarkerId");
            ModelState.Remove("User");
            comment.MarkerId = mId;

            if (ModelState.IsValid)
            {
                comment.User = await _userManager.GetUserAsync(HttpContext.User);
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["MarkerId"] = new SelectList(_context.Marker, "Id", "Address", comment.MarkerId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommentText,MarkerId")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            ViewData["MarkerId"] = new SelectList(_context.Marker, "Id", "Address", comment.MarkerId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Marker)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.Id == id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }
    }
}
