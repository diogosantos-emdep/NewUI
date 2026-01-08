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
                // --- Identificação Básica ---
                IdActionPlan = entity.IdActionPlan,
                Code = entity.Code ?? string.Empty,
                Title = entity.Description ?? string.Empty, // Nota: entity.Description mapeia para Dto.Title
                Responsible = entity.FullName ?? string.Empty,
                EmployeeCode = entity.EmployeeCode ?? string.Empty,

                // --- Localização e Organização ---
                IdDepartment = entity.IdDepartment,
                Department = entity.Department,
                IdLocation = entity.IdLocation, // Assegura que isto vem preenchido do SP
                Location = entity.Location,
                IdSite = entity.IdSite,
                Site = entity.Site,
                IdCustomer = entity.IdCustomer,
                CustomerName = entity.CustomerName ?? string.Empty,
                BusinessUnit = entity.BusinessUnit ?? string.Empty,
                Origin = entity.Origin,

                // --- Estatísticas & Agregados (CRÍTICO PARA OS FILTROS FUNCIONAREM) ---
                // Estes campos vêm do SP (APM_GetActionPlanDetails_WithCounts)
                // Se não forem preenchidos, os filtros laterais e de alerta assumem 0.

                ThemeAggregates = entity.ThemeAggregates,   // Ex: "T|Safety:2;T|Quality:1"
                StatusAggregates = entity.StatusAggregates, // Ex: "S|Done:5;S|To Do:2"

                Stat_Overdue15 = entity.Stat_Overdue15,
                Stat_HighPriorityOverdue = entity.Stat_HighPriorityOverdue,
                Stat_MaxDueDays = entity.Stat_MaxDueDays,

                // --- Contagens Totais ---
                TotalActionItems = entity.TotalActionItems,
                TotalOpenItems = entity.TotalOpenItems,
                TotalClosedItems = entity.TotalClosedItems,

                // Cálculo da Percentagem (Evitar divisão por zero)
                Percentage = entity.TotalActionItems > 0
                    ? Math.Round(((double)entity.TotalClosedItems / entity.TotalActionItems) * 100, 0)
                    : 0,

                // Inicializar coleção de tasks vazia (Lazy Loading)
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
