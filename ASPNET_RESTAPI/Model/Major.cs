using ASPNET_RESTAPI.DbModel;

namespace ASPNET_RESTAPI.Model {
    public class Major {
        public int ID { get;set; }
        public string Name { get;set; }

        public Major() { }

        public Major(DbMajor dbMajor) {
            ID = dbMajor.ID;
            Name = dbMajor.Name;
        }
    }
}
