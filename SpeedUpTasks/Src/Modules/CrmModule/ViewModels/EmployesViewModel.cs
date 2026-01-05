

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class EmployesViewModel
    {
        private ObservableCollection<Employes> employesList;

        public ObservableCollection<Employes> EmployesList
        {
            get { return employesList; }
            set { employesList = value; }
        }
        public EmployesViewModel()
        {
            FillEmployesList();
        }

        private void FillEmployesList()
        {

            EmployesList = new ObservableCollection<Employes>();

            EmployesList.Add(new Employes() { FirstName = "Chanchal", LastName = "Patil", Emailid = "cpatil@emdep.com", Position = "Software Developer", Photo = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/Chanchal.jpg"), Site = "EPIN" });
            EmployesList.Add(new Employes() { FirstName = "Francesc", LastName = "Piñas", Emailid = "fPiñas@emdep.com", Position = "Geos Software Manager", Photo = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/fpinas.png"), Site = "EBRO" });
            EmployesList.Add(new Employes() { FirstName = "Anupam", LastName = "Pawar", Emailid = "apawar@emdep.com", Position = "Software Developer", Photo = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/Anupam.jpg"), Site = "EPIN" });
        }
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

    }
}
