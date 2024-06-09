using System;
using System.Collections.Generic;

namespace InsuranceCompany.Entities;

public partial class ClientGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
