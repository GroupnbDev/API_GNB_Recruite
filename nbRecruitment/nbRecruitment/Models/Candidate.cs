using System;
using System.Collections.Generic;

namespace nbRecruitment.Models;

public partial class Candidate
{
    public int Id { get; set; }

    public int AsignTo { get; set; }

    public int PostingId { get; set; }

    public string JobCode { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Middlename { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public sbyte Polo { get; set; }

    public string Currency { get; set; } = null!;

    public int Salary { get; set; }

    public string Country { get; set; } = null!;

    public string CurrentCountry { get; set; } = null!;

    public string? StatusDescription { get; set; }

    public string? LastStatusDescription { get; set; }

    public int? Status { get; set; }

    public int? IsDelete { get; set; }

    public sbyte? IsViewed { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
