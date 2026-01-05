using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class Family : ModelBase, IDisposable
    {
        #region Fields
        private uint id;
        private string name;
        private string name_es;
        private bool isChecked;//[Sudhir.Jangra][GEOS2-4500][10/07/2023]
        int idType;
        #endregion

        #region Properties   
        [DataMember]
        public uint Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }
        [DataMember]
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
        [DataMember]
        public string Name_es
        {
            get { return name_es; }
            set { name_es = value; OnPropertyChanged("Name_es"); }
        }
        [DataMember]
        public int IdType
        {
            get { return idType; }
            set { idType = value; OnPropertyChanged("IdType"); }
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public override string ToString()
        {
            return Name;
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
