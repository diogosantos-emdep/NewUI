using DevExpress.Mvvm.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc
{
   public class MainViewLocator:IViewLocator
    {
        public object ResolveView(string name)
        {
            Type t = ResolveViewType(name);
            return Activator.CreateInstance(t);
        }
        public Type ResolveViewType(string name)
        {
            if (name == "Emdep.Geos.Modules.Epc.Views.StrategicMapView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.StrategicMapView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.ProjectBoardView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.ProjectBoardView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.ProjectSchedulerView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.ProjectSchedulerView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.TaskView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.TaskView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.TaskDetailsView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.TaskDetailsView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.WatcherView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.WatcherView");
            }            
            if (name == "Emdep.Geos.Modules.Epc.Views.RequestAssistanceView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.RequestAssistanceView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.CalendarView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.CalendarView");
            }           
            if (name == "Emdep.Geos.Modules.Epc.Views.ProductView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.ProductView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.ProjectView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.ProjectView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.KickoffSoftwareAreaView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.KickoffSoftwareAreaView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.KickoffElectronicAreaView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.KickoffElectronicAreaView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.KickoffSupportAreaView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.KickoffSupportAreaView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.CloseSoftwareAreaView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.CloseSoftwareAreaView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.CloseElectronicAreaView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.CloseElectronicAreaiew");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.CloseSupportAreaView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.CloseSupportAreaView");
            }          
            if (name == "Emdep.Geos.Modules.Epc.Views.WbsView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.WbsView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.ProjectDevelopmentView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.ProjectDevelopmentView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.ProjectDocumentView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.ProjectDocumentView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.AnalysisRequestView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.AnalysisRequestView");
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.MilestonesView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.Epc.dll");
                return assembly.GetType("Emdep.Geos.Modules.Epc.Views.MilestonesView");
            }

            return null;
        }
    }

  
}
