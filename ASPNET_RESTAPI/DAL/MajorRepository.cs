using ASPNET_RESTAPI.DbModel;
using ASPNET_RESTAPI.Model;

namespace ASPNET_RESTAPI.DAL {
    public class MajorRepository {
        private readonly UniDbContext _dbContext;

        public MajorRepository(UniDbContext dbContext) {
            this._dbContext = dbContext;
        }

        public IReadOnlyCollection<Major> List() {
            var Majors = new List<Major>();
            foreach (var major in _dbContext.Majors) {
                Majors.Add(new Major(major));
            }
            return Majors;
        }

        public bool TryGetMajorById(int majorId, out Major? major) {
            var dbMajor = _dbContext.Majors.FirstOrDefault(m => m.ID == majorId);
            if (dbMajor == null) {
                major = null;
                return false;
            }

            major = new Major(dbMajor);
            return true;
        }

        public bool AddMajor(Major major) {
            var dbMajor = new DbMajor {
                Name = major.Name,
            };
            try {
                _dbContext.Majors.Add(dbMajor);
                _dbContext.SaveChanges();
                _dbContext.Entry(dbMajor).Reload();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool DeleteMajor(int majorId) {
            var delMajor = _dbContext.Majors.FirstOrDefault(m => m.ID == majorId);
            if (delMajor == null) return false;

            try {
                _dbContext.Majors.Remove(delMajor);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
