using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.PCM;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_GlobalComparisonTimes : ModelBase, IDisposable
    {
        #region Field    
        private string region;
        private string stageName;
        private string plant;
        private string designerName;
        private string templateName;
        private string cpType;
        private string otCode;
        private string designSystem;
        private string designType;
        private string dsaStatus;
        private string detection;
        private string option;
        private string typeOfways;
        private Int32 itemNo;
        private string noOfWays;
        private string noOfDetection;
        private string noOfOption;   
        private Int32 reworkQty;
        private DateTime? fquOkDate;
        TimeSpan expectedTime;
        TimeSpan productionTime;
        TimeSpan reworkTime;
        TimeSpan reworkTimeOws;
        TimeSpan productionTimeOws;
        TimeSpan productionExpectedTime;
        TimeSpan avgproductionTime;
        TimeSpan totalproduction;
        private Int32 iDDrawing;
        private Int32 reworkQTY;
        private string comments;
        private string serialNumber;//[GEOS2-9233][rani dhamankar][13-08-2025]
        // start[GEOS2 - 7102][gulab lakade][13 09 2025]
        TimeSpan eDSTime;
        TimeSpan addinTime;
        TimeSpan postServerTime;
        // end[GEOS2 - 7102][gulab lakade][13 09 2025]
        #endregion

        #region Property
        [DataMember]
        public string StageName
        {
            get
            {
                return stageName;
            }

            set
            {
                stageName = value;
                OnPropertyChanged("StageName");
            }
        }
        [DataMember]
        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }
        [DataMember]
        public string DesignerName
        {
            get
            {
                return designerName;
            }

            set
            {
                designerName = value;
                OnPropertyChanged("DesignerName");
            }
        }

        [DataMember]
        public string TemplateName
        {
            get
            {
                return templateName;
            }

            set
            {
                templateName = value;
                OnPropertyChanged("TemplateName");
            }
        }

        [DataMember]
        public string CpType
        {
            get
            {
                return cpType;
            }

            set
            {
                cpType = value;
                OnPropertyChanged("CpType");
            }
        }

        [DataMember]
        public string OTCode
        {
            get
            {
                return otCode;
            }

            set
            {
                otCode = value;
                OnPropertyChanged("OTCode");
            }
        }

        [DataMember]
        public Int32 ItemNo
        {
            get
            {
                return itemNo;
            }

            set
            {
                itemNo = value;
                OnPropertyChanged("ItemNo");
            }
        }

        [DataMember]
        public string Plant
        {
            get
            {
                return plant;
            }

            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        [DataMember]
        public DateTime? FquOkDate
        {
            get
            {
                return fquOkDate;
            }

            set
            {
                fquOkDate = value;
                OnPropertyChanged("FquOkDate");
            }
        }

       
        [DataMember]
        public TimeSpan ExpectedTime
        {
            get { return expectedTime; }

            set
            {
                expectedTime = value;
                OnPropertyChanged("ExpectedTime");
            }
        }

       
        [DataMember]
        public TimeSpan ProductionTime
        {
            get { return productionTime; }

            set
            {
                productionTime = value;
                OnPropertyChanged("ProductionTime");
            }
        }

        
        [DataMember]
        public TimeSpan ReworkTime
        {
            get { return reworkTime; }

            set
            {
                reworkTime = value;
                OnPropertyChanged("ReworkTime");
            }
        }

       
        [DataMember]
        public TimeSpan ReworkTimeOws
        {
            get { return reworkTimeOws; }

            set
            {
                reworkTimeOws = value;
                OnPropertyChanged("ReworkTimeOws");
            }
        }

       
        [DataMember]
        public TimeSpan ProductionTimeOws
        {
            get { return productionTimeOws; }

            set
            {
                productionTimeOws = value;
                OnPropertyChanged("ProductionTimeOws");
            }
        }

      
        [DataMember]
        public TimeSpan ProductionExpectedTime
        {
            get { return productionExpectedTime; }

            set
            {
                productionExpectedTime = value;
                OnPropertyChanged("ProductionExpectedTime");
            }
        }

    
        [DataMember]
        public TimeSpan AvgproductionTime
        {
            get { return avgproductionTime; }

            set
            {
                avgproductionTime = value;
                OnPropertyChanged("AvgproductionTime");
            }
        }

       
        [DataMember]
        public TimeSpan Totalproduction
        {
            get { return totalproduction; }

            set
            {
                totalproduction = value;
                OnPropertyChanged("Totalproduction");
            }
        }

        [DataMember]
        public string DesignSystem
        {
            get
            {
                return designSystem;
            }

            set
            {
                designSystem = value;
                OnPropertyChanged("DesignSystem");
            }
        }

        [DataMember]
        public string DesignType
        {
            get
            {
                return designType;
            }

            set
            {
                designType = value;
                OnPropertyChanged("DesignType");
            }
        }

        [DataMember]
        public string DsaStatus
        {
            get
            {
                return dsaStatus;
            }

            set
            {
                dsaStatus = value;
                OnPropertyChanged("DsaStatus");
            }
        }

        [DataMember]
        public string NoOfWays
        {
            get
            {
                return noOfWays;
            }

            set
            {
                noOfWays = value;
                OnPropertyChanged("NoOfWays");
            }
        }

        [DataMember]
        public string NoOfDetection
        {
            get
            {
                return noOfDetection;
            }

            set
            {
                noOfDetection = value;
                OnPropertyChanged("NoOfDetection");
            }
        }

        [DataMember]
        public string TypeOfways
        {
            get
            {
                return typeOfways;
            }

            set
            {
                typeOfways = value;
                OnPropertyChanged("TypeOfways");
            }
        }

        [DataMember]
        public string Detection
        {
            get
            {
                return detection;
            }

            set
            {
                detection = value;
                OnPropertyChanged("Detection");
            }
        }

        [DataMember]
        public string NoOfOption
        {
            get
            {
                return noOfOption;
            }

            set
            {
                noOfOption = value;
                OnPropertyChanged("NoOfOption");
            }
        }

        [DataMember]
        public string Option
        {
            get
            {
                return option;
            }

            set
            {
                option = value;
                OnPropertyChanged("Option");
            }
        }

        [DataMember]
        public Int32 IDDrawing
        {
            get
            {
                return iDDrawing;
            }

            set
            {
                iDDrawing = value;
                OnPropertyChanged("IDDrawing");
            }
        }

        [DataMember]
        public Int32 ReworkQTY
        {
            get
            {
                return reworkQTY;
            }

            set
            {
                reworkQTY = value;
                OnPropertyChanged("ReworkQTY");
            }
        }


        [DataMember]
        public string Comments
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }
        #region[GEOS2-9233][rani dhamankar][13-08-2025]
        [DataMember]
        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }

            set
            {
                serialNumber = value;
                OnPropertyChanged("SerialNumber");
            }
        }
        #endregion
        #region   // start[GEOS2 - 7102][gulab lakade][13 09 2025]
        [DataMember]
        public TimeSpan EDSTime
        {
            get { return eDSTime; }

            set
            {
                eDSTime = value;
                OnPropertyChanged("EDSTime");
            }
        }
        [DataMember]
        public TimeSpan AddinTime
        {
            get { return addinTime; }

            set
            {
                addinTime = value;
                OnPropertyChanged("AddinTime");
            }
        }
        [DataMember]
        public TimeSpan PostServerTime
        {
            get { return postServerTime; }

            set
            {
                postServerTime = value;
                OnPropertyChanged("PostServerTime");
            }
        }
        #endregion
        #endregion

        #region Constructor
        public ERM_GlobalComparisonTimes()
        {

        }
        #endregion


        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
