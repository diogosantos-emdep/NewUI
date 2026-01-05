using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class LocationValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "HTMLColor")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "HTML Color");
            }
            if (fieldName == "LocationName")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Location Name");
                //else
                //{
                //    object viewModel = new object();
                //    if (fieldValue.GetType().ToString() == "System.Windows.Data.BindingExpression")
                //    {
                //        System.Windows.Data.BindingExpression bindingExpression = (System.Windows.Data.BindingExpression)fieldValue;
                //        viewModel = bindingExpression.DataItem;
                //    }
                //    else
                //    {
                //        viewModel = fieldValue;
                //    }
                //    var Location = viewModel.GetType().GetProperty("LocationName").GetValue(viewModel, null);
                //    var IsLeaf = viewModel.GetType().GetProperty("IsLeaf").GetValue(viewModel, null);
                //    if (IsLeaf.ToString() == "1")               //3rd Level Location
                //    {
                //        var reg = new Regex(@"^\w{1}\d{2,3}$");
                //        if (!reg.IsMatch(Location.ToString()))
                //        {
                //            errorMessage = string.Format("Enter a valid Location Name");
                //        }
                        
                //    }
                //    else if (IsLeaf.ToString() == "0")         
                //    {
                //        var Parent = viewModel.GetType().GetProperty("SelectedParent").GetValue(viewModel, null);
                //        var FullName= Parent.GetType().GetProperty("FullName").GetValue(Parent, null);
                //        var reg=new Regex("");
                //        if (FullName.ToString() == "---")    //1st Level Location
                //        {
                //            reg = new Regex(@"^\w{1}\d{1}$");
                //        }
                //        else                                  //2nd Level Location
                //        {
                //            reg = new Regex(@"^\d{1}\d{1}$");
                //        }
                            
                //        if (!reg.IsMatch(Location.ToString()))
                //        {
                //            errorMessage = string.Format("Enter a valid Location Name");
                //        }
                //    }
                //}
            }
            if (fieldName == "Position")
            {
                try
                {
                    object viewModel = new object();

                    if (fieldValue.GetType().ToString() == "System.Windows.Data.BindingExpression")
                    {
                        System.Windows.Data.BindingExpression bindingExpression = (System.Windows.Data.BindingExpression)fieldValue;
                        viewModel = bindingExpression.DataItem;
                    }
                    else
                    {
                        viewModel = fieldValue;
                    }
                    var Position = viewModel.GetType().GetProperty("Position").GetValue(viewModel, null);
                    var MaxPosition = viewModel.GetType().GetProperty("MaxPosition").GetValue(viewModel, null);
                    if (int.Parse(Position.ToString()) > int.Parse(MaxPosition.ToString()) || int.Parse(Position.ToString()) == 0)
                        errorMessage = string.Format("Enter a valid position");
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in  LocationValidation Method GetErrorMessage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
            return errorMessage;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}
