using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class CPType : ModelBase, IDisposable
    {
     
            #region Field
            private int idCPType;
            private string name;
            #endregion
            #region Property

            [DataMember]
            public int IdCPType
        {
                get
                {
                    return idCPType;
                }

                set
                {
                idCPType = value;
                    OnPropertyChanged("IdCPType");
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
            public CPType()
            {

            }
            #endregion
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }


