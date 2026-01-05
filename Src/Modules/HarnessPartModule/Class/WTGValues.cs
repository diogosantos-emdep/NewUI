using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
  public  class WTGValues
  {
      private static Image pic;
    //  private static stdole.StdPicture myPic;
      private static string nObject;
      private static string identLabel;

      //public static stdole.StdPicture MyPic
      //{
      //    get
      //    {
      //        return myPic;
      //    }
      //    set
      //    {
      //        myPic = value;
      //    }
      //}


      // ArrayList for all n object
      private static ArrayList objectName;


      private static Dictionary<string, PointF> objectDestination;

      private static Dictionary<string, PointF> objectOrigin;


      private static Dictionary<string, short> objectRadius;
      private static Dictionary<string, byte> objectType;
      private static Dictionary<string, Boolean> objectVisible;
      //  Way 0 or Detection 1
      private static Dictionary<string, byte> objectTypePiece;


      //Color RGB
      private static Dictionary<string, int[]> objectColor;
      private static Dictionary<string, Label> labelArrayList;
      private static Dictionary<string, PointF> labelLocation;

      private static Dictionary<string, Panel> objectArrayList;
      private static Dictionary<string, PointF> objectLocation;

      private static Dictionary<string, Panel> shapeDetails;

      /// <summary>
      /// 
      /// </summary>
      public static Dictionary<string, Panel> Shapedetails
      {
          get
          {
              return shapeDetails;
          }
          set
          {
              shapeDetails = value;
          }
      }

      /// <summary>
      /// Constructor of Class WTGValues
      /// </summary>
      public WTGValues()
      {

      }

      /// <summary>
      /// Gets or sets the WTG picture <see cref="System.Drawing.Image"/> from .wtg file.
      /// </summary>
      /// <remarks>
      /// Image variable.
      /// </remarks>
      /// <example>
      /// <code>
      /// Image img = PaintingToolsBL.WTGValues.Pic
      /// </code>
      /// </example>
      public static Image Pic
      {
          get
          {
              return pic;
          }
          set
          {
              pic = value;
          }
      }

      /// <summary>
      /// Gets or sets the count of way/detection object on WTG image.
      /// </summary>
      public static string NObject
      {
          get
          {
              return nObject;
          }
          set
          {
              nObject = value;
          }
      }

      /// <summary>
      /// Gets or sets the Ident Lable in picture
      /// </summary>
      public static string IdentLabel
      {
          get
          {
              return identLabel;
          }
          set
          {
              identLabel = value;
          }
      }



      /// <summary>
      /// Gets or sets the Label Control ArrayList of WTG image way/detection.
      /// </summary>
      public static Dictionary<string, Label> LabelArrayList
      {
          get
          {
              return labelArrayList;
          }
          set
          {
              labelArrayList = value;
          }
      }

      /// <summary>
      /// Gets or sets the Label Control Location of WTG image way/detection.
      /// </summary>
      public static Dictionary<string, PointF> LabelLocation
      {
          get
          {
              return labelLocation;
          }
          set
          {
              labelLocation = value;
          }
      }

      /// <summary>
      /// Gets or sets the name of way/detection for WTG images.
      /// </summary>
      public static ArrayList ObjectName
      {
          get
          {
              return objectName;
          }
          set
          {
              objectName = value;
          }
      }

      /// <summary>
      /// Gets or sets the destination point of way/detection on WTG image.
      /// </summary>
      public static Dictionary<string, PointF> ObjectDestination
      {
          get
          {
              return objectDestination;
          }
          set
          {
              objectDestination = value;
          }
      }

      /// <summary>
      /// Gets or sets the origin point of way/detection on WTG image.
      /// </summary>
      public static Dictionary<string, PointF> ObjectOrigin
      {
          get
          {
              return objectOrigin;
          }
          set
          {
              objectOrigin = value;
          }
      }

      /// <summary>
      /// Gets or sets the radius of way/detection if circle on WTG image.
      /// </summary>
      public static Dictionary<string, short> ObjectRadius
      {
          get
          {
              return objectRadius;
          }
          set
          {
              objectRadius = value;
          }
      }


      /// <summary>
      /// Gets or sets the type of way/detection is circle or rectangle on WTG image.
      /// </summary>
      public static Dictionary<string, byte> ObjectType
      {
          get
          {
              return objectType;
          }
          set
          {
              objectType = value;
          }
      }

      /// <summary>
      /// Gets or sets the visibility of way/detection on WTG image.
      /// </summary>
      public static Dictionary<string, Boolean> ObjectVisible
      {
          get
          {
              return objectVisible;
          }
          set
          {
              objectVisible = value;
          }
      }

      /// <summary>
      /// Gets or sets the object is way or detection on WTG image.
      /// </summary>
      public static Dictionary<string, byte> ObjectTypePiece
      {
          get
          {
              return objectTypePiece;
          }
          set
          {
              objectTypePiece = value;
          }
      }

      /// <summary>
      /// Gets or sets the color of detection object on WTG image.
      /// </summary>
      public static Dictionary<string, int[]> ObjectColor
      {
          get
          {
              return objectColor;
          }
          set
          {
              objectColor = value;
          }
      }

      /// <summary>
      /// Gets or sets the Object of WTG image way/detection. Array of Panels.
      /// </summary>
      public static Dictionary<string, Panel> ObjectArrayList
      {
          get
          {
              return objectArrayList;
          }
          set
          {
              objectArrayList = value;
          }
      }

      /// <summary>
      /// Gets or sets the Object Location of WTG image way/detection.
      /// </summary>
      public static Dictionary<string, PointF> ObjectLocation
      {
          get
          {
              return objectLocation;
          }
          set
          {
              objectLocation = value;
          }
      }
  }
}
