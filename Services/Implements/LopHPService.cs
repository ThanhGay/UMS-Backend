using Server.DbContexts;
using Server.Dtos.Common;
using Server.Dtos.LopHP;
using Server.Entities;
using Server.Exceptions;
using Server.Services.Interfaces;
using System;

namespace Server.Services.Implements
{
    public class LopHPService : ILopHPService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBaseService _baseService;

        public LopHPService(ApplicationDbContext dbContext, IBaseService baseService)
        {
            _dbContext = dbContext;
            _baseService = baseService;
        }

        public void CreateLopHP(CreateLopHPDto input)
        {
           var existSubject = _baseService.FindSubjectById(input.SubjectId);
            if (existSubject != null)
            {
                var newClass = new LopHP
                {
                    ClassName = input.ClassName,
                    SubjectId = input.SubjectId,
                    TeacherId = input.TeacherId,
                    SoTinChi = existSubject.SoTinChi,
                    TotalLessons = existSubject.SoTinChi * 5,
                    PricePerTinChi = input.PricePerTinChi,
                };

                _dbContext.ClassHPs.Add(newClass);
                _dbContext.SaveChanges();
            }
        }

        

        public PageResultDto<LopHpDto> GetAll(FilterDto input)
        {
            var result = new PageResultDto<LopHpDto>();

            var query = _dbContext.ClassHPs.Join(_dbContext.Subjects, c => c.SubjectId, s => s.Id, (c, s) => new 
            {
                c.Id,
                c.ClassName,
                s.MaHocPhan,
                SubjectName = s.Name,
                c.TeacherId,
                s.SoTinChi,
                c.PricePerTinChi,
                c.TotalLessons,
            });

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                query = query.Where(c => c.SubjectName.ToLower().Contains(input.Keyword.ToLower()));
            }

            var formatQuery = query.Select(c => new LopHpDto
            {
                Id = c.Id,
                MaHocPhan = c.MaHocPhan,
                SubjectName = c.SubjectName,
                TotalLesson = c.TotalLessons,
                PricePerTinChi = c.PricePerTinChi,
                ClassName = c.ClassName,
                SoTinChi = c.SoTinChi,
                TeacherId = c.TeacherId,
            });

            int totalItems = formatQuery.Count();

            formatQuery = formatQuery.Skip(input.SkipCount()).Take(input.PageSize);

            result.TotalItem = totalItems;
            result.Items = formatQuery;

            return result;
        }

        public void CreateScheduleOfLopHP(CreateScheduleOfLopHp input)
        {
            var existClassHP = _dbContext.ClassHPs.Any(c => c.Id == input.LopHpId);
            var existRoom = _dbContext.Rooms.Any(r => r.Id == input.RoomId);

            if (!existRoom)
            {
                throw new UserFriendlyException($"Không tồn tại phòng có Id {input.RoomId}");
            }
            else if (!existClassHP)
            {
                throw new UserFriendlyException($"Không tồn tại lớp học phần {input.LopHpId}");
            }
            else
            {
                List<DateTime> daysInTimes = new List<DateTime>();
                Console.WriteLine("date of week", input.DayOfWeek);

                if (input.StartAt > input.EndAt)
                {
                    throw new ArgumentException("Ngày bắt đầu phải lớn hơn ngày kết thúc.");
                }

                // Iterate through each day in the range
                for (DateTime date = input.StartAt; date <= input.EndAt; date = date.AddDays(1))
                {
                    // Check if the current day is the specified dayOfWeek
                    if (date.DayOfWeek == input.DayOfWeek)
                    {
                        Console.WriteLine("same");
                        daysInTimes.Add(date);
                    }
                }

                Console.WriteLine("Danh sách ngày", daysInTimes);

                foreach (DateTime date in daysInTimes)
                {
                    Console.WriteLine("have");
                    var newScheduleOfClassHP = new LopHP_Room
                    {
                        LopHpId = input.LopHpId,
                        RoomId = input.RoomId,
                        StartAt = date,
                        CaHoc = input.CaHoc,
                    };

                    _dbContext.lopHP_Rooms.Add(newScheduleOfClassHP);
                _dbContext.SaveChanges();
                }

            };
        }

        public List<LopHP_Room> GetSchedule(int lopHpId)
        {
            var existLopHp = _dbContext.lopHP_Rooms.Any(c => c.LopHpId == lopHpId);

            if (existLopHp)
            {
                var scheduleQuery = _dbContext.lopHP_Rooms.Where(sc => sc.LopHpId == lopHpId).ToList();
                return scheduleQuery;
            }
            else
            {
                throw new UserFriendlyException($"Lớp học phần có Id \"{lopHpId}\" chưa có lịch cụ thể");
            }
        }
    }
}
