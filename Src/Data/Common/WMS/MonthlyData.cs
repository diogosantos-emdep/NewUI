using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.WMS
{
    //[nsatpute][12.09.2025][GEOS2-8791]
    [DataContract]
    public class MonthlyData : ModelBase, IDisposable
    {
        #region Declaration

        private int day;
        private int? january;
        private int? february;
        private int? march;
        private int? april;
        private int? may;
        private int? june;
        private int? july;
        private int? august;
        private int? september;
        private int? october;
        private int? november;
        private int? december;
        // [nsatpute][17-09-2025][GEOS2-8793]
        private string januaryData;
        private string februaryData;
        private string marchData;
        private string aprilData;
        private string mayData;
        private string juneData;
        private string julyData;
        private string augustData;
        private string septemberData;
        private string octoberData;
        private string novemberData;
        private string decemberData;

        private System.Windows.Media.Brush januaryBackColor;
        private Visibility januaryShowButton;
        private System.Windows.Media.Brush februaryBackColor;
        private Visibility februaryShowButton;
        private System.Windows.Media.Brush marchBackColor;
        private Visibility marchShowButton;
        private System.Windows.Media.Brush aprilBackColor;
        private Visibility aprilShowButton;
        private System.Windows.Media.Brush mayBackColor;
        private Visibility mayShowButton;
        private System.Windows.Media.Brush juneBackColor;
        private Visibility juneShowButton;
        private System.Windows.Media.Brush julyBackColor;
        private Visibility julyShowButton;
        private System.Windows.Media.Brush augustBackColor;
        private Visibility augustShowButton;
        private System.Windows.Media.Brush septemberBackColor;
        private Visibility septemberShowButton;
        private System.Windows.Media.Brush octoberBackColor;
        private Visibility octoberShowButton;
        private System.Windows.Media.Brush novemberBackColor;
        private Visibility novemberShowButton;
        private System.Windows.Media.Brush decemberBackColor;
        private Visibility decemberShowButton;

        private DayOfWeek? januaryDayOfWeek;
        private int? januaryWeekNumber;
        private string januarySaturday;
        private string januarySunday;

        private DayOfWeek? februaryDayOfWeek;
        private int? februaryWeekNumber;
        private string februarySaturday;
        private string februarySunday;

        private DayOfWeek? marchDayOfWeek;
        private int? marchWeekNumber;
        private string marchSaturday;
        private string marchSunday;

        private DayOfWeek? aprilDayOfWeek;
        private int? aprilWeekNumber;
        private string aprilSaturday;
        private string aprilSunday;

        private DayOfWeek? mayDayOfWeek;
        private int? mayWeekNumber;
        private string maySaturday;
        private string maySunday;

        private DayOfWeek? juneDayOfWeek;
        private int? juneWeekNumber;
        private string juneSaturday;
        private string juneSunday;

        private DayOfWeek? julyDayOfWeek;
        private int? julyWeekNumber;
        private string julySaturday;
        private string julySunday;

        private DayOfWeek? augustDayOfWeek;
        private int? augustWeekNumber;
        private string augustSaturday;
        private string augustSunday;

        private DayOfWeek? septemberDayOfWeek;
        private int? septemberWeekNumber;
        private string septemberSaturday;
        private string septemberSunday;

        private DayOfWeek? octoberDayOfWeek;
        private int? octoberWeekNumber;
        private string octoberSaturday;
        private string octoberSunday;

        private DayOfWeek? novemberDayOfWeek;
        private int? novemberWeekNumber;
        private string novemberSaturday;
        private string novemberSunday;

        private DayOfWeek? decemberDayOfWeek;
        private int? decemberWeekNumber;
        private string decemberSaturday;
        private string decemberSunday;

        private List<ScheduleEvent> januaryScheduleEvents;
        private List<ScheduleEvent> februaryScheduleEvents;
        private List<ScheduleEvent> marchScheduleEvents;
        private List<ScheduleEvent> aprilScheduleEvents;
        private List<ScheduleEvent> mayScheduleEvents;
        private List<ScheduleEvent> juneScheduleEvents;
        private List<ScheduleEvent> julyScheduleEvents;
        private List<ScheduleEvent> augustScheduleEvents;
        private List<ScheduleEvent> septemberScheduleEvents;
        private List<ScheduleEvent> octoberScheduleEvents;
        private List<ScheduleEvent> novemberScheduleEvents;
        private List<ScheduleEvent> decemberScheduleEvents;
		//[nsatpute][16.10.2025][GEOS2-8799]
        private Visibility januaryCircleVisibility;
        private Visibility februaryCircleVisibility;
        private Visibility marchCircleVisibility;
        private Visibility aprilCircleVisibility;
        private Visibility mayCircleVisibility;
        private Visibility juneCircleVisibility;
        private Visibility julyCircleVisibility;
        private Visibility augustCircleVisibility;
        private Visibility septemberCircleVisibility;
        private Visibility octoberCircleVisibility;
        private Visibility novemberCircleVisibility;
        private Visibility decemberCircleVisibility;


        #endregion

        #region Constructor

        public MonthlyData()
        {
        }

        #endregion

        #region Properties
        [DataMember]
        public int Day
        {
            get { return day; }
            set
            {
                if (day != value)
                {
                    day = value;
                    OnPropertyChanged("Day");
                }
            }
        }

        [DataMember]
        public int? January
        {
            get { return january; }
            set
            {
                if (january != value)
                {
                    january = value;
                    OnPropertyChanged("January");
                }
            }
        }

        [DataMember]
        public int? February
        {
            get { return february; }
            set
            {
                if (february != value)
                {
                    february = value;
                    OnPropertyChanged("February");
                }
            }
        }

        [DataMember]
        public int? March
        {
            get { return march; }
            set
            {
                if (march != value)
                {
                    march = value;
                    OnPropertyChanged("March");
                }
            }
        }

        [DataMember]
        public int? April
        {
            get { return april; }
            set
            {
                if (april != value)
                {
                    april = value;
                    OnPropertyChanged("April");
                }
            }
        }

        [DataMember]
        public int? May
        {
            get { return may; }
            set
            {
                if (may != value)
                {
                    may = value;
                    OnPropertyChanged("May");
                }
            }
        }

        [DataMember]
        public int? June
        {
            get { return june; }
            set
            {
                if (june != value)
                {
                    june = value;
                    OnPropertyChanged("June");
                }
            }
        }

        [DataMember]
        public int? July
        {
            get { return july; }
            set
            {
                if (july != value)
                {
                    july = value;
                    OnPropertyChanged("July");
                }
            }
        }

        [DataMember]
        public int? August
        {
            get { return august; }
            set
            {
                if (august != value)
                {
                    august = value;
                    OnPropertyChanged("August");
                }
            }
        }

        [DataMember]
        public int? September
        {
            get { return september; }
            set
            {
                if (september != value)
                {
                    september = value;
                    OnPropertyChanged("September");
                }
            }
        }

        [DataMember]
        public int? October
        {
            get { return october; }
            set
            {
                if (october != value)
                {
                    october = value;
                    OnPropertyChanged("October");
                }
            }
        }

        [DataMember]
        public int? November
        {
            get { return november; }
            set
            {
                if (november != value)
                {
                    november = value;
                    OnPropertyChanged("November");
                }
            }
        }

        [DataMember]
        public int? December
        {
            get { return december; }
            set
            {
                if (december != value)
                {
                    december = value;
                    OnPropertyChanged("December");
                }
            }
        }
        // [nsatpute][17-09-2025][GEOS2-8793]
        [DataMember]
        public string JanuaryData
        {
            get { return januaryData; }
            set
            {
                if (januaryData != value)
                {
                    januaryData = value;
                    OnPropertyChanged("JanuaryData");
                }
            }
        }

        [DataMember]
        public string FebruaryData
        {
            get { return februaryData; }
            set
            {
                if (februaryData != value)
                {
                    februaryData = value;
                    OnPropertyChanged("FebruaryData");
                }
            }
        }

        [DataMember]
        public string MarchData
        {
            get { return marchData; }
            set
            {
                if (marchData != value)
                {
                    marchData = value;
                    OnPropertyChanged("MarchData");
                }
            }
        }

        [DataMember]
        public string AprilData
        {
            get { return aprilData; }
            set
            {
                if (aprilData != value)
                {
                    aprilData = value;
                    OnPropertyChanged("AprilData");
                }
            }
        }

        [DataMember]
        public string MayData
        {
            get { return mayData; }
            set
            {
                if (mayData != value)
                {
                    mayData = value;
                    OnPropertyChanged("MayData");
                }
            }
        }

        [DataMember]
        public string JuneData
        {
            get { return juneData; }
            set
            {
                if (juneData != value)
                {
                    juneData = value;
                    OnPropertyChanged("JuneData");
                }
            }
        }

        [DataMember]
        public string JulyData
        {
            get { return julyData; }
            set
            {
                if (julyData != value)
                {
                    julyData = value;
                    OnPropertyChanged("JulyData");
                }
            }
        }

        [DataMember]
        public string AugustData
        {
            get { return augustData; }
            set
            {
                if (augustData != value)
                {
                    augustData = value;
                    OnPropertyChanged("AugustData");
                }
            }
        }

        [DataMember]
        public string SeptemberData
        {
            get { return septemberData; }
            set
            {
                if (septemberData != value)
                {
                    septemberData = value;
                    OnPropertyChanged("SeptemberData");
                }
            }
        }

        [DataMember]
        public string OctoberData
        {
            get { return octoberData; }
            set
            {
                if (octoberData != value)
                {
                    octoberData = value;
                    OnPropertyChanged("OctoberData");
                }
            }
        }

        [DataMember]
        public string NovemberData
        {
            get { return novemberData; }
            set
            {
                if (novemberData != value)
                {
                    novemberData = value;
                    OnPropertyChanged("NovemberData");
                }
            }
        }

        [DataMember]
        public string DecemberData
        {
            get { return decemberData; }
            set
            {
                if (decemberData != value)
                {
                    decemberData = value;
                    OnPropertyChanged("DecemberData");
                }
            }
        }

        public System.Windows.Media.Brush JanuaryBackColor
        {
            get { return januaryBackColor; }
            set
            {
                januaryBackColor = value;
                OnPropertyChanged("JanuaryBackColor");
            }
        }

        public Visibility JanuaryShowButton
        {
            get { return januaryShowButton; }
            set
            {
                januaryShowButton = value;
                OnPropertyChanged("JanuaryShowButton");
            }
        }

        public System.Windows.Media.Brush FebruaryBackColor
        {
            get { return februaryBackColor; }
            set
            {
                februaryBackColor = value;
                OnPropertyChanged("FebruaryBackColor");
            }
        }

        public Visibility FebruaryShowButton
        {
            get { return februaryShowButton; }
            set
            {
                februaryShowButton = value;
                OnPropertyChanged("FebruaryShowButton");
            }
        }

        public System.Windows.Media.Brush MarchBackColor
        {
            get { return marchBackColor; }
            set
            {
                marchBackColor = value;
                OnPropertyChanged("MarchBackColor");
            }
        }

        public Visibility MarchShowButton
        {
            get { return marchShowButton; }
            set
            {
                marchShowButton = value;
                OnPropertyChanged("MarchShowButton");
            }
        }

        public System.Windows.Media.Brush AprilBackColor
        {
            get { return aprilBackColor; }
            set
            {
                aprilBackColor = value;
                OnPropertyChanged("AprilBackColor");
            }
        }

        public Visibility AprilShowButton
        {
            get { return aprilShowButton; }
            set
            {
                aprilShowButton = value;
                OnPropertyChanged("AprilShowButton");
            }
        }

        public System.Windows.Media.Brush MayBackColor
        {
            get { return mayBackColor; }
            set
            {
                mayBackColor = value;
                OnPropertyChanged("MayBackColor");
            }
        }

        public Visibility MayShowButton
        {
            get { return mayShowButton; }
            set
            {
                mayShowButton = value;
                OnPropertyChanged("MayShowButton");
            }
        }

        public System.Windows.Media.Brush JuneBackColor
        {
            get { return juneBackColor; }
            set
            {
                juneBackColor = value;
                OnPropertyChanged("JuneBackColor");
            }
        }

        public Visibility JuneShowButton
        {
            get { return juneShowButton; }
            set
            {
                juneShowButton = value;
                OnPropertyChanged("JuneShowButton");
            }
        }

        public System.Windows.Media.Brush JulyBackColor
        {
            get { return julyBackColor; }
            set
            {
                julyBackColor = value;
                OnPropertyChanged("JulyBackColor");
            }
        }

        public Visibility JulyShowButton
        {
            get { return julyShowButton; }
            set
            {
                julyShowButton = value;
                OnPropertyChanged("JulyShowButton");
            }
        }

        public System.Windows.Media.Brush AugustBackColor
        {
            get { return augustBackColor; }
            set
            {
                augustBackColor = value;
                OnPropertyChanged("AugustBackColor");
            }
        }

        public Visibility AugustShowButton
        {
            get { return augustShowButton; }
            set
            {
                augustShowButton = value;
                OnPropertyChanged("AugustShowButton");
            }
        }

        public System.Windows.Media.Brush SeptemberBackColor
        {
            get { return septemberBackColor; }
            set
            {
                septemberBackColor = value;
                OnPropertyChanged("SeptemberBackColor");
            }
        }

        public Visibility SeptemberShowButton
        {
            get { return septemberShowButton; }
            set
            {
                septemberShowButton = value;
                OnPropertyChanged("SeptemberShowButton");
            }
        }

        public System.Windows.Media.Brush OctoberBackColor
        {
            get { return octoberBackColor; }
            set
            {
                octoberBackColor = value;
                OnPropertyChanged("OctoberBackColor");
            }
        }

        public Visibility OctoberShowButton
        {
            get { return octoberShowButton; }
            set
            {
                octoberShowButton = value;
                OnPropertyChanged("OctoberShowButton");
            }
        }

        public System.Windows.Media.Brush NovemberBackColor
        {
            get { return novemberBackColor; }
            set
            {
                novemberBackColor = value;
                OnPropertyChanged("NovemberBackColor");
            }
        }

        public Visibility NovemberShowButton
        {
            get { return novemberShowButton; }
            set
            {
                novemberShowButton = value;
                OnPropertyChanged("NovemberShowButton");
            }
        }

        public System.Windows.Media.Brush DecemberBackColor
        {
            get { return decemberBackColor; }
            set
            {
                decemberBackColor = value;
                OnPropertyChanged("DecemberBackColor");
            }
        }

        public Visibility DecemberShowButton
        {
            get { return decemberShowButton; }
            set
            {
                decemberShowButton = value;
                OnPropertyChanged("DecemberShowButton");
            }
        }

        [DataMember]
        public DayOfWeek? JanuaryDayOfWeek
        {
            get { return januaryDayOfWeek; }
            set
            {
                if (januaryDayOfWeek != value)
                {
                    januaryDayOfWeek = value;
                    OnPropertyChanged("JanuaryDayOfWeek");
                }
            }
        }

        [DataMember]
        public int? JanuaryWeekNumber
        {
            get { return januaryWeekNumber; }
            set
            {
                if (januaryWeekNumber != value)
                {
                    januaryWeekNumber = value;
                    OnPropertyChanged("JanuaryWeekNumber");
                }
            }
        }

        [DataMember]
        public string JanuarySaturday
        {
            get { return januarySaturday; }
            set
            {
                if (januarySaturday != value)
                {
                    januarySaturday = value;
                    OnPropertyChanged("JanuarySaturday");
                }
            }
        }

        [DataMember]
        public string JanuarySunday
        {
            get { return januarySunday; }
            set
            {
                if (januarySunday != value)
                {
                    januarySunday = value;
                    OnPropertyChanged("JanuarySunday");
                }
            }
        }

        [DataMember]
        public DayOfWeek? FebruaryDayOfWeek
        {
            get { return februaryDayOfWeek; }
            set
            {
                if (februaryDayOfWeek != value)
                {
                    februaryDayOfWeek = value;
                    OnPropertyChanged("FebruaryDayOfWeek");
                }
            }
        }

        [DataMember]
        public int? FebruaryWeekNumber
        {
            get { return februaryWeekNumber; }
            set
            {
                if (februaryWeekNumber != value)
                {
                    februaryWeekNumber = value;
                    OnPropertyChanged("FebruaryWeekNumber");
                }
            }
        }

        [DataMember]
        public string FebruarySaturday
        {
            get { return februarySaturday; }
            set
            {
                if (februarySaturday != value)
                {
                    februarySaturday = value;
                    OnPropertyChanged("FebruarySaturday");
                }
            }
        }

        [DataMember]
        public string FebruarySunday
        {
            get { return februarySunday; }
            set
            {
                if (februarySunday != value)
                {
                    februarySunday = value;
                    OnPropertyChanged("FebruarySunday");
                }
            }
        }

        [DataMember]
        public DayOfWeek? MarchDayOfWeek
        {
            get { return marchDayOfWeek; }
            set
            {
                if (marchDayOfWeek != value)
                {
                    marchDayOfWeek = value;
                    OnPropertyChanged("MarchDayOfWeek");
                }
            }
        }

        [DataMember]
        public int? MarchWeekNumber
        {
            get { return marchWeekNumber; }
            set
            {
                if (marchWeekNumber != value)
                {
                    marchWeekNumber = value;
                    OnPropertyChanged("MarchWeekNumber");
                }
            }
        }

        [DataMember]
        public string MarchSaturday
        {
            get { return marchSaturday; }
            set
            {
                if (marchSaturday != value)
                {
                    marchSaturday = value;
                    OnPropertyChanged("MarchSaturday");
                }
            }
        }

        [DataMember]
        public string MarchSunday
        {
            get { return marchSunday; }
            set
            {
                if (marchSunday != value)
                {
                    marchSunday = value;
                    OnPropertyChanged("MarchSunday");
                }
            }
        }

        [DataMember]
        public DayOfWeek? AprilDayOfWeek
        {
            get { return aprilDayOfWeek; }
            set
            {
                if (aprilDayOfWeek != value)
                {
                    aprilDayOfWeek = value;
                    OnPropertyChanged("AprilDayOfWeek");
                }
            }
        }

        [DataMember]
        public int? AprilWeekNumber
        {
            get { return aprilWeekNumber; }
            set
            {
                if (aprilWeekNumber != value)
                {
                    aprilWeekNumber = value;
                    OnPropertyChanged("AprilWeekNumber");
                }
            }
        }

        [DataMember]
        public string AprilSaturday
        {
            get { return aprilSaturday; }
            set
            {
                if (aprilSaturday != value)
                {
                    aprilSaturday = value;
                    OnPropertyChanged("AprilSaturday");
                }
            }
        }

        [DataMember]
        public string AprilSunday
        {
            get { return aprilSunday; }
            set
            {
                if (aprilSunday != value)
                {
                    aprilSunday = value;
                    OnPropertyChanged("AprilSunday");
                }
            }
        }

        [DataMember]
        public DayOfWeek? MayDayOfWeek
        {
            get { return mayDayOfWeek; }
            set
            {
                if (mayDayOfWeek != value)
                {
                    mayDayOfWeek = value;
                    OnPropertyChanged("MayDayOfWeek");
                }
            }
        }

        [DataMember]
        public int? MayWeekNumber
        {
            get { return mayWeekNumber; }
            set
            {
                if (mayWeekNumber != value)
                {
                    mayWeekNumber = value;
                    OnPropertyChanged("MayWeekNumber");
                }
            }
        }

        [DataMember]
        public string MaySaturday
        {
            get { return maySaturday; }
            set
            {
                if (maySaturday != value)
                {
                    maySaturday = value;
                    OnPropertyChanged("MaySaturday");
                }
            }
        }

        [DataMember]
        public string MaySunday
        {
            get { return maySunday; }
            set
            {
                if (maySunday != value)
                {
                    maySunday = value;
                    OnPropertyChanged("MaySunday");
                }
            }
        }

        [DataMember]
        public DayOfWeek? JuneDayOfWeek
        {
            get { return juneDayOfWeek; }
            set
            {
                if (juneDayOfWeek != value)
                {
                    juneDayOfWeek = value;
                    OnPropertyChanged("JuneDayOfWeek");
                }
            }
        }

        [DataMember]
        public int? JuneWeekNumber
        {
            get { return juneWeekNumber; }
            set
            {
                if (juneWeekNumber != value)
                {
                    juneWeekNumber = value;
                    OnPropertyChanged("JuneWeekNumber");
                }
            }
        }

        [DataMember]
        public string JuneSaturday
        {
            get { return juneSaturday; }
            set
            {
                if (juneSaturday != value)
                {
                    juneSaturday = value;
                    OnPropertyChanged("JuneSaturday");
                }
            }
        }

        [DataMember]
        public string JuneSunday
        {
            get { return juneSunday; }
            set
            {
                if (juneSunday != value)
                {
                    juneSunday = value;
                    OnPropertyChanged("JuneSunday");
                }
            }
        }

        [DataMember]
        public DayOfWeek? JulyDayOfWeek
        {
            get { return julyDayOfWeek; }
            set
            {
                if (julyDayOfWeek != value)
                {
                    julyDayOfWeek = value;
                    OnPropertyChanged("JulyDayOfWeek");
                }
            }
        }

        [DataMember]
        public int? JulyWeekNumber
        {
            get { return julyWeekNumber; }
            set
            {
                if (julyWeekNumber != value)
                {
                    julyWeekNumber = value;
                    OnPropertyChanged("JulyWeekNumber");
                }
            }
        }

        [DataMember]
        public string JulySaturday
        {
            get { return julySaturday; }
            set
            {
                if (julySaturday != value)
                {
                    julySaturday = value;
                    OnPropertyChanged("JulySaturday");
                }
            }
        }

        [DataMember]
        public string JulySunday
        {
            get { return julySunday; }
            set
            {
                if (julySunday != value)
                {
                    julySunday = value;
                    OnPropertyChanged("JulySunday");
                }
            }
        }

        [DataMember]
        public DayOfWeek? AugustDayOfWeek
        {
            get { return augustDayOfWeek; }
            set
            {
                if (augustDayOfWeek != value)
                {
                    augustDayOfWeek = value;
                    OnPropertyChanged("AugustDayOfWeek");
                }
            }
        }

        [DataMember]
        public int? AugustWeekNumber
        {
            get { return augustWeekNumber; }
            set
            {
                if (augustWeekNumber != value)
                {
                    augustWeekNumber = value;
                    OnPropertyChanged("AugustWeekNumber");
                }
            }
        }

        [DataMember]
        public string AugustSaturday
        {
            get { return augustSaturday; }
            set
            {
                if (augustSaturday != value)
                {
                    augustSaturday = value;
                    OnPropertyChanged("AugustSaturday");
                }
            }
        }

        [DataMember]
        public string AugustSunday
        {
            get { return augustSunday; }
            set
            {
                if (augustSunday != value)
                {
                    augustSunday = value;
                    OnPropertyChanged("AugustSunday");
                }
            }
        }

        [DataMember]
        public DayOfWeek? SeptemberDayOfWeek
        {
            get { return septemberDayOfWeek; }
            set
            {
                if (septemberDayOfWeek != value)
                {
                    septemberDayOfWeek = value;
                    OnPropertyChanged("SeptemberDayOfWeek");
                }
            }
        }

        [DataMember]
        public int? SeptemberWeekNumber
        {
            get { return septemberWeekNumber; }
            set
            {
                if (septemberWeekNumber != value)
                {
                    septemberWeekNumber = value;
                    OnPropertyChanged("SeptemberWeekNumber");
                }
            }
        }

        [DataMember]
        public string SeptemberSaturday
        {
            get { return septemberSaturday; }
            set
            {
                if (septemberSaturday != value)
                {
                    septemberSaturday = value;
                    OnPropertyChanged("SeptemberSaturday");
                }
            }
        }

        [DataMember]
        public string SeptemberSunday
        {
            get { return septemberSunday; }
            set
            {
                if (septemberSunday != value)
                {
                    septemberSunday = value;
                    OnPropertyChanged("SeptemberSunday");
                }
            }
        }

        [DataMember]
        public DayOfWeek? OctoberDayOfWeek
        {
            get { return octoberDayOfWeek; }
            set
            {
                if (octoberDayOfWeek != value)
                {
                    octoberDayOfWeek = value;
                    OnPropertyChanged("OctoberDayOfWeek");
                }
            }
        }

        [DataMember]
        public int? OctoberWeekNumber
        {
            get { return octoberWeekNumber; }
            set
            {
                if (octoberWeekNumber != value)
                {
                    octoberWeekNumber = value;
                    OnPropertyChanged("OctoberWeekNumber");
                }
            }
        }

        [DataMember]
        public string OctoberSaturday
        {
            get { return octoberSaturday; }
            set
            {
                if (octoberSaturday != value)
                {
                    octoberSaturday = value;
                    OnPropertyChanged("OctoberSaturday");
                }
            }
        }

        [DataMember]
        public string OctoberSunday
        {
            get { return octoberSunday; }
            set
            {
                if (octoberSunday != value)
                {
                    octoberSunday = value;
                    OnPropertyChanged("OctoberSunday");
                }
            }
        }

        [DataMember]
        public DayOfWeek? NovemberDayOfWeek
        {
            get { return novemberDayOfWeek; }
            set
            {
                if (novemberDayOfWeek != value)
                {
                    novemberDayOfWeek = value;
                    OnPropertyChanged("NovemberDayOfWeek");
                }
            }
        }

        [DataMember]
        public int? NovemberWeekNumber
        {
            get { return novemberWeekNumber; }
            set
            {
                if (novemberWeekNumber != value)
                {
                    novemberWeekNumber = value;
                    OnPropertyChanged("NovemberWeekNumber");
                }
            }
        }

        [DataMember]
        public string NovemberSaturday
        {
            get { return novemberSaturday; }
            set
            {
                if (novemberSaturday != value)
                {
                    novemberSaturday = value;
                    OnPropertyChanged("NovemberSaturday");
                }
            }
        }

        [DataMember]
        public string NovemberSunday
        {
            get { return novemberSunday; }
            set
            {
                if (novemberSunday != value)
                {
                    novemberSunday = value;
                    OnPropertyChanged("NovemberSunday");
                }
            }
        }

        [DataMember]
        public DayOfWeek? DecemberDayOfWeek
        {
            get { return decemberDayOfWeek; }
            set
            {
                if (decemberDayOfWeek != value)
                {
                    decemberDayOfWeek = value;
                    OnPropertyChanged("DecemberDayOfWeek");
                }
            }
        }

        [DataMember]
        public int? DecemberWeekNumber
        {
            get { return decemberWeekNumber; }
            set
            {
                if (decemberWeekNumber != value)
                {
                    decemberWeekNumber = value;
                    OnPropertyChanged("DecemberWeekNumber");
                }
            }
        }

        [DataMember]
        public string DecemberSaturday
        {
            get { return decemberSaturday; }
            set
            {
                if (decemberSaturday != value)
                {
                    decemberSaturday = value;
                    OnPropertyChanged("DecemberSaturday");
                }
            }
        }

        [DataMember]
        public string DecemberSunday
        {
            get { return decemberSunday; }
            set
            {
                if (decemberSunday != value)
                {
                    decemberSunday = value;
                    OnPropertyChanged("DecemberSunday");
                }
            }
        }

        private string januaryWeekend;
        private string februaryWeekend;
        private string marchWeekend;
        private string aprilWeekend;
        private string mayWeekend;
        private string juneWeekend;
        private string julyWeekend;
        private string augustWeekend;
        private string septemberWeekend;
        private string octoberWeekend;
        private string novemberWeekend;
        private string decemberWeekend;

        [DataMember]
        public string JanuaryWeekend
        {
            get { return januaryWeekend; }
            set
            {
                if (januaryWeekend != value)
                {
                    januaryWeekend = value;
                    OnPropertyChanged("JanuaryWeekend");
                }
            }
        }

        [DataMember]
        public string FebruaryWeekend
        {
            get { return februaryWeekend; }
            set
            {
                if (februaryWeekend != value)
                {
                    februaryWeekend = value;
                    OnPropertyChanged("FebruaryWeekend");
                }
            }
        }

        [DataMember]
        public string MarchWeekend
        {
            get { return marchWeekend; }
            set
            {
                if (marchWeekend != value)
                {
                    marchWeekend = value;
                    OnPropertyChanged("MarchWeekend");
                }
            }
        }

        [DataMember]
        public string AprilWeekend
        {
            get { return aprilWeekend; }
            set
            {
                if (aprilWeekend != value)
                {
                    aprilWeekend = value;
                    OnPropertyChanged("AprilWeekend");
                }
            }
        }

        [DataMember]
        public string MayWeekend
        {
            get { return mayWeekend; }
            set
            {
                if (mayWeekend != value)
                {
                    mayWeekend = value;
                    OnPropertyChanged("MayWeekend");
                }
            }
        }

        [DataMember]
        public string JuneWeekend
        {
            get { return juneWeekend; }
            set
            {
                if (juneWeekend != value)
                {
                    juneWeekend = value;
                    OnPropertyChanged("JuneWeekend");
                }
            }
        }

        [DataMember]
        public string JulyWeekend
        {
            get { return julyWeekend; }
            set
            {
                if (julyWeekend != value)
                {
                    julyWeekend = value;
                    OnPropertyChanged("JulyWeekend");
                }
            }
        }

        [DataMember]
        public string AugustWeekend
        {
            get { return augustWeekend; }
            set
            {
                if (augustWeekend != value)
                {
                    augustWeekend = value;
                    OnPropertyChanged("AugustWeekend");
                }
            }
        }

        [DataMember]
        public string SeptemberWeekend
        {
            get { return septemberWeekend; }
            set
            {
                if (septemberWeekend != value)
                {
                    septemberWeekend = value;
                    OnPropertyChanged("SeptemberWeekend");
                }
            }
        }

        [DataMember]
        public string OctoberWeekend
        {
            get { return octoberWeekend; }
            set
            {
                if (octoberWeekend != value)
                {
                    octoberWeekend = value;
                    OnPropertyChanged("OctoberWeekend");
                }
            }
        }

        [DataMember]
        public string NovemberWeekend
        {
            get { return novemberWeekend; }
            set
            {
                if (novemberWeekend != value)
                {
                    novemberWeekend = value;
                    OnPropertyChanged("NovemberWeekend");
                }
            }
        }

        [DataMember]
        public string DecemberWeekend
        {
            get { return decemberWeekend; }
            set
            {
                if (decemberWeekend != value)
                {
                    decemberWeekend = value;
                    OnPropertyChanged("DecemberWeekend");
                }
            }
        }

        // Private fields for each month
        private System.Windows.Media.SolidColorBrush januaryButtonBackgroundColor;
        private System.Windows.Media.Brush januaryButtonForegroundColor;
        private System.Windows.FontWeight januaryFontWeight;

        private System.Windows.Media.SolidColorBrush februaryButtonBackgroundColor;
        private System.Windows.Media.Brush februaryButtonForegroundColor;
        private System.Windows.FontWeight februaryFontWeight;

        private System.Windows.Media.SolidColorBrush marchButtonBackgroundColor;
        private System.Windows.Media.Brush marchButtonForegroundColor;
        private System.Windows.FontWeight marchFontWeight;

        private System.Windows.Media.SolidColorBrush aprilButtonBackgroundColor;
        private System.Windows.Media.Brush aprilButtonForegroundColor;
        private System.Windows.FontWeight aprilFontWeight;

        private System.Windows.Media.SolidColorBrush mayButtonBackgroundColor;
        private System.Windows.Media.Brush mayButtonForegroundColor;
        private System.Windows.FontWeight mayFontWeight;

        private System.Windows.Media.SolidColorBrush juneButtonBackgroundColor;
        private System.Windows.Media.Brush juneButtonForegroundColor;
        private System.Windows.FontWeight juneFontWeight;

        private System.Windows.Media.SolidColorBrush julyButtonBackgroundColor;
        private System.Windows.Media.Brush julyButtonForegroundColor;
        private System.Windows.FontWeight julyFontWeight;

        private System.Windows.Media.SolidColorBrush augustButtonBackgroundColor;
        private System.Windows.Media.Brush augustButtonForegroundColor;
        private System.Windows.FontWeight augustFontWeight;

        private System.Windows.Media.SolidColorBrush septemberButtonBackgroundColor;
        private System.Windows.Media.Brush septemberButtonForegroundColor;
        private System.Windows.FontWeight septemberFontWeight;

        private System.Windows.Media.SolidColorBrush octoberButtonBackgroundColor;
        private System.Windows.Media.Brush octoberButtonForegroundColor;
        private System.Windows.FontWeight octoberFontWeight;

        private System.Windows.Media.SolidColorBrush novemberButtonBackgroundColor;
        private System.Windows.Media.Brush novemberButtonForegroundColor;
        private System.Windows.FontWeight novemberFontWeight;

        private System.Windows.Media.SolidColorBrush decemberButtonBackgroundColor;
        private System.Windows.Media.Brush decemberButtonForegroundColor;
        private System.Windows.FontWeight decemberFontWeight;

        // January Properties
        [DataMember]
        public System.Windows.Media.SolidColorBrush JanuaryButtonBackgroundColor
        {
            get { return januaryButtonBackgroundColor; }
            set
            {
                if (januaryButtonBackgroundColor != value)
                {
                    januaryButtonBackgroundColor = value;
                    OnPropertyChanged("JanuaryButtonBackgroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.Brush JanuaryButtonForegroundColor
        {
            get { return januaryButtonForegroundColor; }
            set
            {
                if (januaryButtonForegroundColor != value)
                {
                    januaryButtonForegroundColor = value;
                    OnPropertyChanged("JanuaryButtonForegroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.FontWeight JanuaryFontWeight
        {
            get { return januaryFontWeight; }
            set
            {
                if (januaryFontWeight != value)
                {
                    januaryFontWeight = value;
                    OnPropertyChanged("JanuaryFontWeight");
                }
            }
        }

        // February Properties
        [DataMember]
        public System.Windows.Media.SolidColorBrush FebruaryButtonBackgroundColor
        {
            get { return februaryButtonBackgroundColor; }
            set
            {
                if (februaryButtonBackgroundColor != value)
                {
                    februaryButtonBackgroundColor = value;
                    OnPropertyChanged("FebruaryButtonBackgroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.Brush FebruaryButtonForegroundColor
        {
            get { return februaryButtonForegroundColor; }
            set
            {
                if (februaryButtonForegroundColor != value)
                {
                    februaryButtonForegroundColor = value;
                    OnPropertyChanged("FebruaryButtonForegroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.FontWeight FebruaryFontWeight
        {
            get { return februaryFontWeight; }
            set
            {
                if (februaryFontWeight != value)
                {
                    februaryFontWeight = value;
                    OnPropertyChanged("FebruaryFontWeight");
                }
            }
        }

        // March Properties
        [DataMember]
        public System.Windows.Media.SolidColorBrush MarchButtonBackgroundColor
        {
            get { return marchButtonBackgroundColor; }
            set
            {
                if (marchButtonBackgroundColor != value)
                {
                    marchButtonBackgroundColor = value;
                    OnPropertyChanged("MarchButtonBackgroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.Brush MarchButtonForegroundColor
        {
            get { return marchButtonForegroundColor; }
            set
            {
                if (marchButtonForegroundColor != value)
                {
                    marchButtonForegroundColor = value;
                    OnPropertyChanged("MarchButtonForegroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.FontWeight MarchFontWeight
        {
            get { return marchFontWeight; }
            set
            {
                if (marchFontWeight != value)
                {
                    marchFontWeight = value;
                    OnPropertyChanged("MarchFontWeight");
                }
            }
        }

        // April Properties
        [DataMember]
        public System.Windows.Media.SolidColorBrush AprilButtonBackgroundColor
        {
            get { return aprilButtonBackgroundColor; }
            set
            {
                if (aprilButtonBackgroundColor != value)
                {
                    aprilButtonBackgroundColor = value;
                    OnPropertyChanged("AprilButtonBackgroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.Brush AprilButtonForegroundColor
        {
            get { return aprilButtonForegroundColor; }
            set
            {
                if (aprilButtonForegroundColor != value)
                {
                    aprilButtonForegroundColor = value;
                    OnPropertyChanged("AprilButtonForegroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.FontWeight AprilFontWeight
        {
            get { return aprilFontWeight; }
            set
            {
                if (aprilFontWeight != value)
                {
                    aprilFontWeight = value;
                    OnPropertyChanged("AprilFontWeight");
                }
            }
        }

        // May Properties
        [DataMember]
        public System.Windows.Media.SolidColorBrush MayButtonBackgroundColor
        {
            get { return mayButtonBackgroundColor; }
            set
            {
                if (mayButtonBackgroundColor != value)
                {
                    mayButtonBackgroundColor = value;
                    OnPropertyChanged("MayButtonBackgroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.Brush MayButtonForegroundColor
        {
            get { return mayButtonForegroundColor; }
            set
            {
                if (mayButtonForegroundColor != value)
                {
                    mayButtonForegroundColor = value;
                    OnPropertyChanged("MayButtonForegroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.FontWeight MayFontWeight
        {
            get { return mayFontWeight; }
            set
            {
                if (mayFontWeight != value)
                {
                    mayFontWeight = value;
                    OnPropertyChanged("MayFontWeight");
                }
            }
        }

        // June Properties
        [DataMember]
        public System.Windows.Media.SolidColorBrush JuneButtonBackgroundColor
        {
            get { return juneButtonBackgroundColor; }
            set
            {
                if (juneButtonBackgroundColor != value)
                {
                    juneButtonBackgroundColor = value;
                    OnPropertyChanged("JuneButtonBackgroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.Brush JuneButtonForegroundColor
        {
            get { return juneButtonForegroundColor; }
            set
            {
                if (juneButtonForegroundColor != value)
                {
                    juneButtonForegroundColor = value;
                    OnPropertyChanged("JuneButtonForegroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.FontWeight JuneFontWeight
        {
            get { return juneFontWeight; }
            set
            {
                if (juneFontWeight != value)
                {
                    juneFontWeight = value;
                    OnPropertyChanged("JuneFontWeight");
                }
            }
        }

        // July Properties
        [DataMember]
        public System.Windows.Media.SolidColorBrush JulyButtonBackgroundColor
        {
            get { return julyButtonBackgroundColor; }
            set
            {
                if (julyButtonBackgroundColor != value)
                {
                    julyButtonBackgroundColor = value;
                    OnPropertyChanged("JulyButtonBackgroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.Brush JulyButtonForegroundColor
        {
            get { return julyButtonForegroundColor; }
            set
            {
                if (julyButtonForegroundColor != value)
                {
                    julyButtonForegroundColor = value;
                    OnPropertyChanged("JulyButtonForegroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.FontWeight JulyFontWeight
        {
            get { return julyFontWeight; }
            set
            {
                if (julyFontWeight != value)
                {
                    julyFontWeight = value;
                    OnPropertyChanged("JulyFontWeight");
                }
            }
        }

        // August Properties
        [DataMember]
        public System.Windows.Media.SolidColorBrush AugustButtonBackgroundColor
        {
            get { return augustButtonBackgroundColor; }
            set
            {
                if (augustButtonBackgroundColor != value)
                {
                    augustButtonBackgroundColor = value;
                    OnPropertyChanged("AugustButtonBackgroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.Brush AugustButtonForegroundColor
        {
            get { return augustButtonForegroundColor; }
            set
            {
                if (augustButtonForegroundColor != value)
                {
                    augustButtonForegroundColor = value;
                    OnPropertyChanged("AugustButtonForegroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.FontWeight AugustFontWeight
        {
            get { return augustFontWeight; }
            set
            {
                if (augustFontWeight != value)
                {
                    augustFontWeight = value;
                    OnPropertyChanged("AugustFontWeight");
                }
            }
        }

        // September Properties
        [DataMember]
        public System.Windows.Media.SolidColorBrush SeptemberButtonBackgroundColor
        {
            get { return septemberButtonBackgroundColor; }
            set
            {
                if (septemberButtonBackgroundColor != value)
                {
                    septemberButtonBackgroundColor = value;
                    OnPropertyChanged("SeptemberButtonBackgroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.Brush SeptemberButtonForegroundColor
        {
            get { return septemberButtonForegroundColor; }
            set
            {
                if (septemberButtonForegroundColor != value)
                {
                    septemberButtonForegroundColor = value;
                    OnPropertyChanged("SeptemberButtonForegroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.FontWeight SeptemberFontWeight
        {
            get { return septemberFontWeight; }
            set
            {
                if (septemberFontWeight != value)
                {
                    septemberFontWeight = value;
                    OnPropertyChanged("SeptemberFontWeight");
                }
            }
        }

        // October Properties
        [DataMember]
        public System.Windows.Media.SolidColorBrush OctoberButtonBackgroundColor
        {
            get { return octoberButtonBackgroundColor; }
            set
            {
                if (octoberButtonBackgroundColor != value)
                {
                    octoberButtonBackgroundColor = value;
                    OnPropertyChanged("OctoberButtonBackgroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.Brush OctoberButtonForegroundColor
        {
            get { return octoberButtonForegroundColor; }
            set
            {
                if (octoberButtonForegroundColor != value)
                {
                    octoberButtonForegroundColor = value;
                    OnPropertyChanged("OctoberButtonForegroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.FontWeight OctoberFontWeight
        {
            get { return octoberFontWeight; }
            set
            {
                if (octoberFontWeight != value)
                {
                    octoberFontWeight = value;
                    OnPropertyChanged("OctoberFontWeight");
                }
            }
        }

        // November Properties
        [DataMember]
        public System.Windows.Media.SolidColorBrush NovemberButtonBackgroundColor
        {
            get { return novemberButtonBackgroundColor; }
            set
            {
                if (novemberButtonBackgroundColor != value)
                {
                    novemberButtonBackgroundColor = value;
                    OnPropertyChanged("NovemberButtonBackgroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.Brush NovemberButtonForegroundColor
        {
            get { return novemberButtonForegroundColor; }
            set
            {
                if (novemberButtonForegroundColor != value)
                {
                    novemberButtonForegroundColor = value;
                    OnPropertyChanged("NovemberButtonForegroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.FontWeight NovemberFontWeight
        {
            get { return novemberFontWeight; }
            set
            {
                if (novemberFontWeight != value)
                {
                    novemberFontWeight = value;
                    OnPropertyChanged("NovemberFontWeight");
                }
            }
        }

        // December Properties
        [DataMember]
        public System.Windows.Media.SolidColorBrush DecemberButtonBackgroundColor
        {
            get { return decemberButtonBackgroundColor; }
            set
            {
                if (decemberButtonBackgroundColor != value)
                {
                    decemberButtonBackgroundColor = value;
                    OnPropertyChanged("DecemberButtonBackgroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.Brush DecemberButtonForegroundColor
        {
            get { return decemberButtonForegroundColor; }
            set
            {
                if (decemberButtonForegroundColor != value)
                {
                    decemberButtonForegroundColor = value;
                    OnPropertyChanged("DecemberButtonForegroundColor");
                }
            }
        }

        [DataMember]
        public System.Windows.FontWeight DecemberFontWeight
        {
            get { return decemberFontWeight; }
            set
            {
                if (decemberFontWeight != value)
                {
                    decemberFontWeight = value;
                    OnPropertyChanged("DecemberFontWeight");
                }
            }
        }

        [DataMember]
        public List<ScheduleEvent> JanuaryScheduleEvents
        {
            get { return januaryScheduleEvents; }
            set
            {
                januaryScheduleEvents = value;
                OnPropertyChanged("JanuaryScheduleEvents");
            }
        }

        [DataMember]
        public List<ScheduleEvent> FebruaryScheduleEvents
        {
            get { return februaryScheduleEvents; }
            set
            {
                februaryScheduleEvents = value;
                OnPropertyChanged("FebruaryScheduleEvents");
            }
        }

        [DataMember]
        public List<ScheduleEvent> MarchScheduleEvents
        {
            get { return marchScheduleEvents; }
            set
            {
                marchScheduleEvents = value;
                OnPropertyChanged("MarchScheduleEvents");
            }
        }

        [DataMember]
        public List<ScheduleEvent> AprilScheduleEvents
        {
            get { return aprilScheduleEvents; }
            set
            {
                aprilScheduleEvents = value;
                OnPropertyChanged("AprilScheduleEvents");
            }
        }

        [DataMember]
        public List<ScheduleEvent> MayScheduleEvents
        {
            get { return mayScheduleEvents; }
            set
            {
                mayScheduleEvents = value;
                OnPropertyChanged("MayScheduleEvents");
            }
        }

        [DataMember]
        public List<ScheduleEvent> JuneScheduleEvents
        {
            get { return juneScheduleEvents; }
            set
            {
                juneScheduleEvents = value;
                OnPropertyChanged("JuneScheduleEvents");
            }
        }

        [DataMember]
        public List<ScheduleEvent> JulyScheduleEvents
        {
            get { return julyScheduleEvents; }
            set
            {
                julyScheduleEvents = value;
                OnPropertyChanged("JulyScheduleEvents");
            }
        }

        [DataMember]
        public List<ScheduleEvent> AugustScheduleEvents
        {
            get { return augustScheduleEvents; }
            set
            {
                augustScheduleEvents = value;
                OnPropertyChanged("AugustScheduleEvents");
            }
        }

        [DataMember]
        public List<ScheduleEvent> SeptemberScheduleEvents
        {
            get { return septemberScheduleEvents; }
            set
            {
                septemberScheduleEvents = value;
                OnPropertyChanged("SeptemberScheduleEvents");
            }
        }

        [DataMember]
        public List<ScheduleEvent> OctoberScheduleEvents
        {
            get { return octoberScheduleEvents; }
            set
            {
                octoberScheduleEvents = value;
                OnPropertyChanged("OctoberScheduleEvents");
            }
        }

        [DataMember]
        public List<ScheduleEvent> NovemberScheduleEvents
        {
            get { return novemberScheduleEvents; }
            set
            {
                novemberScheduleEvents = value;
                OnPropertyChanged("NovemberScheduleEvents");
            }
        }

        [DataMember]
        public List<ScheduleEvent> DecemberScheduleEvents
        {
            get { return decemberScheduleEvents; }
            set
            {
                decemberScheduleEvents = value;
                OnPropertyChanged("DecemberScheduleEvents");
            }
        }
		//[nsatpute][16.10.2025][GEOS2-8799]

        [DataMember]
        public Visibility JanuaryCircleVisibility
        {
            get { return januaryCircleVisibility; }
            set
            {
                januaryCircleVisibility = value;
                OnPropertyChanged("JanuaryCircleVisibility");

            }
        }

        [DataMember]
        public Visibility FebruaryCircleVisibility
        {
            get { return februaryCircleVisibility; }
            set
            {
                februaryCircleVisibility = value;
                OnPropertyChanged("FebruaryCircleVisibility");
            }
        }

        [DataMember]
        public Visibility MarchCircleVisibility
        {
            get { return marchCircleVisibility; }
            set
            {
                marchCircleVisibility = value;
                OnPropertyChanged("MarchCircleVisibility");
            }
        }

        [DataMember]
        public Visibility AprilCircleVisibility
        {
            get { return aprilCircleVisibility; }
            set
            {
                aprilCircleVisibility = value;
                OnPropertyChanged("AprilCircleVisibility");
            }
        }

        [DataMember]
        public Visibility MayCircleVisibility
        {
            get { return mayCircleVisibility; }
            set
            {
                mayCircleVisibility = value;
                OnPropertyChanged("MayCircleVisibility");
            }
        }

        [DataMember]
        public Visibility JuneCircleVisibility
        {
            get { return juneCircleVisibility; }
            set
            {
                juneCircleVisibility = value;
                OnPropertyChanged("JuneCircleVisibility");
            }
        }

        [DataMember]
        public Visibility JulyCircleVisibility
        {
            get { return julyCircleVisibility; }
            set
            {
                julyCircleVisibility = value;
                OnPropertyChanged("JulyCircleVisibility");
            }
        }

        [DataMember]
        public Visibility AugustCircleVisibility
        {
            get { return augustCircleVisibility; }
            set
            {
                augustCircleVisibility = value;
                OnPropertyChanged("AugustCircleVisibility");
            }
        }

        [DataMember]
        public Visibility SeptemberCircleVisibility
        {
            get { return septemberCircleVisibility; }
            set
            {
                septemberCircleVisibility = value;
                OnPropertyChanged("SeptemberCircleVisibility");
            }
        }

        [DataMember]
        public Visibility OctoberCircleVisibility
        {
            get { return octoberCircleVisibility; }
            set
            {
                octoberCircleVisibility = value;
                OnPropertyChanged("OctoberCircleVisibility");
            }
        }

        [DataMember]
        public Visibility NovemberCircleVisibility
        {
            get { return novemberCircleVisibility; }
            set
            {
                novemberCircleVisibility = value;
                OnPropertyChanged("NovemberCircleVisibility");
            }
        }

        [DataMember]
        public Visibility DecemberCircleVisibility
        {
            get { return decemberCircleVisibility; }
            set
            {
                decemberCircleVisibility = value;
                OnPropertyChanged("DecemberCircleVisibility");
            }
        }


        private Visibility januaryFirstEllipseVisibility;
        private Visibility januarySecondEllipseVisibility;
        private string januaryFirstEllipse;
        private string januarySecondEllipse;
        private System.Windows.Media.SolidColorBrush januaryFirstEllipseHtmlColor;
        private System.Windows.Media.SolidColorBrush januarySecondEllipseHtmlColor;

        private Visibility februaryFirstEllipseVisibility;
        private Visibility februarySecondEllipseVisibility;
        private string februaryFirstEllipse;
        private string februarySecondEllipse;
        private System.Windows.Media.SolidColorBrush februaryFirstEllipseHtmlColor;
        private System.Windows.Media.SolidColorBrush februarySecondEllipseHtmlColor;

        private Visibility marchFirstEllipseVisibility;
        private Visibility marchSecondEllipseVisibility;
        private string marchFirstEllipse;
        private string marchSecondEllipse;
        private System.Windows.Media.SolidColorBrush marchFirstEllipseHtmlColor;
        private System.Windows.Media.SolidColorBrush marchSecondEllipseHtmlColor;

        private Visibility aprilFirstEllipseVisibility;
        private Visibility aprilSecondEllipseVisibility;
        private string aprilFirstEllipse;
        private string aprilSecondEllipse;
        private System.Windows.Media.SolidColorBrush aprilFirstEllipseHtmlColor;
        private System.Windows.Media.SolidColorBrush aprilSecondEllipseHtmlColor;

        private Visibility mayFirstEllipseVisibility;
        private Visibility maySecondEllipseVisibility;
        private string mayFirstEllipse;
        private string maySecondEllipse;
        private System.Windows.Media.SolidColorBrush mayFirstEllipseHtmlColor;
        private System.Windows.Media.SolidColorBrush maySecondEllipseHtmlColor;

        private Visibility juneFirstEllipseVisibility;
        private Visibility juneSecondEllipseVisibility;
        private string juneFirstEllipse;
        private string juneSecondEllipse;
        private System.Windows.Media.SolidColorBrush juneFirstEllipseHtmlColor;
        private System.Windows.Media.SolidColorBrush juneSecondEllipseHtmlColor;

        private Visibility julyFirstEllipseVisibility;
        private Visibility julySecondEllipseVisibility;
        private string julyFirstEllipse;
        private string julySecondEllipse;
        private System.Windows.Media.SolidColorBrush julyFirstEllipseHtmlColor;
        private System.Windows.Media.SolidColorBrush julySecondEllipseHtmlColor;

        private Visibility augustFirstEllipseVisibility;
        private Visibility augustSecondEllipseVisibility;
        private string augustFirstEllipse;
        private string augustSecondEllipse;
        private System.Windows.Media.SolidColorBrush augustFirstEllipseHtmlColor;
        private System.Windows.Media.SolidColorBrush augustSecondEllipseHtmlColor;

        private Visibility septemberFirstEllipseVisibility;
        private Visibility septemberSecondEllipseVisibility;
        private string septemberFirstEllipse;
        private string septemberSecondEllipse;
        private System.Windows.Media.SolidColorBrush septemberFirstEllipseHtmlColor;
        private System.Windows.Media.SolidColorBrush septemberSecondEllipseHtmlColor;

        private Visibility octoberFirstEllipseVisibility;
        private Visibility octoberSecondEllipseVisibility;
        private string octoberFirstEllipse;
        private string octoberSecondEllipse;
        private System.Windows.Media.SolidColorBrush octoberFirstEllipseHtmlColor;
        private System.Windows.Media.SolidColorBrush octoberSecondEllipseHtmlColor;

        private Visibility novemberFirstEllipseVisibility;
        private Visibility novemberSecondEllipseVisibility;
        private string novemberFirstEllipse;
        private string novemberSecondEllipse;
        private System.Windows.Media.SolidColorBrush novemberFirstEllipseHtmlColor;
        private System.Windows.Media.SolidColorBrush novemberSecondEllipseHtmlColor;

        private Visibility decemberFirstEllipseVisibility;
        private Visibility decemberSecondEllipseVisibility;
        private string decemberFirstEllipse;
        private string decemberSecondEllipse;
        private System.Windows.Media.SolidColorBrush decemberFirstEllipseHtmlColor;
        private System.Windows.Media.SolidColorBrush decemberSecondEllipseHtmlColor;

        // January Properties
        [DataMember]
        public Visibility JanuaryFirstEllipseVisibility
        {
            get { return januaryFirstEllipseVisibility; }
            set
            {
                januaryFirstEllipseVisibility = value;
                OnPropertyChanged("JanuaryFirstEllipseVisibility");
            }
        }

        [DataMember]
        public Visibility JanuarySecondEllipseVisibility
        {
            get { return januarySecondEllipseVisibility; }
            set
            {
                januarySecondEllipseVisibility = value;
                OnPropertyChanged("JanuarySecondEllipseVisibility");
            }
        }

        [DataMember]
        public string JanuaryFirstEllipse
        {
            get { return januaryFirstEllipse; }
            set
            {
                januaryFirstEllipse = value;
                OnPropertyChanged("JanuaryFirstEllipse");
            }
        }

        [DataMember]
        public string JanuarySecondEllipse
        {
            get { return januarySecondEllipse; }
            set
            {
                januarySecondEllipse = value;
                OnPropertyChanged("JanuarySecondEllipse");
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush JanuaryFirstEllipseHtmlColor
        {
            get { return januaryFirstEllipseHtmlColor; }
            set
            {
                if (januaryFirstEllipseHtmlColor != value)
                {
                    januaryFirstEllipseHtmlColor = value;
                    OnPropertyChanged("JanuaryFirstEllipseHtmlColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush JanuarySecondEllipseHtmlColor
        {
            get { return januarySecondEllipseHtmlColor; }
            set
            {
                if (januarySecondEllipseHtmlColor != value)
                {
                    januarySecondEllipseHtmlColor = value;
                    OnPropertyChanged("JanuarySecondEllipseHtmlColor");
                }
            }
        }

        // February Properties
        [DataMember]
        public Visibility FebruaryFirstEllipseVisibility
        {
            get { return februaryFirstEllipseVisibility; }
            set
            {
                februaryFirstEllipseVisibility = value;
                OnPropertyChanged("FebruaryFirstEllipseVisibility");
            }
        }

        [DataMember]
        public Visibility FebruarySecondEllipseVisibility
        {
            get { return februarySecondEllipseVisibility; }
            set
            {
                februarySecondEllipseVisibility = value;
                OnPropertyChanged("FebruarySecondEllipseVisibility");
            }
        }

        [DataMember]
        public string FebruaryFirstEllipse
        {
            get { return februaryFirstEllipse; }
            set
            {
                februaryFirstEllipse = value;
                OnPropertyChanged("FebruaryFirstEllipse");
            }
        }

        [DataMember]
        public string FebruarySecondEllipse
        {
            get { return februarySecondEllipse; }
            set
            {
                februarySecondEllipse = value;
                OnPropertyChanged("FebruarySecondEllipse");
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush FebruaryFirstEllipseHtmlColor
        {
            get { return februaryFirstEllipseHtmlColor; }
            set
            {
                if (februaryFirstEllipseHtmlColor != value)
                {
                    februaryFirstEllipseHtmlColor = value;
                    OnPropertyChanged("FebruaryFirstEllipseHtmlColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush FebruarySecondEllipseHtmlColor
        {
            get { return februarySecondEllipseHtmlColor; }
            set
            {
                if (februarySecondEllipseHtmlColor != value)
                {
                    februarySecondEllipseHtmlColor = value;
                    OnPropertyChanged("FebruarySecondEllipseHtmlColor");
                }
            }
        }

        // March Properties
        [DataMember]
        public Visibility MarchFirstEllipseVisibility
        {
            get { return marchFirstEllipseVisibility; }
            set
            {
                marchFirstEllipseVisibility = value;
                OnPropertyChanged("MarchFirstEllipseVisibility");
            }
        }

        [DataMember]
        public Visibility MarchSecondEllipseVisibility
        {
            get { return marchSecondEllipseVisibility; }
            set
            {
                marchSecondEllipseVisibility = value;
                OnPropertyChanged("MarchSecondEllipseVisibility");
            }
        }

        [DataMember]
        public string MarchFirstEllipse
        {
            get { return marchFirstEllipse; }
            set
            {
                marchFirstEllipse = value;
                OnPropertyChanged("MarchFirstEllipse");
            }
        }

        [DataMember]
        public string MarchSecondEllipse
        {
            get { return marchSecondEllipse; }
            set
            {
                marchSecondEllipse = value;
                OnPropertyChanged("MarchSecondEllipse");
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush MarchFirstEllipseHtmlColor
        {
            get { return marchFirstEllipseHtmlColor; }
            set
            {
                if (marchFirstEllipseHtmlColor != value)
                {
                    marchFirstEllipseHtmlColor = value;
                    OnPropertyChanged("MarchFirstEllipseHtmlColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush MarchSecondEllipseHtmlColor
        {
            get { return marchSecondEllipseHtmlColor; }
            set
            {
                if (marchSecondEllipseHtmlColor != value)
                {
                    marchSecondEllipseHtmlColor = value;
                    OnPropertyChanged("MarchSecondEllipseHtmlColor");
                }
            }
        }

        // April Properties
        [DataMember]
        public Visibility AprilFirstEllipseVisibility
        {
            get { return aprilFirstEllipseVisibility; }
            set
            {
                aprilFirstEllipseVisibility = value;
                OnPropertyChanged("AprilFirstEllipseVisibility");
            }
        }

        [DataMember]
        public Visibility AprilSecondEllipseVisibility
        {
            get { return aprilSecondEllipseVisibility; }
            set
            {
                aprilSecondEllipseVisibility = value;
                OnPropertyChanged("AprilSecondEllipseVisibility");
            }
        }

        [DataMember]
        public string AprilFirstEllipse
        {
            get { return aprilFirstEllipse; }
            set
            {
                aprilFirstEllipse = value;
                OnPropertyChanged("AprilFirstEllipse");
            }
        }

        [DataMember]
        public string AprilSecondEllipse
        {
            get { return aprilSecondEllipse; }
            set
            {
                aprilSecondEllipse = value;
                OnPropertyChanged("AprilSecondEllipse");
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush AprilFirstEllipseHtmlColor
        {
            get { return aprilFirstEllipseHtmlColor; }
            set
            {
                if (aprilFirstEllipseHtmlColor != value)
                {
                    aprilFirstEllipseHtmlColor = value;
                    OnPropertyChanged("AprilFirstEllipseHtmlColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush AprilSecondEllipseHtmlColor
        {
            get { return aprilSecondEllipseHtmlColor; }
            set
            {
                if (aprilSecondEllipseHtmlColor != value)
                {
                    aprilSecondEllipseHtmlColor = value;
                    OnPropertyChanged("AprilSecondEllipseHtmlColor");
                }
            }
        }

        // May Properties
        [DataMember]
        public Visibility MayFirstEllipseVisibility
        {
            get { return mayFirstEllipseVisibility; }
            set
            {
                mayFirstEllipseVisibility = value;
                OnPropertyChanged("MayFirstEllipseVisibility");
            }
        }

        [DataMember]
        public Visibility MaySecondEllipseVisibility
        {
            get { return maySecondEllipseVisibility; }
            set
            {
                maySecondEllipseVisibility = value;
                OnPropertyChanged("MaySecondEllipseVisibility");
            }
        }

        [DataMember]
        public string MayFirstEllipse
        {
            get { return mayFirstEllipse; }
            set
            {
                mayFirstEllipse = value;
                OnPropertyChanged("MayFirstEllipse");
            }
        }

        [DataMember]
        public string MaySecondEllipse
        {
            get { return maySecondEllipse; }
            set
            {
                maySecondEllipse = value;
                OnPropertyChanged("MaySecondEllipse");
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush MayFirstEllipseHtmlColor
        {
            get { return mayFirstEllipseHtmlColor; }
            set
            {
                if (mayFirstEllipseHtmlColor != value)
                {
                    mayFirstEllipseHtmlColor = value;
                    OnPropertyChanged("MayFirstEllipseHtmlColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush MaySecondEllipseHtmlColor
        {
            get { return maySecondEllipseHtmlColor; }
            set
            {
                if (maySecondEllipseHtmlColor != value)
                {
                    maySecondEllipseHtmlColor = value;
                    OnPropertyChanged("MaySecondEllipseHtmlColor");
                }
            }
        }

        // June Properties
        [DataMember]
        public Visibility JuneFirstEllipseVisibility
        {
            get { return juneFirstEllipseVisibility; }
            set
            {
                juneFirstEllipseVisibility = value;
                OnPropertyChanged("JuneFirstEllipseVisibility");
            }
        }

        [DataMember]
        public Visibility JuneSecondEllipseVisibility
        {
            get { return juneSecondEllipseVisibility; }
            set
            {
                juneSecondEllipseVisibility = value;
                OnPropertyChanged("JuneSecondEllipseVisibility");
            }
        }

        [DataMember]
        public string JuneFirstEllipse
        {
            get { return juneFirstEllipse; }
            set
            {
                juneFirstEllipse = value;
                OnPropertyChanged("JuneFirstEllipse");
            }
        }

        [DataMember]
        public string JuneSecondEllipse
        {
            get { return juneSecondEllipse; }
            set
            {
                juneSecondEllipse = value;
                OnPropertyChanged("JuneSecondEllipse");
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush JuneFirstEllipseHtmlColor
        {
            get { return juneFirstEllipseHtmlColor; }
            set
            {
                if (juneFirstEllipseHtmlColor != value)
                {
                    juneFirstEllipseHtmlColor = value;
                    OnPropertyChanged("JuneFirstEllipseHtmlColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush JuneSecondEllipseHtmlColor
        {
            get { return juneSecondEllipseHtmlColor; }
            set
            {
                if (juneSecondEllipseHtmlColor != value)
                {
                    juneSecondEllipseHtmlColor = value;
                    OnPropertyChanged("JuneSecondEllipseHtmlColor");
                }
            }
        }

        // July Properties
        [DataMember]
        public Visibility JulyFirstEllipseVisibility
        {
            get { return julyFirstEllipseVisibility; }
            set
            {
                julyFirstEllipseVisibility = value;
                OnPropertyChanged("JulyFirstEllipseVisibility");
            }
        }

        [DataMember]
        public Visibility JulySecondEllipseVisibility
        {
            get { return julySecondEllipseVisibility; }
            set
            {
                julySecondEllipseVisibility = value;
                OnPropertyChanged("JulySecondEllipseVisibility");
            }
        }

        [DataMember]
        public string JulyFirstEllipse
        {
            get { return julyFirstEllipse; }
            set
            {
                julyFirstEllipse = value;
                OnPropertyChanged("JulyFirstEllipse");
            }
        }

        [DataMember]
        public string JulySecondEllipse
        {
            get { return julySecondEllipse; }
            set
            {
                julySecondEllipse = value;
                OnPropertyChanged("JulySecondEllipse");
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush JulyFirstEllipseHtmlColor
        {
            get { return julyFirstEllipseHtmlColor; }
            set
            {
                if (julyFirstEllipseHtmlColor != value)
                {
                    julyFirstEllipseHtmlColor = value;
                    OnPropertyChanged("JulyFirstEllipseHtmlColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush JulySecondEllipseHtmlColor
        {
            get { return julySecondEllipseHtmlColor; }
            set
            {
                if (julySecondEllipseHtmlColor != value)
                {
                    julySecondEllipseHtmlColor = value;
                    OnPropertyChanged("JulySecondEllipseHtmlColor");
                }
            }
        }

        // August Properties
        [DataMember]
        public Visibility AugustFirstEllipseVisibility
        {
            get { return augustFirstEllipseVisibility; }
            set
            {
                augustFirstEllipseVisibility = value;
                OnPropertyChanged("AugustFirstEllipseVisibility");
            }
        }

        [DataMember]
        public Visibility AugustSecondEllipseVisibility
        {
            get { return augustSecondEllipseVisibility; }
            set
            {
                augustSecondEllipseVisibility = value;
                OnPropertyChanged("AugustSecondEllipseVisibility");
            }
        }

        [DataMember]
        public string AugustFirstEllipse
        {
            get { return augustFirstEllipse; }
            set
            {
                augustFirstEllipse = value;
                OnPropertyChanged("AugustFirstEllipse");
            }
        }

        [DataMember]
        public string AugustSecondEllipse
        {
            get { return augustSecondEllipse; }
            set
            {
                augustSecondEllipse = value;
                OnPropertyChanged("AugustSecondEllipse");
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush AugustFirstEllipseHtmlColor
        {
            get { return augustFirstEllipseHtmlColor; }
            set
            {
                if (augustFirstEllipseHtmlColor != value)
                {
                    augustFirstEllipseHtmlColor = value;
                    OnPropertyChanged("AugustFirstEllipseHtmlColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush AugustSecondEllipseHtmlColor
        {
            get { return augustSecondEllipseHtmlColor; }
            set
            {
                if (augustSecondEllipseHtmlColor != value)
                {
                    augustSecondEllipseHtmlColor = value;
                    OnPropertyChanged("AugustSecondEllipseHtmlColor");
                }
            }
        }

        // September Properties
        [DataMember]
        public Visibility SeptemberFirstEllipseVisibility
        {
            get { return septemberFirstEllipseVisibility; }
            set
            {
                septemberFirstEllipseVisibility = value;
                OnPropertyChanged("SeptemberFirstEllipseVisibility");
            }
        }

        [DataMember]
        public Visibility SeptemberSecondEllipseVisibility
        {
            get { return septemberSecondEllipseVisibility; }
            set
            {
                septemberSecondEllipseVisibility = value;
                OnPropertyChanged("SeptemberSecondEllipseVisibility");
            }
        }

        [DataMember]
        public string SeptemberFirstEllipse
        {
            get { return septemberFirstEllipse; }
            set
            {
                septemberFirstEllipse = value;
                OnPropertyChanged("SeptemberFirstEllipse");
            }
        }

        [DataMember]
        public string SeptemberSecondEllipse
        {
            get { return septemberSecondEllipse; }
            set
            {
                septemberSecondEllipse = value;
                OnPropertyChanged("SeptemberSecondEllipse");
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush SeptemberFirstEllipseHtmlColor
        {
            get { return septemberFirstEllipseHtmlColor; }
            set
            {
                if (septemberFirstEllipseHtmlColor != value)
                {
                    septemberFirstEllipseHtmlColor = value;
                    OnPropertyChanged("SeptemberFirstEllipseHtmlColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush SeptemberSecondEllipseHtmlColor
        {
            get { return septemberSecondEllipseHtmlColor; }
            set
            {
                if (septemberSecondEllipseHtmlColor != value)
                {
                    septemberSecondEllipseHtmlColor = value;
                    OnPropertyChanged("SeptemberSecondEllipseHtmlColor");
                }
            }
        }

        // October Properties
        [DataMember]
        public Visibility OctoberFirstEllipseVisibility
        {
            get { return octoberFirstEllipseVisibility; }
            set
            {
                octoberFirstEllipseVisibility = value;
                OnPropertyChanged("OctoberFirstEllipseVisibility");
            }
        }

        [DataMember]
        public Visibility OctoberSecondEllipseVisibility
        {
            get { return octoberSecondEllipseVisibility; }
            set
            {
                octoberSecondEllipseVisibility = value;
                OnPropertyChanged("OctoberSecondEllipseVisibility");
            }
        }

        [DataMember]
        public string OctoberFirstEllipse
        {
            get { return octoberFirstEllipse; }
            set
            {
                octoberFirstEllipse = value;
                OnPropertyChanged("OctoberFirstEllipse");
            }
        }

        [DataMember]
        public string OctoberSecondEllipse
        {
            get { return octoberSecondEllipse; }
            set
            {
                octoberSecondEllipse = value;
                OnPropertyChanged("OctoberSecondEllipse");
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush OctoberFirstEllipseHtmlColor
        {
            get { return octoberFirstEllipseHtmlColor; }
            set
            {
                if (octoberFirstEllipseHtmlColor != value)
                {
                    octoberFirstEllipseHtmlColor = value;
                    OnPropertyChanged("OctoberFirstEllipseHtmlColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush OctoberSecondEllipseHtmlColor
        {
            get { return octoberSecondEllipseHtmlColor; }
            set
            {
                if (octoberSecondEllipseHtmlColor != value)
                {
                    octoberSecondEllipseHtmlColor = value;
                    OnPropertyChanged("OctoberSecondEllipseHtmlColor");
                }
            }
        }

        // November Properties
        [DataMember]
        public Visibility NovemberFirstEllipseVisibility
        {
            get { return novemberFirstEllipseVisibility; }
            set
            {
                novemberFirstEllipseVisibility = value;
                OnPropertyChanged("NovemberFirstEllipseVisibility");
            }
        }

        [DataMember]
        public Visibility NovemberSecondEllipseVisibility
        {
            get { return novemberSecondEllipseVisibility; }
            set
            {
                novemberSecondEllipseVisibility = value;
                OnPropertyChanged("NovemberSecondEllipseVisibility");
            }
        }

        [DataMember]
        public string NovemberFirstEllipse
        {
            get { return novemberFirstEllipse; }
            set
            {
                novemberFirstEllipse = value;
                OnPropertyChanged("NovemberFirstEllipse");
            }
        }

        [DataMember]
        public string NovemberSecondEllipse
        {
            get { return novemberSecondEllipse; }
            set
            {
                novemberSecondEllipse = value;
                OnPropertyChanged("NovemberSecondEllipse");
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush NovemberFirstEllipseHtmlColor
        {
            get { return novemberFirstEllipseHtmlColor; }
            set
            {
                if (novemberFirstEllipseHtmlColor != value)
                {
                    novemberFirstEllipseHtmlColor = value;
                    OnPropertyChanged("NovemberFirstEllipseHtmlColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush NovemberSecondEllipseHtmlColor
        {
            get { return novemberSecondEllipseHtmlColor; }
            set
            {
                if (novemberSecondEllipseHtmlColor != value)
                {
                    novemberSecondEllipseHtmlColor = value;
                    OnPropertyChanged("NovemberSecondEllipseHtmlColor");
                }
            }
        }

        // December Properties
        [DataMember]
        public Visibility DecemberFirstEllipseVisibility
        {
            get { return decemberFirstEllipseVisibility; }
            set
            {
                decemberFirstEllipseVisibility = value;
                OnPropertyChanged("DecemberFirstEllipseVisibility");
            }
        }

        [DataMember]
        public Visibility DecemberSecondEllipseVisibility
        {
            get { return decemberSecondEllipseVisibility; }
            set
            {
                decemberSecondEllipseVisibility = value;
                OnPropertyChanged("DecemberSecondEllipseVisibility");
            }
        }

        [DataMember]
        public string DecemberFirstEllipse
        {
            get { return decemberFirstEllipse; }
            set
            {
                decemberFirstEllipse = value;
                OnPropertyChanged("DecemberFirstEllipse");
            }
        }

        [DataMember]
        public string DecemberSecondEllipse
        {
            get { return decemberSecondEllipse; }
            set
            {
                decemberSecondEllipse = value;
                OnPropertyChanged("DecemberSecondEllipse");
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush DecemberFirstEllipseHtmlColor
        {
            get { return decemberFirstEllipseHtmlColor; }
            set
            {
                if (decemberFirstEllipseHtmlColor != value)
                {
                    decemberFirstEllipseHtmlColor = value;
                    OnPropertyChanged("DecemberFirstEllipseHtmlColor");
                }
            }
        }

        [DataMember]
        public System.Windows.Media.SolidColorBrush DecemberSecondEllipseHtmlColor
        {
            get { return decemberSecondEllipseHtmlColor; }
            set
            {
                if (decemberSecondEllipseHtmlColor != value)
                {
                    decemberSecondEllipseHtmlColor = value;
                    OnPropertyChanged("DecemberSecondEllipseHtmlColor");
                }
            }
        }

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }

}
