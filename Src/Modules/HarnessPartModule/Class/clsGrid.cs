using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    public class clsGrid
    {
        private string reference;

        public string Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        private string referenceS;

        public string ReferenceS
        {
            get { return referenceS; }
            set { referenceS = value; }
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
        private string partner;

        public string Partner
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

        private string damaged;

        public string Damaged
        {
            get { return damaged; }
            set { damaged = value; }
        }

        private string withWires;

        public string WithWires
        {
            get { return withWires; }
            set { withWires = value; }
        }

        private string withoutWires;

        public string WithoutWires
        {
            get { return withoutWires; }
            set { withoutWires = value; }
        }

        private string inHarness;

        public string InHarness
        {
            get { return inHarness; }
            set { inHarness = value; }
        }
        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        //public bool Isduplication
        //{
        //    get { return isduplication; }
        //    set { isduplication = value; }
        //}
        public clsGrid(int Reference, int cavities, string type, string color, string gender, string saled, string mylocation, string partner, int Dimendions, ImageSource ConImage, bool Isduplication)
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



        }

        public clsGrid()
        {
            // TODO: Complete member initialization
        }
    }

}
