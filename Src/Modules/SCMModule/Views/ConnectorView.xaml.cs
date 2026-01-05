using DevExpress.Xpf.Editors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using System.Globalization;
using System.ComponentModel;
using Emdep.Geos.Modules.SCM.ViewModels;
using DevExpress.Xpf.Docking;
using System.IO;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Utility;
using DevExpress.Xpf.WindowsUI;

namespace Emdep.Geos.Modules.SCM.Views
{
    /// <summary>
    /// </summary>
    public partial class ConnectorView : UserControl
    /// Interaction logic for ConnectorView.xaml
    {
        //[rdixit][GEOS2-8327][22.07.2025] Removed Pin-Unpin
        CaptionLocation CaptionLocation = CaptionLocation.Top;
        List<Key> textableKeys = new List<Key>
            {
                Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J, Key.K, Key.L,
                Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z,
                Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9,
                Key.Oem1, Key.Oem2, Key.Oem3, Key.Oem4, Key.Oem5, Key.Oem6, Key.Oem7, Key.Oem8
            };
        public ConnectorView()
        {
            InitializeComponent();
            CaptionLocation = CaptionLocation.Bottom;
            DevExpress.Xpf.Grid.GridControl.AllowInfiniteGridSize = true;
        }

        private void ScrollUpButton_Click(object sender, RoutedEventArgs e)
        {
            scroll.PageUp();
        }

        private void ScrollDownButton_Click(object sender, RoutedEventArgs e)
        {
            scroll.PageDown();
        }

        //Shubham[skadam] GEOS2-4595 SCM - Search results (1/4) 08 09 2023
        //private void bUp_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Top;
        //    documentContainer.CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Top;
        //}
        //private void bDown_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Bottom;
        //    documentContainer.CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Bottom;
        //}
        //private void bLeft_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Left;
        //    documentContainer.CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Left;
        //}
        //private void bRight_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Right;
        //    documentContainer.CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Right;
        //}

        //[rdixit][16.09.2023]
        #region To Open Combo when user input some value
        private void cmbFamily_KeyUp(object sender, KeyEventArgs e)
        {
            if (textableKeys.Contains(e.Key))
            {
                // Open the ComboBox popup
                cmbFamily.IsPopupOpen = true;
            }
        }

        private void cmbSubfamily_KeyUp(object sender, KeyEventArgs e)
        {
            if (textableKeys.Contains(e.Key))
            {
                // Open the ComboBox popup
                cmbSubfamily.IsPopupOpen = true;
            }
        }

        private void Name_KeyUp(object sender, KeyEventArgs e)
        {
            if (textableKeys.Contains(e.Key))
            {
                // Open the ComboBox popup
                Name.IsPopupOpen = true;
            }
        }

        private void cmbColor_KeyUp(object sender, KeyEventArgs e)
        {
            if (textableKeys.Contains(e.Key))
            {
                // Open the ComboBox popup
                cmbColor.IsPopupOpen = true;
            }
        }

        private void cmbGender_KeyUp(object sender, KeyEventArgs e)
        {
            if (textableKeys.Contains(e.Key))
            {
                // Open the ComboBox popup
                cmbGender.IsPopupOpen = true;
            }
        }
        #endregion

        //[rdixit][16.09.2023]
        private void scroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;

            // Check if the user has scrolled to the top
            if (scrollViewer.VerticalOffset == 0)
            {
                downButton.Visibility = Visibility.Visible;
                upButton.Visibility = Visibility.Hidden;
            }

            // Check if the user has scrolled to the bottom
            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                downButton.Visibility = Visibility.Hidden;
                upButton.Visibility = Visibility.Visible;
            }
        }
        //private void clearbtn_Click(object sender, RoutedEventArgs e)
        //{
        //Emdep.Geos.Modules.SCM.ViewModels.ConnectorViewModel ConnectorViewModel = new Emdep.Geos.Modules.SCM.ViewModels.ConnectorViewModel();
        //AllCustomData(SCMCommon.Instance.Grid, ConnectorViewModel.CustomListByIdFamily, ConnectorViewModel.CustomList, (Family)ConnectorViewModel.SelectedFamilyList?.FirstOrDefault(), ConnectorViewModel.StandardPropertiedbyFamily);
        //}
        //private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    SCMCommon.Instance.Grid = ((Emdep.Geos.Modules.SCM.Views.ConnectorView)sender).Grid1;
        //    Emdep.Geos.Modules.SCM.ViewModels.ConnectorViewModel ConnectorViewModel = new Emdep.Geos.Modules.SCM.ViewModels.ConnectorViewModel();
        //    AllCustomData(SCMCommon.Instance.Grid, ConnectorViewModel.CustomListByIdFamily, ConnectorViewModel.CustomList,
        //        (Family)ConnectorViewModel.SelectedFamilyList?.FirstOrDefault(), ConnectorViewModel.StandardPropertiedbyFamily);
        //}
        #region RND
        /*
        public void AllCustomData(Grid MainGrid, List<CustomProperty> CustomListByIdFamily, List<CustomProperty> CustomList, Family selectedfmly, List<ConnectorProperties> standardPropertiedbyFamily)//rajashri GEOS2-5227 Added this parameter-  List<ConnectorProperties> standardPropertiedbyFamily
        {

            try
            {
                DevExpress.Xpf.LayoutControl.LayoutGroup LayoutGroup = null;
                if (MainGrid != null)
                {
                    LayoutGroup = (DevExpress.Xpf.LayoutControl.LayoutGroup)MainGrid.Parent;
                }

                #region
                if (CustomListByIdFamily == null)
                    CustomListByIdFamily = new List<CustomProperty>();
                if (SCMCommon.Instance.CustomProperties == null)
                    SCMCommon.Instance.CustomProperties = new Dictionary<string, string>();
                if (SCMCommon.Instance.CustomPropertiesMin == null)
                    SCMCommon.Instance.CustomPropertiesMin = new Dictionary<string, string>();
                if (SCMCommon.Instance.CustomPropertiesMax == null)
                    SCMCommon.Instance.CustomPropertiesMax = new Dictionary<string, string>();
                if (SCMCommon.Instance.CustomPropertiesList == null)
                    SCMCommon.Instance.CustomPropertiesList = new Dictionary<string, List<string>>();
                //[GEOS2-6073][rdixit][29.11.2024]
                if (selectedfmly != null)
                    MainGrid.Height = CustomListByIdFamily.Count * 50;
                else
                    MainGrid.Height = double.NaN;
                #endregion
                if (CustomListByIdFamily.Count > 0)
                {
                    #region Not null                  
                    SCMCommon.Instance.SelectedList = new ObservableCollection<Data.Common.SCM.ValueType>();
                    MainGrid.Children.Clear();
                    LayoutGroup.IsCollapsed = true;
                    LayoutGroup.IsCollapsible = true;
                    var TempCustomPropertyGroup = CustomListByIdFamily.GroupBy(x => x.IdCustomConnectorProperty).ToList();
                    int GridRow = TempCustomPropertyGroup.Count();
                    if (GridRow > 0)
                    {
                        #region ColumnDefinition
                        for (var a = 0; a <= 2; a++)
                        {
                            if (a == 0)
                            {
                                var c = new ColumnDefinition();
                                c.Width = GridLength.Auto;
                                MainGrid.ColumnDefinitions.Add(c);
                            }
                            else if (a == 1)
                            {
                                var c = new ColumnDefinition();
                                c.Width = new GridLength(10, GridUnitType.Pixel);
                                MainGrid.ColumnDefinitions.Add(c);
                            }
                            else if (a == 2)
                            {
                                var c = new ColumnDefinition();
                                c.Width = new GridLength(200, GridUnitType.Pixel);
                                MainGrid.ColumnDefinitions.Add(c);
                            }
                            else
                            {
                                var c = new ColumnDefinition();
                                c.Width = GridLength.Auto;
                                MainGrid.ColumnDefinitions.Add(c);
                            }
                        }
                        #endregion

                        #region RowDefinition 
                        for (var a = 0; a < GridRow; a++)
                        {
                            var r = new RowDefinition();
                            r.Height = new GridLength(40, GridUnitType.Pixel);
                            MainGrid.RowDefinitions.Add(r);
                        }

                        #endregion
                        int Rowcount = 0;
                        foreach (var item in TempCustomPropertyGroup)
                        {

                            var TempCustomListByName = CustomListByIdFamily.Where(x => x.IdCustomConnectorProperty == item.Key).ToList();
                            string CustomPropertyName = CustomListByIdFamily.Where(x => x.IdCustomConnectorProperty == item.Key).FirstOrDefault().Name;
                            var myDefinition = new ColumnDefinition();
                            #region Label
                            TextBlock lbl = new TextBlock();
                            lbl.Text = CustomPropertyName;
                            lbl.ToolTip = CustomPropertyName;
                            lbl.Width = 100;
                            lbl.TextAlignment = TextAlignment.Left;
                            lbl.VerticalAlignment = VerticalAlignment.Center;
                            Grid.SetRow(lbl, Rowcount);
                            Grid.SetColumn(lbl, 0);
                            MainGrid.Children.Add(lbl);
                            #endregion
                            var ValueGroup = CustomListByIdFamily.Where(x => x.IdCustomConnectorProperty == item.Key).GroupBy(a => a.ValueKey.LookupKeyName).FirstOrDefault();
                            if (ValueGroup != null)
                            {

                                myDefinition.Width = new GridLength(68);
                                MainGrid.ColumnDefinitions.Add(myDefinition);
                                var matchingStandardProperty = standardPropertiedbyFamily.FirstOrDefault(prop => prop.IdConnectorProperty == item.Key);//rajashri GEOS2-5227
                                if (matchingStandardProperty == null)
                                    matchingStandardProperty = new ConnectorProperties();
                                switch (Convert.ToString(ValueGroup.Key))
                                {
                                    case "List":
                                        #region ComboBox
                                        List<Data.Common.SCM.ValueType> TempValueList = CustomListByIdFamily.Where(x => x.IdCustomConnectorProperty == item.Key).Select(a => a.ValueType).ToList();
                                        ComboBoxEdit ComboBox = new ComboBoxEdit();
                                        CheckedComboBoxStyleSettings styleSettings = new CheckedComboBoxStyleSettings();
                                        ComboBox.EditValueChanged += (sender, e) => ComboBox_EditValueChanged(sender, e, matchingStandardProperty?.PropertyName);
                                        ComboBox.Width = 160;
                                        ComboBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                                        ComboBox.Margin = new Thickness(0, 5, 0, 5);
                                        ComboBox.ItemsSource = TempValueList;
                                        ComboBox.SelectedIndex = 0;
                                        ComboBox.StyleSettings = styleSettings;
                                        ComboBox.DisplayMember = "Name";
                                        #region //rajashri GEOS2-5227                                     
                                        if (matchingStandardProperty != null && !string.IsNullOrEmpty(matchingStandardProperty?.DefaultValue))
                                        {
                                            if (!matchingStandardProperty.IsEnabled)
                                            {
                                                ComboBox.IsReadOnly = true;
                                                ComboBox.PreviewMouseLeftButtonDown += (sender, e) => { e.Handled = true; };
                                            }

                                            if (matchingStandardProperty.DefaultValue == "False")
                                                ComboBox.SelectedItem = null;
                                            else
                                            {
                                                var selectedItem = TempValueList.FirstOrDefault(i => i.Name == matchingStandardProperty.DefaultValue);
                                                if (selectedItem != null)
                                                {
                                                    ComboBox.EditValue = new ObservableCollection<object>() { (object)selectedItem };//.Add(selectedItem);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!matchingStandardProperty.IsEnabled)
                                            {
                                                ComboBox.IsReadOnly = true;
                                                ComboBox.PreviewMouseLeftButtonDown += (sender, e) => { e.Handled = true; };
                                                ComboBox.SelectedItem = null;
                                            }
                                            else
                                            {
                                                ComboBox.SelectedItem = null;
                                            }
                                        }
                                        #endregion
                                        Grid.SetRow(ComboBox, Rowcount);
                                        Grid.SetColumn(ComboBox, 2);
                                        MainGrid.Children.Add(ComboBox);
                                        LayoutGroup.IsCollapsed = false;
                                        LayoutGroup.IsCollapsible = true;

                                        #endregion
                                        break;
                                    case "Number":
                                        #region TextBox
                                        Grid Grid1 = new Grid();
                                        Grid1.Width = 170;
                                        #region ColumnDefinition
                                        for (var a = 0; a <= 6; a++)
                                        {
                                            if (a % 2 == 1)
                                            {
                                                var c = new ColumnDefinition();
                                                c.Width = new GridLength(5, GridUnitType.Pixel);
                                                Grid1.ColumnDefinitions.Add(c);
                                            }
                                            else if (a == 0)
                                            {
                                                var c = new ColumnDefinition();
                                                c.Width = new GridLength(40, GridUnitType.Pixel);
                                                Grid1.ColumnDefinitions.Add(c);
                                            }
                                            else if (a == 4)
                                            {
                                                var c = new ColumnDefinition();
                                                c.Width = new GridLength(25, GridUnitType.Pixel);
                                                Grid1.ColumnDefinitions.Add(c);
                                            }
                                            else if (a == 2 || a == 6)
                                            {
                                                var c = new ColumnDefinition();
                                                c.Width = new GridLength(40, GridUnitType.Pixel);
                                                Grid1.ColumnDefinitions.Add(c);
                                            }
                                            else
                                            {
                                                var c = new ColumnDefinition();
                                                c.Width = GridLength.Auto;
                                                Grid1.ColumnDefinitions.Add(c);
                                            }
                                        }
                                        #endregion
                                        #region Label Number                       
                                        TextBlock lblNumMin = new TextBlock();
                                        lblNumMin.Text = System.Windows.Application.Current.FindResource("SCMFrom").ToString();
                                        lblNumMin.ToolTip = System.Windows.Application.Current.FindResource("SCMFrom").ToString();
                                        lblNumMin.Width = 40;
                                        lblNumMin.TextAlignment = TextAlignment.Left;
                                        lblNumMin.Margin = new Thickness(5, 7, 0, 5);
                                        Grid.SetColumn(lblNumMin, 0);
                                        Grid1.Children.Add(lblNumMin);
                                        TextBlock lblNumMax = new TextBlock();
                                        lblNumMax.Text = System.Windows.Application.Current.FindResource("SCMTo").ToString();
                                        lblNumMax.ToolTip = System.Windows.Application.Current.FindResource("SCMTo").ToString();
                                        lblNumMax.Width = 25;
                                        lblNumMax.TextAlignment = TextAlignment.Left;
                                        lblNumMax.Margin = new Thickness(0, 7, 0, 5);
                                        Grid.SetColumn(lblNumMax, 4);
                                        Grid1.Children.Add(lblNumMax);
                                        #endregion
                                        TextEdit EditNumber = new TextEdit();
                                        EditNumber.Width = 40;
                                        EditNumber.Margin = new Thickness(0, 5, 0, 10);
                                        EditNumber.Mask = "######0;";
                                        EditNumber.MaskUseAsDisplayFormat = true;
                                        EditNumber.MaskType = MaskType.Numeric;
                                        EditNumber.NullText = null;
                                        EditNumber.Text = string.Empty;
                                        EditNumber.EditValueChanged += (sender, e) => NumberMinEditNumber_SelectionChanged(sender, e, matchingStandardProperty?.PropertyName);
                                        #region //rajashri GEOS2-5227                                
                                        if (matchingStandardProperty == null)
                                            matchingStandardProperty = new ConnectorProperties();
                                        if (matchingStandardProperty.IsEnabled)
                                        {
                                            EditNumber.PreviewTextInput += (sender, e) =>
                                            {
                                                if (!char.IsDigit(e.Text, 0) && e.Text != "-")
                                                {
                                                    e.Handled = true;
                                                }
                                            };
                                        }
                                        else
                                        {
                                            EditNumber.IsReadOnly = true; // Set the TextBox as read-only if editing is not allowed
                                        }

                                        if (matchingStandardProperty != null)
                                        {
                                            string selectedItem = string.Empty;
                                            if ((string.IsNullOrEmpty(matchingStandardProperty.MinValueNew) && string.IsNullOrEmpty(matchingStandardProperty.MaxValueNew)) && !string.IsNullOrEmpty(matchingStandardProperty.DefaultValue))
                                                selectedItem = matchingStandardProperty.DefaultValue;
                                            else
                                                selectedItem = matchingStandardProperty.MinValueNew;
                                            if (selectedItem != null)
                                            {
                                                EditNumber.Text = Convert.ToString(selectedItem);
                                                var matching = SCMCommon.Instance.CustomPropertiesMin.FirstOrDefault(i => i.Key == matchingStandardProperty?.PropertyName);

                                                if (matching.Key != null) // Check if a match was found
                                                {
                                                    SCMCommon.Instance.CustomPropertiesMin[matching.Key] = selectedItem.ToString();
                                                }
                                                else
                                                {
                                                    SCMCommon.Instance.CustomPropertiesMin.Add(matchingStandardProperty?.PropertyName, selectedItem.ToString());
                                                }
                                            }
                                        }
                                        #endregion

                                        Grid.SetColumn(EditNumber, 2);
                                        Grid1.Children.Add(EditNumber);

                                        TextEdit EditNumberMax = new TextEdit();
                                        EditNumberMax.Width = 40;
                                        EditNumberMax.Margin = new Thickness(0, 5, 0, 10);
                                        EditNumberMax.Text = string.Empty;
                                        EditNumberMax.Mask = "######0;";
                                        EditNumberMax.MaskUseAsDisplayFormat = true;
                                        EditNumberMax.MaskType = MaskType.Numeric;
                                        EditNumberMax.NullText = null;
                                        EditNumberMax.Text = string.Empty;
                                        EditNumberMax.EditValueChanged += (sender, e) => NumberMaxEditNumber_SelectionChanged(sender, e, matchingStandardProperty?.PropertyName);
                                        #region //rajashri GEOS2-5227                                
                                        if (matchingStandardProperty == null)
                                            matchingStandardProperty = new ConnectorProperties();
                                        if (matchingStandardProperty.IsEnabled)
                                        {
                                            EditNumberMax.PreviewTextInput += (sender, e) =>
                                            {
                                                if (!char.IsDigit(e.Text, 0) && e.Text != "-")
                                                {
                                                    e.Handled = true;
                                                }
                                            };
                                        }
                                        else
                                        {
                                            EditNumberMax.IsReadOnly = true; // Set the TextBox as read-only if editing is not allowed
                                        }

                                        if (matchingStandardProperty != null)
                                        {
                                            string selectedItem = string.Empty;
                                            if ((string.IsNullOrEmpty(matchingStandardProperty.MinValueNew) && string.IsNullOrEmpty(matchingStandardProperty.MaxValueNew)) && !string.IsNullOrEmpty(matchingStandardProperty.DefaultValue))
                                                selectedItem = matchingStandardProperty.DefaultValue;
                                            else
                                                selectedItem = matchingStandardProperty.MaxValueNew;
                                            if (selectedItem != null)
                                            {
                                                EditNumberMax.Text = Convert.ToString(selectedItem);
                                                var matching = SCMCommon.Instance.CustomPropertiesMax.FirstOrDefault(i => i.Key == matchingStandardProperty?.PropertyName);

                                                if (matching.Key != null) // Check if a match was found
                                                {
                                                    SCMCommon.Instance.CustomPropertiesMax[matching.Key] = selectedItem.ToString();
                                                }
                                                else
                                                {
                                                    SCMCommon.Instance.CustomPropertiesMax.Add(matchingStandardProperty?.PropertyName, selectedItem.ToString());
                                                }
                                            }
                                        }
                                        #endregion

                                        Grid.SetColumn(EditNumberMax, 6);
                                        Grid1.Children.Add(EditNumberMax);
                                        Grid.SetRow(Grid1, Rowcount);
                                        Grid.SetColumn(Grid1, 2);
                                        MainGrid.Children.Add(Grid1);
                                        LayoutGroup.IsCollapsed = false;
                                        LayoutGroup.IsCollapsible = true;
                                        #endregion
                                        break;
                                    case "Text":
                                        #region TextBox
                                        string TempValueText = "";
                                        TextBox dynamicTexttEdit = new TextBox();
                                        dynamicTexttEdit.Width = 160;
                                        dynamicTexttEdit.Margin = new Thickness(0, 5, 0, 5);
                                        dynamicTexttEdit.Text = TempValueText;

                                        dynamicTexttEdit.TextChanged += (sender, e) => DynamicTextEditNumber_SelectionChanged(sender, e, matchingStandardProperty?.PropertyName);
                                        dynamicTexttEdit.Text = SCMCommon.Instance.SelectedList.ToString();
                                        #region //rajashri GEOS2-5227
                                        if (matchingStandardProperty == null)
                                            matchingStandardProperty = new ConnectorProperties();
                                        if (!matchingStandardProperty.IsEnabled)
                                        {
                                            dynamicTexttEdit.IsReadOnly = true; // Set the TextBox as read-only if editing is not allowed
                                        }

                                        if (matchingStandardProperty != null)
                                        {
                                            var selectedItem = matchingStandardProperty.DefaultValue;

                                            if (selectedItem != null)
                                            {
                                                dynamicTexttEdit.Text = Convert.ToString(selectedItem);
                                                var matching = SCMCommon.Instance.CustomProperties.FirstOrDefault(i => i.Key == matchingStandardProperty?.PropertyName);

                                                if (matching.Key != null) // Check if a match was found
                                                {
                                                    SCMCommon.Instance.CustomProperties[matching.Key] = selectedItem.ToString();
                                                }
                                                else
                                                {
                                                    SCMCommon.Instance.CustomProperties.Add(matchingStandardProperty?.PropertyName, selectedItem.ToString());
                                                }
                                            }
                                        }
                                        #endregion
                                        Grid.SetRow(dynamicTexttEdit, Rowcount);
                                        Grid.SetColumn(dynamicTexttEdit, 2);
                                        MainGrid.Children.Add(dynamicTexttEdit);
                                        LayoutGroup.IsCollapsed = false;
                                        LayoutGroup.IsCollapsible = true;
                                        #endregion
                                        break;
                                    case "Boolean":
                                        #region ToggleSwitch
                                        ToggleSwitch ToggleSwitch = new ToggleSwitch();
                                        ToggleSwitch.Width = 70;
                                        bool isItemKeyBoolean = ValueGroup.Key == "Boolean" && TempCustomPropertyGroup.Count == 1;

                                        if (isItemKeyBoolean)
                                        {
                                            ToggleSwitch.Margin = new Thickness(10, 5, 0, 5);
                                        }
                                        else
                                        {
                                            ToggleSwitch.Margin = new Thickness(-95, 5, 0, 5);
                                        }

                                        ToggleSwitch.IsEnabled = true;
                                        #region //rajashri GEOS2-5227
                                        ToggleSwitch.Checked += (sender, e) => ToggleSwitch_Checked(sender, e, matchingStandardProperty?.PropertyName);
                                        //ToggleSwitch.Checked += ToggleSwitch_Checked;
                                        if (matchingStandardProperty == null)
                                            matchingStandardProperty = new ConnectorProperties();
                                        if (matchingStandardProperty != null)
                                        {
                                            var selectedItem = matchingStandardProperty.DefaultValue;
                                            if (selectedItem != null)
                                            {
                                                if (selectedItem == "")
                                                {
                                                    ToggleSwitch.IsChecked = false;
                                                }
                                                else
                                                {
                                                    ToggleSwitch.IsChecked = Convert.ToBoolean(selectedItem);
                                                    var matching = SCMCommon.Instance.CustomProperties.FirstOrDefault(i => i.Key == matchingStandardProperty?.PropertyName);

                                                    if (matching.Key != null) // Check if a match was found
                                                    {
                                                        SCMCommon.Instance.CustomProperties[matching.Key] = selectedItem.ToString();
                                                    }
                                                    else
                                                    {
                                                        SCMCommon.Instance.CustomProperties.Add(matchingStandardProperty?.PropertyName, selectedItem.ToString());
                                                    }
                                                }
                                            }



                                        }
                                        if (ToggleSwitch.IsChecked.HasValue)
                                        {
                                            if (ToggleSwitch.IsChecked.Value)
                                            {
                                                ToggleSwitch.CheckedStateContent = "On";
                                                ToggleSwitch.UncheckedStateContent = "Off";
                                            }
                                            else
                                            {
                                                ToggleSwitch.CheckedStateContent = "On";
                                                ToggleSwitch.UncheckedStateContent = "Off";
                                            }
                                        }
                                        ToggleSwitch.ContentPlacement = DevExpress.Xpf.Editors.ToggleSwitchContentPlacement.Inside;
                                        // Set IsHitTestVisible based on IsEnabled
                                        ToggleSwitch.IsHitTestVisible = matchingStandardProperty.IsEnabled;

                                        ToggleSwitch.IsEnabledChanged += (sender, e) =>
                                        {
                                            // Update IsHitTestVisible when IsEnabled changes
                                            ToggleSwitch.IsHitTestVisible = ToggleSwitch.IsEnabled;
                                        };
                                        #endregion
                                        Grid.SetRow(ToggleSwitch, Rowcount);
                                        Grid.SetColumn(ToggleSwitch, 2);
                                        MainGrid.Children.Add(ToggleSwitch);
                                        LayoutGroup.IsCollapsed = false;
                                        LayoutGroup.IsCollapsible = true;
                                        #endregion
                                        break;
                                    default:
                                        break;
                                }
                                Rowcount++;
                            }

                        }
                        if (Rowcount == 1)
                            LayoutGroup.MaxHeight = 100;
                        else if (Rowcount > 4)
                            LayoutGroup.MaxHeight = 50 * Rowcount;
                        else
                            LayoutGroup.MaxHeight = 60 * Rowcount;
                    }
                    #endregion
                }
                else if (selectedfmly == null)
                {
                    #region null
                    var TempCustomPropertyGroup = CustomList.GroupBy(x => x.IdCustomConnectorProperty).ToList();
                    var TempCustomPropertyGroup1 = CustomList.GroupBy(a => a.ValueType).ToList();
                    int GridRow = TempCustomPropertyGroup.Count();
                    if (GridRow > 0)
                    {
                        SCMCommon.Instance.SelectedList = new ObservableCollection<Data.Common.SCM.ValueType>();
                        MainGrid.Children.Clear();
                        LayoutGroup.IsCollapsed = true;
                        LayoutGroup.IsCollapsible = true;
                        #region ColumnDefinition
                        for (var a = 0; a <= 2; a++)
                        {
                            if (a == 0)
                            {
                                var c = new ColumnDefinition();
                                c.Width = GridLength.Auto;
                                this.Grid1.ColumnDefinitions.Add(c);
                            }
                            else if (a == 1)
                            {
                                var c = new ColumnDefinition();
                                c.Width = new GridLength(10, GridUnitType.Pixel);
                                MainGrid.ColumnDefinitions.Add(c);
                            }
                            else if (a == 2)
                            {
                                var c = new ColumnDefinition();
                                c.Width = new GridLength(200, GridUnitType.Pixel);
                                MainGrid.ColumnDefinitions.Add(c);
                            }
                            else
                            {
                                var c = new ColumnDefinition();
                                c.Width = GridLength.Auto;
                                MainGrid.ColumnDefinitions.Add(c);
                            }
                        }
                        #endregion

                        #region RowDefinition 
                        for (var a = 0; a <= GridRow; a++)
                        {
                            var r = new RowDefinition();
                            r.Height = new GridLength(40, GridUnitType.Pixel);
                            MainGrid.RowDefinitions.Add(r);
                        }
                        #endregion

                        int Rowcount = 0;
                        foreach (var item in TempCustomPropertyGroup)
                        {
                            try
                            {
                                var TempCustomListByName = CustomListByIdFamily.Where(x => x.IdCustomConnectorProperty == item.Key).ToList();
                                string CustomPropertyName = CustomList.Where(x => x.IdCustomConnectorProperty == item.Key).FirstOrDefault().Name;
                                var myDefinition = new ColumnDefinition();
                                #region Label
                                Label lbl = new Label();
                                lbl.Content = CustomPropertyName;
                                lbl.VerticalAlignment = VerticalAlignment.Center;
                                lbl.Width = 100;
                                lbl.ToolTip = CustomPropertyName;
                                //lbl.Margin = new Thickness(0, 0, 0, 5);
                                Grid.SetRow(lbl, Rowcount);
                                Grid.SetColumn(lbl, 0);
                                MainGrid.Children.Add(lbl);
                                #endregion
                                var ValueGroup = CustomList.Where(x => x.IdCustomConnectorProperty == item.Key).GroupBy(a => a.ValueKey.LookupKeyName).FirstOrDefault();
                                ValueGroup.Select(i => i.ValueType).Distinct();
                                if (ValueGroup != null)
                                {
                                    myDefinition.Width = new GridLength(68);
                                    MainGrid.ColumnDefinitions.Add(myDefinition);
                                    ConnectorProperties matchingStandardProperty = null;
                                    if (standardPropertiedbyFamily != null)
                                        matchingStandardProperty = standardPropertiedbyFamily.FirstOrDefault(prop => prop.IdConnectorProperty == item.Key);//rajashri GEOS2-5227
                                                                                                                                                           //ValueGroup.Key
                                    switch (Convert.ToString(ValueGroup.Key))
                                    {
                                        case "List":
                                            #region ComboBox
                                            List<Data.Common.SCM.ValueType> TempValueList = CustomList.Where(x => x.IdCustomConnectorProperty == item.Key).Select(a => a.ValueType).ToList();
                                            var temp = (from a in TempValueList select new { a.IdLookupValue, a.Name }).Distinct().ToList();
                                            TempValueList = new List<Data.Common.SCM.ValueType>();
                                            foreach (var item1 in temp)
                                            {
                                                Data.Common.SCM.ValueType selectedvalue = new Data.Common.SCM.ValueType();
                                                selectedvalue.IdLookupValue = item1.IdLookupValue;
                                                selectedvalue.Name = item1.Name;
                                                TempValueList.Add(selectedvalue);
                                            }
                                            CheckedComboBoxStyleSettings styleSettings = new CheckedComboBoxStyleSettings();
                                            ComboBoxEdit ComboBox = new ComboBoxEdit();
                                            ComboBox.Width = 160;
                                            ComboBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                                            ComboBox.Margin = new Thickness(0, 5, 0, 5);
                                            ComboBox.ItemsSource = TempValueList;
                                            ComboBox.DisplayMember = "Name";
                                            ComboBox.StyleSettings = styleSettings;
                                            ComboBox.SelectedItem = null;
                                            ComboBox.EditValueChanged += (sender, e) => ComboBox_EditValueChanged(sender, e, matchingStandardProperty?.PropertyName);
                                            Grid.SetRow(ComboBox, Rowcount);
                                            Grid.SetColumn(ComboBox, 2);
                                            MainGrid.Children.Add(ComboBox);
                                            LayoutGroup.IsCollapsed = false;
                                            LayoutGroup.IsCollapsible = true;
                                            #endregion
                                            break;
                                        case "Number":
                                            #region TextBox
                                            Grid Grid1 = new Grid();

                                            Grid1.Width = 170;
                                            #region ColumnDefinition
                                            for (var a = 0; a <= 6; a++)
                                            {
                                                if (a % 2 == 1)
                                                {
                                                    var c = new ColumnDefinition();
                                                    c.Width = new GridLength(5, GridUnitType.Pixel);
                                                    Grid1.ColumnDefinitions.Add(c);
                                                }
                                                else if (a == 0)
                                                {
                                                    var c = new ColumnDefinition();
                                                    c.Width = new GridLength(40, GridUnitType.Pixel);
                                                    Grid1.ColumnDefinitions.Add(c);
                                                }
                                                else if (a == 4)
                                                {
                                                    var c = new ColumnDefinition();
                                                    c.Width = new GridLength(25, GridUnitType.Pixel);
                                                    Grid1.ColumnDefinitions.Add(c);
                                                }
                                                else if (a == 2 || a == 6)
                                                {
                                                    var c = new ColumnDefinition();
                                                    c.Width = new GridLength(40, GridUnitType.Pixel);
                                                    Grid1.ColumnDefinitions.Add(c);
                                                }
                                                else
                                                {
                                                    var c = new ColumnDefinition();
                                                    c.Width = GridLength.Auto;
                                                    Grid1.ColumnDefinitions.Add(c);
                                                }
                                            }
                                            #endregion
                                            #region Label Number                       
                                            TextBlock lblNumMin = new TextBlock();
                                            lblNumMin.Text = System.Windows.Application.Current.FindResource("SCMFrom").ToString();
                                            lblNumMin.ToolTip = System.Windows.Application.Current.FindResource("SCMFrom").ToString();
                                            lblNumMin.Width = 40; lblNumMin.HorizontalAlignment = HorizontalAlignment.Left;
                                            lblNumMin.TextAlignment = TextAlignment.Left;
                                            lblNumMin.Margin = new Thickness(5, 7, 0, 5);
                                            Grid.SetColumn(lblNumMin, 0);
                                            Grid1.Children.Add(lblNumMin);
                                            TextBlock lblNumMax = new TextBlock();
                                            lblNumMax.Text = System.Windows.Application.Current.FindResource("SCMTo").ToString();
                                            lblNumMax.ToolTip = System.Windows.Application.Current.FindResource("SCMTo").ToString();
                                            lblNumMax.Width = 25;
                                            lblNumMax.TextAlignment = TextAlignment.Left;
                                            lblNumMax.Margin = new Thickness(0, 7, 0, 5);
                                            Grid.SetColumn(lblNumMax, 4);
                                            Grid1.Children.Add(lblNumMax);
                                            #endregion                                 
                                            TextEdit EditNumber = new TextEdit();
                                            EditNumber.Width = 40;
                                            EditNumber.Margin = new Thickness(0, 5, 0, 5);
                                            EditNumber.Text = string.Empty;
                                            EditNumber.Mask = "######0;";
                                            EditNumber.MaskUseAsDisplayFormat = true;
                                            EditNumber.MaskType = MaskType.Numeric;
                                            EditNumber.NullText = null;
                                            EditNumber.Text = string.Empty;
                                            EditNumber.EditValueChanged += (sender, e) => NumberMinEditNumber_SelectionChanged(sender, e, matchingStandardProperty?.PropertyName);
                                            EditNumber.PreviewTextInput += (sender, e) =>
                                            {
                                                if (!char.IsDigit(e.Text, 0) && e.Text != "-")
                                                {
                                                    e.Handled = true; // Prevent non-numeric input
                                                }
                                            };
                                            Grid.SetColumn(EditNumber, 2);
                                            Grid1.Children.Add(EditNumber);
                                            TextEdit EditNumberMax = new TextEdit();
                                            EditNumberMax.Width = 40;
                                            EditNumberMax.Margin = new Thickness(0, 5, 0, 5);
                                            EditNumberMax.Text = string.Empty;
                                            EditNumberMax.Mask = "######0;";
                                            EditNumberMax.MaskUseAsDisplayFormat = true;
                                            EditNumberMax.MaskType = MaskType.Numeric;
                                            EditNumberMax.NullText = null;
                                            EditNumberMax.EditValueChanged += (sender, e) => NumberMaxEditNumber_SelectionChanged(sender, e, matchingStandardProperty?.PropertyName);
                                            EditNumberMax.PreviewTextInput += (sender, e) =>
                                            {
                                                if (!char.IsDigit(e.Text, 0) && e.Text != "-")
                                                {
                                                    e.Handled = true; // Prevent non-numeric input
                                                }
                                            };
                                            Grid.SetColumn(EditNumberMax, 6);
                                            Grid1.Children.Add(EditNumberMax);
                                            Grid.SetRow(Grid1, Rowcount);
                                            Grid.SetColumn(Grid1, 2);
                                            MainGrid.Children.Add(Grid1);
                                            LayoutGroup.IsCollapsed = false;
                                            LayoutGroup.IsCollapsible = true;
                                            #endregion
                                            break;
                                        case "Text":
                                            #region TextBox
                                            string TempValueText = "";
                                            TextBox dynamicTexttEdit = new TextBox();
                                            dynamicTexttEdit.Width = 160;
                                            dynamicTexttEdit.Margin = new Thickness(0, 5, 0, 5);
                                            dynamicTexttEdit.Text = TempValueText;
                                            dynamicTexttEdit.TextChanged += (sender, e) => DynamicTextEditNumber_SelectionChanged(sender, e, matchingStandardProperty?.PropertyName);
                                            Grid.SetRow(dynamicTexttEdit, Rowcount);
                                            Grid.SetColumn(dynamicTexttEdit, 2);
                                            MainGrid.Children.Add(dynamicTexttEdit);
                                            LayoutGroup.IsCollapsed = false;
                                            LayoutGroup.IsCollapsible = true;
                                            #endregion
                                            break;
                                        case "Boolean":
                                            #region ToggleSwitch
                                            ToggleSwitch ToggleSwitch = new ToggleSwitch();
                                            ToggleSwitch.Width = 70;
                                            ToggleSwitch.Margin = new Thickness(-90, 5, 0, 5);
                                            ToggleSwitch.IsEnabled = true;
                                            ToggleSwitch.Checked += (sender, e) => ToggleSwitch_Checked(sender, e, matchingStandardProperty?.PropertyName);
                                            #region //rajashri GEOS2-5227                                      
                                            if (ToggleSwitch.IsChecked.HasValue)
                                            {
                                                if (ToggleSwitch.IsChecked.Value)
                                                {
                                                    ToggleSwitch.CheckedStateContent = "On";
                                                    ToggleSwitch.UncheckedStateContent = "Off";
                                                }
                                                else
                                                {
                                                    ToggleSwitch.CheckedStateContent = "On";
                                                    ToggleSwitch.UncheckedStateContent = "Off";
                                                }
                                            }
                                            ToggleSwitch.ContentPlacement = DevExpress.Xpf.Editors.ToggleSwitchContentPlacement.Inside;
                                            #endregion
                                            Grid.SetRow(ToggleSwitch, Rowcount);
                                            Grid.SetColumn(ToggleSwitch, 2);
                                            MainGrid.Children.Add(ToggleSwitch);
                                            LayoutGroup.IsCollapsed = false;
                                            LayoutGroup.IsCollapsible = true;
                                            #endregion
                                            break;
                                        default:
                                            break;
                                    }
                                    Rowcount++;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        if (Rowcount == 1)
                            LayoutGroup.MaxHeight = 100;
                        else if (Rowcount > 3)
                            LayoutGroup.MaxHeight = 50 * Rowcount;
                        else
                            LayoutGroup.MaxHeight = 60 * Rowcount;
                    }
                    #endregion
                }
                else
                {
                    SCMCommon.Instance.SelectedList = new ObservableCollection<Data.Common.SCM.ValueType>();
                    MainGrid.Children.Clear();
                    LayoutGroup.IsCollapsed = true;
                    LayoutGroup.IsCollapsible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        #region [GEOS2-5297][rdixit][29.02.2024]
      
        public void AllCustomDataForMultipleFamily(List<Family> selectedfamily, Grid MainGrid, List<int> CustomListByIdFamily, List<CustomProperty> CustomList, List<ConnectorProperties> standardPropertiedbyFamily)
        {

            try
            {
                DevExpress.Xpf.LayoutControl.LayoutGroup LayoutGroup = null;
                if (MainGrid != null)
                {
                    LayoutGroup = (DevExpress.Xpf.LayoutControl.LayoutGroup)MainGrid.Parent;
                }
                #region
                if (CustomListByIdFamily == null)
                    CustomListByIdFamily = new List<int>();
                if (SCMCommon.Instance.CustomProperties == null)
                    SCMCommon.Instance.CustomProperties = new Dictionary<string, string>();
                if (SCMCommon.Instance.CustomPropertiesMin == null)
                    SCMCommon.Instance.CustomPropertiesMin = new Dictionary<string, string>();
                if (SCMCommon.Instance.CustomPropertiesMax == null)
                    SCMCommon.Instance.CustomPropertiesMax = new Dictionary<string, string>();
                if (SCMCommon.Instance.CustomPropertiesList == null)
                    SCMCommon.Instance.CustomPropertiesList = new Dictionary<string, List<string>>();
                #endregion
                #region null               
                int GridRow = CustomListByIdFamily.Count();
                if (GridRow > 0)
                {
                    SCMCommon.Instance.SelectedList = new ObservableCollection<Data.Common.SCM.ValueType>();
                    MainGrid.Children.Clear();
                    LayoutGroup.IsCollapsed = true;
                    LayoutGroup.IsCollapsible = true;
                    #region ColumnDefinition
                    for (var a = 0; a <= 2; a++)
                    {
                        if (a == 0)
                        {
                            var c = new ColumnDefinition();
                            c.Width = GridLength.Auto;
                            this.Grid1.ColumnDefinitions.Add(c);
                        }
                        else if (a == 1)
                        {
                            var c = new ColumnDefinition();
                            c.Width = new GridLength(10, GridUnitType.Pixel);
                            MainGrid.ColumnDefinitions.Add(c);
                        }
                        else if (a == 2)
                        {
                            var c = new ColumnDefinition();
                            c.Width = new GridLength(200, GridUnitType.Pixel);
                            MainGrid.ColumnDefinitions.Add(c);
                        }
                        else
                        {
                            var c = new ColumnDefinition();
                            c.Width = GridLength.Auto;
                            MainGrid.ColumnDefinitions.Add(c);
                        }
                    }
                    #endregion

                    #region RowDefinition 
                    for (var a = 0; a <= GridRow; a++)
                    {
                        var r = new RowDefinition();
                        r.Height = new GridLength(40, GridUnitType.Pixel);
                        MainGrid.RowDefinitions.Add(r);
                    }
                    #endregion

                    int Rowcount = 0;
                    foreach (var item in CustomListByIdFamily)
                    {
                        try
                        {
                            var TempCustomListByName = CustomListByIdFamily.Where(x => x == item).ToList();
                            string CustomPropertyName = CustomList.Where(x => x.IdCustomConnectorProperty == item).FirstOrDefault().Name;
                            var myDefinition = new ColumnDefinition();
                            #region Label
                            Label lbl = new Label();
                            lbl.Content = CustomPropertyName;
                            lbl.VerticalAlignment = VerticalAlignment.Center;
                            lbl.Width = 100;
                            lbl.ToolTip = CustomPropertyName;
                            //lbl.Margin = new Thickness(0, 0, 0, 5);
                            Grid.SetRow(lbl, Rowcount);
                            Grid.SetColumn(lbl, 0);
                            MainGrid.Children.Add(lbl);
                            #endregion
                            IGrouping<string, CustomProperty> ValueGroup = CustomList.Where(x => x.IdCustomConnectorProperty == item).GroupBy(a => a.ValueKey.LookupKeyName).FirstOrDefault();
                            ValueGroup.Select(i => i.ValueType).Distinct();
                            if (ValueGroup != null)
                            {
                                myDefinition.Width = new GridLength(68);
                                MainGrid.ColumnDefinitions.Add(myDefinition);
                                ConnectorProperties matchingStandardProperty = null;
                                if (standardPropertiedbyFamily != null)
                                    matchingStandardProperty = standardPropertiedbyFamily.FirstOrDefault(prop => prop.IdConnectorProperty == item);

                                switch (Convert.ToString(ValueGroup.Key))
                                {
                                    case "List":
                                        #region ComboBox
                                        string PropName = CustomList?.FirstOrDefault(i => i.IdCustomConnectorProperty == item)?.Name;
                                        List<Data.Common.SCM.ValueType> TempValueList = CustomList.Where(x => x.IdCustomConnectorProperty == item).Select(a => a.ValueType).ToList();
                                        var temp = (from a in TempValueList select new { a.IdLookupValue, a.Name }).Distinct().ToList();
                                        TempValueList = new List<Data.Common.SCM.ValueType>();
                                        foreach (var item1 in temp)
                                        {
                                            Data.Common.SCM.ValueType selectedvalue = new Data.Common.SCM.ValueType();
                                            selectedvalue.IdLookupValue = item1.IdLookupValue;
                                            selectedvalue.Name = item1.Name;
                                            TempValueList.Add(selectedvalue);
                                        }
                                        CheckedComboBoxStyleSettings styleSettings = new CheckedComboBoxStyleSettings();
                                        ComboBoxEdit ComboBox = new ComboBoxEdit();
                                        ComboBox.Width = 160;
                                        ComboBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                                        ComboBox.Margin = new Thickness(0, 5, 0, 5);
                                        ComboBox.ItemsSource = TempValueList;
                                        ComboBox.DisplayMember = "Name";
                                        ComboBox.StyleSettings = styleSettings;
                                        ComboBox.EditValueChanged += (sender, e) => ComboBox_EditValueChanged(sender, e, PropName);
                                        ComboBox.SelectedItem = null;
                                        Grid.SetRow(ComboBox, Rowcount);
                                        Grid.SetColumn(ComboBox, 2);
                                        MainGrid.Children.Add(ComboBox);
                                        LayoutGroup.IsCollapsed = false;
                                        LayoutGroup.IsCollapsible = true;

                                        var CustomPropList = ValueGroup.Where(i => selectedfamily.Any(f => f?.Id == i.IdFamily)).ToList();
                                        //bool allNamesSame = t.Select(f => f?.Name).Distinct().Count() == 1;
                                        bool allNamesSame = false;
                                        List<ConnectorProperties> CommonCustomProp = standardPropertiedbyFamily.Where(prop => prop.IdConnectorProperty == item).ToList();
                                        if (CustomPropList.Select(f => f?.IdFamily).Distinct().Count() > 1)
                                        {
                                            allNamesSame = CommonCustomProp.Select(f => f?.DefaultValue).Distinct().Count() == 1;
                                            if (allNamesSame)
                                            {
                                                var selectedItem = CommonCustomProp.FirstOrDefault();
                                                Data.Common.SCM.ValueType selectedval = TempValueList?.FirstOrDefault(i => CommonCustomProp.Select(s => s.DefaultValue).ToList().Any(d => d == i.Name));
                                                if (selectedval != null)
                                                {
                                                    ComboBox.EditValue = new ObservableCollection<object>() { (object)selectedval };
                                                }
                                            }
                                        }


                                        #endregion
                                        break;
                                    case "Number":
                                        #region TextBox
                                        Grid Grid1 = new Grid();
                                        Grid1.Width = 170;
                                        #region ColumnDefinition
                                        for (var a = 0; a <= 6; a++)
                                        {
                                            if (a % 2 == 1)
                                            {
                                                var c = new ColumnDefinition();
                                                c.Width = new GridLength(5, GridUnitType.Pixel);
                                                Grid1.ColumnDefinitions.Add(c);
                                            }
                                            else if (a == 0)
                                            {
                                                var c = new ColumnDefinition();
                                                c.Width = new GridLength(40, GridUnitType.Pixel);
                                                Grid1.ColumnDefinitions.Add(c);
                                            }
                                            else if (a == 4)
                                            {
                                                var c = new ColumnDefinition();
                                                c.Width = new GridLength(25, GridUnitType.Pixel);
                                                Grid1.ColumnDefinitions.Add(c);
                                            }
                                            else if (a == 2 || a == 6)
                                            {
                                                var c = new ColumnDefinition();
                                                c.Width = new GridLength(40, GridUnitType.Pixel);
                                                Grid1.ColumnDefinitions.Add(c);
                                            }
                                            else
                                            {
                                                var c = new ColumnDefinition();
                                                c.Width = GridLength.Auto;
                                                Grid1.ColumnDefinitions.Add(c);
                                            }
                                        }
                                        #endregion
                                        #region Label Number                                      
                                        TextBlock lblNumMin = new TextBlock();
                                        lblNumMin.Text = System.Windows.Application.Current.FindResource("SCMFrom").ToString();
                                        lblNumMin.ToolTip = System.Windows.Application.Current.FindResource("SCMFrom").ToString();
                                        lblNumMin.Width = 40; lblNumMin.HorizontalAlignment = HorizontalAlignment.Left;
                                        lblNumMin.TextAlignment = TextAlignment.Left;
                                        lblNumMin.Margin = new Thickness(5, 7, 0, 5);
                                        Grid.SetColumn(lblNumMin, 0);
                                        Grid1.Children.Add(lblNumMin);

                                        TextBlock lblNumMax = new TextBlock();
                                        lblNumMax.Text = System.Windows.Application.Current.FindResource("SCMTo").ToString();
                                        lblNumMax.ToolTip = System.Windows.Application.Current.FindResource("SCMTo").ToString();
                                        lblNumMax.Width = 25;
                                        lblNumMax.TextAlignment = TextAlignment.Left;
                                        lblNumMax.Margin = new Thickness(0, 7, 0, 5);
                                        Grid.SetColumn(lblNumMax, 4);
                                        Grid1.Children.Add(lblNumMax);
                                        #endregion
                                        string PropName1 = CustomList?.FirstOrDefault(i => i.IdCustomConnectorProperty == item)?.Name;

                                        #region Min                                 
                                        TextEdit EditNumber = new TextEdit();
                                        EditNumber.Width = 40;
                                        EditNumber.Margin = new Thickness(0, 5, 0, 10);
                                        EditNumber.Text = string.Empty;
                                        EditNumber.Mask = "######0;";
                                        EditNumber.MaskUseAsDisplayFormat = true;
                                        EditNumber.MaskType = MaskType.Numeric;
                                        EditNumber.NullText = null;
                                        #region [rdixit][19.03.2024][GEOS2-5447]
                                        List<ConnectorProperties> CommonCustomProp1 = standardPropertiedbyFamily.Where(prop => prop.IdConnectorProperty == item).ToList();
                                        var CustomPropList1 = ValueGroup.Where(i => selectedfamily.Any(f => f?.Id == i.IdFamily)).ToList();
                                        bool allDefaultSame = false;
                                        bool allMinSame = false;
                                        if (CustomPropList1.Select(f => f?.IdFamily).Distinct().Count() > 1)
                                        {
                                            allMinSame = CommonCustomProp1.Select(f => f?.MinValueNew).Distinct().Count() == 1;
                                            allDefaultSame = CommonCustomProp1.Select(f => f?.DefaultValue).Distinct().Count() == 1;
                                            if (allMinSame)
                                            {
                                                EditNumber.Text = CommonCustomProp1?.FirstOrDefault()?.MinValueNew;
                                            }
                                            else if (allDefaultSame)
                                            {
                                                EditNumber.Text = CommonCustomProp1?.FirstOrDefault()?.DefaultValue;
                                            }
                                        }
                                        var matching = SCMCommon.Instance.CustomPropertiesMin.FirstOrDefault(i => i.Key == matchingStandardProperty?.PropertyName);

                                        if (matching.Key != null) // Check if a match was found
                                        {
                                            SCMCommon.Instance.CustomPropertiesMin[matching.Key] = EditNumber.Text;
                                        }
                                        else
                                        {
                                            SCMCommon.Instance.CustomPropertiesMin.Add(matchingStandardProperty?.PropertyName, EditNumber.Text);
                                        }
                                        #endregion

                                        EditNumber.EditValueChanged += (sender, e) => NumberMinEditNumber_SelectionChanged(sender, e, PropName1);
                                        EditNumber.PreviewTextInput += (sender, e) =>
                                        {
                                            if (!char.IsDigit(e.Text, 0) && e.Text != "-")
                                            {
                                                e.Handled = true; // Prevent non-numeric input
                                            }
                                        };
                                        #endregion

                                        #region Max
                                        TextEdit EditNumberMax = new TextEdit();
                                        EditNumberMax.Width = 40;
                                        EditNumberMax.Margin = new Thickness(0, 5, 0, 10);
                                        EditNumberMax.Text = string.Empty;
                                        EditNumberMax.Mask = "######0;";
                                        EditNumberMax.MaskUseAsDisplayFormat = true;
                                        EditNumberMax.MaskType = MaskType.Numeric;
                                        EditNumberMax.NullText = null;
                                        EditNumberMax.EditValueChanged += (sender, e) => NumberMaxEditNumber_SelectionChanged(sender, e, PropName1);
                                        EditNumberMax.PreviewTextInput += (sender, e) =>
                                        {
                                            if (!char.IsDigit(e.Text, 0) && e.Text != "-")
                                            {
                                                e.Handled = true; // Prevent non-numeric input
                                            }
                                        };

                                        #region [rdixit][19.03.2024][GEOS2-5447]
                                        List<ConnectorProperties> CommonCustomProp2 = standardPropertiedbyFamily.Where(prop => prop.IdConnectorProperty == item).ToList();
                                        var CustomPropList2 = ValueGroup.Where(i => selectedfamily.Any(f => f?.Id == i.IdFamily)).ToList();
                                        bool allMaxSame = false;
                                        if (CustomPropList2.Select(f => f?.IdFamily).Distinct().Count() > 1)
                                        {
                                            allMaxSame = CommonCustomProp2.Select(f => f?.MaxValueNew).Distinct().Count() == 1;
                                            allDefaultSame = CommonCustomProp2.Select(f => f?.DefaultValue).Distinct().Count() == 1;
                                            if (allMaxSame)
                                            {
                                                EditNumberMax.Text = CommonCustomProp2?.FirstOrDefault()?.MaxValueNew;
                                            }
                                            else if (allDefaultSame)
                                            {
                                                EditNumberMax.Text = CommonCustomProp2?.FirstOrDefault()?.DefaultValue;
                                            }
                                        }
                                        var matchingMax = SCMCommon.Instance.CustomPropertiesMax.FirstOrDefault(i => i.Key == matchingStandardProperty?.PropertyName);

                                        if (matchingMax.Key != null) // Check if a match was found
                                        {
                                            SCMCommon.Instance.CustomPropertiesMax[matching.Key] = EditNumberMax.Text;
                                        }
                                        else
                                        {
                                            SCMCommon.Instance.CustomPropertiesMax.Add(matchingStandardProperty?.PropertyName, EditNumberMax.Text);
                                        }
                                        #endregion
                                        #endregion

                                        Grid.SetColumn(EditNumber, 2);
                                        Grid1.Children.Add(EditNumber);
                                        Grid.SetColumn(EditNumberMax, 6);
                                        Grid1.Children.Add(EditNumberMax);
                                        Grid.SetRow(Grid1, Rowcount);
                                        Grid.SetColumn(Grid1, 2);
                                        MainGrid.Children.Add(Grid1);
                                        LayoutGroup.IsCollapsed = false;
                                        LayoutGroup.IsCollapsible = true;

                                        #region Assign Value
                                        var CustomPropNumList = ValueGroup.Where(i => selectedfamily.Any(f => f?.Id == i.IdFamily)).ToList();
                                        bool allNumSame = false;
                                        List<ConnectorProperties> CommonCustomNumProp = standardPropertiedbyFamily.Where(prop => prop.IdConnectorProperty == item).ToList();
                                        if (CustomPropNumList.Select(f => f?.IdFamily).Distinct().Count() > 1)
                                        {
                                            allNumSame = CommonCustomNumProp.Select(f => f?.DefaultValue).Distinct().Count() == 1;
                                            if (allNumSame)
                                            {
                                                var selectedItem = CommonCustomNumProp.FirstOrDefault();
                                                if (selectedItem != null)
                                                {
                                                    EditNumberMax.Text = selectedItem.DefaultValue;
                                                    EditNumber.Text = selectedItem.DefaultValue;
                                                }
                                            }
                                        }
                                        #endregion
                                        #endregion
                                        break;
                                    case "Text":
                                        #region TextBox          
                                        string PropName2 = CustomList?.FirstOrDefault(i => i.IdCustomConnectorProperty == item)?.Name;
                                        TextBox dynamicTexttEdit = new TextBox();
                                        dynamicTexttEdit.Width = 160;
                                        dynamicTexttEdit.Margin = new Thickness(0, 5, 0, 5);
                                        dynamicTexttEdit.Text = "";
                                        dynamicTexttEdit.TextChanged += (sender, e) => DynamicTextEditNumber_SelectionChanged(sender, e, PropName2);
                                        Grid.SetRow(dynamicTexttEdit, Rowcount);
                                        Grid.SetColumn(dynamicTexttEdit, 2);
                                        MainGrid.Children.Add(dynamicTexttEdit);
                                        LayoutGroup.IsCollapsed = false;
                                        LayoutGroup.IsCollapsible = true;

                                        #region Assign Value
                                        var CustomProptextList = ValueGroup.Where(i => selectedfamily.Any(f => f?.Id == i.IdFamily)).ToList();
                                        bool allTextSame = false;
                                        List<ConnectorProperties> CommonCustomTextProp = standardPropertiedbyFamily.Where(prop => prop.IdConnectorProperty == item).ToList();
                                        if (CustomProptextList.Select(f => f?.IdFamily).Distinct().Count() > 1)
                                        {
                                            allTextSame = CommonCustomTextProp.Select(f => f?.DefaultValue).Distinct().Count() == 1;
                                            if (allTextSame)
                                            {
                                                var selectedItem = CommonCustomTextProp.FirstOrDefault();
                                                if (selectedItem != null)
                                                {
                                                    dynamicTexttEdit.Text = selectedItem.DefaultValue;
                                                }
                                            }
                                        }
                                        #endregion

                                        #endregion
                                        break;
                                    case "Boolean":
                                        #region ToggleSwitch
                                        string PropName3 = CustomList?.FirstOrDefault(i => i.IdCustomConnectorProperty == item)?.Name;
                                        ToggleSwitch ToggleSwitch = new ToggleSwitch();
                                        ToggleSwitch.Width = 70;
                                        ToggleSwitch.Margin = new Thickness(-90, 5, 0, 5);
                                        ToggleSwitch.IsEnabled = true;
                                        ToggleSwitch.Checked += (sender, e) => ToggleSwitch_Checked(sender, e, PropName3);
                                        #region //rajashri GEOS2-5227                                      
                                        if (ToggleSwitch.IsChecked.HasValue)
                                        {
                                            if (ToggleSwitch.IsChecked.Value)
                                            {
                                                ToggleSwitch.CheckedStateContent = "On";
                                                ToggleSwitch.UncheckedStateContent = "Off";
                                            }
                                            else
                                            {
                                                ToggleSwitch.CheckedStateContent = "On";
                                                ToggleSwitch.UncheckedStateContent = "Off";
                                            }
                                        }
                                        ToggleSwitch.ContentPlacement = DevExpress.Xpf.Editors.ToggleSwitchContentPlacement.Inside;
                                        #endregion
                                        Grid.SetRow(ToggleSwitch, Rowcount);
                                        Grid.SetColumn(ToggleSwitch, 2);
                                        MainGrid.Children.Add(ToggleSwitch);
                                        LayoutGroup.IsCollapsed = false;
                                        LayoutGroup.IsCollapsible = true;

                                        #region Assign Value
                                        var CustomPropboolList = ValueGroup.Where(i => selectedfamily.Any(f => f?.Id == i.IdFamily)).ToList();
                                        bool allboolSame = false;
                                        List<ConnectorProperties> CommonCustomboolProp = standardPropertiedbyFamily.Where(prop => prop.IdConnectorProperty == item).ToList();
                                        if (CustomPropboolList.Select(f => f?.IdFamily).Distinct().Count() > 1)
                                        {
                                            allboolSame = CommonCustomboolProp.Select(f => f?.DefaultValue).Distinct().Count() == 1;
                                            if (allboolSame)
                                            {
                                                var selectedItem = CommonCustomboolProp.FirstOrDefault();
                                                if (selectedItem != null)
                                                {
                                                    ToggleSwitch.IsChecked = selectedItem.DefaultValue.ToLower() == "true" ? true : false;
                                                }
                                            }
                                        }
                                        #endregion  
                                        #endregion
                                        break;
                                    default:
                                        break;
                                }
                                Rowcount++;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    if (Rowcount == 1)
                        LayoutGroup.MaxHeight = 100;
                    else if (Rowcount > 3)
                        LayoutGroup.MaxHeight = 50 * Rowcount;
                    else
                        LayoutGroup.MaxHeight = 60 * Rowcount;
                }
                #endregion

                else
                {
                    SCMCommon.Instance.SelectedList = new ObservableCollection<Data.Common.SCM.ValueType>();
                    MainGrid.Children.Clear();
                    LayoutGroup.IsCollapsed = true;
                    LayoutGroup.IsCollapsible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        Emdep.Geos.Modules.SCM.ViewModels.ConnectorViewModel ConnectorViewModel = new Emdep.Geos.Modules.SCM.ViewModels.ConnectorViewModel();
        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e, string propName)
        {
            try
            {
                var temp = (ToggleSwitch)sender;
                var matching = SCMCommon.Instance.CustomProperties.FirstOrDefault(i => i.Key == propName);
                if (matching.Key != null)
                    SCMCommon.Instance.CustomProperties[matching.Key] = temp.IsChecked.ToString();
                else
                {
                    if (!string.IsNullOrEmpty(propName))
                        SCMCommon.Instance.CustomProperties.Add(propName, temp.IsChecked.ToString());
                }
            }
            catch (Exception ex) { }
        }
        private void DynamicTextEditNumber_SelectionChanged(object sender, RoutedEventArgs e, string propName)
        {
            try
            {
                var matching = SCMCommon.Instance.CustomProperties.FirstOrDefault(i => i.Key == propName);
                if (matching.Key != null)
                    SCMCommon.Instance.CustomProperties[matching.Key] = ((TextBox)e.OriginalSource).Text;
                else
                {
                    if (!string.IsNullOrEmpty(propName))
                        SCMCommon.Instance.CustomProperties.Add(propName, ((TextBox)e.OriginalSource).Text);
                }
            }
            catch (Exception ex) { }
        }
        private void NumberMinEditNumber_SelectionChanged(object sender, EditValueChangedEventArgs e, string propName)
        {
            try
            {
                var matching = SCMCommon.Instance.CustomPropertiesMin.FirstOrDefault(i => i.Key == propName);
                if (matching.Key != null)
                    SCMCommon.Instance.CustomPropertiesMin[matching.Key] = ((TextEdit)e.OriginalSource).Text;
                else
                {
                    if (!string.IsNullOrEmpty(propName))
                        SCMCommon.Instance.CustomPropertiesMin.Add(propName, ((TextEdit)e.OriginalSource).Text);
                }
            }
            catch (Exception ex) { }
        }
        private void NumberMaxEditNumber_SelectionChanged(object sender, EditValueChangedEventArgs e, string propName)
        {
            try
            {
                var matching = SCMCommon.Instance.CustomPropertiesMax.FirstOrDefault(i => i.Key == propName);
                if (matching.Key != null)
                    SCMCommon.Instance.CustomPropertiesMax[matching.Key] = ((TextEdit)e.OriginalSource).Text;
                else
                {
                    if (!string.IsNullOrEmpty(propName))
                        SCMCommon.Instance.CustomPropertiesMax.Add(propName, ((TextEdit)e.OriginalSource).Text);
                }
            }
            catch (Exception ex) { }
        }
        private void ComboBox_EditValueChanged(object sender, EditValueChangedEventArgs e, string PropName)
        {
            try
            {
                var matching = SCMCommon.Instance.CustomPropertiesList.FirstOrDefault(i => i.Key == PropName);
                if (matching.Key != null)
                    SCMCommon.Instance.CustomPropertiesList[matching.Key] = ((ComboBoxEdit)e.OriginalSource).SelectedItems.Select(item => ((Data.Common.SCM.ValueType)item).Name).ToList();
                else
                {
                    List<string> newList = ((ComboBoxEdit)e.OriginalSource).SelectedItems.Select(item => ((Data.Common.SCM.ValueType)item).Name).ToList();
                    if (!string.IsNullOrEmpty(PropName))
                        SCMCommon.Instance.CustomPropertiesList.Add(PropName, newList);
                }
            }
            catch (Exception ex) { }
        }
        
        #endregion
        private void WidthAdjust(object sender, RoutedEventArgs e)
        {
            DevExpress.Xpf.Grid.GridControl obj = (DevExpress.Xpf.Grid.GridControl)sender;

            foreach (var column in obj.Columns)
            {
                if (column.FieldName == "Family" || column.FieldName == "Terminal")
                {
                    column.Width = CalculateColumnWidthh(obj, column.FieldName);
                }
            }
        }

        private double CalculateColumnWidthh(DevExpress.Xpf.Grid.GridControl gridControl, string fieldName)
        {
            double maxWidth = 0;

            for (int rowHandle = 0; rowHandle < gridControl.VisibleRowCount; rowHandle++)
            {
                var cellValue = gridControl.GetCellValue(rowHandle, fieldName);
                if (fieldName == "Terminal" || fieldName == "Family")
                {
                    if (cellValue == null)
                    {
                        var cellValue1 = fieldName;
                        var formattedText = new FormattedText(
                            cellValue1?.ToString(),
                            CultureInfo.CurrentUICulture,
                            FlowDirection.LeftToRight,
                            new Typeface("Arial"),
                            12,
                            Brushes.Black
                        );

                        maxWidth = Math.Max(maxWidth, formattedText.Width);
                    }
                }
                if (cellValue != null)
                {
                    var formattedText = new FormattedText(
                        cellValue.ToString(),
                        CultureInfo.CurrentUICulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        12,
                        Brushes.Black
                    );

                    maxWidth = Math.Max(maxWidth, formattedText.Width);
                }
            }

            return maxWidth + 40; // Add some padding or adjust as needed
        }
        */
        #endregion



    }
}