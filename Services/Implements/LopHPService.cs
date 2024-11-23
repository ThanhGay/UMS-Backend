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
        private readonly IScheduleService _scheduleService;

        public LopHPService(
            ApplicationDbContext dbContext,
            IBaseService baseService,
            IScheduleService scheduleService
        )
        {
            _dbContext = dbContext;
            _baseService = baseService;
            _scheduleService = scheduleService;
        }

        public PageResultDto<GetAllLopHpDto> GetAll(FilterDto input)
        {
            var result = new PageResultDto<GetAllLopHpDto>();

            var resultQuery = _dbContext.ClassHPs.Select(lhp => new GetAllLopHpDto
            {
                Id = lhp.Id,
                ClassName = lhp.ClassName,
                MaMonHoc = lhp.MaMonHoc,
                TenMonHoc = lhp.TenMonHoc,
                RealityStudent = _dbContext.LopHP_Students.Count(c => c.LopHpId == lhp.Id),
                TeacherIds = _dbContext
                    .LopHP_Teachers.Where(t => t.LopHpId == lhp.Id)
                    .Select(c => c.TeacherId)
                    .ToList(),
                PricePerTinChi = lhp.PricePerTinChi,
                SoTinChi = lhp.SoTinChi,
                TotalLesson = lhp.TotalLessons,
                TotalStudents = lhp.TotalStudents,
            });

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                resultQuery = resultQuery.Where(l =>
                    l.MaMonHoc.ToLower().Contains(input.Keyword.ToLower())
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
                .Select(lhp => new GetDetailLopHpDto
                {
                    Id = lhp.Id,
                    ClassName = lhp.ClassName,
                    MaMonHoc = lhp.MaMonHoc,
                    TenMonHoc = lhp.TenMonHoc,
                    RealityStudent = _dbContext.LopHP_Students.Count(c => c.LopHpId == lhp.Id),
                    PricePerTinChi = lhp.PricePerTinChi,
                    SoTinChi = lhp.SoTinChi,
                    TotalLesson = lhp.TotalLessons,
                    TotalStudents = lhp.TotalStudents,
                })
                .ToList();

            var listSchedule = _scheduleService.GetScheduleOfClassHp(lopHpId);
            var listStudent = GetStudents(lopHpId);
            var listTeacher = GetTeachers(lopHpId);

            var result = inforClassQuery[0];
            result.Schedules = listSchedule;
            result.Students = listStudent;
            result.Teachers = listTeacher;

            return result;
        }

        public void CreateLopHP(CreateLopHPDto input)
        {
            var existClass = _dbContext.ClassHPs.Any(c =>
                c.ClassName == input.ClassName && c.MaMonHoc == input.MaMonHoc
            );

            if (existClass)
            {
                throw new Exception(
                    $"Đã có lớp \"{input.ClassName}\" của môn \"{input.MaMonHoc}\"."
                );
            }
            else
            {
                var newClass = new LopHP
                {
                    ClassName = input.ClassName,
                    MaMonHoc = input.MaMonHoc,
                    TenMonHoc = input.TenMonHoc,
                    SoTinChi = input.SoTinChi,
                    TotalLessons = input.SoTinChi * 5,
                    PricePerTinChi = input.PricePerTinChi,
                };

                _dbContext.ClassHPs.Add(newClass);
                _dbContext.SaveChanges();

                if (input.TeacherIds.Length > 0)
                {
                    foreach (var item in input.TeacherIds)
                    {
                        _dbContext.LopHP_Teachers.Add(
                            new LopHP_Teacher { TeacherId = item, LopHpId = newClass.Id }
                        );
                    }

                    _dbContext.SaveChanges();
                }
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

        public List<string> GetTeachers(int lopHpId)
        {
            var existLopHp = _dbContext.LopHP_Teachers.Any(c => c.LopHpId == lopHpId);
            if (existLopHp)
            {
                var listStuQuery = _dbContext
                    .LopHP_Teachers.Where(c => c.LopHpId == lopHpId)
                    .Select(c => c.TeacherId);

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

        public void AddTeacherToLopHp(AddTeacherIntoLopHpDto input)
        {
            var existLopHp = _dbContext.ClassHPs.FirstOrDefault(c => c.Id == input.LopHpId);

            if (existLopHp != null)
            {
                var existTeacher = _dbContext.LopHP_Teachers.Any(c =>
                    c.LopHpId == input.LopHpId && c.TeacherId == input.TeacherId
                );
                if (existTeacher)
                {
                    throw new Exception(
                        $"Giảng viên \"{input.TeacherId}\" đã được thêm vào lớp học"
                    );
                }
                else
                {
                    _dbContext.LopHP_Teachers.Add(
                        new LopHP_Teacher { TeacherId = input.TeacherId, LopHpId = input.LopHpId }
                    );
                    _dbContext.SaveChanges();
                }
            }
            else
            {
                throw new UserFriendlyException($"Không tồn tại lớp học phần {input.LopHpId}");
            }
        }

        public List<AllLopHpByStudentIdDto> GetAllLopHpByStudentId(string studentId)
        {
            var existStudent = _dbContext.LopHP_Students.Any(s => s.StudentId.Equals(studentId));
            if (existStudent)
            {
                var query = _dbContext
                    .LopHP_Students.Where(s => s.StudentId.Equals(studentId))
                    .Join(
                        _dbContext.ClassHPs,
                        s => s.LopHpId,
                        c => c.Id,
                        (s, c) =>
                            new
                            {
                                c.ClassName,
                                LopHpId = c.Id,
                                c.MaMonHoc,
                                c.TenMonHoc,
                            }
                    );

                var resultQuery = query
                    .Select(r => new AllLopHpByStudentIdDto
                    {
                        TeacherIds = _dbContext
                            .LopHP_Teachers.Where(c => c.LopHpId == r.LopHpId)
                            .Select(c => c.TeacherId)
                            .ToList(),
                        ClassName = r.ClassName,
                        MaMonHoc = r.MaMonHoc,
                        LopHpId = r.LopHpId,
                        TenMonHoc = r.TenMonHoc,
                    })
                    .ToList();

                return resultQuery;
            }
            else
            {
                return [];
            }
        }

        public void DeleteLopHP(int lopHpId)
        {
            var existClass = _dbContext.ClassHPs.FirstOrDefault(c => c.Id == lopHpId);

            if (existClass != null)
            {
                var existStudentInClass = _dbContext
                    .LopHP_Students.Where(s => s.LopHpId == lopHpId)
                    .Count();
                Console.WriteLine("So luong Sinh vien", existStudentInClass);
                if (existStudentInClass > 0)
                {
                    if (existStudentInClass < 10)
                    {
                        var listStudentInClass = _dbContext.LopHP_Students.Where(s =>
                            s.LopHpId == lopHpId
                        );
                        var listTeacherInClass = _dbContext.LopHP_Teachers.Where(t =>
                            t.LopHpId == lopHpId
                        );
                        var listSchedule = _dbContext.LopHP_Rooms.Where(r => r.LopHpId == lopHpId );

                        foreach (var item in listStudentInClass)
                        {
                            Console.WriteLine("sv", item.StudentId);
                        }

                        foreach (var item in listTeacherInClass)
                        {
                            Console.WriteLine("teacher", item.TeacherId);
                        }

                        _dbContext.LopHP_Students.RemoveRange(listStudentInClass);
                        _dbContext.LopHP_Teachers.RemoveRange(listTeacherInClass);
                        _dbContext.LopHP_Rooms.RemoveRange(listSchedule);
                        _dbContext.ClassHPs.Remove(existClass);

                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        throw new Exception(
                            $"Không thể xóa lớp học phần {lopHpId} do có trên 10 sinh viên trong lớp"
                        );
                    }
                }
                else
                {
                    _dbContext.ClassHPs.Remove(existClass);
                    _dbContext.SaveChanges();
                }
            }
            else
            {
                throw new Exception($"Lớp học phần {lopHpId} không tồn tại hoặc đã bị xóa");
            }
        }
    }
}
