using Server.DbContexts;
using Server.Dtos.Common;
using Server.Dtos.Subject;
using Server.Entities;
using Server.Exceptions;
using Server.Services.Interfaces;

namespace Server.Services.Implements
{
    public class SubjectService : ISubjectService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBaseService _baseService;
        public SubjectService(ApplicationDbContext dbContext, IBaseService baseService)
        {
            _dbContext = dbContext;
            _baseService = baseService;
        }

        public void CreateSubject(CreateSubjectDto input)
        {
            var existSubject = _dbContext.Subjects.Any(s => s.MaHocPhan == (input.MaHocPhan));

            if (!existSubject)
            {
                var newSubject = new Subject()
                {
                    MaHocPhan = input.MaHocPhan,
                    Name = input.Name,
                    SoTinChi = input.SoTinChi,
                    BoMonId = input.BoMonId,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                };

                _dbContext.Subjects.Add(newSubject);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new UserFriendlyException($"Đã tồn tại môn học có mã học phần \"{input.MaHocPhan}\"");
            }
        }

        public void DeleteSubject(int id)
        {
            var existSubject = _baseService.FindSubjectById(id);

            _dbContext.Subjects.Remove(existSubject);
            _dbContext.SaveChanges();
        }

        public PageResultDto<SubjectDto> GetAll(FilterDto input)
        {
            var result = new PageResultDto<SubjectDto>();
            var subjectQuery = _dbContext.Subjects.Select(s => new SubjectDto
            {
                Id = s.Id,
                Name = s.Name,
                MaHocPhan = s.MaHocPhan,
                SoTinChi = s.SoTinChi,
                BoMonId= s.BoMonId,
                CreateAt = s.CreateAt,
                UpdateAt = s.UpdateAt,
            });

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                subjectQuery = subjectQuery.Where(s => s.Name.ToLower().Contains(input.Keyword.ToLower()));
            }

            int totalItems = subjectQuery.Count();

            subjectQuery = subjectQuery.Skip(input.SkipCount()).Take(input.PageSize);

            result.TotalItem = totalItems;
            result.Items = subjectQuery;

            return result;
        }

        public SubjectDto GetSubjectById(int id)
        {
            var existSubject = _baseService.FindSubjectById(id);

            var newSubject = new SubjectDto()
            {
                Id = existSubject.Id,
                Name = existSubject.Name,
                MaHocPhan = existSubject.MaHocPhan,
                SoTinChi = existSubject.SoTinChi,
                BoMonId = existSubject.BoMonId,
                CreateAt = existSubject.CreateAt,
                UpdateAt = existSubject.UpdateAt,
            };

            return newSubject;
        }

        public void UpdateSubject(UpdateSubjectDto input)
        {
            var existSubjectName = _dbContext.Subjects.Any(s => s.MaHocPhan == input.MaHocPhan && s.Id != input.Id);
            if (!existSubjectName)
            {
                var existSubject = _baseService.FindSubjectById(input.Id);
                if (existSubject != null)
                {
                    existSubject.Name = input.Name;
                    existSubject.MaHocPhan = input.MaHocPhan;
                    existSubject.SoTinChi = input.SoTinChi;
                    existSubject.BoMonId = input.BoMonId;
                    existSubject.UpdateAt = DateTime.Now;
                }

                _dbContext.SaveChanges();
            }
            else
            {
                throw new UserFriendlyException($"Đã tồn tại môn học có mã học phần {input.MaHocPhan}");

            }
        }
    }
}
