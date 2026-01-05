using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class Templates : ModelBase, IDisposable
    {
        #region Field
        private int idTemplate;
        private string name;
        #endregion
        #region Property

        [DataMember]
        public int IdTemplate
        {
            get
            {
                return idTemplate;
            }

            set
            {
                idTemplate = value;
                OnPropertyChanged("IdTemplate");
            }
        }
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        #endregion

        #region Constructor
        public Templates()
        {

        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
