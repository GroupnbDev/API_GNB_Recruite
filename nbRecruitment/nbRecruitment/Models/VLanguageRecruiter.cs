using System;
using System.Collections.Generic;

namespace nbRecruitment.Models;

public partial class VLanguageRecruiter
{
    public string LangCode { get; set; } = null!;

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Middlename { get; set; }

    public string Lastname { get; set; } = null!;

    public string? Fullname { get; set; }

    public int? Status { get; set; }
}
