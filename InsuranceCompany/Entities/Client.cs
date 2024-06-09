using System;
using System.Collections.Generic;

namespace InsuranceCompany.Entities;

public partial class Client
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public int UserId { get; set; }

    public virtual ClientGroup Group { get; set; } = null!;

    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();

    public virtual User User { get; set; } = null!;
}
