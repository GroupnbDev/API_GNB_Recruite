using System;
using System.Collections.Generic;

namespace nbRecruitment.Models;

public partial class Menu
{
    public int Id { get; set; }

    public int ParentId { get; set; }

    public string Name { get; set; } = null!;

    public int Sort { get; set; }

    public int? Status { get; set; }

    public int? IsDelete { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
