using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.WindowsUI;
using System.Globalization;
using System.Threading;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Modules.Hrm.Views
{
    /// <summary>
    /// Interaction logic for EditTravelExpenseView.xaml
    /// </summary>
    public partial class EditTravelExpenseView : WinUIDialogWindow
    {
        public static double amountt,tipt;
        public EditTravelExpenseView()
        {
            InitializeComponent();
            
        }

        // shubham[skadam]  GEOS2-3957 Add a NEW section called “EXPENSES” in the Expense Report Edit/View screen 13 OCT 2022
        private void CustomSummaryCommandAction(object obj, EventArgs eve)
        {
            //[GEOS2-3957][rdixit][07.10.2022]
            CultureInfo[] ci = CultureInfo.GetCultures(CultureTypes.AllCultures);
            DevExpress.Xpf.Grid.GridCustomSummaryEventArgs e = (DevExpress.Xpf.Grid.GridCustomSummaryEventArgs)eve;
            try
            {
                if (e.FieldValue!=null)
                {                  
                    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
                    {
                        DevExpress.Xpf.Grid.GridSummaryItem t = (DevExpress.Xpf.Grid.GridSummaryItem)e.Item;
                        Emdep.Geos.Data.Common.Hrm.Expenses CurrentRow = (Emdep.Geos.Data.Common.Hrm.Expenses)e.Row;
                        CultureInfo c = CultureInfo.CurrentCulture;

                        if (t.FieldName == "Amount")
                        {
                            if (CurrentRow.ExistAsAttendee != 1)//[GEOS2-4438][rdixit][15.06.2023]
                            {
                                if (e.TotalValue is string)
                                {
                                    string TotalValue = e.TotalValue.ToString();
                                    TotalValue = TotalValue.Remove(0, 1);
                                    amountt = Convert.ToDouble(amountt) + CurrentRow.AmtCal;
                                    e.TotalValue = t;
                                    if (e.TotalValue != null)
                                        //e.TotalValue = Math.Round(amountt, 2) + CurrentRow.CurSymbol;
                                        //[pramod.misal][GEOS2-5286][21.02.2024]
                                        e.TotalValue = Math.Round(amountt, 2).ToString("F2") + " " + CurrentRow.CurSymbol;

                                }
                                else
                                {
                                    amountt = Convert.ToDouble(e.TotalValue) + CurrentRow.AmtCal;
                                    if (amountt != null)
                                        //e.TotalValue = Math.Round(amountt, 2) + CurrentRow.CurSymbol;
                                        //[pramod.misal][GEOS2-5286][21.02.2024]
                                        e.TotalValue = Math.Round(amountt, 2).ToString("F2") + " " + CurrentRow.CurSymbol;
                                }

                                CurrentRow.Amount = CurrentRow.AmtCal.ToString();
                            }
                        }
                        if (t.FieldName == "Tip")
                        {
                            if (CurrentRow.ExistAsAttendee != 1)//[GEOS2-4438][rdixit][15.06.2023]
                            {
                                if (e.TotalValue is string)
                                {
                                    string TotalValue = e.TotalValue.ToString();
                                    TotalValue = TotalValue.Remove(0, 1);
                                    tipt = Convert.ToDouble(tipt) + CurrentRow.Tipcal;
                                    e.TotalValue = t;
                                    if (e.TotalValue != null)
                                        //e.TotalValue = Math.Round(tipt, 2) + CurrentRow.CurSymbol;
                                        //[pramod.misal][GEOS2-5286][21.02.2024]
                                        e.TotalValue = Math.Round(tipt, 2).ToString("F2")+ " " + CurrentRow.CurSymbol;
                                }
                                else
                                {
                                    tipt = Convert.ToDouble(e.TotalValue) + CurrentRow.Tipcal;
                                    if (tipt != null)
                                        //e.TotalValue = Math.Round(tipt, 2) + CurrentRow.CurSymbol;
                                        //[pramod.misal][GEOS2-5286][21.02.2024]
                                        e.TotalValue = Math.Round(tipt, 2).ToString("F2") + " " + CurrentRow.CurSymbol;
                                }
                                CurrentRow.Tip = CurrentRow.Tipcal.ToString();
                            }
                        }
                    }
                                   
                }
            }
            catch (Exception ex)
            {

            }
        }
        //[GEOS2-3958][rdixit][05.11.2022]
        private void GridControlEmpolyeeTravelExpense_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs eve)
        {
            CultureInfo[] ci = CultureInfo.GetCultures(CultureTypes.AllCultures);
            DevExpress.Xpf.Grid.GridCustomSummaryEventArgs e = (DevExpress.Xpf.Grid.GridCustomSummaryEventArgs)eve;
            try
            {
                if (e.Item != null)
                {
                    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
                    {
                        DevExpress.Xpf.Grid.GridSummaryItem t = (DevExpress.Xpf.Grid.GridSummaryItem)e.Item;
                        Emdep.Geos.Data.Common.Hrm.WeekTravelExpenseList CurrentRow = (Emdep.Geos.Data.Common.Hrm.WeekTravelExpenseList)e.Row;
                        CultureInfo c = ci.ToList().Where(i => i.NumberFormat.CurrencySymbol == CurrentRow.CurSymbol).FirstOrDefault();
                        #region Sunday
                        if (t.FieldName == "SunExpenses")
                        {
                            if (e.TotalValue is string)
                            {
                                string TotalValue = e.TotalValue.ToString();
                                TotalValue = TotalValue.Remove(0, 1);
                                amountt = Convert.ToDouble(amountt) + Convert.ToDouble(CurrentRow.SunExpenses);
                                e.TotalValue = t;
                                if (e.TotalValue != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }
                            else
                            {
                                amountt = Convert.ToDouble(e.TotalValue) + Convert.ToDouble(CurrentRow.SunExpenses);
                                if (amountt != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }

                            CurrentRow.SunExpenses = CurrentRow.SunExpenses;
                        }
                        #endregion

                        #region Monday
                        if (t.FieldName == "MonExpenses")
                        {
                            if (e.TotalValue is string)
                            {
                                string TotalValue = e.TotalValue.ToString();
                                TotalValue = TotalValue.Remove(0, 1);
                                amountt = Convert.ToDouble(amountt) + Convert.ToDouble(CurrentRow.MonExpenses);
                                e.TotalValue = t;                                
                                if (e.TotalValue != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;                      
                            }
                            else
                            {
                                amountt = Convert.ToDouble(e.TotalValue) + Convert.ToDouble(CurrentRow.MonExpenses);
                                if (amountt != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }

                            CurrentRow.MonExpenses = CurrentRow.MonExpenses;
                        }
                        #endregion

                        #region Tuesday
                        if (t.FieldName == "TueExpenses")
                        {
                            if (e.TotalValue is string)
                            {
                                string TotalValue = e.TotalValue.ToString();
                                TotalValue = TotalValue.Remove(0, 1);
                                amountt = Convert.ToDouble(amountt) + Convert.ToDouble(CurrentRow.TueExpenses);
                                e.TotalValue = t;
                                if (e.TotalValue != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }
                            else
                            {
                                amountt = Convert.ToDouble(e.TotalValue) + Convert.ToDouble(CurrentRow.TueExpenses);
                                if (amountt != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }

                            CurrentRow.TueExpenses = CurrentRow.TueExpenses;
                        }
                        #endregion

                        #region Wednesday
                        if (t.FieldName == "WedExpenses")
                        {
                            if (e.TotalValue is string)
                            {
                                string TotalValue = e.TotalValue.ToString();
                                TotalValue = TotalValue.Remove(0, 1);
                                amountt = Convert.ToDouble(amountt) + Convert.ToDouble(CurrentRow.WedExpenses);
                                e.TotalValue = t;
                                if (e.TotalValue != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }
                            else
                            {
                                amountt = Convert.ToDouble(e.TotalValue) + Convert.ToDouble(CurrentRow.WedExpenses);
                                if (amountt != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }

                            CurrentRow.WedExpenses = CurrentRow.WedExpenses;
                        }
                        #endregion

                        #region Thursday
                        if (t.FieldName == "ThuExpenses")
                        {
                            if (e.TotalValue is string)
                            {
                                string TotalValue = e.TotalValue.ToString();
                                TotalValue = TotalValue.Remove(0, 1);
                                amountt = Convert.ToDouble(amountt) + Convert.ToDouble(CurrentRow.ThuExpenses);
                                e.TotalValue = t;
                                if (e.TotalValue != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }
                            else
                            {
                                amountt = Convert.ToDouble(e.TotalValue) + Convert.ToDouble(CurrentRow.ThuExpenses);
                                if (amountt != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }

                            CurrentRow.ThuExpenses = CurrentRow.ThuExpenses;
                        }
                        #endregion

                        #region Friday
                        if (t.FieldName == "FriExpenses")
                        {
                            if (e.TotalValue is string)
                            {
                                string TotalValue = e.TotalValue.ToString();
                                TotalValue = TotalValue.Remove(0, 1);
                                amountt = Convert.ToDouble(amountt) + Convert.ToDouble(CurrentRow.FriExpenses);
                                e.TotalValue = t;
                                if (e.TotalValue != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }
                            else
                            {
                                amountt = Convert.ToDouble(e.TotalValue) + Convert.ToDouble(CurrentRow.FriExpenses);
                                if (amountt != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }

                            CurrentRow.FriExpenses = CurrentRow.FriExpenses;
                        }
                        #endregion

                        #region Saturday
                        if (t.FieldName == "SatExpenses")
                        {
                            if (e.TotalValue is string)
                            {
                                string TotalValue = e.TotalValue.ToString();
                                TotalValue = TotalValue.Remove(0, 1);
                                amountt = Convert.ToDouble(amountt) + Convert.ToDouble(CurrentRow.SatExpenses);
                                e.TotalValue = t;
                                if (e.TotalValue != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }
                            else
                            {
                                amountt = Convert.ToDouble(e.TotalValue) + Convert.ToDouble(CurrentRow.SatExpenses);
                                if (amountt != null)
                                    e.TotalValue = (Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol);                                
                            }

                            CurrentRow.SatExpenses = CurrentRow.SatExpenses;
                        }
                        #endregion

                        #region WeekTotal
                        if (t.FieldName == "WeekTotalExpenses")
                        {
                            if (e.TotalValue is string)
                            {
                                string TotalValue = e.TotalValue.ToString();
                                TotalValue = TotalValue.Remove(0, 1);
                                amountt = Convert.ToDouble(amountt) + Convert.ToDouble(CurrentRow.WeekTotalExpenses);
                                e.TotalValue = t;
                                if (e.TotalValue != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }
                            else
                            {
                                amountt = Convert.ToDouble(e.TotalValue) + Convert.ToDouble(CurrentRow.WeekTotalExpenses);
                                if (amountt != null)
                                    e.TotalValue = Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture) + CurrentRow.CurSymbol;
                            }

                            CurrentRow.WeekTotalExpenses = CurrentRow.WeekTotalExpenses;
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        //[GEOS2-4178][rdixit][21.03.2023]
        private void GridControlEmpolyeeTravelExpense_FilterChanged(object sender, RoutedEventArgs e)
        {

            DevExpress.Xpf.Grid.GridControl temp = (DevExpress.Xpf.Grid.GridControl)e.Source;
            List<Emdep.Geos.Data.Common.Hrm.WeekTravelExpenseList> weeklyExpensesListTemp = new List<Data.Common.Hrm.WeekTravelExpenseList>();
            foreach (var item in temp.VisibleItems)
            {
                weeklyExpensesListTemp.Add(item as Emdep.Geos.Data.Common.Hrm.WeekTravelExpenseList);
            }
            List<DevExpress.Xpf.Grid.GridSummaryItem> summuries = new List<DevExpress.Xpf.Grid.GridSummaryItem>(temp.TotalSummary);
            double MealExpenceAllowanceAmount = weeklyExpensesListTemp.FirstOrDefault().MealExpense;
            Style RedForColorStyle = new Style() { TargetType = typeof(Run) };
            RedForColorStyle.Setters.Add(new Setter() { Property = Run.ForegroundProperty, Value = System.Windows.Media.Brushes.Red });

            Style GreenForColorStyle = new Style() { TargetType = typeof(Run) };
            GreenForColorStyle.Setters.Add(new Setter() { Property = Run.ForegroundProperty, Value = System.Windows.Media.Brushes.Green });
            #region ExpenseSummery

            if (weeklyExpensesListTemp != null)
            {

                List<Emdep.Geos.Data.Common.Hrm.WeekTravelExpenseList> weeklyExpensesListTemp1 = weeklyExpensesListTemp.Where(i => i.Category.ToLower() == "meal").ToList();

                if (weeklyExpensesListTemp1 != null)
                {
                    
                    #region calculate Week Days Meal Exp
                    double MonMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.MonMealExpenses), 2);
                    double TueMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.TueMealExpenses), 2);
                    double WedMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.WedMealExpenses), 2);
                    double ThuMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.ThuMealExpenses), 2);
                    double FriMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.FriMealExpenses), 2);
                    double SatMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.SatMealExpenses), 2);
                    double SunMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.SunMealExpenses), 2);
                    double mealAllowanceTotal = Math.Round((7 * MealExpenceAllowanceAmount), 2);
                    double mealtotal = Math.Round((MonMealExp + TueMealExp + WedMealExp + ThuMealExp + FriMealExp + SatMealExp + SunMealExp), 2);
                    #endregion

                    #region Assign to summary
                    summuries.Where(i => i.FieldName == "MonExpenses2").FirstOrDefault().DisplayFormat = " {0}" + MonMealExp.ToString("n", CultureInfo.CurrentCulture) + weeklyExpensesListTemp.FirstOrDefault().CurSymbol;
                    summuries.Where(i => i.FieldName == "TueExpenses2").FirstOrDefault().DisplayFormat = " {0}" + TueMealExp.ToString("n", CultureInfo.CurrentCulture) + weeklyExpensesListTemp.FirstOrDefault().CurSymbol;
                    summuries.Where(i => i.FieldName == "WedExpenses2").FirstOrDefault().DisplayFormat = " {0}" + WedMealExp.ToString("n", CultureInfo.CurrentCulture) + weeklyExpensesListTemp.FirstOrDefault().CurSymbol;
                    summuries.Where(i => i.FieldName == "ThuExpenses2").FirstOrDefault().DisplayFormat = " {0}" + ThuMealExp.ToString("n", CultureInfo.CurrentCulture) + weeklyExpensesListTemp.FirstOrDefault().CurSymbol;
                    summuries.Where(i => i.FieldName == "FriExpenses2").FirstOrDefault().DisplayFormat = " {0}" + FriMealExp.ToString("n", CultureInfo.CurrentCulture) + weeklyExpensesListTemp.FirstOrDefault().CurSymbol;
                    summuries.Where(i => i.FieldName == "SatExpenses2").FirstOrDefault().DisplayFormat = " {0}" + SatMealExp.ToString("n", CultureInfo.CurrentCulture) + weeklyExpensesListTemp.FirstOrDefault().CurSymbol;
                    summuries.Where(i => i.FieldName == "SunExpenses2").FirstOrDefault().DisplayFormat = " {0}" + SunMealExp.ToString("n", CultureInfo.CurrentCulture) + weeklyExpensesListTemp.FirstOrDefault().CurSymbol;

                    summuries.Where(t => t.FieldName == "MonExpenses1" || t.FieldName == "TueExpenses1" || t.FieldName == "WedExpenses1" || t.FieldName == "ThuExpenses1" || t.FieldName == "FriExpenses1"
                    || t.FieldName == "SatExpenses1" || t.FieldName == "SunExpenses1").ToList().ForEach(j => j.DisplayFormat = " {0}" + Math.Round(MealExpenceAllowanceAmount, 2).ToString("n", CultureInfo.CurrentCulture) + weeklyExpensesListTemp.FirstOrDefault().CurSymbol);
                    summuries.Where(i => i.FieldName == "TotalExpenses2").FirstOrDefault().DisplayFormat = " {0}" + mealtotal.ToString("n", CultureInfo.CurrentCulture) + weeklyExpensesListTemp.FirstOrDefault().CurSymbol;
                    #endregion

                    #region Summary Style

                    if (mealtotal > mealAllowanceTotal)
                        summuries.Where(i => i.FieldName == "TotalExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if(mealtotal > 0)  //[pramod.misal][10-06-2025][GEOS2-8024]
                        summuries.Where(i => i.FieldName == "TotalExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;

                    if (MonMealExp > MealExpenceAllowanceAmount)
                        summuries.Where(i => i.FieldName == "MonExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (MonMealExp < MealExpenceAllowanceAmount && MonMealExp > 0)
                        summuries.Where(i => i.FieldName == "MonExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;
                    //[pramod.misal][10-06-2025][GEOS2-8024]
                    if (TueMealExp > MealExpenceAllowanceAmount)
                        summuries.Where(i => i.FieldName == "TueExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (TueMealExp < MealExpenceAllowanceAmount && TueMealExp > 0)
                        summuries.Where(i => i.FieldName == "TueExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;

                    if (WedMealExp > MealExpenceAllowanceAmount)
                        summuries.Where(i => i.FieldName == "WedExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (WedMealExp < MealExpenceAllowanceAmount && WedMealExp > 0)
                        summuries.Where(i => i.FieldName == "WedExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;

                    if (ThuMealExp > MealExpenceAllowanceAmount)
                        summuries.Where(i => i.FieldName == "ThuExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (ThuMealExp < MealExpenceAllowanceAmount && ThuMealExp > 0)
                        summuries.Where(i => i.FieldName == "ThuExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;

                    if (FriMealExp > MealExpenceAllowanceAmount)
                        summuries.Where(i => i.FieldName == "FriExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (FriMealExp < MealExpenceAllowanceAmount && FriMealExp > 0)
                        summuries.Where(i => i.FieldName == "FriExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;

                    if (SatMealExp > MealExpenceAllowanceAmount)
                        summuries.Where(i => i.FieldName == "SatExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (SatMealExp < MealExpenceAllowanceAmount && SatMealExp > 0)
                        summuries.Where(i => i.FieldName == "SatExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;

                    if (SunMealExp > MealExpenceAllowanceAmount)
                        summuries.Where(i => i.FieldName == "SunExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (SunMealExp < MealExpenceAllowanceAmount && SunMealExp > 0)
                        summuries.Where(i => i.FieldName == "SunExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;
                    #endregion
                }
            }
            #endregion
        }

        private void GridControl_LayoutUpdated(object sender, EventArgs e)
        {
            amountt = 0;
            tipt = 0;
        }
    }
}
