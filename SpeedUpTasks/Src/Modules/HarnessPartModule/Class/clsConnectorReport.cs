using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
   public  class clsConnectorReport
    {

        private int reference;

        public int Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        

        private int cavities;

        public int Cavities
        {
            get { return cavities; }
            set { cavities = value; }
        }
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private string color;

        public string Color
        {
            get { return color; }
            set { color = value; }
        }
        private string gender;

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        private string saled;

        public string Saled
        {
            get { return saled; }
            set { saled = value; }
        }
       
        private string mylocation;

        public string Mylocation
        {
            get { return mylocation; }
            set { mylocation = value; }
        }
        private int partner;

        public int Partner
        {
            get { return partner; }
            set { partner = value; }
        }
        private int Dimendions;

        public int Dimendions1
        {
            get { return Dimendions; }
            set { Dimendions = value; }
        }
        private ImageSource ConImage;

        public ImageSource ConImage1
        {
            get { return ConImage; }
            set { ConImage = value; }
        }
        private bool isduplication;

        public bool Isduplication
        {
            get { return isduplication; }
            set { isduplication = value; }
        }
        private Image img;

        public Image Img
        {
            get { return img; }
            set { img = value; }
        }

        private string hyperlink;

        public string Hyperlink
        {
            get { return hyperlink; }
            set { hyperlink = value; }
        }
        private string connector;

        public string Connector
        {
            get { return connector; }
            set { connector = value; }
        }
        private string refclient;

        public string Refclient
        {
            get { return refclient; }
            set { refclient = value; }
        }

        double _internaldiameter;

        public double Internaldiameter
        {
            get { return _internaldiameter; }
            set { _internaldiameter = value; }
        }
        double _Externaldiameter;

        public double Externaldiameter
        {
            get { return _Externaldiameter; }
            set { _Externaldiameter = value; }
        }
      public  double _thickness;

        public double Thickness
        {
            get { return _thickness; }
            set { _thickness = value; }
        }

        public clsConnectorReport(int Reference, int cavities, string type, string color, string gender, string saled, string mylocation, int partner, int Dimendions, ImageSource ConImage, bool Isduplication, double _internaldiameter, double _Externaldiameter, double _thickness)
        {
            Reference = Reference;
            Cavities = cavities;
            Type = type;
            Color = color;
            Gender = gender;
            Saled = saled;
            Mylocation = mylocation;
            Partner = partner;
            Dimendions1 = Dimendions;
            ConImage1 = ConImage;
            Isduplication = isduplication;
            Internaldiameter = _internaldiameter;
            Externaldiameter = _Externaldiameter;
            Thickness = _thickness;



        }

        public clsConnectorReport()
        {
            // TODO: Complete member initialization
        }
    }
    
}