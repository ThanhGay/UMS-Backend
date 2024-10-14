using Server.Dtos.Common;
using Server.Dtos.Subject;

namespace Server.Services.Interfaces
{
    public interface ISubjectService
    {
        public void CreateSubject(CreateSubjectDto input);
        public void UpdateSubject(UpdateSubjectDto input);
        public void DeleteSubject(int id);
        public SubjectDto GetSubjectById(int id);
        public PageResultDto<SubjectDto> GetAll(FilterDto input);
    }
}
