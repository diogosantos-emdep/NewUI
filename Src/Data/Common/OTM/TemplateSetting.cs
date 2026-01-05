using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    public class TemplateSetting : ModelBase, IDisposable
    {
        #region Fields
        long idTemplateSettings;
        string templateName;
        List<LookupValue> tagList;
        #endregion     

        #region Constructor
        public TemplateSetting()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public Int64 IdTemplateSettings
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
        public string TemplateName
        {
            get
            {
                return templateName;
            }

            set
            {
                templateName = value;
                OnPropertyChanged("TemplateName");
            }
        }

        [DataMember]
        public List<LookupValue> TagList
        {
            get
            {
                return tagList;
            }

            set
            {
                tagList = value;
                OnPropertyChanged("TagList");
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
