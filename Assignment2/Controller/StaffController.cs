using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment2.Database;
using Assignment2.Teaching;

namespace Assignment2.Controller
{
    

    class StaffController
    {
        DbConnection sql = new DbConnection();
        List<Staff> staffList;
        List<Unit> unitList;

        public StaffController(){
            sql.LoadAll(out staffList, out unitList);
        }

        public List<Staff> GetAllStaff() {
            return staffList;
        }


    }
}
