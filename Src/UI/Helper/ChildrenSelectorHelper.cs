using DevExpress.Xpf.Accordion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.UI.Helper
{
    public class ChildrenSelectorHelper : IChildrenSelector
    {
        public IEnumerable SelectChildren(object item)
        {
            if (item is PackingCompany)
            {
                return ((PackingCompany)item).PackingBoxes;
            }
            else return null;
        }
    }
}
