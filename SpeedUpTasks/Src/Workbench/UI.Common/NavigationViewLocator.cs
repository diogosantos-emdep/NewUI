using DevExpress.Mvvm.UI;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Common
{
 public   class NavigationViewLocator : IViewLocator
    {
        public string GetViewTypeName(Type type)
        {
            throw new NotImplementedException();
        }

        public object ResolveView(string name)
        {

            if (name.StartsWith("Emdep.Geos.Modules."))
            {
                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);
            }
            else if (name.StartsWith("Workbench."))
            {

                Type t = ResolveViewType(name);
                return Activator.CreateInstance(t);

            }

            return null;
        }
        public Type ResolveViewType(string name)
        {
            if (name.StartsWith("Emdep.Geos.Modules."))
            {
                foreach (var item in GeosApplication.Instance.ModulesList)
                {
                    try
                    {
                        var type = item.GetExportedTypes().SingleOrDefault(x => x.FullName == name);
                        if (type != null)
                        {
                            return item.GetType(name);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }


            }
            else if (name.StartsWith("Workbench."))
            {

                return Type.GetType(name);
            }


            return null;
        }
    }
}
