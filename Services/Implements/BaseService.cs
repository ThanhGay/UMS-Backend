using Server.DbContexts;
using Server.Entities;
using Server.Exceptions;
using Server.Services.Interfaces;

namespace Server.Services.Implements
{
    public class BaseService : IBaseService
    {
        private readonly ApplicationDbContext _dbContext;

        public BaseService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Subject FindSubjectById(int id)
        {
            var existSubject = _dbContext.Subjects.FirstOrDefault(s => s.Id == id);
            if (existSubject == null)
            {
                throw new UserFriendlyException($"Không tồn tại môn học có \"{id}\".");
            }
            return existSubject;
        }

        public Subject FindSubjectByMaHocPhan(string mahocPhan)
        {
            var existSubject = _dbContext.Subjects.FirstOrDefault(s =>
                s.MaHocPhan.Equals(mahocPhan)
            );
            if (existSubject == null)
            {
                throw new UserFriendlyException($"Mã học phần \"{mahocPhan}\" đã tồn tại");
            }
            return existSubject;
        }
    }
}
