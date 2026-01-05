using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.Emdep.Geos.Data.Common
{
    public class Leads
    {
        private string code;
        private string country;
        private string group;
        private string plant;
        private string project;
        private string description;
        private string status;
        private string amount;
        private string currency;
        private float confidenceLevel;
        private string month;
        private string year;
        private string datetimeoffer;
        private string pOdate;
        private string testboard;
        private string pneumatic;
        private string vision;
        private string electricControl;
        private string assembly;
        private string highvoltage;
        private string gIT; private string tightening;
        private string wireless;
        private Brush color;

        public Brush Color
        {
            get { return color; }
            set { color = value; }
        }

        public string POdate
        {
            get { return pOdate; }
            set { pOdate = value; }
        }


        public string Testboard
        {
            get { return testboard; }
            set { testboard = value; }
        }


        public string Pneumatic
        {
            get { return pneumatic; }
            set { pneumatic = value; }
        }


        public string Vision
        {
            get { return vision; }
            set { vision = value; }
        }


        public string ElectricControl
        {
            get { return electricControl; }
            set { electricControl = value; }
        }


        public string Assembly
        {
            get { return assembly; }
            set { assembly = value; }
        }

        public string Highvoltage
        {
            get { return highvoltage; }
            set { highvoltage = value; }
        }


        public string GIT
        {
            get { return gIT; }
            set { gIT = value; }
        }

        public string Tightening
        {
            get { return tightening; }
            set { tightening = value; }
        }


        public string Wireless
        {
            get { return wireless; }
            set { wireless = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        public string Group
        {
            get { return group; }
            set { group = value; }
        }
        public string Plant
        {
            get { return plant; }
            set { plant = value; }
        }
        public string Project
        {
            get { return project; }
            set { project = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }
        public float ConfidenceLevel
        {
            get { return confidenceLevel; }
            set { confidenceLevel = value; }
        }
        public string Month
        {
            get { return month; }
            set { month = value; }
        }
        public string Year
        {
            get { return year; }
            set { year = value; }
        }

        public string Datetimeoffer
        {
            get { return datetimeoffer; }
            set { datetimeoffer = value; }
        }
    }
}
