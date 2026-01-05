using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class OTItem
    {
        public byte rework;
        public DateTime? modifiedIn;
        public Int32 modifiedBy;
        public DateTime? latestStatusChange;
        public byte isBatch;
        public Int64 idRevisionItem;
        public Int64 idOTItem;
        public Int64 idOT;
        public byte idItemOtStatus;
        public DateTime? docGeneratedIn;
        public byte customerCommentRead;
        public string customerComment;
        public DateTime? createdIn;
        public Int32 createdBy;
        public string attachedFiles;
        public Int32 assignedTo;
        // People assignedToUser;
        public GEOSAPIRevisionItem revisionItem;
        public GEOSAPIItemOTStatusType itemOTStatusType;
        public GEOSAPIOts ot;
        public GEOSAPIQuotation quotation;

        public Int32 keyId;
        public Int32 parentId;

        // List<PickingMaterials> pickingMaterialsList;

        public DateTime? shippingDate;

        //    List<Counterpart> counterparts;

        // List<OtItem> articleDecomposedList;
        public Int64 articleStock;
        public Int64 articleMinimumStock;
        public Int64 parentArticleType;
        public CreateRevisionItem createRevisionItem;
    }

    public class GEOSAPIItemOTStatusType
    {
        public Int32 idItemOtStatus;
        public string name;
        public string htmlColor;
        public Int32 sequence;
    }
}
