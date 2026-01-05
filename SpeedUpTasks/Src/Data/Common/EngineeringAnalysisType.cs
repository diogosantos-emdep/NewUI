using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    public class EngineeringAnalysisType : ModelBase, IDisposable
    {
        #region Fields
        Int64 idArticle;
        string reference;
        bool isSelected;
        string quantity;
        byte idRevisionItemStatus;
        bool isArticleEnabled;
        #endregion

        #region Constructor
        public EngineeringAnalysisType()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public Int64 IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
            }
        }

        [DataMember]
        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                reference = value;
            }
        }


        [DataMember]
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
            }
        }


        [DataMember]
        public string Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
            }
        }

        [DataMember]
        public byte IdRevisionItemStatus
        {
            get
            {
                return idRevisionItemStatus;
            }

            set
            {
                idRevisionItemStatus = value;
            }
        }

        [DataMember]
        public bool IsArticleEnabled
        {
            get
            {
                return isArticleEnabled;
            }

            set
            {
                isArticleEnabled = value;
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
