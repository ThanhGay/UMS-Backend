using Microsoft.EntityFrameworkCore;
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
            var existTeacher = _dbContext.LopHP_Teachers.Any(t =>
                t.TeacherId == input.TeacherId && t.LopHpId == input.LopHpId
            );

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
                var totalExistLesson =
                    existClassHP.TotalLessons
                    - _dbContext.LopHP_Rooms.Count(r => r.LopHpId == input.LopHpId);

                List<DateOnly> daysInTimes = new List<DateOnly>();

                if (input.StartAt > input.EndAt)
                {
                    throw new ArgumentException("Ngày bắt đầu phải lớn hơn ngày kết thúc.");
                }

                // Iterate through each day in the range
                for (DateOnly date = input.StartAt; date <= input.EndAt; date = date.AddDays(1))
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
                    foreach (DateOnly date in daysInTimes)
                    {
                        var exist = _dbContext.LopHP_Rooms.Any(s =>
                            s.StartAt == date
                            && s.CaHoc == input.CaHoc
                            && s.LopHpId == input.LopHpId
                        );
                        if (exist)
                        {
                            throw new Exception(
                                $"Đã có lịch học ca {input.CaHoc} tại phòng này vào ngày {date}"
                            );
                        }
                        else
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
                    }
                }
                else
                {
                    throw new Exception(
                        $"Số buổi chèn vào ({totalLessonInput}) nhiều hơn tổng số buổi học của lớp ({existClassHP.TotalLessons}). Hiện còn {totalExistLesson} buổi."
                    );
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
                    )
                    .OrderBy(sc => sc.StartAt)
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
            }
            else
            {
                throw new Exception("Không tìm thấy");
            }
        }

        public List<ScheduleDto> ScheduleOfTeacher(string teacherId)
        {
            /*
             * Query SQL
             *
             * Declare @TeacherId nvarchar(255) = '2165'
             * print @TeacherId
             *
             * select A.Id, B.ClassName, A.CaHoc, Room.Id as RoomId, A.StartAt, C.Name as SubjectName, A.Status
             * from LopHP_Room as A
             *      join Room on A.RoomId = Room.Id
             *      join LopHP as B on A.LopHpId = B.Id
             *      join Subject as C on B.MaMonHoc = C.Id
             * where TeacherId = @TeacherId
             * order by A.StartAt, A.CaHoc
             *
             */

            var existTeacherSchedule = _dbContext.LopHP_Rooms.Any(c => c.TeacherId == teacherId);
            if (existTeacherSchedule)
            {
                var resultQuery =
                    from a in _dbContext.LopHP_Rooms
                    join r in _dbContext.Rooms on a.RoomId equals r.Id
                    join b in _dbContext.ClassHPs on a.LopHpId equals b.Id
                    where a.TeacherId == teacherId
                    orderby a.StartAt, a.CaHoc
                    select new ScheduleDto
                    {
                        ScheduleId = a.Id,
                        LopHpId = a.LopHpId,
                        ClassName = b.ClassName,
                        CaHoc = a.CaHoc,
                        RoomName = r.Name + '.' + r.Building,
                        StartAt = a.StartAt,
                        SubjectName = b.TenMonHoc,
                        Status = a.Status,
                    };

                return [.. resultQuery];
            }
            else
            {
                throw new Exception($"Mã giảng viên không tồn tại hoặc không chính xác.");
            }
        }

        public List<ScheduleDto> ScheduleOfStudent(string stuId)
        {
            /*
             * Query SQL
             *
             * Declare @StudentId nvarchar(255) = '314269'
             * print @StudentId
             *
             * select B.Id, A.LopHpId, C.ClassName, C.MaMonHoc, C.TenMonHoc, R.Name, R.Building, B.CaHoc, B.StartAt, B.Status	
             * from LopHP_Student as A 
             *      join LopHP_Room as B on A.LopHpId = B.LopHpId 
             *      join LopHP as C on A.LopHpId = C.Id 
             *      join Room as R on B.RoomId = R.Id
             * where A.StudentId = @StudentId
             * order by B.StartAt, B.CaHoc
             *
             */

            var existStudentInClass = _dbContext.LopHP_Students.Any(c => c.StudentId == stuId);
            if (existStudentInClass)
            {
                var resultQuery =
                    from A in _dbContext.LopHP_Students
                    join B in _dbContext.LopHP_Rooms on A.LopHpId equals B.LopHpId
                    join C in _dbContext.ClassHPs on A.LopHpId equals C.Id
                    join R in _dbContext.Rooms on B.RoomId equals R.Id
                    where A.StudentId == stuId
                    orderby B.StartAt, B.CaHoc
                    select new ScheduleDto
                    {
                        ScheduleId = B.Id,
                        LopHpId = A.LopHpId,
                        ClassName = C.ClassName,
                        MaMonHoc = C.MaMonHoc,
                        SubjectName = C.TenMonHoc,
                        RoomName = R.Name + '.' + R.Building,
                        CaHoc = B.CaHoc,
                        StartAt = B.StartAt,
                        Status = B.Status,
                    };
                return [.. resultQuery];
            }
            else
            {
                throw new Exception($"Sinh viên chưa tham gia lớp học nào hoặc mã sinh viên không chính xác.");
            }
        }
    }
}
