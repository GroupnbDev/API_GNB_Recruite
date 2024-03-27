using System;
using System.Collections.Generic;

namespace nbRecruitment.ModelsERP;

public partial class Profile
{
    public ulong Id { get; set; }

    public ulong? UserId { get; set; }

    public string? Name { get; set; }

    public string? Number { get; set; }

    public string? Address { get; set; }

    public string? Date { get; set; }

    public string? Image { get; set; }

    public string? LastUpdate { get; set; }

    public DateTime? Deleted { get; set; }

    public string? File1 { get; set; }

    public string? File2 { get; set; }

    public string? File3 { get; set; }

    public string? File4 { get; set; }

    public string? File5 { get; set; }

    public string? File6 { get; set; }

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();

    public virtual User? User { get; set; }
}
