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

    public DateOnly DateOfBirth { get; set; }

    public string Email { get; set; } = null!;

    public sbyte Polo { get; set; }

    public int NumCode { get; set; }

    public string Num { get; set; } = null!;

    public string? StatusDescription { get; set; }

    public string? LastStatusDescription { get; set; }

    public int? Status { get; set; }

    public int? IsDelete { get; set; }

    public sbyte? IsViewed { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string Nationality { get; set; } = null!;

    public string? CurrentResidingAddress { get; set; }
}
