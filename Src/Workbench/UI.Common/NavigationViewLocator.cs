using DevExpress.Mvvm.UI;
using Emdep.Geos.UI.Common;
using System;
using System.Linq;
using System.Reflection;

namespace Emdep.Geos.UI.Common
{
    public class NavigationViewLocator : IViewLocator
    {
        public string GetViewTypeName(Type type)
        {
            throw new NotImplementedException();
        }

        public object ResolveView(string name)
        {
            var t = ResolveViewType(name);
            if (t == null)
            {
                return null;
            }
            return Activator.CreateInstance(t);
        }

        public Type ResolveViewType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            if (name.StartsWith("Emdep.Geos.Modules."))
            {
                if (GeosApplication.Instance.ModulesList != null)
                {
                    foreach (var asm in GeosApplication.Instance.ModulesList)
                    {
                        try
                        {
                            var type = asm.GetType(name, throwOnError: false, ignoreCase: false);
                            if (type != null)
                                return type;

                            type = asm.GetExportedTypes().SingleOrDefault(x => x.FullName == name);
                            if (type != null)
                                return type;
                        }
                        catch
                        {
                        }
                    }
                }

                var loadedType = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(a =>
                    {
                        try { return a.GetTypes(); }
                        catch { return Type.EmptyTypes; }
                    })
                    .FirstOrDefault(t => t.FullName == name);
                if (loadedType != null)
                    return loadedType;

                var segments = name.Split('.');
                var assemblyName = string.Join(".", segments.TakeWhile(s => s != "Views"));
                try
                {
                    var asm = Assembly.Load(assemblyName);
                    var candidate = asm.GetType(name, throwOnError: false, ignoreCase: false);
                    if (candidate != null)
                        return candidate;
                }
                catch
                {
                }

                return null;
            }
            else if (name.StartsWith("Workbench."))
            {
                return Type.GetType(name);
            }

            return null;
        }
    }
}