using Emdep.Geos.UI.CustomControls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Emdep.Geos.UI.CustomControls
{
    public static class CustomMessageBox
    {
        public enum MessageImagePath
        {
            Info,
            NotOk,
            Ok,
            Warning,
            Question,
        }
        public static MessageBoxResult msgboxresult { get; set; }
        public static void SetValueToMessage(string Message, string color, Enum messageImage, MessageBoxButton MessageBoxButton)
        {
            MessageBoxViewModel MessageBoxViewModel = new MessageBoxViewModel();
            MessageBoxViewModel.Message = Message;
            MessageBoxView objMessageBoxView = new MessageBoxView();

            objMessageBoxView.DataContext = MessageBoxViewModel;
            objMessageBoxView.Height = 10;
            objMessageBoxView.FontSize = 12;
            objMessageBoxView.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(color)); ;
            objMessageBoxView.BorderThickness = new Thickness(10);

            if (messageImage.Equals(MessageImagePath.NotOk))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/NotOk.png";
            }
            if (messageImage.Equals(MessageImagePath.Ok))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/Ok.png";
            }
            if (messageImage.Equals(MessageImagePath.Info))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/Info.png";
            }
            if (messageImage.Equals(MessageImagePath.Warning))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/Warning.png";
            }
            msgboxresult = objMessageBoxView.ShowDialogWindow(MessageBoxButton);
        }

        public static MessageBoxResult Show(string Message, string color, Enum messageImage, MessageBoxButton MessageBoxButton, Window window = null)
        {
            MessageBoxViewModel MessageBoxViewModel = new MessageBoxViewModel();
            MessageBoxViewModel.Message = Message;
            MessageBoxView objMessageBoxView = new MessageBoxView();

            if (window != null)
            {
                objMessageBoxView.Owner = window;
            }

            objMessageBoxView.DataContext = MessageBoxViewModel;
            objMessageBoxView.Height = 10;
            objMessageBoxView.FontSize = 12;

            objMessageBoxView.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(color)); ;
            objMessageBoxView.BorderThickness = new Thickness(10);
            if (messageImage.Equals(MessageImagePath.NotOk))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/NotOk.png";
            }
            if (messageImage.Equals(MessageImagePath.Ok))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/Ok.png";

            }
            if (messageImage.Equals(MessageImagePath.Info))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/Info.png";

            }
            if (messageImage.Equals(MessageImagePath.Warning))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/Warning.png";
            }
            if (messageImage.Equals(MessageImagePath.Question))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/QuestionMark.png";
            }
            msgboxresult = objMessageBoxView.ShowDialogWindow(MessageBoxButton);

            return msgboxresult;
        }

        public static MessageBoxResult Show(string Message, string color, Enum messageImage, MessageBoxButton MessageBoxButton, MessageBoxResult DefaultButtonFocus, Window window = null)
        {
            MessageBoxViewModel MessageBoxViewModel = new MessageBoxViewModel();
            MessageBoxViewModel.Message = Message;
            MessageBoxView objMessageBoxView = new MessageBoxView();
            if (window != null)
            {
                objMessageBoxView.Owner = window;
            }
            objMessageBoxView.DataContext = MessageBoxViewModel;
            objMessageBoxView.Height = 10;
            objMessageBoxView.FontSize = 12;
            objMessageBoxView.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(color)); ;
            objMessageBoxView.BorderThickness = new Thickness(10);
            if (messageImage.Equals(MessageImagePath.NotOk))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/NotOk.png";
            }
            if (messageImage.Equals(MessageImagePath.Ok))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/Ok.png";
            }
            if (messageImage.Equals(MessageImagePath.Info))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/Info.png";
            }
            if (messageImage.Equals(MessageImagePath.Warning))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/Warning.png";
            }
            if (messageImage.Equals(MessageImagePath.Question))
            {
                MessageBoxViewModel.ImagePath = @"Assets/Images/QuestionMark.png";
            }
                msgboxresult = objMessageBoxView.ShowDialogWindow(MessageBoxButton, DefaultButtonFocus);
            return msgboxresult;
        }
    }

}
