using DevExpress.Diagram.Core;
using DevExpress.Diagram.Core.Layout;
using DevExpress.Diagram.Core.Localization;
using DevExpress.Xpf.Diagram;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class DiagramOrgChartBehaviorEx : DiagramOrgChartBehavior
    {
        public DiagramOrgChartBehaviorEx()
        {
            CustomLayoutItems += DiagramOrgChartBehaviorEx_CustomLayoutItems;
        }

        private void DiagramOrgChartBehaviorEx_CustomLayoutItems(object sender, DiagramCustomLayoutItemsEventArgs e)
        {
            try
            {
                //GeosApplication.Instance.Logger.Log("Method DiagramOrgChartBehaviorEx_CustomLayoutItems ...", category: Category.Info, priority: Priority.Low);

                e.Handled = true;

                AssociatedObject.ApplyTreeLayout();
                AssociatedObject.ApplyTipOverTreeLayout();

                List<DiagramItem> heirachyItem = new List<DiagramItem>();
                List<DiagramItem> verticalItem = new List<DiagramItem>();
                List<DiagramItem> otherItem = new List<DiagramItem>();
                List<DiagramItem> firstTwoLevelItems = new List<DiagramItem>();
                List<DiagramItem> listItems = e.Items.ToList();
                List<DiagramItem> HorizontalChildOrientationList = e.Items.Where(item => ((EmployeeHeirarchy)item.DataContext).ChildOrientation == "H" || ((EmployeeHeirarchy)item.DataContext).ChildOrientation == null).ToList();
                AssociatedObject.TreeLayoutHorizontalSpacing = 0;
                AssociatedObject.TipOverTreeLayoutVerticalSpacing = 10;
                var list = new List<DiagramConnector>();
                double xCordinate = 500.0, yCordinate = 50.0;
                int cnt = 0;

                foreach (DiagramItem it in listItems)
                {
                    heirachyItem = new List<DiagramItem>();
                    verticalItem = new List<DiagramItem>();
                    otherItem = new List<DiagramItem>();

                    EmployeeHeirarchy empHeirarchy = (EmployeeHeirarchy)it.DataContext;

                    if (empHeirarchy.ID == empHeirarchy.ParentID)
                    {
                        if (cnt != 0)
                        {
                            yCordinate = yCordinate + 100;
                            xCordinate = 500;
                            it.Position = new System.Windows.Point(xCordinate, yCordinate);
                        }
                        else
                        {
                            it.Position = new System.Windows.Point(xCordinate, yCordinate);
                        }
                    }

                    if (empHeirarchy.ChildOrientation == "H")
                    {
                        heirachyItem.Add(it);
                        heirachyItem.AddRange(it.OutgoingConnectors.Cast<DiagramConnector>().Concat(it.OutgoingConnectors.Select(c => c.EndItem).Cast<DiagramItem>()));

                        System.Windows.Point initialPosition = it.Position;
                        AssociatedObject.ApplyTreeLayout(new TreeLayoutSettings(420, 100, DevExpress.Diagram.Core.Layout.LayoutDirection.TopToBottom), SplitToConnectedComponentsMode.AllComponents, heirachyItem);

                        Vector offset = initialPosition - it.Position;
                        foreach (DiagramItem item in heirachyItem)
                        {
                            if (item is DiagramConnector)
                                continue;
                            item.Position = new System.Windows.Point(item.Position.X + offset.X, item.Position.Y + offset.Y);
                        }

                        // AssociatedObject.ApplyTreeLayoutForSubordinates(heirachyItem, new DevExpress.Diagram.Core.Layout.TreeLayoutSettings(0.1, 100, DevExpress.Diagram.Core.Layout.LayoutDirection.TopToBottom));
                    }
                    else if (empHeirarchy.ChildOrientation == "V")
                    {
                        verticalItem.Add(it);
                        AssociatedObject.ApplyTipOverTreeLayoutForSubordinates(verticalItem);
                    }
                    else if (empHeirarchy.ChildOrientation == null)
                    {
                        otherItem.Add(it);
                        AssociatedObject.ApplyTreeLayoutForSubordinates(otherItem, new DevExpress.Diagram.Core.Layout.TreeLayoutSettings(0.1, 100, DevExpress.Diagram.Core.Layout.LayoutDirection.TopToBottom));
                    }

                    if (yCordinate <= it.Position.Y)
                    {
                        yCordinate = it.Position.Y;
                    }

                    list.AddRange(it.OutgoingConnectors.Concat(it.IncomingConnectors).Cast<DiagramConnector>());

                    // This Code for display horizontal item connector in right way.
                    if (empHeirarchy.ID != empHeirarchy.ParentID)
                    {
                        DiagramItem isUnderHorizontalOrientation = HorizontalChildOrientationList.FirstOrDefault(x => ((EmployeeHeirarchy)x.DataContext).ID == empHeirarchy.ParentID);
                        if (isUnderHorizontalOrientation != null)
                        {
                            if (cnt > 0)
                            {
                                DiagramConnector firstIncomingConnector = (DiagramConnector)it.IncomingConnectors.First();
                                firstIncomingConnector.EndItemPointIndex = 0;
                                AssociatedObject.UpdateConnectorsRoute(new DiagramConnector[] { firstIncomingConnector });
                            }
                        }
                    }
                    cnt++;
                }

                AssociatedObject.UpdateConnectorsRoute(AssociatedObject.Items.OfType<DiagramConnector>());

                if (e.Items.Any(x => ((EmployeeHeirarchy)x.DataContext).IdJobDescription == 21))
                {
                    AssociatedObject.FitToWidth();
                }
                else
                {
                    AssociatedObject.FitToDrawing();
                }

                //GeosApplication.Instance.Logger.Log("Method DiagramOrgChartBehaviorEx_CustomLayoutItems() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DiagramOrgChartBehaviorEx_CustomLayoutItems()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
    }

    public static class DiagramControlExportActionsEx
    {
        public static string ExportDiagramEx(this IDiagramControl diagram, DiagramExportFormat exportFormat = DiagramExportFormat.PNG)
        {
            var filter = DiagramController.ExportDialogFilter;

            var defaultExt = string.Format("*.{0}", exportFormat.ToString().ToLower());
            int filterIndex = 1 + (filter.Split('|').ToList().FindIndex(x => x.Equals(defaultExt)) - 1) / 2;
            var path = diagram.ShowSaveFileDialog("sdfsdf", filter,
                        DiagramControlLocalizer.GetString(DiagramControlStringId.ExportAs),
                        string.Empty, filterIndex);
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            ((DiagramControl)diagram).ExportDiagram(path);
            return path;
        }

        /// <summary>
        /// Method For Export Normal Diagram
        /// </summary>
        public static string ExportNormalDiagram(this IDiagramControl diagram, DiagramExportFormat exportFormat = DiagramExportFormat.PNG)
        {
            var filter = DiagramController.ExportDialogFilter;

            var defaultExt = string.Format("*.{0}", exportFormat.ToString().ToLower());
            int filterIndex = 1 + (filter.Split('|').ToList().FindIndex(x => x.Equals(defaultExt)) - 1) / 2;
            var path = diagram.ShowSaveFileDialog("NormalDepartment", filter,
                        DiagramControlLocalizer.GetString(DiagramControlStringId.ExportAs),
                        string.Empty, filterIndex);
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            ((DiagramControl)diagram).ExportDiagram(path);
            return path;
        }

        /// <summary>
        /// Method For Export Isolated Department Diagram
        /// </summary>
        public static string ExportIsolatedDiagram(this IDiagramControl diagram, DiagramExportFormat exportFormat = DiagramExportFormat.PNG)
        {
            var filter = DiagramController.ExportDialogFilter;

            var defaultExt = string.Format("*.{0}", exportFormat.ToString().ToLower());
            int filterIndex = 1 + (filter.Split('|').ToList().FindIndex(x => x.Equals(defaultExt)) - 1) / 2;
            var path = diagram.ShowSaveFileDialog("IsolatedDepartment", filter,
                        DiagramControlLocalizer.GetString(DiagramControlStringId.ExportAs),
                        string.Empty, filterIndex);
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            ((DiagramControl)diagram).ExportDiagram(path);
            return path;
        }

    }
}
