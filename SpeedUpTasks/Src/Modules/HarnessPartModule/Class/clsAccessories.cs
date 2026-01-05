using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    public class clsAccessories
    {
        //Name = RandomStringHelper.GetRandomString();
        static Random rnd = new Random();
        private Color color;
        private string Type1;
        private int idCompany;
        private string companyname;
        private string connector;
        private int cavities;
        private bool isShow;

        public bool IsShow
        {
            get { return isShow; }
            set { isShow = value; }
        }

        
        private List<clsCompany> _company;

        public List<clsCompany> Company
        {
            get { return _company; }
            set { _company = value; }
        }

        private List<clsharnessPartAccessoryTypes> _type;
        private List<clsColor> _color;

        internal List<clsColor> _Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public List<clsharnessPartAccessoryTypes> Type
        {
            get { return _type; }
            set { _type = value; }
        }


        public int IdCompany
        {
            get { return idCompany; }
            set { idCompany = value; }
        }
        public Color Color
        {
            get { return color; }
            set { color = ColorProvider.Colors[rnd.Next(ColorProvider.Colors.Count)]; }
        }
        bool? _status;

        public bool? Status
        {
            get { return _status; }
            set { _status = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string reference;

        public string Reference
        {
            get { return reference; }
            set { reference = value; }
        }
        private string colorname;

        public string Colorname
        {
            get { return colorname; }
            set { colorname = value; }
        }

        public string TYPE1
        {
            get { return Type1; }
            set { Type1 = value; }
        }
        public string Companyname
        {
            get { return companyname; }
            set { companyname = value; }
        }

        public int Cavities
        {
            get { return cavities; }
            set { cavities = value; }
        }

        public string Connector
        {
            get { return connector; }
            set { connector = value; }
        }

    }

    public class RandomStringHelper
    {
        static Random rnd = new Random();

        public static string GetRandomString(int min = 6, int max = 20)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append((char)rnd.Next(0x41, 0x5A));

            int length = rnd.Next(min, max);
            for (int i = 0; i < length - 1; i++)
                strb.Append((char)rnd.Next(0x61, 0x7A));

            return strb.ToString();
        }
    }
    public class ColorProvider
    {
        public static List<Color> Colors { get; private set; }
        public static ColorConverter Converter { get; private set; }

        static ColorProvider()
        {
            Converter = new ColorConverter();
            Colors = new List<Color>();
            var colors = typeof(Colors).GetProperties();
            foreach (var color in colors)
                Colors.Add((Color)color.GetValue(null, null));
        }
    }
    
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is Color)) return null;
            //Colorname=(Color)value).Name;
            return typeof(Colors).GetProperties().FirstOrDefault(p => (Color)p.GetValue(null, null) == (Color)value).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
