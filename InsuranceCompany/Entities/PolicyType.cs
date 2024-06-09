using System;
using System.Collections.Generic;

namespace InsuranceCompany.Entities;

public partial class PolicyType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Conditions { get; set; } = null!;

    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();

    public virtual ICollection<PolicyTypesRisk> PolicyTypesRisks { get; set; } = new List<PolicyTypesRisk>();
}
