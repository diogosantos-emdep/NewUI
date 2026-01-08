using Emdep.Geos.Data.Common.APM;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Emdep.Geos.Services.Contracts
{
    [DataContract]
    public class APMActionPlanModern
    {
        // --- IDENTIFICAÇÃO ---
        [DataMember] public long IdActionPlan { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Description { get; set; }

        // --- LOCALIZAÇÃO ---
        [DataMember] public int IdCompany { get; set; }
        [DataMember] public string Location { get; set; }

        [DataMember] public int IdSite { get; set; } // ADICIONADO: Resolve erro CS1061 'IdSite'
        [DataMember] public string Site { get; set; }

        [DataMember] public string GroupName { get; set; }
        [DataMember] public string Group { get; set; }

        [DataMember] public string Zone { get; set; }
        [DataMember] public int IdZone { get; set; } // ADICIONADO: Necessário para filtros

        [DataMember] public int IdLocation { get; set; }
        // --- PESSOAS ---
        [DataMember] public int IdEmployee { get; set; }

        [DataMember] public string EmployeeCode { get; set; } // ADICIONADO: Resolve erro CS1061 'EmployeeCode'
        [DataMember] public string FirstName { get; set; }
        [DataMember] public string LastName { get; set; }

        [DataMember] public string FullName { get; set; } // ADICIONADO: Resolve erro CS1061 'FullName'

        [DataMember] public int IdGender { get; set; }
        [DataMember] public string Responsible { get; set; }
        [DataMember] public string ActionPlanResponsibleDisplayName { get; set; }

        [DataMember] public int ResponsibleIdUser { get; set; } // ADICIONADO: Resolve erro CS1061 'ResponsibleIdUser'

        // --- CLASSIFICAÇÃO ---
        [DataMember] public string BusinessUnit { get; set; }
        [DataMember] public int IdLookupBusinessUnit { get; set; } // ADICIONADO
        [DataMember] public string BusinessUnitHTMLColor { get; set; }

        [DataMember] public string Origin { get; set; }
        [DataMember] public int IdLookupOrigin { get; set; } // ADICIONADO
        [DataMember] public string OriginDescription { get; set; } // ADICIONADO

        [DataMember] public string Department { get; set; }
        [DataMember] public int IdDepartment { get; set; }

        // --- ESTATÍSTICAS ---
        [DataMember] public int TotalActionItems { get; set; }
        [DataMember] public int TotalOpenItems { get; set; }
        [DataMember] public int TotalClosedItems { get; set; }
        [DataMember] public int Percentage { get; set; }
        [DataMember] public string TotalClosedColor { get; set; }

        // --- NOVAS ESTATÍSTICAS ---
        [DataMember] public int Stat_Overdue15 { get; set; }
        [DataMember] public int Stat_HighPriorityOverdue { get; set; }
        [DataMember] public int Stat_MaxDueDays { get; set; }
        [DataMember] public string Stat_ThemesList { get; set; }

        // --- AGREGADOS PRECISOS (para contagens com lazy-loading) ---
        // Formatos esperados:
        // ThemeAggregates  -> "T|Safety:3;T|Maintenance:2"
        // StatusAggregates -> "S|To do:5;S|In progress:2;S|Blocked:1;S|Closed:8"
        [DataMember] public string ThemeAggregates { get; set; }
        [DataMember] public string StatusAggregates { get; set; }

        // --- METADADOS ---
        [DataMember] public DateTime? CreatedIn { get; set; }

        [DataMember] public int CreatedBy { get; set; } // ADICIONADO
        [DataMember] public string CreatedByName { get; set; }

        // --- PAÍS ---
        [DataMember] public string CountryName { get; set; }
        [DataMember] public string CountryIso { get; set; }
        [DataMember] public string CountryIconUrl { get; set; }

        // --- LISTA DE TAREFAS ---
        [DataMember] public List<APMActionPlanTask> TaskList { get; set; }

        // --- MÉTODO CLONE (Resolve o erro CS1061 'Clone') ---
        public APMActionPlanModern Clone()
        {
            // Cria uma cópia superficial (necessária para os filtros funcionarem sem estragar o cache)
            return (APMActionPlanModern)this.MemberwiseClone();
        }
    }
}