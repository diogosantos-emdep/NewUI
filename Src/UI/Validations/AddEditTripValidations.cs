using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering.Templates;
using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{//[Sudhir.Jangra][GEOS2-4816]
    public class AddEditTripValidations : ValidationRule
    {
        public string FieldName { get; set; }
        public string Value1 { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, string fieldValue1, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (fieldName == "SelectedType")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Travel Type empty.");
                else if (fieldValue.ToString().Trim().Equals("---"))
                    errorMessage = string.Format("You cannot leave the Travel Type empty.");
            }
            else if (fieldName == "SelectedPropose")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Purpose empty.");
                else if (fieldValue.ToString().Trim().Equals("---"))
                    errorMessage = string.Format("You cannot leave the Purpose empty.");
            }
            else if (fieldName == "SelectedTraveller")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Traveller empty.");
                else if (fieldValue.ToString().Trim().Equals("---") && string.IsNullOrEmpty(fieldValue1))
                    errorMessage = string.Format("You cannot leave the Traveller empty.");
            }
            //[GEOS2-6760][rdixit][14.01.2025]
            else if (fieldName == "SelectedResponsible")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Responsible empty.");                
            }
            else if (fieldName == "CustomTraveler")
            {
                if ((fieldValue == null || fieldValue.ToString().Trim() == "") && string.IsNullOrEmpty(fieldValue1))
                    errorMessage = string.Format("You cannot leave the Traveller Name empty.");
            }
            else if (fieldName == "TravelerEmail")
            {
                if ((fieldValue == null || fieldValue.ToString().Trim() == "") && string.IsNullOrEmpty(fieldValue1))
                    errorMessage = string.Format("You cannot leave the Traveller Email empty.");
            }
            // [nsatpute][16-09-2024][GEOS2-5929]
            else if (fieldName == "SelectedWorkShift")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Work Time empty.");
                else if (fieldValue.ToString().Trim().Equals("---"))
                    errorMessage = string.Format("You cannot leave the Work Time empty.");
            }
            if (fieldName == "SelectedOrigin")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Origin empty.");
                else if (fieldValue.ToString().Trim().Equals("---"))
                    errorMessage = string.Format("You cannot leave the Origin empty.");               
            }
            else if (fieldName == "SelectedDestination")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Destination empty.");
                else if (fieldValue.ToString().Trim().Equals("---"))
                    errorMessage = string.Format("You cannot leave the Destination empty.");
            }
            else if (fieldName == "FromDate")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the From Date empty.");
            }
            else if (fieldName == "ToDate")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the To Date empty.");
            }
            #region [rdixit][21.09.2024][GEOS2-5930]
            else if (fieldName == "ArrivalTransportationNumber")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Arrival Transportation Number empty.");
            }
            else if (fieldName == "SelectedArrivalTransportationType")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Arrival Transportation Type empty.");
                else if (fieldValue.ToString().Trim().Equals("---"))
                    errorMessage = string.Format("You cannot leave the Arrival Transportation Type empty.");
            }
            else if (fieldName == "DepartureTransportationNumber")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Departure Transportation Number empty.");
            }
            else if (fieldName == "SelectedDepartureTransportationType")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Departure Transportation Type empty.");
                else if (fieldValue.ToString().Trim().Equals("---"))
                    errorMessage = string.Format("You cannot leave the Departure Transportation Type empty.");
            }

            else if (fieldName == "ArrivalTransporterName")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Arrival Transporter Name empty.");
            }
            else if (fieldName == "SelectedArrivalTransport")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Arrival Transport Method empty.");
                else if (fieldValue.ToString().Trim().Equals("---"))
                    errorMessage = string.Format("You cannot leave the Arrival Transport Method empty.");
            }
            else if (fieldName == "ArrivalProvider")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Arrival Provider empty.");
            }
            else if (fieldName == "ArrivalTransporterContact")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Arrival Transporter Contact empty.");
            }
            else if (fieldName == "SelectedDepartureTransport")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Departure Transport Method empty.");
                else if (fieldValue.ToString().Trim().Equals("---"))
                    errorMessage = string.Format("You cannot leave the Departure Transport Method empty.");
            }
            else if (fieldName == "DepartureTransporterName")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Departure Transporter Name empty.");
            }
            else if (fieldName == "DepartureProvider")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Departure Provider empty.");
            }
            else if (fieldName == "DepartureTransporterContact")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Departure Transporter Contact empty.");
            }

            else if (fieldName == "SelectedAccommodationType")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Accommodation Type empty.");
                else if (fieldValue.ToString().Trim().Equals("---"))
                    errorMessage = string.Format("You cannot leave the Accommodation Type empty.");
            }

            else if (fieldName == "AccommodationAddress")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Address empty.");
            }

            else if (fieldName == "AccommodationCoordinates")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Coordinates empty.");
            }
            #endregion
            return errorMessage;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value, Value1);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }

    }
}
