using Server.Entities;

namespace Server.Services.Interfaces
{
    public interface IBaseService
    {
        public Subject FindSubjectById(int id);
        public Subject FindSubjectByMaHocPhan(string maHocPhan);
    }
}
