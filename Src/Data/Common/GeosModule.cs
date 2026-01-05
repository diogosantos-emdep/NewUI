using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace Emdep.Geos.Data.Common
{
    [Table("geos_modules")]
    [DataContract(IsReference = true)]
    public class GeosModule : ModelBase
    {
        #region Fields
        Byte idGeosModule;
        string name;
        string acronym;
        Int16? idCurrentVersion;
        string htmlColor;
        string imagePath;
        bool isFlowBreak;
        string navigateTo;
        ImageSource siteImageSource;
        string foreColor;
        List<Tuple<string, string>> tupleColor;
        string backColor;
        Int32 idLookupValue;
        bool isPermission;
        Int32 idGeosModulepermission;
        string forgroundColorAcronym;
        string forgroundColorName;


        #endregion

        #region Constructor
        public GeosModule()
        {
            this.UIModuleThemes = new List<UIModuleTheme>();
            this.GeosModuleDocumentations = new List<GeosModuleDocumentation>();
            this.GeosWorkbenchVersionsFiles = new List<GeosWorkbenchVersionsFile>();
            this.GeosReleaseNotes = new List<GeosReleaseNote>();
            this.Permissions = new List<Permission>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdGeosModule")]
        [DataMember]
        public Byte IdGeosModule
        {
            get { return idGeosModule; }
            set { idGeosModule = value; }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("Acronym")]
        [DataMember]
        public string Acronym
        {
            get { return acronym; }
            set { acronym = value; }
        }

        [Column("IdCurrentVersion")]
        [DataMember]
        public Int16? IdCurrentVersion
        {
            get { return idCurrentVersion; }
            set { idCurrentVersion = value; }
        }

        [Column("HtmlColor")]
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set { htmlColor = value; }
        }

        [NotMapped]
        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        [NotMapped]
        public bool IsFlowBreak
        {
            get { return isFlowBreak; }
            set { isFlowBreak = value; }
        }

        [NotMapped]
        public string NavigateTo
        {
            get { return navigateTo; }
            set { navigateTo = value; }
        }

        [NotMapped]
        public ImageSource SiteImageSource
        {
            get { return siteImageSource; }
            set { siteImageSource = value; }
        }
        [NotMapped]
        public string ForeColor
        {
            get { return foreColor; }
            set { foreColor = value; }
        }

        [NotMapped]
        public string BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        [NotMapped]
        public List<Tuple<string, string>> TupleColor
        {
            get { return tupleColor; }
            set { tupleColor = value; }
        }
        // [pallavi.jadhav][Date:20/11/2025][GEOS2-10143]

        [NotMapped]
        [DataMember]
        public Int32 IdLookupValue
        {
            get { return idLookupValue; }
            set { idLookupValue = value; }
        }

        [NotMapped]
        public bool IsPermission
        {
            get { return isPermission; }
            set
            {
                isPermission = value;

            }
        }
        [NotMapped]
        public string ForgroundColorAcronym
        {
            get { return forgroundColorAcronym; }
            set { forgroundColorAcronym = value; }
        }
        [NotMapped]
        public string ForgroundColorName
        {
            get { return forgroundColorName; }
            set { forgroundColorName = value; }
        }



        [NotMapped]
        [DataMember]
        public Int32 IdGeosModulepermission
        {
            get { return idGeosModulepermission; }
            set { idGeosModulepermission = value; }
        }
        [DataMember]
        public virtual List<Permission> Permissions { get; set; }
        [DataMember]
        public virtual List<GeosModuleDocumentation> GeosModuleDocumentations { get; set; }
        [DataMember]
        public virtual List<GeosWorkbenchVersionsFile> GeosWorkbenchVersionsFiles { get; set; }
        [DataMember]
        public virtual List<GeosReleaseNote> GeosReleaseNotes { get; set; }
        [DataMember]
        public virtual List<UIModuleTheme> UIModuleThemes { get; set; }
        #endregion
    }
}
