using System;
using System.Collections.Generic;

namespace InsuranceCompany.Entities;

public partial class Risk
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double AverageProbability { get; set; }

    public virtual ICollection<PolicyTypesRisk> PolicyTypesRisks { get; set; } = new List<PolicyTypesRisk>();
}
