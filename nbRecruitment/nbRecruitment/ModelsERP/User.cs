using System;
using System.Collections.Generic;

namespace nbRecruitment.ModelsERP;

public partial class User
{
    public ulong Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Email { get; set; }

    public string? Team { get; set; }

    public string? Position { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
