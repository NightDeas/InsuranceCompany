using System;
using System.Collections.Generic;

namespace InsuranceCompany.Entities;

public partial class User
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int GenreId { get; set; }

    public string Address { get; set; } = null!;

    public string Telephone { get; set; } = null!;

    public string Passport { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Genre Genre { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
