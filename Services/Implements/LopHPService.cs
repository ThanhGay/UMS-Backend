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

            var listSchedule = _scheduleService.GetScheduleOfClassHp(lopHpId);
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
                                c.TeacherId,
                                LopHpId = c.Id,
                                c.SubjectId,
                                SubjectName = _dbContext
                                    .Subjects.Where(s => s.Id == c.SubjectId)
                                    .Select(s => s.Name)
                                    .ToList(),
                            }
                    );

                var resultQuery = query
                    .Select(r => new AllLopHpByStudentIdDto
                    {
                        TeacherId = r.TeacherId,
                        ClassName = r.ClassName,
                        SubjectId = r.SubjectId,
                        LopHpId = r.LopHpId,
                        SubjectName = r.SubjectName[0],
                    })
                    .ToList();

                return resultQuery;
            }
            else
            {
                return [];
            }
        }
    }
}
