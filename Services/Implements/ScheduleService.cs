using Server.DbContexts;
using Server.Dtos.Schedule;
using Server.Entities;
using Server.Exceptions;
using Server.Services.Interfaces;

namespace Server.Services.Implements
{
    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBaseService _baseService;

        public ScheduleService(ApplicationDbContext dbContext, IBaseService baseService)
        {
            _dbContext = dbContext;
            _baseService = baseService;
        }

        public void CreateScheduleOfClassHP(CreateScheduleOfLopHp input)
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

                foreach (DateTime date in daysInTimes)
                {
                    var newScheduleOfClassHP = new LopHP_Room
                    {
                        LopHpId = input.LopHpId,
                        RoomId = input.RoomId,
                        StartAt = date,
                        CaHoc = input.CaHoc,
                    };

                    _dbContext.LopHP_Rooms.Add(newScheduleOfClassHP);
                    _dbContext.SaveChanges();
                }
            }
            ;
        }

        public List<GetScheduleDto> GetScheduleOfClassHp(int lopHpId)
        {
            var existLopHp = _dbContext.LopHP_Rooms.Any(c => c.LopHpId == lopHpId);

            if (existLopHp)
            {
                var scheduleQuery = _dbContext
                    .LopHP_Rooms.Where(sc => sc.LopHpId == lopHpId)
                    .Join(
                        _dbContext.Rooms,
                        sc => sc.RoomId,
                        r => r.Id,
                        (sc, r) =>
                            new GetScheduleDto
                            {
                                Id = sc.Id,
                                LopHpId = lopHpId,
                                RoomId = sc.RoomId,
                                RoomName = r.Name + "." + r.Building,
                                CaHoc = sc.CaHoc,
                                StartAt = sc.StartAt,
                                Status = sc.Status,
                            }
                    )
                    .ToList();
                return scheduleQuery;
            }
            else
            {
                return [];
                //throw new UserFriendlyException($"Lớp học phần có Id \"{lopHpId}\" chưa có lịch cụ thể");
            }
        }

        public List<ScheduleTeacherDto> ScheduleOfTeacher(string teacherId)
        {
            var existTeacherSchedule = _dbContext.ClassHPs.Any(c => c.TeacherId.Equals(teacherId));
            if (existTeacherSchedule)
            {
                var scheduleQuery = _dbContext.ClassHPs.Where(c => c.TeacherId.Equals(teacherId)).Join(_dbContext.LopHP_Rooms, c => c.Id, sc => sc.LopHpId, (c, sc) =>
                new
                {
                    sc.Id,
                    c.ClassName,
                    SubjectName = _dbContext.Subjects.Where(s => s.Id == c.SubjectId).Select(s => s.Name).ToList(),
                    c.TeacherId,
                    sc.LopHpId,
                    sc.CaHoc,
                    sc.StartAt,
                    sc.RoomId,
                }).Select(r => new ScheduleTeacherDto
                {
                    Id = r.Id,
                    ClassName = r.ClassName,
                    CaHoc = r.CaHoc,
                    StartAt = r.StartAt,
                    SubjectName = r.SubjectName[0],
                    RoomName = r.RoomId.ToString(),
                });

                
            return scheduleQuery.ToList();
            }
            else
            {
                return [];
            }
        }
    }
}
