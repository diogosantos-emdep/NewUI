using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.CustomControls.ViewModels
{/// <summary>
/// [pramod.misal][GEOS2-][09-07-2025]https://helpdesk.emdep.com/browse/GEOS2-8321
/// </summary>
    public class PCMPromptViewModel : INotifyPropertyChanged
    {
        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Message"));
            }
        }

        private string color;

        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Color"));
            }
        }

        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagePath"));
            }
        }

        private bool isDetailsChecked;
        public bool IsDetailsChecked
        {
            get { return isDetailsChecked; }
            set
            {
                isDetailsChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDetailsChecked"));
                
            }
        }

        private bool isPricesChecked;
        public bool IsPricesChecked
        {
            get { return isPricesChecked; }
            set
            {
                isPricesChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDetailsChecked"));

                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsPricesChecked)));
            }
        }

        public PCMPromptViewModel()
        {

            IsDetailsChecked = true;
            IsPricesChecked = true;

        }
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
