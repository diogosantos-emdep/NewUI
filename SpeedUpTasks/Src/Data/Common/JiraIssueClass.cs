using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    public class JiraIssueClass : ModelBase, IDisposable
    {
        #region Declaration

        Int64 id;
        string key;
        string self;
        #endregion

        #region Properties


        [DataMember]
        public Int64 Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }


        [DataMember]
        public String Key
        {
            get { return key; }
            set
            {
                key = value;
                OnPropertyChanged("Key");
            }
        }

        [DataMember]
        public String Self
        {
            get { return self; }
            set
            {
                self = value;
                OnPropertyChanged("Self");
            }
        }
        #endregion

        #region Constructor

        public JiraIssueClass()
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
}
