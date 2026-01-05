using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
//[shweta.thube][GEOS2-6630][04.04.2025]
namespace Emdep.Geos.UI.Common
{
    public class SCMSections
    {
        private int idSection;
        private string sectionName;
        private ImageSource sectionImage;

        public int IdSection
        {
            get { return idSection; }
            set { idSection = value; }
        }

        public string SectionName
        {
            get { return sectionName; }
            set { sectionName = value; }
        }

        public ImageSource SectionImage
        {
            get { return sectionImage; }
            set { sectionImage = value; }
        }
    }
}
