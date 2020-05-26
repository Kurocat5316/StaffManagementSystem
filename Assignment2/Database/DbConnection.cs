using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using Assignment2.Teaching;

namespace Assignment2.Database
{
    public class DbConnection
    {
        //Note that ordinarily these would (1) be stored in a settings file and (2) have some basic encryption applied
        private const string db = "kit206";
        private const string user = "kit206";
        private const string pass = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";

        private static MySqlConnection conn = null;

        public DbConnection()
        {
            //Create the connection object (does not actually make the connection yet)
            //Note that the HRIS case study database has the same values for its name, user name and password (to keep things simple)
            string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
            conn = new MySqlConnection(connectionString);
        }

        private static MySqlConnection GetConnection()
        {
            if (conn == null)
            {
                //Note: This approach is not thread-safe
                string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
        }


        public void LoadAll(out List<Staff> staffList, out List<Unit> unitList)
        {
            staffList = LoadStaff();

            LoadUnit(staffList, out staffList, out unitList);
        }

        private List<Staff> LoadStaff() {
            List<Staff> staffList = new List<Staff>();
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select id, given_name, family_name, title, campus, phone, room, email, photo, category from staff order by family_name", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Enum.TryParse(rdr.GetString(4), out Teaching.Type.Campus campus);
                    Enum.TryParse(rdr.GetString(9), out Teaching.Type.Category category);
                    Staff staff = new Staff { Id = rdr.GetInt32(0), giveName = rdr.GetString(1), familyName = rdr.GetString(2), title = rdr.GetString(3), campus = campus, phone = rdr.GetString(5), room = rdr.GetString(6), email = rdr.GetString(7), photo = new Uri(rdr.GetString(8)), category = category };

                    staff.consult = LoadConsult(staff.Id);



                    staffList.Add(staff);
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading staff", e);
            }

            return staffList;
        }

        private List<Event> LoadConsult(int Id) {
            //conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM consultation where consultation.staff_id = " + Id, conn);
            MySqlDataReader rdr2 = null;

            List<Event> consult = new List<Event>();
            rdr2 = cmd2.ExecuteReader();

            while (rdr2.Read())
            {
                Enum.TryParse(rdr2.GetString(1), out DayOfWeek day);
                consult.Add( new Event { day = day, startTime = TimeSpan.Parse(rdr2.GetString(2)), endTime = TimeSpan.Parse(rdr2.GetString(3)) });
            }

            return consult;
        }

        public void LoadUnit(List<Staff> tempStaffList, out List<Staff> staffList, out List<Unit> unitList) {
            unitList = new List<Unit>();
            staffList = tempStaffList;
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            try
            {
                //conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from unit order by code", conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                     LoadClass(rdr.GetString(0), staffList, out List<UnitClass> classList, out staffList);

                    Unit tempUnit = new Unit { code = rdr.GetString(0), title = rdr.GetString(1), unitClass = classList };

                    foreach (Staff staff in staffList)
                        if (staff.Id == rdr.GetInt32(2)) {
                            tempUnit.coordinator = staff;
                            break;
                        }

                    unitList.Add(tempUnit);


                }
            }
            catch (MySqlException e)
            {
                ReportError("loading staff", e);
            }


        }


        public void LoadClass(string unitCode, List<Staff> tempstaffList, out List<UnitClass> classes, out List<Staff> staffList) {
            classes = new List<UnitClass>();
            staffList = tempstaffList;
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            try
            {
                //conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from class where unit_code = " + unitCode + " order by code", conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Enum.TryParse(rdr.GetString(1), out Teaching.Type.Campus campus);
                    Enum.TryParse(rdr.GetString(2), out DayOfWeek day);
                    Event time = new Event { day = day, startTime = TimeSpan.Parse(rdr.GetString(3)), endTime = TimeSpan.Parse(rdr.GetString(4)) };

                    UnitClass tempClass = new UnitClass { campus = campus, events = time, type = rdr.GetString(5), room = rdr.GetString(6) };

                    foreach (Staff staff in staffList) {
                        if (staff.Id == rdr.GetInt32(7)) {
                            tempClass.staff = staff;
                            staff.classTime.Add(tempClass.events);
                            break;
                        }
                    }
                    classes.Add(tempClass);
                    
                }

                
            }
            catch (MySqlException e)
            {
                ReportError("loading staff", e);
            }


        }



        private static void ReportError(string msg, Exception e)
        {

        }
    }

}
