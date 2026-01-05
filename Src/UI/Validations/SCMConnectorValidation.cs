using Emdep.Geos.Data.Common.SCM;
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
    public class SCMConnectorValidation : ValidationRule
    {
        //[GEOS2-4600][rdixit][20.09.2023]
        public string FieldName { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            try
            {
                if (fieldName == "SelectedLinkedType")
                {
                    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                        errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");                   
                }

                #region Ways       
                if (fieldName == "Ways.MinValueNew" || fieldName == "Ways.MaxValueNew")
                {
                    if (fieldValue != null)
                    {
                        if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;
                            Validation2(ref errorMessage, obj);
                        }
                        else if (fieldValue.GetType().Equals(typeof(ConnectorProperties)))
                        {
                            ConnectorProperties Inputvalue = ((ConnectorProperties)fieldValue);
                            Validation1(ref errorMessage, Inputvalue);
                        }
                    }
                }
                #endregion

                #region DiameterInternal
                if (fieldName == "DiameterInternal.MinValueNew" || fieldName == "DiameterInternal.MaxValueNew")
                {
                    if (fieldValue != null)
                    {
                        if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;
                            Validation2(ref errorMessage, obj);
                        }
                        else if (fieldValue.GetType().Equals(typeof(ConnectorProperties)))
                        {
                            ConnectorProperties Inputvalue = ((ConnectorProperties)fieldValue);
                            Validation1(ref errorMessage, Inputvalue);
                        }
                    }
                }
                #endregion

                #region DiameterExternal
                if (fieldName == "DiameterExternal.MinValueNew" || fieldName == "DiameterExternal.MaxValueNew")
                {
                    if (fieldValue != null)
                    {
                        if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;
                            Validation2(ref errorMessage, obj);
                        }
                        else if (fieldValue.GetType().Equals(typeof(ConnectorProperties)))
                        {
                            ConnectorProperties Inputvalue = ((ConnectorProperties)fieldValue);
                            Validation1(ref errorMessage, Inputvalue);
                        }
                    }
                }
                #endregion

                #region Height
                if (fieldName == "Height.MinValueNew" || fieldName == "Height.MaxValueNew")
                {
                    if (fieldValue != null)
                    {
                        if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;
                            Validation2(ref errorMessage, obj);
                        }
                        else if (fieldValue.GetType().Equals(typeof(ConnectorProperties)))
                        {
                            ConnectorProperties Inputvalue = ((ConnectorProperties)fieldValue);
                            Validation1(ref errorMessage, Inputvalue);
                        }
                    }
                }
                #endregion

                #region Length
                if (fieldName == "Length.MinValueNew" || fieldName == "Length.MaxValueNew")
                {
                    if (fieldValue != null)
                    {
                        if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;
                            Validation2(ref errorMessage, obj);
                        }
                        else if (fieldValue.GetType().Equals(typeof(ConnectorProperties)))
                        {
                            ConnectorProperties Inputvalue = ((ConnectorProperties)fieldValue);
                            Validation1(ref errorMessage, Inputvalue);
                        }
                    }
                }
                #endregion

                #region Width
                if (fieldName == "Width.MinValueNew" || fieldName == "Width.MaxValueNew")
                {
                    if (fieldValue != null)
                    {
                        if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;
                            Validation2(ref errorMessage, obj);
                        }
                        else if (fieldValue.GetType().Equals(typeof(ConnectorProperties)))
                        {
                            ConnectorProperties Inputvalue = ((ConnectorProperties)fieldValue);
                            Validation1(ref errorMessage, Inputvalue);
                        }
                    }
                }
                #endregion

                #region Reference
                if (fieldName == "Reference")
                {
                    if (fieldValue != null)
                    {
                        if (!fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            string val = fieldValue.ToString();
                            string pattern = @"\b%*[a-zA-Z0-9]+\b";//[GEOS2-5454][06.03.2024][rdixit]
                            bool containsPattern = Regex.IsMatch(val, pattern);
                            if (!containsPattern)
                                errorMessage = string.Format("Invalid Reference entry.", fieldName);
                        }
                        else
                        {
                            DevExpress.Xpf.Editors.TextEdit test = (DevExpress.Xpf.Editors.TextEdit)((System.Windows.Data.BindingExpression)fieldValue).Target;
                            string val = test.Text.ToString();
                            string pattern = @"\b%*[a-zA-Z0-9]+\b";//[GEOS2-5454][06.03.2024][rdixit]
                            bool containsPattern = Regex.IsMatch(val, pattern);
                            if (!containsPattern)
                                errorMessage = string.Format("Invalid Reference entry.", fieldName);
                        }
                    }
                }
                #endregion


                #region Ways       
                if (fieldName == "Ways.DefaultValue")
                {
                    if (fieldValue != null)
                    {
                        if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;
                            Validation2(ref errorMessage, obj);
                        }
                        else if (fieldValue.GetType().Equals(typeof(ConnectorProperties)))
                        {
                            ConnectorProperties Inputvalue = ((ConnectorProperties)fieldValue);
                            Validation1(ref errorMessage, Inputvalue);
                        }
                    }
                }
                #endregion

                #region [GEOS2-5803][rdixit][13.09.2024] 
                #region DiameterInternal
                if (fieldName == "DiameterInternal.DefaultValue")
                {
                    if (fieldValue != null)
                    {
                        if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;
                            Validation2(ref errorMessage, obj);
                        }
                        else if (fieldValue.GetType().Equals(typeof(ConnectorProperties)))
                        {
                            ConnectorProperties Inputvalue = ((ConnectorProperties)fieldValue);
                            Validation1(ref errorMessage, Inputvalue);
                        }
                    }
                }
                #endregion

                #region DiameterExternal
                if (fieldName == "DiameterExternal.DefaultValue")
                {
                    if (fieldValue != null)
                    {
                        if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;
                            Validation2(ref errorMessage, obj);
                        }
                        else if (fieldValue.GetType().Equals(typeof(ConnectorProperties)))
                        {
                            ConnectorProperties Inputvalue = ((ConnectorProperties)fieldValue);
                            Validation1(ref errorMessage, Inputvalue);
                        }
                    }
                }
                #endregion

                #region Height
                if (fieldName == "Height.DefaultValue")
                {
                    if (fieldValue != null)
                    {
                        if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;
                            Validation2(ref errorMessage, obj);
                        }
                        else if (fieldValue.GetType().Equals(typeof(ConnectorProperties)))
                        {
                            ConnectorProperties Inputvalue = ((ConnectorProperties)fieldValue);
                            Validation1(ref errorMessage, Inputvalue);
                        }
                    }
                }
                #endregion

                #region Length
                if (fieldName == "Length.DefaultValue")
                {
                    if (fieldValue != null)
                    {
                        if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;
                            Validation2(ref errorMessage, obj);
                        }
                        else if (fieldValue.GetType().Equals(typeof(ConnectorProperties)))
                        {
                            ConnectorProperties Inputvalue = ((ConnectorProperties)fieldValue);
                            Validation1(ref errorMessage, Inputvalue);
                        }
                    }
                }
                #endregion

                #region Width
                if (fieldName == "Width.DefaultValue")
                {
                    if (fieldValue != null)
                    {
                        if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                        {
                            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;
                            Validation2(ref errorMessage, obj);
                        }
                        else if (fieldValue.GetType().Equals(typeof(ConnectorProperties)))
                        {
                            ConnectorProperties Inputvalue = ((ConnectorProperties)fieldValue);
                            Validation1(ref errorMessage, Inputvalue);
                        }
                    }
                }
                #endregion
                #endregion

            }
            catch (Exception ex)
            {

            }
            return errorMessage;            
        }

        public static void Validation1(ref string errorMessage, ConnectorProperties Inputvalue)
        {           
            if (Inputvalue != null)
            {
                if (!string.IsNullOrEmpty(Inputvalue.DefaultValue) && !string.IsNullOrEmpty(Inputvalue.MinValueValidation))
                {
                    if (Convert.ToDouble(Inputvalue.DefaultValue) < Convert.ToDouble(Inputvalue.MinValueValidation))
                    {
                        errorMessage = string.Format("Min value is {0}", Inputvalue.MinValueValidation);
                    }
                }
                if (!string.IsNullOrEmpty(Inputvalue.DefaultValue) && !string.IsNullOrEmpty(Inputvalue.MaxValueValidation))
                {
                    if (Convert.ToDouble(Inputvalue.DefaultValue) > Convert.ToDouble(Inputvalue.MaxValueValidation))
                    {
                        errorMessage = string.Format("Max value is {0}", Inputvalue.MaxValueValidation);
                    }
                }
            }
        }

        public static void Validation2(ref string errorMessage, System.Windows.Data.BindingExpression fieldValue)
        {
            System.Windows.Data.BindingExpression obj = (System.Windows.Data.BindingExpression)fieldValue;

            if (obj != null)
            {
                if (obj.ResolvedSource != null)
                {
                    var MaxValue = ((ConnectorProperties)obj.ResolvedSource).MaxValueValidation;
                    var MinValue = ((ConnectorProperties)obj.ResolvedSource).MinValueValidation;
                    DevExpress.Xpf.Editors.TextEdit textval = (DevExpress.Xpf.Editors.TextEdit)obj.Target;

                    if (!string.IsNullOrEmpty(textval.Text))
                    {                      
                        if (!string.IsNullOrEmpty(textval.Text) && !string.IsNullOrEmpty(MinValue))
                        {
                            if (Convert.ToInt32(textval.Text) < Convert.ToInt32(MinValue))
                            {
                                errorMessage = string.Format("Min value is {0}", MinValue);
                            }
                        }
                        if (!string.IsNullOrEmpty(textval.Text) && !string.IsNullOrEmpty(MaxValue))
                        {
                            if (Convert.ToInt32(textval.Text) > Convert.ToInt32(MaxValue))
                            {
                                errorMessage = string.Format("Max value is {0}", MaxValue);
                            }
                        }
                    }
                }
            }
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
