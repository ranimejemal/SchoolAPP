namespace SchoolApp.ViewModels;

public class DashboardViewModel
{
    public int EtudiantsCount { get; set; }
    public int ProfesseursCount { get; set; }
    public int ReferentielsCount { get; set; }
    public int AbsencesCount { get; set; }
    public double JustifiedRate { get; set; }
    public List<AbsenceStatItemViewModel> AbsencesByReferentiel { get; set; } = [];
    public List<RecentAbsenceViewModel> RecentAbsences { get; set; } = [];
}

public class AbsenceStatItemViewModel
{
    public string Label { get; set; } = string.Empty;
    public int Value { get; set; }
}

public class RecentAbsenceViewModel
{
    public string Etudiant { get; set; } = string.Empty;
    public string Referentiel { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public bool Justifiee { get; set; }
}
