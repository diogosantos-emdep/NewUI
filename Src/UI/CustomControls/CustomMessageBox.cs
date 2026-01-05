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

        //[pramod.misal][GEOS2-8321][date:09.07.2025]https://helpdesk.emdep.com/browse/GEOS2-8321
        public class CustomPromptResult
        {
            public MessageBoxResult Result { get; set; }
            public PCMPromptViewModel ViewModel { get; set; }
        }

        public class CustomPromptResultOTMEmail
        {
            public MessageBoxResult Result { get; set; }
            public OTMPromptViewModel ViewModel { get; set; }
        }


        /// <summary>
        ///  //[pramod.misal][GEOS2-8321][date:09.07.2025]https://helpdesk.emdep.com/browse/GEOS2-8321
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <param name="messageImage"></param>
        /// <param name="messageBoxButton"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public static CustomPromptResult ShowbPCMPrompt(string message, string color, Enum messageImage, MessageBoxButton messageBoxButton, Window window = null)
        {
            PCMPromptViewModel messageBoxViewModel = new PCMPromptViewModel
            {
                Message = message
            };

            PCMPromptView objMessageBoxView = new PCMPromptView
            {
                Owner = window,
                DataContext = messageBoxViewModel,
                Height = 10,
                FontSize = 12,
                BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(color)),
                BorderThickness = new Thickness(10)
            };
            
            switch (messageImage)
            {
                case MessageImagePath.NotOk:
                    messageBoxViewModel.ImagePath = @"Assets/Images/NotOk.png";
                    break;
                case MessageImagePath.Ok:
                    messageBoxViewModel.ImagePath = @"Assets/Images/Ok.png";
                    break;
                case MessageImagePath.Info:
                    messageBoxViewModel.ImagePath = @"Assets/Images/Info.png";
                    break;
                case MessageImagePath.Warning:
                    messageBoxViewModel.ImagePath = @"Assets/Images/Warning.png";
                    break;
                case MessageImagePath.Question:
                    messageBoxViewModel.ImagePath = @"Assets/Images/QuestionMark.png";
                    break;
            }

            MessageBoxResult result = objMessageBoxView.ShowDialogWindow(messageBoxButton);

            return new CustomPromptResult
            {
                Result = result,
                ViewModel = messageBoxViewModel
            };
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


        public static MessageBoxResult Show(List<PickingComment> CommentList, string color, Enum messageImage, MessageBoxButton MessageBoxButton, Window window = null)
        {
            PickingInfoMessageBoxViewModel pickingInfoMessageBoxViewModel = new PickingInfoMessageBoxViewModel();
            pickingInfoMessageBoxViewModel.CommentList = CommentList;
            PickingInfoMessageBoxView objPickingInfoMessageBoxView = new PickingInfoMessageBoxView();

            if (window != null)
            {
                objPickingInfoMessageBoxView.Owner = window;
            }

            objPickingInfoMessageBoxView.DataContext = pickingInfoMessageBoxViewModel;
            objPickingInfoMessageBoxView.Height = 10;
            objPickingInfoMessageBoxView.FontSize = 12;

            objPickingInfoMessageBoxView.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(color)); ;
            objPickingInfoMessageBoxView.BorderThickness = new Thickness(10);
            if (messageImage.Equals(MessageImagePath.NotOk))
            {
                pickingInfoMessageBoxViewModel.ImagePath = @"Assets/Images/NotOk.png";
            }
            if (messageImage.Equals(MessageImagePath.Ok))
            {
                pickingInfoMessageBoxViewModel.ImagePath = @"Assets/Images/Ok.png";

            }
            if (messageImage.Equals(MessageImagePath.Info))
            {
                pickingInfoMessageBoxViewModel.ImagePath = @"Assets/Images/Info.png";

            }
            if (messageImage.Equals(MessageImagePath.Warning))
            {
                pickingInfoMessageBoxViewModel.ImagePath = @"Assets/Images/Warning.png";
            }
            if (messageImage.Equals(MessageImagePath.Question))
            {
                pickingInfoMessageBoxViewModel.ImagePath = @"Assets/Images/QuestionMark.png";
            }
            msgboxresult = objPickingInfoMessageBoxView.ShowDialogWindow(MessageBoxButton);

            return msgboxresult;
        }



        public static bool Show(string Message, string color, Window window = null)
        {
            bool isNext = false;
            RefundInfoMessageBoxViewModel refundInfoMessageBoxViewModel = new RefundInfoMessageBoxViewModel();
            //  refundInfoMessageBoxViewModel.CommentList = CommentList;
            RefundInfoMessageBoxView objRefundInfoMessageBoxView = new RefundInfoMessageBoxView();
            EventHandler handle = delegate { objRefundInfoMessageBoxView.Close(); };
            refundInfoMessageBoxViewModel.RequestClose += handle;

            if (window != null)
            {
                objRefundInfoMessageBoxView.Owner = window;
            }

            objRefundInfoMessageBoxView.DataContext = refundInfoMessageBoxViewModel;
            objRefundInfoMessageBoxView.Height = 10;
            objRefundInfoMessageBoxView.FontSize = 12;
            objRefundInfoMessageBoxView.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(color)); ;
            objRefundInfoMessageBoxView.BorderThickness = new Thickness(10);
            refundInfoMessageBoxViewModel.Message = Message;
            refundInfoMessageBoxViewModel.ImagePath = @"Assets/Images/Warning.png";
            objRefundInfoMessageBoxView.ShowDialog();

            if (refundInfoMessageBoxViewModel.IsNext)
            {
                isNext = true;
            }
            else if (refundInfoMessageBoxViewModel.IsCancel)
            {
                isNext = false;
            }
            return isNext;
        }

        //[rdixit] Updated the code to avoid the message box window from going beyond the screen width.[GEOS2-4012][24.04.2023]
        public static MessageBoxResult ShowMessage(string Message, string color, Enum messageImage, MessageBoxButton MessageBoxButton, Window window = null)
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
            
            objMessageBoxView.Width = Convert.ToDouble(System.Windows.SystemParameters.WorkArea.Width);
            objMessageBoxView.MaxWidth = Convert.ToDouble(System.Windows.SystemParameters.WorkArea.Width);

            msgboxresult = objMessageBoxView.ShowDialogWindow(MessageBoxButton);

            return msgboxresult;
        }

        //[pramod.misal][GEOS2-2821][27-02-2024]Import all valid records to all valid employees and Show the message with the detected errors 
        public static MessageBoxResult ShowWithEdit(string Message, string color, Enum messageImage, MessageBoxButton MessageBoxButton, Window window = null)
        {
            MessageBoxEditViewModel MessageBoxEditViewModel = new MessageBoxEditViewModel();
            MessageBoxEditViewModel.Message = Message;
            MessageBoxEditView objMessageBoxView = new MessageBoxEditView();

            if (window != null)
            {
                objMessageBoxView.Owner = window;
            }

            objMessageBoxView.DataContext = MessageBoxEditViewModel;
            objMessageBoxView.Height = 10;
            objMessageBoxView.FontSize = 12;

            objMessageBoxView.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(color)); ;
            objMessageBoxView.BorderThickness = new Thickness(10);
            if (messageImage.Equals(MessageImagePath.NotOk))
            {
                MessageBoxEditViewModel.ImagePath = @"Assets/Images/NotOk.png";
            }
            if (messageImage.Equals(MessageImagePath.Ok))
            {
                MessageBoxEditViewModel.ImagePath = @"Assets/Images/Ok.png";

            }
            if (messageImage.Equals(MessageImagePath.Info))
            {
                MessageBoxEditViewModel.ImagePath = @"Assets/Images/Info.png";

            }
            if (messageImage.Equals(MessageImagePath.Warning))
            {
                MessageBoxEditViewModel.ImagePath = @"Assets/Images/Warning.png";
            }
            if (messageImage.Equals(MessageImagePath.Question))
            {
                MessageBoxEditViewModel.ImagePath = @"Assets/Images/QuestionMark.png";
            }
            msgboxresult = objMessageBoxView.ShowDialogWindow(MessageBoxButton);

            return msgboxresult;
        }



        /// <summary>
        ///  //[pramod.misal][GEOS2-8321][date:09.07.2025]https://helpdesk.emdep.com/browse/GEOS2-8321
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <param name="messageImage"></param>
        /// <param name="messageBoxButton"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public static CustomPromptResultOTMEmail ShowOTMMPrompt(string message, string color, Enum messageImage, MessageBoxButton messageBoxButton, Window window = null)
        {
            OTMPromptViewModel messageBoxViewModel = new OTMPromptViewModel
            {
                Message = message
            };

            OTMPromptView objMessageBoxView = new OTMPromptView
            {
                Owner = window,
                DataContext = messageBoxViewModel,
                Height = 10,
                FontSize = 12,
                BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(color)),
                BorderThickness = new Thickness(10)
            };

            switch (messageImage)
            {
                case MessageImagePath.NotOk:
                    messageBoxViewModel.ImagePath = @"Assets/Images/NotOk.png";
                    break;
                case MessageImagePath.Ok:
                    messageBoxViewModel.ImagePath = @"Assets/Images/Ok.png";
                    break;
                case MessageImagePath.Info:
                    messageBoxViewModel.ImagePath = @"Assets/Images/Info.png";
                    break;
                case MessageImagePath.Warning:
                    messageBoxViewModel.ImagePath = @"Assets/Images/Warning.png";
                    break;
                case MessageImagePath.Question:
                    messageBoxViewModel.ImagePath = @"Assets/Images/QuestionMark.png";
                    break;
            }

            MessageBoxResult result = objMessageBoxView.ShowDialogWindow(messageBoxButton);

            return new CustomPromptResultOTMEmail
            {
                Result = result,
                ViewModel = messageBoxViewModel
            };
        }

    }

}
