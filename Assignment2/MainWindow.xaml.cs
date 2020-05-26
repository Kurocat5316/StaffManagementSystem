using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Assignment2.Teaching;
using Assignment2.Controller;
using System.Collections.ObjectModel;

namespace Assignment2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Staff> currentStaffList;
        List<Unit> currentUnitList;


        StaffController staffController;
        UnitController unitController;

        public MainWindow()
        {
            InitializeComponent();

            staffController = new StaffController();
            unitController = new UnitController();

            currentStaffList = staffController.GetAllStaff();
            currentUnitList = unitController.GetAllUnit();

            //Inital staff view page
            List<Teaching.Type.Category> temp = Enum.GetValues(typeof(Teaching.Type.Category)).Cast<Teaching.Type.Category>().ToList();
            staff_category.Items.Add("All");
            foreach (Teaching.Type.Category campus in temp) {
                staff_category.Items.Add(campus);
            }
            staff_category.SelectedItem = "All";

            foreach (Staff staff in currentStaffList)
                staff_staffList.Items.Add(staff.giveName + " " + staff.familyName);
        }
    }
}
