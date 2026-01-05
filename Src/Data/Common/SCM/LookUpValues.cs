using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    public class LookUpValues : ModelBase, IDisposable
    {
        #region Field
        Int32 idLookupValue;
        string value_en;
        string value_es;
        string value_fr;
        string value_pt;
        string value_ro;
        string value_ru;
        string value_zh;
        Int32 idLookupKey;
        string htmlColor;
        Int32 position;
        string abbreviation;
        int inUse;
        #endregion

        #region Property
        [DataMember]
        public Int32 IdLookupValue
        {
            get { return idLookupValue; }
            set
            {
                idLookupValue = value;
                OnPropertyChanged("IdLookupValue");
            }

        }
        [DataMember]
        public string Value_en
        {
            get { return value_en; }
            set
            {
                value_en=value;
                OnPropertyChanged("Value_en");
            }
        }

        [DataMember]
        public string Value_es
        {
            get { return value_es; }
            set
            {
                  value_es= value;
                OnPropertyChanged("Value_es");
            }
        }
        [DataMember]
        public string Value_fr
        {
            get { return value_fr; }
            set
            {
                 value_fr= value;
                OnPropertyChanged("Value_fr");
            }
        }
        [DataMember]
        public string Value_pt
        {
            get { return value_pt; }
            set
            {
                 value_pt= value;
                OnPropertyChanged("Value_pt");
            }
        }
        [DataMember]
        public string Value_ro
        {
            get { return value_ro; }
            set
            {
                value_ro= value;
                OnPropertyChanged("Value_ro");
            }
        }
        [DataMember]
        public string Value_ru
        {
            get { return value_ru; }
            set
            {
                value_ru= value;
                OnPropertyChanged("Value_ru");
            }
        }
        [DataMember]
        public string Value_zh
        {
            get { return value_zh; }
            set
            {
                value_zh= value;
                OnPropertyChanged("Value_zh");
            }
        }

        [DataMember]
        public Int32 IdLookupKey
        {
            get { return idLookupKey; }
            set
            {
                idLookupKey = value;
                OnPropertyChanged("IdLookupKey");
            }

        }
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set
            {
                 htmlColor= value;
                OnPropertyChanged("HtmlColor");
            }
        }
        [DataMember]
        public Int32 Position
        {
            get { return position; }
            set
            {
               position= value;
                OnPropertyChanged("Position");
            }
        }
        [DataMember]
        public string Abbreviation
        {
            get { return abbreviation; }
            set
            {
                 abbreviation= value;
                OnPropertyChanged("Abbreviation");
            }
        }
        [DataMember]
        public int InUse
        {
            get { return inUse; }
            set
            {
                 inUse= value;
                OnPropertyChanged("InUse");
            }
        }
        #endregion

        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return Value_en;
        }
        #endregion
    }
}
