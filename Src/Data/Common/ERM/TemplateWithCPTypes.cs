using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class TemplateWithCPTypes
    {
        public string Name { get; set; }
        public ObservableCollection<Template> TemplatesMenuList { get; set; }
        public List<ProductTypes> ProductTypesMenu { get; set; }
        public override string ToString()
        {
            return Name;
        }

        public TemplateWithCPTypes()
        {
        }
        public TemplateWithCPTypes(string name, IEnumerable<ProductTypes> productTypes)
        {
            Name = name;
            ProductTypesMenu = new List<ProductTypes>(productTypes);
        }

    }
}
