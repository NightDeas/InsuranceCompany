using System;
using System.Collections.Generic;

namespace InsuranceCompany.Entities;

public partial class Post
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Salary { get; set; }

    public string Requirements { get; set; } = null!;

    public string Responsibilities { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
