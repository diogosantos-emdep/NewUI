using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.UI.Common;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    // Partial adds in-memory filtering for VisibilityPerBUViewModel
    public partial class VisibilityPerBUViewModel
    {
        private bool _fastFilterEnabled;
        private CancellationTokenSource _filterCts;
        // Track selection filters (example: SelectedBusinessUnit, SelectedOrganization)
        private readonly HashSet<string> _filterTriggerProps = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "SelectedBusinessUnit","SelectedOrganization"
        };

        public void EnableVisibilityFastFilter()
        {
            if (_fastFilterEnabled) return;
            PropertyChanged += OnVisibilityFastFilterPropertyChanged;
            _fastFilterEnabled = true;
            ApplyVisibilityFiltersAsync();
        }
        public void DisableVisibilityFastFilter()
        {
            if (!_fastFilterEnabled) return;
            PropertyChanged -= OnVisibilityFastFilterPropertyChanged;
            _fastFilterEnabled = false;
            CancelVisibilityFilter();
        }
        private void OnVisibilityFastFilterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_fastFilterEnabled) return;
            if (_filterTriggerProps.Contains(e.PropertyName))
            {
                ApplyVisibilityFiltersAsync(120);
            }
        }
        private void CancelVisibilityFilter()
        {
            try { _filterCts?.Cancel(); } catch { }
            try { _filterCts?.Dispose(); } catch { }
            _filterCts = null;
        }

        /// <param name="debounceMs"></param>
        private void ApplyVisibilityFiltersAsync(int debounceMs = 0)
        {
            try
            {
                if (EmployeeListAsPerOrganization == null) return;
                CancelVisibilityFilter();
                _filterCts = new CancellationTokenSource();
                var token = _filterCts.Token;

                // Capture criteria
                var selectedBUId = SelectedBusinessUnit?.IdLookupValue;
                var selectedOrgId = SelectedOrganization?.IdCompany;

                var source = EmployeeListAsPerOrganization.Select(e => (VisibilityPerBU)e.Clone()).ToList();

                Task.Run(async () =>
                {
                    if (debounceMs > 0)
                        await Task.Delay(debounceMs, token).ConfigureAwait(false);

                    IEnumerable<VisibilityPerBU> query = source;

                    if (selectedOrgId.HasValue)
                        ;

                    if (selectedBUId.HasValue)
                    {
                        string buIdStr = selectedBUId.Value.ToString();
                        query = query.Where(emp => !string.IsNullOrEmpty(emp.BusinessUnits) && emp.BusinessUnits.Split(new[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Contains(buIdStr));
                    }

                    var filtered = query.ToList();

                    // Build DataTable subset
                    DataTable filteredTable = localDataTable.Clone();
                    foreach (var item in filtered)
                    {
                        var dr = filteredTable.NewRow();
                        dr["FullName"] = item.FullName;
                        dr["Organizations"] = item.Organization;
                        dr["JobDescription"] = item.JobDescription;
                        dr["Scope"] = item.JDScope;
                        dr["YesNoList"] = new List<string>() { "Yes", "No" };
                        dr["EmployeeProperty"] = item;
                        dr["IsChecked"] = false;
                        dr["IdEmployee"] = item.IdEmployee;
                        dr["BusinessUnits"] = item.BusinessUnits;
                        // Dynamic BU columns
                        if (BusinessUnitList != null)
                        {
                            var buTokens = (item.BusinessUnits ?? string.Empty).Split(new[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToHashSet();
                            foreach (var bu in BusinessUnitList)
                            {
                                if (!filteredTable.Columns.Contains(bu.IdLookupValue.ToString())) continue;
                                dr[bu.IdLookupValue.ToString()] = buTokens.Contains(bu.IdLookupValue.ToString()) ? "Yes" : "No";
                            }
                        }
                        filteredTable.Rows.Add(dr);
                    }
                    token.ThrowIfCancellationRequested();
                    return filteredTable;
                }, token).ContinueWith(t =>
                {
                    if (t.IsCanceled || token.IsCancellationRequested) return;
                    if (t.Exception != null)
                    {
                        GeosApplication.Instance.Logger.Log("Visibility fast filter error: " + t.Exception.GetBaseException().Message, category: Prism.Logging.Category.Exception, priority: Prism.Logging.Priority.Low);
                        return;
                    }
                    DataTableForGridLayoutCopy = t.Result;
                    DataTable = DataTableForGridLayoutCopy;
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("ApplyVisibilityFiltersAsync setup error: " + ex.Message, category: Prism.Logging.Category.Exception, priority: Prism.Logging.Priority.Low);
            }
        }
    }
}
