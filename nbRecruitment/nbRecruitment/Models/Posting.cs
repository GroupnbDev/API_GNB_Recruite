using System;
using System.Collections.Generic;

namespace nbRecruitment.Models;

public partial class Posting
{
    public int Id { get; set; }

    public string JobCode { get; set; } = null!;

    public string JobType { get; set; } = null!;

    public string LanguageCodes { get; set; } = null!;

    public int PositionCount { get; set; }

    public string Location { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public string Per { get; set; } = null!;

    public string Salary { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Requirements { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int? Status { get; set; }

    public int? IsDelete { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
