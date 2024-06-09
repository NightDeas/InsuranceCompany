using System;
using System.Collections.Generic;

namespace InsuranceCompany.Entities;

public partial class PolicyTypesRisk
{
    public int Id { get; set; }

    public int PolicyTypeId { get; set; }

    public int RiskId { get; set; }

    public virtual PolicyType PolicyType { get; set; } = null!;

    public virtual Risk Risk { get; set; } = null!;
}
