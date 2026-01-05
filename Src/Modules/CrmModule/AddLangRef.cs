using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Crm
{
   public class AddLangRef
    {
        public void Init()
        {
            if (Application.Current == null)
            {
                // create the Application object
                try
                {
                    new Application();
                }
                catch (Exception ex)
                {
                    //Application.
                }
                // merge in your application resources
                Application.Current.Resources.MergedDictionaries.Add(
                Application.LoadComponent(
                new Uri("/Emdep.Geos.Modules.Crm;component/Resources/Language.xaml",
                UriKind.Relative)) as ResourceDictionary);
            }
        }
    }
}
