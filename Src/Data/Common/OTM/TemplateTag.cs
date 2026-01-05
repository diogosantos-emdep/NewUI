using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    public class TemplateTag : ModelBase, IDisposable
    {
        #region Fields
        long idTemplateTags;
        int idTemplateSettings;
        string tagValue;
        int skipValue;
        int idTag;
        int takeValue;
        string nextValue;
        #endregion

        #region Constructor
        public TemplateTag()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public long IdTemplateTags
        {
            get
            {
                return idTemplateTags;
            }

            set
            {
                idTemplateTags = value;
                OnPropertyChanged("IdTemplateTags");
            }
        }

        [DataMember]
        public int IdTemplateSettings
        {
            get
            {
                return idTemplateSettings;
            }

            set
            {
                idTemplateSettings = value;
                OnPropertyChanged("IdTemplateSettings");
            }
        }

        [DataMember]
        public int SkipValue
        {
            get
            {
                return skipValue;
            }

            set
            {
                skipValue = value;
                OnPropertyChanged("SkipValue");
            }
        }
        
        [DataMember]
        public int TakeValue
        {
            get
            {
                return takeValue;
            }

            set
            {
                takeValue = value;
                OnPropertyChanged("TakeValue");
            }
        }

        [DataMember]
        public string TagValue
        {
            get
            {
                return tagValue;
            }

            set
            {
                tagValue = value;
                OnPropertyChanged("TagValue");
            }
        }

        [DataMember]
        public int IdTag
        {
            get
            {
                return idTag;
            }

            set
            {
                idTag = value;
                OnPropertyChanged("IdTag");
            }
        }


        [DataMember]
        public string NextValue
        {
            get
            {
                return nextValue;
            }

            set
            {
                nextValue = value;
                OnPropertyChanged("NextValue");
            }
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
}
