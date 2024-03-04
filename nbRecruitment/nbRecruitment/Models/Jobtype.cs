using System;
using System.Collections.Generic;

namespace nbRecruitment.Models;

public partial class Jobtype
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? Status { get; set; }

    public int? IsDelete { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
