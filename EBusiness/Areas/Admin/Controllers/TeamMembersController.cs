using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EBusiness.DAL;
using EBusiness.Models;
using EBusiness.Dtos.TeamMemberDtos;
using Microsoft.AspNetCore.Authorization;

namespace EBusiness.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class TeamMembersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public TeamMembersController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/TeamMembers
        public async Task<IActionResult> Index()
        {
            return View(await _context.TeamMembers.ToListAsync());
        }

        // GET: Admin/TeamMembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TeamMembers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamMemberPostDto teamMemberPostDto)
        {
            if (!ModelState.IsValid) return View(teamMemberPostDto);
            string imageName = Guid.NewGuid() + teamMemberPostDto.FormFile.FileName;
            string imagePath = Path.Combine(_env.WebRootPath, "assets/img", imageName);
            using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
            {
                teamMemberPostDto.FormFile.CopyTo(fileStream);
            }
            if(teamMemberPostDto.FormFile == null)
            {
                ModelState.AddModelError("", "File must not be null");
                return View(teamMemberPostDto);
            }
            await _context.AddAsync(new TeamMember
            {
                Name = teamMemberPostDto.Name,
                Position = teamMemberPostDto.Position,
                ImageName = imageName
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Admin/TeamMembers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            TeamMember teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember == null)
            {
                return NotFound();
            }

            TeamMemberUpdateDto teamMemberUpdateDto = new TeamMemberUpdateDto()
            {
                teamMemberGetDto = new TeamMemberGetDto()
                {
                    Id = id,
                    Name = teamMember.Name,
                    Position = teamMember.Position,
                    ImageName = teamMember.ImageName,

                }
            };

            return View(teamMemberUpdateDto);
        }

        // POST: Admin/TeamMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeamMemberUpdateDto teamMemberUpdateDto)
        {
            TeamMember teamMember = await _context.TeamMembers.FindAsync(teamMemberUpdateDto.teamMemberGetDto.Id);
            if (teamMember == null)
            {
                return NotFound();
            }

            string imageName = Guid.NewGuid() + teamMemberUpdateDto.teamMemberPostDto.FormFile.FileName;
            string imagePath = Path.Combine(_env.WebRootPath, "assets/img", imageName);
            using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
            {
                teamMemberUpdateDto.teamMemberPostDto.FormFile.CopyTo(fileStream);
            }

            teamMember.Name = teamMemberUpdateDto.teamMemberPostDto.Name;
            teamMember.Position = teamMemberUpdateDto.teamMemberPostDto.Position;
            teamMember.ImageName = imageName;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Admin/TeamMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TeamMembers == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        // POST: Admin/TeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TeamMembers == null)
            {
                return Problem("Entity set 'AppDbContext.TeamMembers'  is null.");
            }
            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember != null)
            {
                _context.TeamMembers.Remove(teamMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamMemberExists(int id)
        {
            return _context.TeamMembers.Any(e => e.Id == id);
        }
    }
}
