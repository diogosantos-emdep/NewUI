using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class DetailViewModel
    {
        private ObservableCollection<Employees> employeeList;

        private ObservableCollection<Employees> delemployeeList;

        private ObservableCollection<Department> departmentList;

        private List<string> fName;
        public ICommand DeleteItemRowCommand { get; set; }
        public ICommand CellValueChangedCommand { get; set; }
        public ICommand InitNewRowCommand { get; set; }
        public ICommand RowCommand { get; set; }

        public ICommand RefreshButton { get; set; }

        private Employees selectedGridRow;
        public Employees SelectedGridRow
        {
            get
            {
                return selectedGridRow;
            }
            set
            {
                selectedGridRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGridRow"));

            }
        }

        public ObservableCollection<Employees> EmployeeList
        {
            get
            {
                return employeeList;
            }
            set
            {
                employeeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeList"));
            }
        }

        public ObservableCollection<Employees> DelEmployeeList
        {
            get
            {
                return delemployeeList;
            }
            set
            {
                delemployeeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DelEmployeeList"));
            }
        }
        public List<string> FName
        {
            get
            {
                return fName;
            }

            set
            {
                fName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FName"));
            }
        }

        public ObservableCollection<Department> DepartmentList
        {
            get
            {
                return departmentList;
            }

            set
            {
                departmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DepartmentList"));
            }
        }

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }


        #endregion // Events
        public DetailViewModel()
        {



            EmployeeList = new ObservableCollection<Employees>();
            DelEmployeeList = new ObservableCollection<Employees>();
            DepartmentList = new ObservableCollection<Department>();
            EmployeeList = FillEmployeeDetails();

            //DelEmployeeList = FillDelEmployeeDetails();
            //var firstname = from c in FillEmployeeDetails() select c.FirstName;
            //this.FName = firstname.ToList();
        }
        public ObservableCollection<Employees> FillEmployeeDetails()
        {

            //EmployeeList.Add(new Employees()
            //{
            //    IdEmployee = 1,
            //    DeptID = 1,
            //    FirstName = "Alex",
            //    LastName = "Dakuna",
            //    CorporateEmail = "alexd@rthd.com",
            //    CorporateId = "8669536",
            //    CorporatePhoneNumber = "02323-253625",
            //    JoinDate = Convert.ToDateTime("25/12/2011"),
            //    LeaveDate = null,
            //    Photo = "",
            //    Department = new Department() { DeptID = 1, Location = "", Name = "Account" },
            //});

            //EmployeeList.Add(new Employees()
            //{
            //    IdEmployee = 2,
            //    FirstName = "Paul",
            //    LastName = "Ereka",
            //    CorporateEmail = "pereka@rthd.com",
            //    CorporateId = "8669546",
            //    CorporatePhoneNumber = "02323-263569",
            //    JoinDate = Convert.ToDateTime("05/10/2008"),
            //    LeaveDate = Convert.ToDateTime("31/03/2014"),
            //    Photo = "",
            //    Department = new Department() { DeptID = 2, Location = "", Name = "Human Resource" },
            //});
            //EmployeeList.Add(new Employees()
            //{
            //    IdEmployee = 3,
            //    FirstName = "Ken",
            //    LastName = "Rgrata",
            //    DeptID = 1,
            //    CorporateEmail = "krgrata@rthd.com",
            //    CorporateId = "8669556",
            //    CorporatePhoneNumber = "02323-125636",
            //    JoinDate = Convert.ToDateTime("17/03/2012"),
            //    LeaveDate = null,
            //    Photo = "",
            //    Department = new Department() { DeptID = 3, Location = "", Name = "Management" },
            //});
            //EmployeeList.Add(new Employees()
            //{
            //    IdEmployee = 4,
            //    FirstName = "John",
            //    DeptID = 2,
            //    LastName = "Martin",
            //    CorporateEmail = "jmartin@rthd.com",
            //    CorporateId = "8669566",
            //    CorporatePhoneNumber = "02323-253626",
            //    JoinDate = Convert.ToDateTime("08/08/2009"),
            //    LeaveDate = Convert.ToDateTime("17/03/2013"),
            //    Photo = "",
            //    Department = new Department() { DeptID = 4, Location = "", Name = "Production" },
            //});
            return EmployeeList;
        }





        public void DeleteItemCommandAction(object parameter)
        {
            try
            {
                DelEmployeeList.Add(SelectedGridRow);
                EmployeeList.Remove(SelectedGridRow);
            }
            catch (Exception ex)
            {

            }
        }


    }



    public class Employees
    {
        public Employees()
        {
            //Department = new Department();
        }
        public string CorporateEmail { get; set; }
        public string CorporateId { get; set; }
        public int DeptID { get; set; }
        public string CorporatePhoneNumber { get; set; }
        public string FirstName { get; set; }
        public int IdDepartment { get; set; }
        public short IdEmployee { get; set; }
        public int IdJobDescription { get; set; }
        public short IdSite { get; set; }
        public sbyte IsValidated { get; set; }
        public DateTime? JoinDate { get; set; }
        public string LastName { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string Photo { get; set; }
        //public Department Department { get; set; }

    }

    //public class Department
    //{
    //    public int DeptID { get; set; }
    //    public string Name { get; set; }
    //    public string Location { get; set; }
    //}
}


