using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    class clsRevision
    {
        private int idDrawing;
        private int parentID;
        private string type;
        private string site;
        private string problem;
        private string rootCause;
        private string changeLog;
        private DateTime createdIn;
        private string createdBy;
        private DateTime aproovedIn;
        private string aproovedBy;
        private string revisionNumber;
        private bool isShow;

        public bool IsShow
        {
            get { return isShow; }
            set { isShow = value; }
        }

        public int ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }
        public string RevisionNumber
        {
            get { return revisionNumber; }
            set { revisionNumber = value; }
        }

        public string AproovedBy
        {
            get { return aproovedBy; }
            set { aproovedBy = value; }
        }

        public DateTime AproovedIn
        {
            get { return aproovedIn; }
            set { aproovedIn = value; }
        }

        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public DateTime CreatedIn
        {
            get { return createdIn; }
            set { createdIn = value; }
        }

        public string ChangeLog
        {
            get { return changeLog; }
            set { changeLog = value; }
        }

        public string RootCause
        {
            get { return rootCause; }
            set { rootCause = value; }
        }

        public string Problem
        {
            get { return problem; }
            set { problem = value; }
        }

        public string Site
        {
            get { return site; }
            set { site = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public int IdDrawing
        {
            get { return idDrawing; }
            set { idDrawing = value; }
        }

    }
}
