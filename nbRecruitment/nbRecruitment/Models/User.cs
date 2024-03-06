using System;
using System.Collections.Generic;

namespace nbRecruitment.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Middlename { get; set; }

    public string Lastname { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Fullname2 { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Sbucode { get; set; }

    public int? Status { get; set; }

    public int? IsDelete { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
