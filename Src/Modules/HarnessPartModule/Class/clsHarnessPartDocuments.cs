using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    public class clsHarnessPartDocuments
    {
        private string _originalFileName;

        public string OriginalFileName
        {
            get { return _originalFileName; }
            set { _originalFileName = value; }
        }
        private string _documentTypeName;

        public string DocumentTypeName
        {
            get { return _documentTypeName; }
            set { _documentTypeName = value; }
        }


        private List<clsCompany> _company;

        public List<clsCompany> Company
        {
            get { return _company; }
            set { _company = value; }
        }




        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string selectedCompanyId;

        public string SelectedCompanyId
        {
            get { return selectedCompanyId; }
            set { selectedCompanyId = value; }
        }

        private string _selectedIdType;

        public string SelectedIdType
        {
            get { return _selectedIdType; }
            set { _selectedIdType = value; }


        }

        private List<clsharnessPartAccessoryTypes> _type;

        public List<clsharnessPartAccessoryTypes> Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
