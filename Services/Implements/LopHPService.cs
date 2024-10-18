using System;
using Microsoft.OpenApi.Any;
using Server.DbContexts;
using Server.Dtos.Common;
using Server.Dtos.LopHP;
using Server.Entities;
using Server.Exceptions;
using Server.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public PageResultDto<GetAllLopHpDto> GetAll(FilterDto input)
        {
            var result = new PageResultDto<GetAllLopHpDto>();

            var resultQuery = _dbContext
                .ClassHPs.Join(
                    _dbContext.Subjects,
                    a => a.SubjectId,
                    b => b.Id,
                    (a, b) =>
                        new
                        {
                            a.Id,
                            a.ClassName,
                            b.MaHocPhan,
                            SubjectName = b.Name,
                            a.SoTinChi,
                            a.PricePerTinChi,
                            a.TotalLessons,
                            a.TeacherId,
                            RealityStudent = _dbContext.LopHP_Students.Count(c =>
                                c.LopHpId == a.Id
                            ),
                            a.TotalStudents,
                        }
                )
                .Select(lhp => new GetAllLopHpDto
                {
                    Id = lhp.Id,
                    ClassName = lhp.ClassName,
                    MaHocPhan = lhp.MaHocPhan,
                    RealityStudent = lhp.RealityStudent,
                    SubjectName = lhp.SubjectName,
                    TeacherId = lhp.TeacherId,
                    PricePerTinChi = lhp.PricePerTinChi,
                    SoTinChi = lhp.SoTinChi,
                    TotalLesson = lhp.TotalLessons,
                    TotalStudents = lhp.TotalStudents,
                });

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                resultQuery = resultQuery.Where(l =>
                    l.SubjectName.ToLower().Contains(input.Keyword.ToLower())
                );
            }

            int totalItems = resultQuery.Count();

            resultQuery = resultQuery.Skip(input.SkipCount()).Take(input.PageSize);

            result.TotalItem = totalItems;
            result.Items = resultQuery;

            return result;
        }

        public GetDetailLopHpDto GetDetailLopHp(int lopHpId)
        {
            var inforClassQuery = _dbContext
                .ClassHPs.Where(c => c.Id == lopHpId)
                .Join(
                    _dbContext.Subjects,
                    a => a.SubjectId,
                    b => b.Id,
                    (a, b) =>
                        new
                        {
                            a.Id,
                            a.ClassName,
                            b.MaHocPhan,
                            SubjectName = b.Name,
                            a.SoTinChi,
                            a.PricePerTinChi,
                            a.TotalLessons,
                            a.TeacherId,
                            RealityStudent = _dbContext.LopHP_Students.Count(c =>
                                c.LopHpId == a.Id
                            ),
                            a.TotalStudents,
                        }
                )
                .Select(lhp => new GetDetailLopHpDto
                {
                    Id = lhp.Id,
                    ClassName = lhp.ClassName,
                    MaHocPhan = lhp.MaHocPhan,
                    RealityStudent = lhp.RealityStudent,
                    SubjectName = lhp.SubjectName,
                    TeacherId = lhp.TeacherId,
                    PricePerTinChi = lhp.PricePerTinChi,
                    SoTinChi = lhp.SoTinChi,
                    TotalLesson = lhp.TotalLessons,
                    TotalStudents = lhp.TotalStudents,
                })
                .ToList();

            var listSchedule = GetSchedule(lopHpId);
            var listStudent = GetStudents(lopHpId);

            var result = inforClassQuery[0];
            result.Schedules = listSchedule;
            result.Students = listStudent;

            return result;
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

        public List<GetScheduleDto> GetSchedule(int lopHpId)
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

        public List<string> GetStudents(int lopHpId)
        {
            var existLopHp = _dbContext.LopHP_Students.Any(c => c.LopHpId == lopHpId);
            if (existLopHp)
            {
                var listStuQuery = _dbContext
                    .LopHP_Students.Where(c => c.LopHpId == lopHpId)
                    .Select(c => c.StudentId);

                return listStuQuery.ToList();
            }
            else
            {
                return [];
            }
        }

        public void AddStudents(AddStudentIntoLopHpDto input)
        {
            var existLopHp = _dbContext.ClassHPs.Any(c => c.Id == input.LopHpId);

            if (existLopHp)
            {
                foreach (var studentId in input.StudentIds)
                {
                    var existStudentInClass = _dbContext.LopHP_Students.Any(s =>
                        s.StudentId == studentId && s.LopHpId == input.LopHpId
                    );

                    if (!existStudentInClass)
                    {
                        var newItem = new LopHP_Student
                        {
                            LopHpId = input.LopHpId,
                            StudentId = studentId,
                        };
                        _dbContext.LopHP_Students.Add(newItem);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                throw new UserFriendlyException($"Không tồn tại lớp học phần {input.LopHpId}");
            }
        }
    }
}
