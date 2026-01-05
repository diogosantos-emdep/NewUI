using DevExpress.XtraRichEdit.API.Layout;
using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
    public sealed class PageLayoutHelper
    {
        private PageLayoutHelper() { }
        public static System.Drawing.Point GetInformationAboutCurrentPage(DocumentLayout currentDocumentLayout, DocumentPosition currentPosition)
        {
            int totalPagesHeight = 0;

            int totalPageCount = currentDocumentLayout.GetFormattedPageCount();
            for (var pageIndex = 0; pageIndex < totalPageCount; pageIndex++)
            {
                LayoutPage currentPage = currentDocumentLayout.GetPage(pageIndex);
                totalPagesHeight = totalPagesHeight + currentPage.Bounds.Height;
            }
            return new System.Drawing.Point(currentDocumentLayout.GetPage(0).Bounds.Width, totalPagesHeight);
        }

    }
}
