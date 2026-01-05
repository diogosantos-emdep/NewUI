using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Emdep.Geos.Modules.HarnessPart
{
    /// <summary>
    /// Interaction logic for EmdepSite.xaml
    /// </summary>
    public partial class EmdepSite : WinUIDialogWindow, INotifyPropertyChanged
    {
        public ObservableCollection<Emdepsite> Emdepsite { get; set; }
        protected string _ActiveItemIndexValue;

        public string ActiveItemIndexValue
        {
            get
            {
                return this._ActiveItemIndexValue;
            }

            set
            {
                if (this._ActiveItemIndexValue != value)
                {
                    this._ActiveItemIndexValue = value;
                    this.OnPropertyChanged("ActiveItemIndexValue");
                }
            }
        }

        protected int _ActiveItemIndexValue1;

        public int ActiveItemIndexValue1
        {
            get
            {
                return this._ActiveItemIndexValue1;
            }

            set
            {
                if (this._ActiveItemIndexValue1 != value)
                {
                    this._ActiveItemIndexValue1 = value;
                    this.OnPropertyChanged("ActiveItemIndexValue1");
                }
            }
        }

        public void OnPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public EmdepSite()
        {
            InitializeComponent();
            Emdepsite = new ObservableCollection<Emdepsite>() {
                new Emdepsite { Name= "EJMX1", ConImage=GetImage("/Image/JUAREZ.jpg"),Sitename="JUAREZ"},
                  new Emdepsite { Name= "ESCH", ConImage=GetImage("/Image/CHINA.jpg"),Sitename="CHINA"},
                   new Emdepsite { Name= "EPIN", ConImage=GetImage("/Image/India.jpg"),Sitename="INDIA"},
                     new Emdepsite { Name= "EIBR", ConImage=GetImage("/Image/BRAZIL.jpg"),Sitename="BRAZIL"},
                       new Emdepsite { Name= "EASP", ConImage=GetImage("/Image/Spain.JPG"),Sitename="SPAIN"},
                         new Emdepsite { Name= "ETTU", ConImage=GetImage("/Image/India.jpg"),Sitename="INDIA"},
                 //  new clsGrid { ReferenceS= "EPN150900", Cavities= 2,  Type= "conn", Color= "Blue", Gender= "Mail", Saled= "saled", Mylocation= "W443", Partner= 050503, Dimendions1= 111, ConImage1 = GetImage("/Image/con4.jpg"),Isduplication=true  },
                 //   new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "Pink", Gender= "Mail", Saled= "saled", Mylocation= "W443", Partner= 050503, Dimendions1= 111, ConImage1 = GetImage("/Image/con5.jpg"),Isduplication=false  },
                 //    new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "Green", Gender= "Mail", Saled= "saled", Mylocation= "W443", Partner= 050503, Dimendions1= 111, ConImage1 = GetImage("/Image/con6.jpg"), Isduplication=false },
                 //    new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "Red", Gender= "Mail", Saled= "saled", Mylocation= "W443", Partner= 050503, Dimendions1= 111, ConImage1 = GetImage("/Image/con1.jpg"),Isduplication=false  },
                 //new clsGrid { ReferenceS= "EPN150900", Cavities= 5,  Type= "conn", Color= "Yellow", Gender= "Mail", Saled= "saled", Mylocation= "W443", Partner= 050503, Dimendions1= 111, ConImage1 = GetImage("/Image/con2.jpg"), Isduplication=true },
                 // new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "black", Gender= "Mail", Saled= "saled", Mylocation= "W443", Partner= 050503, Dimendions1= 111, ConImage1 = GetImage("/Image/con3.jpg"),Isduplication=false  },
                 //  new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "black", Gender= "Mail", Saled= "saled", Mylocation= "W443", Partner= 050503, Dimendions1= 111, ConImage1 = GetImage("/Image/con4.jpg"),Isduplication=true  },
                 //   new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "black", Gender= "Mail", Saled= "saled", Mylocation= "W443", Partner= 050503, Dimendions1= 111, ConImage1 = GetImage("/Image/con5.jpg"),Isduplication=false  },
                 //    new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "black", Gender= "Mail", Saled= "saled", Mylocation= "W443", Partner= 050503, Dimendions1= 111, ConImage1 = GetImage("/Image/con6.jpg"),Isduplication=true  },
               
            };
           // _ActiveItemIndexValue1 = this.Emdepsite.IndexOf(Emdepsite[1]);



            this.DataContext = this;

        }
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }



        private void CarouselPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {


        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }


        private void btnsitename_Click(object sender, RoutedEventArgs e)
        {


            FrmShowMap frmmap = new FrmShowMap();
            frmmap.ShowDialogWindow();
            //  mapconrol1.Visibility = System.Windows.Visibility.Visible;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                _ActiveItemIndexValue = this.Emdepsite[ActiveItemIndexValue1].Name.ToString();
                this.Close();
            }
        }

        private void WinUIDialogWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Escape")
            {
                Close();
            }
        }



    }
    public class Emdepsite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sitename { get; set; }
        //public bool Bool { get; set; }
        public ImageSource ConImage { get; set; }


    }
}
