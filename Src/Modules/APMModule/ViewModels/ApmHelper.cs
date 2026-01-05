using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.APM;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    public static class ApmHelpers
    {
        public static int GetActualDueDays(this APMActionPlanTask task)
        {
            if (task == null) return 0;

            if (task.DueDate == DateTime.MinValue) return 0;

            if (task.Status == "Done" || task.Status == "Closed") return 0;

            var days = (DateTime.Now.Date - task.DueDate.Date).Days;
            return days > 0 ? days : 0;
        }

        public static ActionPlanModernDto MapToPlanDto(APMActionPlan entity)
        {
            if (entity == null) return null;

            return new ActionPlanModernDto
            {
                IdActionPlan = entity.IdActionPlan,
                Code = entity.Code ?? string.Empty,
                Title = entity.Description ?? string.Empty,
                Responsible = entity.FullName ?? string.Empty,
                EmployeeCode = entity.EmployeeCode ?? string.Empty,
                IdDepartment = entity.IdDepartment,
                Department = entity.Department,
                IdLocation = 0,
                Location = entity.Location,
                BusinessUnit = string.Empty,
                Origin = entity.Origin,
                IdSite = entity.IdSite,
                Site = entity.Site,
                IdCustomer = entity.IdCustomer,
                CustomerName = string.Empty,

                Tasks = new ObservableCollection<ActionPlanTaskModernDto>()
            };
        }

        public static ActionPlanTaskModernDto MapToTaskDto(APMActionPlanTask t)
        {
            if (t == null) return null;

            return new ActionPlanTaskModernDto
            {
                IdTask = t.IdActionPlanTask,

                IdActionPlan = t.IdActionPlan,
                TaskNumber = t.TaskNumber,
                Code = t.Code,
                Title = t.Title,
                Description = t.Description,
                Responsible = t.Responsible ?? t.TaskResponsibleDisplayName,
                Status = t.Status,
                Priority = t.Priority,
                Theme = t.Theme,
                OpenDate = t.OpenDate,

                DueDate = t.DueDate,
                DueDateDisplay = t.DueDate != DateTime.MinValue ? t.DueDate.ToString("dd-MM-yyyy") : "-",

                DueDays = t.GetActualDueDays(),
                IdLookupStatus = t.IdLookupStatus,
                StatusColor = System.Drawing.ColorTranslator.FromHtml(t.CardDueColor ?? "#FFFFFF"),

                Percentage = 0
            };
        }
    }
}
