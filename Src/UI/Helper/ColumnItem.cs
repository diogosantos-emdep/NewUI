using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Media;
using static Emdep.Geos.UI.Helper.TimeTrackingColumnTemplateSelector;
using static Emdep.Geos.UI.Helper.PlantOperationPlanningColumnTemplateSelector; ////[GEOS2-4336][gulab lakade][18 04 2023]
using static Emdep.Geos.UI.Helper.RealTimeMonitorColumnTemplateSelector; //[GEOS2-4351][Rupali Sarode][29-05-2023]
using static Emdep.Geos.UI.Helper.RealTimeMonitorHRResourcesColumnTemplateSelector; //[GEOS2-4729][rupali sarode][07-08-2023]
using static Emdep.Geos.UI.Helper.PlantLoadAnalysisTemplateSelector;
using static Emdep.Geos.UI.Helper.ProductionTimeLineHelper;//[GEOS2-5394][gulab lakade]
using static Emdep.Geos.UI.Helper.DTDColumnTemplateSelector; //[GEOS2-5354][Rupali Sarode][16-02-2024]
using static Emdep.Geos.UI.Helper.PcmArticleColumnTemplateSelector;
using DevExpress.Xpf.Grid;


namespace Emdep.Geos.UI.Helper
{
    public class ColumnItem 
    {
        public string ColumnFieldName{get;set;}
        public string Binding{get;set;}
        public string HeaderText { get; set; }
        public double Width { get; set; }
        public string Tag { get; set; }
        public SettingsType Settings { get; set; }
        public ProductTypeSettingsType ProductTypeSettings { get; set; }
        public WorkPlanningSettingsType WorkPlanningSettingsType { get; set; }
        public bool Visible { get; set; }
        public bool IsVertical { get; set; }
        public ImageSource ImageSrc { get; set; }
        public DiscountArticleSettingsType DiscountArticleSettings { get; set; }
        public TimetrackingSettingType TimetrackingSetting { get; set; }
        public PlantOperationPlanningSettingType PlantOperationPlanningSetting { get; set; }    //[GEOS2-4336][gulab lakade][18 04 2023]
        public RealTimeMonitorSettingType RealTimeMonitorSetting { get; set; } //[GEOS2-4351][Rupali Sarode][29-05-2023]
        public RealTimeMonitorHRResourcesSettingType RealTimeMonitorHRResourcesSetting { get; set; } //[GEOS2-4729][rupali sarode][07-08-2023]
        public PlantLoadAnalysisSettingType PlantLoadAnalysisetting { get; set; } //[GEOS2-5035][pallavi jadhav][18-12-2023]
        public ProductionTimeLineSettingType ProductionTimelineSetting { get; set; }//[GEOS2-5394][gulab lakade]
        public DTDSettingType DTDSetting { get; set; } //[GEOS2-5354][Rupali Sarode][16-02-2024]
        public ArticleTypeSettingsType ArticleTypeSetting { get; set; } //[rdixit][15.07.2024][rdixit]
        public SiteTypeSettingsType SiteTypeSettings { get; set; }

        public DrawingSettingsType DrawingSettings { get; set; }
        public VisibilityPerBUSettingsType VisibilityPerBUSettings { get; set; } //[GEOS2-6696][shweta.thube]

        //public TSMUsersSettingsType TSMUsersSettings { get; set; } //[GEOS2-5388][pallavi.kale]
        public bool Readonly { get; set; }

        public TSMUsersSettingsType TSMUsersSettings { get; set; }//[GEOS2-5388][pallavi.kale]
        public string ToolTipValue { get; set; } //[GEOS2-5388][pallavi.kale]
		//[nsatpute][GEOS2-8090][22.07.2025]
        public int GroupIndex { get; set; }
        public bool AllowEditing { get; set; }
        public bool AllowGrouping { get; set; }
        public bool AllowSorting { get; set; }
        public FixedStyle FixedStyle { get; set; }

    }
  
}
