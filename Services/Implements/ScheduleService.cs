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
            var existClassHP = _dbContext.ClassHPs.FirstOrDefault(c => c.Id == input.LopHpId);
            var existRoom = _dbContext.Rooms.Any(r => r.Id == input.RoomId);
            var existTeacher = _dbContext.LopHP_Teachers.Any(t => t.TeacherId == input.TeacherId && t.LopHpId == input.LopHpId);

            if (!existRoom)
            {
                throw new UserFriendlyException($"Không tồn tại phòng có Id {input.RoomId}");
            }
            else if (existClassHP == null)
            {
                throw new UserFriendlyException($"Không tồn tại lớp học phần {input.LopHpId}");
            }
            else if (!existTeacher)
            {
                throw new Exception(
                    $"Giảng viên \"{input.TeacherId}\" chưa được tham gia lớp học này"
                );
            }
            else
            {
                var totalExistLesson = existClassHP.TotalLessons -  _dbContext.LopHP_Rooms.Count(r => r.LopHpId == input.LopHpId);


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
                        daysInTimes.Add(date);
                    }
                }
                var totalLessonInput = daysInTimes.Count;

                if (totalLessonInput > 0 && totalLessonInput <= totalExistLesson)
                {

                    foreach (DateTime date in daysInTimes)
                    {
                        var newScheduleOfClassHP = new LopHP_Room
                        {
                            LopHpId = input.LopHpId,
                            RoomId = input.RoomId,
                            StartAt = date,
                            CaHoc = input.CaHoc,
                            TeacherId = input.TeacherId,
                        };

                        _dbContext.LopHP_Rooms.Add(newScheduleOfClassHP);
                        _dbContext.SaveChanges();
                    }
                } else
                {
                    throw new Exception($"Số buổi chèn vào ({totalLessonInput}) nhiều hơn tổng số buổi học của lớp ({existClassHP.TotalLessons}). Hiện còn {totalExistLesson} buổi.");
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
                                TeacherId = sc.TeacherId,
                            }
                    ).OrderBy(sc => sc.StartAt)
                    .ToList();
                return scheduleQuery;
            }
            else
            {
                return [];
                //throw new UserFriendlyException($"Lớp học phần có Id \"{lopHpId}\" chưa có lịch cụ thể");
            }
        }

        public void PostponeLesson(int id)
        {
            var existItem = _dbContext.LopHP_Rooms.FirstOrDefault(sc => sc.Id == id);
            if (existItem != null)
            {
                existItem.Status = 2;

                _dbContext.LopHP_Rooms.Update(existItem);
                _dbContext.SaveChanges();
            }else
            {
                throw new Exception("Không tìm thấy");
            }
        }

        public List<ScheduleTeacherDto> ScheduleOfTeacher(string teacherId)
        {
            /*
             * Query SQL
             * 
             * Declare @TeacherId nvarchar(255) = '2165'
             * print @TeacherId
             * 
             * select A.Id, B.ClassName, A.CaHoc, Room.Id as RoomId, A.StartAt, C.Name as SubjectName, A.Status 
             * from LopHP_Room as A join Room on A.RoomId = Room.Id join LopHP as B on A.LopHpId = B.Id join Subject as C on B.MaMonHoc = C.Id
             * where TeacherId = @TeacherId
             *
             */

            var existTeacherSchedule = _dbContext.LopHP_Rooms.Any(c => c.TeacherId == teacherId);
            if (existTeacherSchedule)
            {
                return [];
            }
            else
            {
                return [];
            }
        }
    }
}
