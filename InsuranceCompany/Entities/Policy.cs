using System;
using System.Collections.Generic;

namespace InsuranceCompany.Entities;

public partial class Policy
{
    public int Id { get; set; }

    public string Number { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }

    public decimal PaymentAmount { get; set; }

    public int TypeId { get; set; }

    public bool PaymentMark { get; set; }

    public int ClientId { get; set; }

    public int EmployeeId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual PolicyType Type { get; set; } = null!;
}
