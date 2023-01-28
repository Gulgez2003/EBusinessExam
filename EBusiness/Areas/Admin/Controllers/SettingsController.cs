using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EBusiness.DAL;
using EBusiness.Models;
using EBusiness.Dtos.SettingDtos;

namespace EBusiness.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;

        public SettingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Settings
        public async Task<IActionResult> Index()
        {
              return View(await _context.Settings.ToListAsync());
        }

      
        public async Task<IActionResult> Edit(int id)
        {
            Setting setting = await _context.Settings.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }

            SettingUpdateDto settingUpdateDto = new SettingUpdateDto()
            {
                settingGetDto = new SettingGetDto
                {
                    Id = id,
                    Information = setting.Information,
                    PhoneNumber = setting.PhoneNumber,
                    Email = setting.Email,
                    WorkingHour = setting.WorkingHour
                }
            };
            return View(settingUpdateDto);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SettingUpdateDto settingUpdateDto)
        {
            Setting setting = await _context.Settings.FindAsync(settingUpdateDto.settingGetDto.Id);
            if (setting == null)
            {
                return NotFound();
            }

            setting.Information = settingUpdateDto.settingPostDto.Information;
            setting.PhoneNumber = settingUpdateDto.settingPostDto.PhoneNumber;
            setting.Email=settingUpdateDto.settingPostDto.Email;
            setting.WorkingHour = settingUpdateDto.settingPostDto.WorkingHour;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

       
    }
}
