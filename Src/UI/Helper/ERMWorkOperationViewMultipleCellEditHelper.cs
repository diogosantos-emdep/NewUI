using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.ERM;

using System.Windows;
using System.Globalization;
using Emdep.Geos.UI.Common;

namespace Emdep.Geos.UI.Helper
{
   public class ERMWorkOperationViewMultipleCellEditHelper
    {

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ERMWorkOperationViewMultipleCellEditHelper), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));

        private static bool isLoad;
        public static bool IsLoad
        {
            get { return isLoad; }
            set { isLoad = value; }
        }

        public static void SetIsEnabled(UIElement element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(UIElement element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }

        private static void IsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            TableView view = source as TableView;
            view.CellValueChanging += view_CellValueChanging;
        }

        public static bool GetIsValueChanged(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsValueChangedProperty);
        }

        public static void SetIsValueChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(IsValueChangedProperty, value);
        }

        private static bool isValueChanged;

        public static bool IsValueChanged
        {
            get { return isValueChanged; }
            set {

                if (GeosApplication.Instance.IsEditWOPermissionERM)
                {
                    isValueChanged = value;
                }
                else
                {
                    isValueChanged = false;
                }
             }
        }

        public static string Checkview { get; set; }

        public static TableView viewtableview;
        public static TableView Viewtableview
        {
            get { return viewtableview; }
            set
            {
                viewtableview = value;
                Clonedviewtableview = viewtableview;
            }
        }

        private static TableView clonedviewtableview;
        public static TableView Clonedviewtableview
        {
            get { return clonedviewtableview; }
            set
            {
                clonedviewtableview = value;
            }
        }

        private static bool isChecked;
        public static bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        private static int idStatus;
        public static int IdStatus
        {
            get { return idStatus; }
            set { idStatus = value; }
        }
        #region GEOS2-4138
        private static string status;
        
        public static string Status
        {
            get { return status; }
            set { status = value; }
        }
        #endregion
        private static string htmlColor;
        public static string HtmlColor
        {
            get { return htmlColor; }
            set { htmlColor = value; }
        }

        private static string name;
        public static string Name
        {
            get { return name; }
            set { name = value; }
        }

        private static string description;
        public static string Description
        {
            get { return description; }
            set { description = value; }
        }

        private static UInt64 idParent;
        public static UInt64 IdParent
        {
            get { return idParent; }
            set { idParent = value; }
        }

        private static string parent;
        public static string Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private static UInt32? idType;
        public static UInt32? IdType
        {
            get { return idType; }
            set { idType = value; }
        }
        private static string type;
        public static string Type
        {
            get { return type; }
            set { type = value; }
        }

        private static float? distance;
        public static float? Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        private static float observedTime;
        public static float ObservedTime
        {
            get { return observedTime; }
            set { observedTime = value; }
        }

        private static Int32 activity;
        public static Int32 Activity
        {
            get { return activity; }
            set { activity = value; }
        }

        private static float normalTime;
        public static float NormalTime
        {
            get { return normalTime; }
            set { normalTime = value; }
        }
        #region GEOS2-3954 Time Format HH:mm:ss
        private static TimeSpan uITempobservedTime;
        public static TimeSpan UITempobservedTime
        {
            get { return uITempobservedTime; }
            set { uITempobservedTime = value; }
        }

        private static TimeSpan uITempNormalTime;
        public static TimeSpan UITempNormalTime
        {
            get { return uITempNormalTime; }
            set { uITempNormalTime = value; }
        }
        #endregion

        //[GEOS2-3933][Rupali Sarode][19/09/2022]
        private static string remarks;

        public static string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        private static string detectedProblem;
        public static string DetectedProblem
        {
            get { return detectedProblem; }
            set { detectedProblem = value; }
        }

        private static string improvementProposal;
        public static string ImprovementProposal
        {
            get { return improvementProposal; }
            set { improvementProposal = value; }
        }

        public static readonly DependencyProperty IsValueChangedProperty =
        DependencyProperty.RegisterAttached("IsValueChanged", typeof(bool), typeof(ERMWorkOperationViewMultipleCellEditHelper), new PropertyMetadata(false));

        public static List<int> MmodifiedRowHandles = new List<int>();

        static void View_CellValueChanged1(object sender, CellValueChangedEventArgs e)
        {
            MmodifiedRowHandles.Add(e.RowHandle);
        }

        static void view_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                TableView view = sender as TableView;
                Checkview = view.Name;
                Viewtableview = view;

                List<GridCell> cells = view.GetSelectedCells().ToList<GridCell>();
                List<GridCell> selectedCells = cells.Where(c => c.Column == e.Column).ToList<GridCell>();
                IList<GridRowInfo> newRowData = view.GetSelectedRows();

                view.PostEditor();

                if (e.Cell.Row != null)
                {
                    IsValueChanged = true;
                    if (IsValueChanged)
                    {
                        foreach (GridCell cell in selectedCells)
                        {
                            int row = cell.RowHandle;
                            string columnName = cell.Column.FieldName;

                            ((GridControl)view.DataControl).SetCellValue(row, "IsChecked", true);
                            IsChecked = true;

                            if (columnName == "WOStatus")       //GEOS2-4138- gulab lakade 19/01/2023
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                //IdStatus = Convert.ToInt32(e.Value);
                                Status = Convert.ToString(e.Value);     //GEOS2-4138- gulab lakade 19/01/2023
                                //IdStatus = Convert.ToInt32(StatusList.FirstOrDefault(a => a.Va == e.Value));
                                foreach (var item in newRowData)
                                {
                                    //((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).StatusHtmlColor = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).HtmlColor;
                                    //((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).StatusHTMLColor = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).StatusList.FirstOrDefault(a => a.IdLookupValue == IdStatus).HtmlColor;
                                    //((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).WOStatus = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).StatusList.FirstOrDefault(a => a.IdLookupValue == IdStatus).Value;
                                    #region GEOS2-4138- gulab lakade 19/01/2023
                                    ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).StatusHTMLColor = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).StatusList.FirstOrDefault(a => a.Value == Status).HtmlColor;
                                    ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).WOStatus = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).StatusList.FirstOrDefault(a => a.Value == Status).Value;
                                    ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).IdStatus =Convert.ToUInt32( ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).StatusList.FirstOrDefault(a => a.Value == Status).IdLookupValue);
                                    IdStatus = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).StatusList.FirstOrDefault(a => a.Value == Status).IdLookupValue;
                                    #endregion
                                }
                            }
                            else if (columnName == "Name")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                Name = (string)e.Value;
                                foreach (var item in newRowData)
                                {
                                    ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Name = Name;
                                }
                            }

                            else if (columnName == "Parent")        //GEOS2-4138- gulab lakade 19/01/2023
                            {
                                if (e.Value is WorkOperation)
                                {
                                    WorkOperation tempWorkOperation = (WorkOperation)e.Value;
                                    ((GridControl)view.DataControl).SetCellValue(row, columnName, tempWorkOperation.IdParent);
                                    ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                    Parent=(string)(e.Value);       //GEOS2-4138- gulab lakade 19/01/2023
                                    // IdParent = Convert.ToUInt64(tempWorkOperation.IdWorkOperation);
                                    foreach (var item in newRowData)
                                    {
                                        //((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Parent = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ParentList.FirstOrDefault(a => a.IdWorkOperation == IdParent).Name;
                                        ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Parent = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ParentList.FirstOrDefault(a => a.Name == Parent).Name;       //GEOS2-4138- gulab lakade 19/01/2023
                                    }
                                }
                                else
                                {
                                    ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                    ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                    Parent=(string)(e.Value);       //GEOS2-4138- gulab lakade 19/01/2023

                                    //IdParent = Convert.ToUInt64(e.Value);
                                    foreach (var item in newRowData)
                                    {
                                        //((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Parent = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ParentList.FirstOrDefault(a => a.IdWorkOperation == IdParent).Name;
                                        ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Parent = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ParentList.FirstOrDefault(a => a.Name == Parent).Name;
                                    }
                                }
                            }

                            else if (columnName == "Type.Value")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);

                                Type = (string)(e.Value);
                                foreach (var item in newRowData)
                                {
                                    if (((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Type == null)
                                        ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Type = new Data.Common.Epc.LookupValue();

                                    ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Type.Value = Type;
                                }
                            }
                            else if (columnName == "Description")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                Description = Convert.ToString(e.Value);
                                Description = Description.Trim(' ', '\r');
                                foreach (var item in newRowData)
                                {
                                    ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Description = Description;
                                }
                            }
                            else if (columnName == "Distance")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                if (e.Value != null)
                                    Distance = (float)Convert.ToDecimal(e.Value);
                                else
                                    Distance = null;

                                foreach (var item in newRowData)
                                {
                                    ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Distance = Distance.Value;
                                }
                            }
                            else if (columnName == "ObservedTime")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                if (e.Value != null)
                                    ObservedTime = (float)Convert.ToDecimal(e.Value);


                                foreach (var item in newRowData)
                                {
                                    ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ObservedTime = ObservedTime;
                                    if (((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ObservedTime != null && ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ObservedTime > 0 && ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity > 0)
                                        ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).NormalTime = (float)Math.Round(((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ObservedTime.Value * ((float)((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity / 100), 2);
                                    else
                                        ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).NormalTime = 0;


                                }
                            }
                            //else if (columnName == "Activity")
                            //{
                            //    ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                            //    ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);

                            //    if (e.Value != null)
                            //        Activity = Convert.ToInt32(e.Value);
                            //    else
                            //        Activity = 0;

                            //    if (Activity < 0)
                            //        Activity = 0;
                            //    foreach (var item in newRowData)
                            //    {
                            //        ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity = Activity;
                            //        if (((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ObservedTime != null && ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ObservedTime > 0 && ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity > 0)
                            //            ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).NormalTime = (float)Math.Round(((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ObservedTime.Value * ((float)((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity / 100), 2);
                            //        else
                            //            ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).NormalTime = 0;
                            //    }
                            //}

                            else if (columnName == "UITempobservedTime")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                if (e.Value != null)

                                    foreach (var item in newRowData)
                                    {
                                        // var currentculter = CultureInfo.CurrentCulture;
                                        // string DecimalSeperator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                                        // int TempOTDay = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempobservedTime.Days;
                                        // int TempOTHours = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempobservedTime.Hours;
                                        // int TempOTminute = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempobservedTime.Minutes;
                                        // int TempOTSecond = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempobservedTime.Seconds;
                                        // string tempstring = Convert.ToString(((TempOTDay * 24) + TempOTHours) * 60 + TempOTminute) + DecimalSeperator + TempOTSecond;
                                        // if (TempOTHours > 0)
                                        //     ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).IsObservedTimeHoursExist = true;
                                        // else
                                        //     ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).IsObservedTimeHoursExist = false;
                                        // float tempfloat = float.Parse(tempstring);
                                        // ObservedTime = tempfloat;
                                        // if (tempfloat != null && tempfloat > 0 && ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity > 0)
                                        //     NormalTime = (float)Math.Round(tempfloat * ((float)((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity / 100), 2);
                                        // else
                                        //     NormalTime = 0;

                                        // string temnormaltime = Convert.ToString(NormalTime);
                                        // string[] NormaltimeArr = new string[2];
                                        // int nt1 = 0;
                                        // int nt2 = 0;
                                        // if (temnormaltime.Contains(DecimalSeperator))
                                        // {
                                        //     NormaltimeArr = temnormaltime.Split(Convert.ToChar(DecimalSeperator));
                                        //     nt1 = int.Parse(NormaltimeArr[0]);
                                        //     nt2 = int.Parse(NormaltimeArr[1]);
                                        //     nt1 = (nt1 * 60) + nt2;
                                        // }
                                        // else
                                        // {
                                        //     NormaltimeArr = temnormaltime.Split(Convert.ToChar(DecimalSeperator));
                                        //     nt1 = int.Parse(NormaltimeArr[0]);
                                        //     nt1 = (nt1 * 60);
                                        // }
                                        //((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempNormalTime = TimeSpan.FromSeconds(nt1);
                                        float tempfloat = (float)Math.Round(Convert.ToDouble(((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempobservedTime.TotalMinutes), 2);
                                        if (tempfloat != null && tempfloat > 0 && ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity > 0)
                                            NormalTime = (float)Math.Round(tempfloat * ((float)((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity / 100), 2);
                                        else
                                            NormalTime = 0;
                                        #region [GEOS2-4982][gulab lakade][25 10 2023]
                                        //((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempNormalTime = TimeSpan.FromMinutes(NormalTime);
                                        TimeSpan tempnormalTime = TimeSpan.FromMinutes(Convert.ToDouble(NormalTime));
                                        if (tempnormalTime.Milliseconds >= 600)
                                        {

                                            tempnormalTime = tempnormalTime.Add(-TimeSpan.FromMilliseconds(tempnormalTime.Milliseconds));
                                            TimeSpan Second = TimeSpan.FromMilliseconds(1000);
                                            tempnormalTime = tempnormalTime.Add(Second);
                                            ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempNormalTime = tempnormalTime;// TimeSpan.FromMinutes(Convert.ToDouble(temp.ObservedTime.Value));

                                        }
                                        else
                                        {
                                            tempnormalTime = tempnormalTime.Add(-TimeSpan.FromMilliseconds(tempnormalTime.Milliseconds));

                                            ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempNormalTime = tempnormalTime;

                                        }
                                        if (((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempNormalTime.Hours > 0)
                                            ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).IsNormalTimeHoursExist = true;
                                        else
                                            ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).IsNormalTimeHoursExist = false;
                                        #endregion
                                    }
                            }
                            else if (columnName == "Activity")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);

                                if (e.Value != null)
                                    Activity = Convert.ToInt32(e.Value);
                                else
                                    Activity = 0;

                                if (Activity < 0)
                                    Activity = 0;
                                foreach (var item in newRowData)
                                {
                                    try
                                    {


                                        ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity = Activity;
                                        if (((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ObservedTime != null && ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ObservedTime > 0 && ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity > 0)
                                            ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).NormalTime = (float)Math.Round(((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ObservedTime.Value * ((float)((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity / 100), 2);
                                        else
                                            ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).NormalTime = 0;
                                    }
                                    catch (Exception ex) { }

                                    try
                                    {
                                        #region [GEOS2-4982][gulab lakade][25 10 2023]
                                        //     var currentculter = CultureInfo.CurrentCulture;
                                        //     string DecimalSeperator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                                        //     int TempOTDay = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempobservedTime.Days;
                                        //     int TempOTHours = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempobservedTime.Hours;
                                        //     int TempOTminute = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempobservedTime.Minutes;
                                        //     int TempOTSecond = ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempobservedTime.Seconds;
                                        //     string tempstring = Convert.ToString(((TempOTDay * 24) + TempOTHours) * 60 + TempOTminute) + DecimalSeperator + TempOTSecond;
                                        //     float tempfloat = float.Parse(tempstring);
                                        //     ObservedTime = tempfloat;
                                        //     if (tempfloat != null && tempfloat > 0 && Activity > 0)
                                        //         NormalTime = (float)Math.Round(tempfloat * ((float)Activity / 100), 2);
                                        //     else
                                        //         NormalTime = 0;

                                        //     string temnormaltime = Convert.ToString(NormalTime);
                                        //     string[] NormaltimeArr = new string[2];
                                        //     int nt1 = 0;
                                        //     int nt2 = 0;
                                        //     if (temnormaltime.Contains(DecimalSeperator))
                                        //     {
                                        //         NormaltimeArr = temnormaltime.Split(Convert.ToChar(DecimalSeperator));
                                        //         nt1 = int.Parse(NormaltimeArr[0]);
                                        //         nt2 = int.Parse(NormaltimeArr[1]);
                                        //         nt1 = (nt1 * 60) + nt2;
                                        //     }
                                        //     else
                                        //     {
                                        //         NormaltimeArr = temnormaltime.Split(Convert.ToChar(DecimalSeperator));
                                        //         nt1 = int.Parse(NormaltimeArr[0]);
                                        //         nt1 = (nt1 * 60);
                                        //     }



                                        //((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempNormalTime = TimeSpan.FromSeconds(nt1);

                                        float tempfloat = (float)Math.Round(Convert.ToDouble(((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempobservedTime.TotalMinutes), 2);
                                        if (tempfloat != null && tempfloat > 0 && ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity > 0)
                                            NormalTime = (float)Math.Round(tempfloat * ((float)((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Activity / 100), 2);
                                        else
                                            NormalTime = 0;

                                        //((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempNormalTime = TimeSpan.FromMinutes(NormalTime);

                                        TimeSpan tempnormalTime = TimeSpan.FromMinutes(Convert.ToDouble(NormalTime));
                                        if (tempnormalTime.Milliseconds >= 600)
                                        {

                                            tempnormalTime = tempnormalTime.Add(-TimeSpan.FromMilliseconds(tempnormalTime.Milliseconds));
                                            TimeSpan Second = TimeSpan.FromMilliseconds(1000);
                                            tempnormalTime = tempnormalTime.Add(Second);
                                            ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempNormalTime = tempnormalTime;// TimeSpan.FromMinutes(Convert.ToDouble(temp.ObservedTime.Value));

                                        }
                                        else
                                        {
                                            tempnormalTime = tempnormalTime.Add(-TimeSpan.FromMilliseconds(tempnormalTime.Milliseconds));

                                            ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).UITempNormalTime = tempnormalTime;

                                        }
                                        #endregion
                                    }
                                    catch (Exception ex) { }
                                }
                            }

                            //[GEOS2-3933][Rupali Sarode][20/09/2022]
                            else if (columnName == "Remarks")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                Remarks = Convert.ToString(e.Value);

                                foreach (var item in newRowData)
                                {
                                    ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).Remarks = Remarks;
                                }
                            }

                            else if (columnName == "DetectedProblems")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                DetectedProblem = Convert.ToString(e.Value);

                                foreach (var item in newRowData)
                                {
                                    ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).DetectedProblems = DetectedProblem;
                                }
                            }
                            else if (columnName == "ImprovementsProposals")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                ImprovementProposal = Convert.ToString(e.Value);

                                foreach (var item in newRowData)
                                {
                                    ((Emdep.Geos.Data.Common.ERM.WorkOperation)item.Row).ImprovementsProposals = ImprovementProposal;
                                }
                            }
                            else
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                            }

                        ((GridControl)view.DataControl).RefreshRow(row);
                            ((GridControl)view.DataControl).UpdateLayout();
                        }

                        SetIsValueChanged(view, true);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
