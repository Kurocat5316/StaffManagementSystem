using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment2.Teaching;
using Assignment2.Database;

namespace Assignment2.Controller
{

    class UnitController
    {
        DbConnection sql = new DbConnection();
        List<Staff> staffList;
        List<Unit> unitList;

        public UnitController() {
            sql.LoadAll(out staffList, out unitList);
        }

        public List<Unit> GetAllUnit() {
            return unitList;
        }
    }
}
