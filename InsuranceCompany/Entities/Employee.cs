using System;
using System.Collections.Generic;

namespace InsuranceCompany.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();

    public virtual Post Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
