using Server.Dtos.Common;
using Server.Dtos.LopHP;

namespace Server.Services.Interfaces
{
    public interface ILopHPService
    {
        public void CreateLopHP(CreateLopHPDto input);
        public PageResultDto<GetAllLopHpDto> GetAll(FilterDto input);
        public List<string> GetStudents(int lopHpId);
        public List<string> GetTeachers(int lopHpId);
        public void AddStudents(AddStudentIntoLopHpDto input);
        public GetDetailLopHpDto GetDetailLopHp(int lopHpId);
        public PageResultDto<AllLopHpByStudentIdDto> GetAllLopHpByStudentId(string studentId, FilterDto input);
        public PageResultDto<AllLopHpByTeacherIdDto> GetAllLopHpByTeacherId(string teacherId, FilterDto input);
        public void AddTeacherToLopHp(AddTeacherIntoLopHpDto input);
        public void DeleteLopHP (int lopHpId);
    }
}
