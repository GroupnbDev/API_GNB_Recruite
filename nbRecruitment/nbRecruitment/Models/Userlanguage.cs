using System;
using System.Collections.Generic;

namespace nbRecruitment.Models;

public partial class Userlanguage
{
    public int Id { get; set; }

    public string LangCode { get; set; } = null!;

    public int UserId { get; set; }

    public int? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
