using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;

namespace WebApp.Controllers
{
    [Authorize]
    public class AuthUsersController : Controller
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UsersDbContext _context;
        public AuthUsersController(UsersDbContext context, SignInManager<Users> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }
        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Delete
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'UsersDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Block(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ActionName("Block")]
        public async Task<IActionResult> BlockConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (user is null)
            {
                return RedirectToAction("Index");
            }

            user.IsActive = false;
            await _context.SaveChangesAsync();

            return View(user);
        }

        public async Task<IActionResult> Unblock(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Unblock")]
        public async Task<IActionResult> UnblockConfirm(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (id is null)
            {
                return RedirectToAction("Index");
            }

            user.IsActive = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

    }
}
