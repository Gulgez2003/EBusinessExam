using EBusiness.DAL;
using EBusiness.Dtos.SettingDtos;
using EBusiness.Models;

namespace EBusiness.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _context;

        public SettingService(AppDbContext context)
        {
            _context = context;
        }

        public SettingGetDto GetSetting()
        {
            Setting setting = _context.Settings.FirstOrDefault();

            SettingGetDto dto = new SettingGetDto()
            {
                Information = setting.Information,
                PhoneNumber = setting.PhoneNumber,
                Email = setting.Email,
                WorkingHour = setting.WorkingHour
            };
            return dto;
        }
    }
}
