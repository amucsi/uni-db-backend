using ASPNET_RESTAPI.DbModel;
using ASPNET_RESTAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_RESTAPI.DAL {
    public class MajorRepository {
        private readonly UniDbContext _dbContext;

        public MajorRepository(UniDbContext dbContext) {
            this._dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<Major>> ListAsync() {
            var majors = await _dbContext.Majors.ToListAsync();
            return majors.Select(m => new Major(m)).ToList();
        }

        public async Task<(bool, Major?)> GetMajorByIdAsync(int majorId) {
            var dbMajor = await _dbContext.Majors.FirstOrDefaultAsync(m => m.ID == majorId);
            if (dbMajor == null)
                return (false, null);
            else
                return (true, new Major(dbMajor));
        }

        public async Task<bool> AddMajorAsync(Major major) {
            var dbMajor = new DbMajor {
                Name = major.Name,
            };
            try {
                _dbContext.Majors.Add(dbMajor);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Entry(dbMajor).ReloadAsync();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteMajorAsync(int majorId) {
            var delMajor = await _dbContext.Majors.FirstOrDefaultAsync(m => m.ID == majorId);
            if (delMajor == null)
                return false;

            try {
                _dbContext.Majors.Remove(delMajor);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
