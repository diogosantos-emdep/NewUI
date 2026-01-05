using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SAM
{
	//[nsatpute][04-07-2024][GEOS2-5408]
    [DataContract]
    public class DocumentsIdentification : ModelBase, IDisposable
    {
        #region Fields
        private string fileName;
        private LookupValue structureDocumenttype;
        private bool isSelected;
        private byte[] fileData;
        #endregion Properties

        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged("FileName"); }
        }
        [DataMember]
        public LookupValue StructureDocumenttype
        {
            get { return structureDocumenttype; }
            set { structureDocumenttype = value; OnPropertyChanged("StructureDocumenttype"); }
        }

        [DataMember]
        public byte[] FileData
        {
            get { return fileData; }
            set { fileData = value; OnPropertyChanged("FileData"); }
        }
        [DataMember]
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged("IsSelected"); }
        }

        #region



        #endregion

        #region Constructor
        public DocumentsIdentification()
        {

        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
