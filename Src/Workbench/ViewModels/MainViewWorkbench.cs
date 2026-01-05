using DevExpress.Mvvm.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Workbench.ViewModels
{
    class MainViewWorkbench : IViewLocator,IDisposable
    {
        

        public object ResolveView(string name)
        {

            if (name == "Workbench.Views.DashboardView")
            {

                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);

            }
            if (name == "Workbench.Views.DesignView")
            {

                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);


            }
            if (name == "Workbench.Views.EpcWindow")
            {

                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Workbench.Views.WorkshopView")
            {

                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Workbench.Views.HarnessPartsView")
            {

                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);

            }

            if (name == "Workbench.Views.CrmWindow")
            {

                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);

            }

            if (name == "Emdep.Geos.Modules.CrmModule.Views.CrmMainView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.CrmModule.Views.Dashboard")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.CrmModule.Views.AnnualSalesPerformanceView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }


            if (name == "Emdep.Geos.Modules.Epc.Views.StrategicMapView")
            {

                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);

            }
            if (name == "Emdep.Geos.Modules.Epc.Views.ProjectBoardView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.ProjectSchedulerView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.TaskView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.TaskDetailsView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.WatcherView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.RequestAssistanceView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.CalendarView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.ProductView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.ProjectView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.KickoffSoftwareAreaView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.KickoffElectronicAreaView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.KickoffSupportAreaView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.CloseSoftwareAreaView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.CloseElectronicAreaView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.CloseSupportAreaView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.WbsView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.ProjectDevelopmentView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.ProjectDocumentView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.AnalysisRequestView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.MilestonesView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }


          
            if (name == "Emdep.Geos.Modules.CrmModule.View.CrmMainView")
            {
              
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.CrmModule.dll");
                return assembly.GetType("Emdep.Geos.Modules.CrmModule.View.CrmMainView");
            }
            if (name == "Emdep.Geos.Modules.CrmModule.View.Dashboard")
            {
                
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.CrmModule.dll");
                return assembly.GetType("Emdep.Geos.Modules.CrmModule.View.Dashboard");
            }

            if (name == "Emdep.Geos.Modules.CrmModule.Views.MyWorkView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }

            if (name == "Emdep.Geos.Modules.CrmModule.Views.CalendarView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.CrmModule.Views.LeadsView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.CrmModule.Views.OrderView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.CrmModule.Views.ExchangeRateView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
             
            }
            if (name == "Emdep.Geos.Modules.CrmModule.Views.EmployesView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
              
            }

            if (name == "Emdep.Geos.Modules.CrmModule.Views.AccountView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            if (name == "Emdep.Geos.Modules.Epc.Views.ProjectView")
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            return null;
        }
        public Type ResolveViewType(string name)
        {
            if (name == "Workbench.Views.DashboardView")
            {

                return Type.GetType("Workbench.Views.DashboardView");
            }
            if (name == "Workbench.Views.DesignView")
            {

                return Type.GetType("Workbench.Views.DesignView");
            }
            if (name == "Workbench.Views.EpcWindow")
            {

                return Type.GetType("Workbench.Views.EpcWindow");
            }
            if (name == "Workbench.Views.WorkshopView")
            {

                return Type.GetType("Workbench.Views.WorkshopView");
            }
            if (name == "Workbench.Views.HarnessPartsView")
            {

                return Type.GetType("Workbench.Views.HarnessPartsView");

            }
            if (name == "Workbench.Views.CrmWindow")
            {

                return Type.GetType("Workbench.Views.CrmWindow");
            }
            if (name == "Emdep.Geos.Modules.CrmModule.Views.CrmMainView")
            {

                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.CrmModule.dll");
                return assembly.GetType("Emdep.Geos.Modules.CrmModule.Views.CrmMainView");

            }
            if (name == "Emdep.Geos.Modules.CrmModule.Views.Dashboard")
            {

                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.CrmModule.dll");
                return assembly.GetType("Emdep.Geos.Modules.CrmModule.Views.Dashboard");

            }
            if (name == "Emdep.Geos.Modules.CrmModule.Views.AnnualSalesPerformanceView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.CrmModule.dll");
                return assembly.GetType("Emdep.Geos.Modules.CrmModule.Views.AnnualSalesPerformanceView");
            }

            if (name == "Emdep.Geos.Modules.CrmModule.Views.LeadsView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.CrmModule.dll");
                return assembly.GetType("Emdep.Geos.Modules.CrmModule.Views.LeadsView");
            }

            if (name == "Emdep.Geos.Modules.CrmModule.Views.OrderView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.CrmModule.dll");
                return assembly.GetType("Emdep.Geos.Modules.CrmModule.Views.OrderView");
            }
            if (name == "Emdep.Geos.Modules.CrmModule.Views.MyWorkView")
            {

                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.CrmModule.dll");
                return assembly.GetType("Emdep.Geos.Modules.CrmModule.Views.MyWorkView");

            }

            if (name == "Emdep.Geos.Modules.CrmModule.Views.CalendarView")
            {

                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.CrmModule.dll");
                return assembly.GetType("Emdep.Geos.Modules.CrmModule.Views.CalendarView");

            }

            if (name == "Emdep.Geos.Modules.CrmModule.Views.ExchangeRateView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.CrmModule.dll");
                return assembly.GetType("Emdep.Geos.Modules.CrmModule.Views.ExchangeRateView");
            }

            if (name == "Emdep.Geos.Modules.CrmModule.Views.EmployesView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.CrmModule.dll");
                return assembly.GetType("Emdep.Geos.Modules.CrmModule.Views.EmployesView");
            }

            if (name == "Emdep.Geos.Modules.CrmModule.Views.AccountView")
            {
                Assembly assembly = Assembly.LoadFrom(@"Emdep.Geos.Modules.CrmModule.dll");
                return assembly.GetType("Emdep.Geos.Modules.CrmModule.Views.AccountView");
            }





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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Environment.Exit(0);
        }

        public string GetViewTypeName(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
