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
using System.Xml;
using System.IO;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Workbench.Downloader.ViewModels;
using Emdep.Geos.Utility;



namespace Emdep.Geos.Workbench.Updater
{
    /// <summary>
    /// Interaction logic for CheckForVersionWindow.xaml
    /// </summary>
    public partial class VersionUpdateWindow : Window
    {
        public VersionUpdateWindow()
        {
          
            InitializeComponent();
        }
    }
}