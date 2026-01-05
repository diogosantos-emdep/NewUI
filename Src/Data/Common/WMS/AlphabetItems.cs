using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{

    public class AlphabetItems : ModelBase, IDisposable
    {

        Int32 asciiValue;
        string character;

        [DataMember]
        public Int32 AsciiValue
        {
            get
            {
                return asciiValue;
            }

            set
            {
                asciiValue = value;
                OnPropertyChanged("AsciiValue");
            }
        }




        [DataMember]
        public string Character
        {
            get
            {
                return character;
            }

            set
            {
                character = value;
                OnPropertyChanged("Character");
            }
        }


        #region Constructor
        public AlphabetItems()
        {

        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }

    public class RangeItems : ModelBase, IDisposable
    {

        #region Constructor
        public RangeItems()
        {

        }
        #endregion

       private Int32 range;
       private string level;
       private string concatLevel;
       private long position;


        [DataMember]
        public Int32 Range
        {
            get
            {
                return range;
            }

            set
            {
                range = value;
                OnPropertyChanged("Range");
            }
        }




        [DataMember]
        public string Lavel
        {
            get
            {
                return level;
            }

            set
            {
                level = value;
                OnPropertyChanged("Lavel");
            }
        }

        [DataMember]
        public string ConcatLevel
        {
            get
            {
                return concatLevel;
            }

            set
            {
                concatLevel = value;
                OnPropertyChanged("ConcatLevel ");
            }
        }
        [DataMember]
        public long Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }
        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }

}
