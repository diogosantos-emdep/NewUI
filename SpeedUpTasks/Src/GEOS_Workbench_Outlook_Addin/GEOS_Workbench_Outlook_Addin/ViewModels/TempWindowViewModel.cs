using DevExpress.Mvvm;
using Emdep.Geos.UI.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    class TempWindowViewModel : INotifyPropertyChanged
    {

        private int statusSuccess;
        private int selectedStarIndex;
        private int hoveredStarIndex;

        public virtual Color Background { get; set; }
        private Color hoverBackground;
        private Color selectedBackground;

        public int StatusSuccess
        {
            get
            {
                return statusSuccess;
            }

            set
            {
                statusSuccess = value; OnPropertyChanged(new PropertyChangedEventArgs("StatusSuccess"));
            }
        }

        public int SelectedStarIndex
        {
            get
            {
                return selectedStarIndex;
            }

            set
            {
                selectedStarIndex = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedStarIndex"));

                switch (selectedStarIndex)
                {

                    case 1:
                        SelectedBackground = Colors.Red;
                        break;

                    case 2:
                        SelectedBackground = Colors.Orange;
                        break;

                    case 3:
                        SelectedBackground = Colors.Yellow;
                        break;

                    case 4:
                        SelectedBackground = Colors.DeepSkyBlue;
                        break;

                    case 5:
                        SelectedBackground = Colors.Green;
                        break;
                    default:
                        SelectedBackground = Colors.Transparent;
                        break;
                }

            }
        }

        public virtual Color SelectedBackground
        {
            get
            {
                return selectedBackground;
            }

            set
            {
                selectedBackground = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedBackground"));
            }
        }

        public int HoveredStarIndex
        {
            get
            {
                return hoveredStarIndex;
            }

            set
            {
                hoveredStarIndex = value; OnPropertyChanged(new PropertyChangedEventArgs("HoveredStarIndex"));
            }
        }

        public virtual Color HoverBackground
        {
            get
            {
                return hoverBackground;
            }

            set
            {
                hoverBackground = value; OnPropertyChanged(new PropertyChangedEventArgs("HoverBackground"));
            }
        }


        public ICommand MouseOverCommand
        {
            get;
            set;
        }

        #region Events

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler RequestClose;
        #endregion


        #region Constructor
        public TempWindowViewModel()
        {
            StatusSuccess = 25;
            SelectedStarIndex = 0;
            Background = Colors.Transparent;
            HoverBackground = Colors.Transparent;
            SelectedBackground = Colors.Transparent;

            // MouseOverCommand = new RelayCommand(new Action<object>(MouseOverStar));

        }

        public void MouseOverStar(object obj)
        {

        }

        #endregion
    }
}
