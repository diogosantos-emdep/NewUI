using DevExpress.Mvvm.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Grid;
using System.Windows;
using Emdep.Geos.UI.Common;
using System.ComponentModel;
using DevExpress.Xpf.Core.Serialization;
using Prism.Logging;

namespace Emdep.Geos.UI.Helper
{
    public class OrderCustomGridService : ServiceBase, ICustomGridService, INotifyPropertyChanged
    {
        public string OrderGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "OrderGridSetting.Xml";
        public GridControl Grid
        {
            get { return (GridControl)GetValue(GridProperty); }
            set
            {
                SetValue(GridProperty, value);
                OnPropertyChanged(new PropertyChangedEventArgs("Grid"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }


        public static readonly DependencyProperty GridProperty = DependencyProperty.Register("Grid", typeof(GridControl), typeof(OrderCustomGridService), null);
        void ICustomGridService.Refresh()
        {
            if (Grid != null)
            {
                //Grid.RefreshData();
                Grid.RestoreLayoutFromXml(OrderGridSettingFilePath);

                Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));
                Grid.View.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                foreach (GridColumn column in Grid.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }

                }

            }

        }

        /// <summary>
        /// Method for remove filter save on grid layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            if (e.DependencyProperty == GridControl.FilterStringProperty)
                e.Allow = false;

            if (e.Property.Name == "GroupCount")
                e.Allow = false;

            if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                e.Allow = false;
        }

        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(OrderGridSettingFilePath);
                }



                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(OrderGridSettingFilePath);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        
    }

    /// <summary>
    ///  interface ICustomGridService already creted on TimeLineCustomGridService.cs
    /// </summary>
    //public interface ICustomGridService
    //{
    //    void Refresh();
    //}


}
