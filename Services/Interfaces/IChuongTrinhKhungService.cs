using Server.Dtos.ChuongTrinhKhung;
using Server.Dtos.Common;
using Server.Entities;

namespace Server.Services.Interfaces
{
    public interface IChuongTrinhKhungService
    {
        public void CreateCTK(CreateCTKDto input);
        public CTKDto DetailCTK(int id);
        public CTKDto DetailCTKByChuyenNganhId(int chuyenNganhId);
        public PageResultDto<ChuongTrinhKhung> GetAll(FilterDto innput);
        public void DeleteCtk(int id);
    }
}
