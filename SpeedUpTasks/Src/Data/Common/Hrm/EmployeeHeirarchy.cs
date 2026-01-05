using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    public class EmployeeHeirarchy
    {
        public string ParentID
        {
            get;
            set;
        }

        public string ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
        public string ContentType
        {
            get;
            set;
        }
        public uint IdGender
        {
            get;
            set;
        }
        public byte[] EmployeeProfileImageInBytes
        {
            get;
            set;
        }

        public string DepartmentHtmlColor
        {
            get;
            set;
        }

        public string DepartmentHead
        {
            get;
            set;
        }

        public string JobDescriptionTitle
        {
            get;
            set;
        }

        public UInt32 IdJobDescription
        {
            get;
            set;
        }

        public UInt16 JobDescriptionUsage
        {
            get;
            set;
        }

        public List<Usage> JobDescriptionUsages
        {
            get;
            set;
        }

        public string ChildOrientation
        {
            get;
            set;
        }
    }


    public class Usage
    {
        public Int64 Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public double Percentage
        {
            get;
            set;
        }
    }
}
