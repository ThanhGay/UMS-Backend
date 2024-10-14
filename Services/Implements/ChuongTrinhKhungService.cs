using Server.DbContexts;
using Server.Dtos.ChuongTrinhKhung;
using Server.Dtos.Common;
using Server.Entities;
using Server.Exceptions;
using Server.Services.Interfaces;

namespace Server.Services.Implements
{
    public class ChuongTrinhKhungService : IChuongTrinhKhungService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBaseService _baseService;

        public ChuongTrinhKhungService(ApplicationDbContext dbContext, IBaseService baseService)
        {
            _dbContext = dbContext;
            _baseService = baseService;
        }

        public void CreateCTK(CreateCTKDto input)
        {
            var existCTK = _dbContext.ChuongTrinhKhungs.Any(ctk =>
                ctk.ChuyenNganhId.Equals(input.ChuyenNganhId)
            );
            if (existCTK)
            {
                throw new UserFriendlyException(
                    $"Ngành \"{input.ChuyenNganhId}\" đã có chương trình khung"
                );
            }
            else
            {
                var newCTK = new ChuongTrinhKhung()
                {
                    ChuyenNganhId = input.ChuyenNganhId,
                    CreateAt = DateTime.Now,
                    TongTinChi = 0,
                };

                _dbContext.ChuongTrinhKhungs.Add(newCTK);
                _dbContext.SaveChanges();

                foreach (var item in input.Details)
                {
                    foreach (var monId in item.MonHocIds)
                    {
                        var existSubject = _dbContext.Subjects.Any(s => s.Id == monId);
                        if (!existSubject)
                        {
                            throw new UserFriendlyException($"Không tồn tại môn \"{monId}\"");
                        }
                    }
                }

                int totalCredits = 0;
                foreach (var item in input.Details)
                {
                    foreach (var monId in item.MonHocIds)
                    {
                        var newDetailCTK = new MonHoc_ChuongTrinhKhung
                        {
                            KiHoc = item.KiHoc,
                            SubjectId = monId,
                            ChuongTrinhKhungId = newCTK.Id,
                            CreateAt = DateTime.Now,
                            UpdateAt = DateTime.Now,
                        };

                        var exSub = _baseService.FindSubjectById(monId);
                        if (exSub != null)
                        {
                            totalCredits += exSub.SoTinChi;
                        }

                        _dbContext.DetailCTKs.Add(newDetailCTK);
                    }
                }

                newCTK.TongTinChi = totalCredits;
                _dbContext.ChuongTrinhKhungs.Update(newCTK);
                _dbContext.SaveChanges();
            }
        }

        public PageResultDto<ChuongTrinhKhung> GetAll(FilterDto input)
        {
            var result = new PageResultDto<ChuongTrinhKhung>();

            var query = _dbContext.ChuongTrinhKhungs.Select(ctk => new ChuongTrinhKhung
            {
                Id = ctk.Id,
                ChuyenNganhId = ctk.ChuyenNganhId,
                CreateAt = ctk.CreateAt,
                TongTinChi = ctk.TongTinChi,
            });

            int totalItems = query.Count();

            query = query.Skip(input.SkipCount()).Take(input.PageSize);

            result.TotalItem = totalItems;
            result.Items = query;

            return result;
        }

        public CTKDto DetailCTK(int id)
        {
            var existCTK = _dbContext.ChuongTrinhKhungs.FirstOrDefault(s => s.Id == id);
            if (existCTK == null)
            {
                throw new UserFriendlyException($"Chưa tồn tại chương trình khung có Id: {id}");
            }
            else
            {
                // join detailCtk with Subject to get information of subject
                var queryDetailCTKJoinSubJect = _dbContext
                    .DetailCTKs.Where(dCtk => dCtk.ChuongTrinhKhungId == id)
                    .Join(
                        _dbContext.Subjects,
                        d => d.SubjectId,
                        s => s.Id,
                        (d, s) =>
                            new
                            {
                                d.Id,
                                SubjectId = s.Id,
                                SubjectName = s.Name,
                                s.SoTinChi,
                                s.MaHocPhan,
                                d.KiHoc,
                            }
                    );

                // group by ki hoc
                var group = queryDetailCTKJoinSubJect
                    .GroupBy(dCtk => dCtk.KiHoc)
                    .Select(g => new
                    {
                        KiHoc = g.Key,
                        Subjects = g.Select(s => new DetailCTK_Subject_Dto
                            {
                                Id = s.Id,
                                SubjectId = s.SubjectId,
                                SubjectName = s.SubjectName,
                                SoTinChi = s.SoTinChi,
                                MaHocPhan = s.MaHocPhan,
                            })
                            .ToList(),
                    });

                // create list to restore information about Thong tin chi tiet chuong trinh khung cua tung ki hoc
                var listSemester = new List<SemesterDto>();

                foreach (var item in group)
                {
                    listSemester.Add(
                        new SemesterDto { KiHoc = item.KiHoc, Subjects = item.Subjects }
                    );
                }

                var rtnCTK = new CTKDto
                {
                    Id = existCTK.Id,
                    ChuyenNganhId = existCTK.ChuyenNganhId,
                    TongTinChi = existCTK.TongTinChi,
                    detailCTKByKiHocDtos = listSemester,
                };

                return rtnCTK;
            }
        }

        public CTKDto DetailCTKByChuyenNganhId(int chuyenNganhId)
        {
            var existCTK = _dbContext.ChuongTrinhKhungs.FirstOrDefault(s =>
                s.ChuyenNganhId.Equals(chuyenNganhId)
            );
            if (existCTK == null)
            {
                throw new UserFriendlyException(
                    $"Chưa tồn tại chương trình khung của chuyên ngành: {chuyenNganhId}"
                );
            }
            else
            {
                // join detailCtk with Subject to get information of subject
                var queryDetailCTKJoinSubJect = _dbContext
                    .DetailCTKs.Where(dCtk => dCtk.ChuongTrinhKhungId == existCTK.Id)
                    .Join(
                        _dbContext.Subjects,
                        d => d.SubjectId,
                        s => s.Id,
                        (d, s) =>
                            new
                            {
                                d.Id,
                                SubjectId = s.Id,
                                SubjectName = s.Name,
                                s.SoTinChi,
                                s.MaHocPhan,
                                d.KiHoc,
                            }
                    );

                // group by ki hoc
                var group = queryDetailCTKJoinSubJect
                    .GroupBy(dCtk => dCtk.KiHoc)
                    .Select(g => new
                    {
                        KiHoc = g.Key,
                        Subjects = g.Select(s => new DetailCTK_Subject_Dto
                            {
                                Id = s.Id,
                                SubjectId = s.SubjectId,
                                SubjectName = s.SubjectName,
                                SoTinChi = s.SoTinChi,
                                MaHocPhan = s.MaHocPhan,
                            })
                            .ToList(),
                    });

                // create list to restore information about Thong tin chi tiet chuong trinh khung cua tung ki hoc
                var listSemester = new List<SemesterDto>();

                foreach (var item in group)
                {
                    listSemester.Add(
                        new SemesterDto { KiHoc = item.KiHoc, Subjects = item.Subjects }
                    );
                }

                var rtnCTK = new CTKDto
                {
                    Id = existCTK.Id,
                    ChuyenNganhId = existCTK.ChuyenNganhId,
                    TongTinChi = existCTK.TongTinChi,
                    detailCTKByKiHocDtos = listSemester,
                };

                return rtnCTK;
            }
        }

        public void DeleteCtk(int id)
        {
            var existCTK = _dbContext.ChuongTrinhKhungs.FirstOrDefault(s => s.Id == id);
            if (existCTK == null)
            {
                throw new UserFriendlyException($"Chưa tồn tại chương trình khung có Id: {id}");
            }
            else
            {
                var query = _dbContext.DetailCTKs.Where(s => s.ChuongTrinhKhungId == id);
                foreach (var item in query)
                {
                    _dbContext.DetailCTKs.Remove(item);
                }
                _dbContext.ChuongTrinhKhungs.Remove(existCTK);
                _dbContext.SaveChanges();
            }
        }
    }
}
