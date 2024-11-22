using Server.Dtos.Common;
using Server.Dtos.LopHP;

namespace Server.Services.Interfaces
{
    public interface ILopHPService
    {
        public void CreateLopHP(CreateLopHPDto input);
        public PageResultDto<GetAllLopHpDto> GetAll(FilterDto input);
        public List<string> GetStudents(int lopHpId);
        public void AddStudents(AddStudentIntoLopHpDto input);
        public GetDetailLopHpDto GetDetailLopHp(int lopHpId);
        public List<AllLopHpByStudentIdDto> GetAllLopHpByStudentId(string studentId);
        public void AddTeacherToLopHp(AddTeacherIntoLopHpDto input);
        public void DeleteLopHP (int lopHpId);
    }
}
