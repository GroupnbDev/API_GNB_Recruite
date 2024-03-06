using System;
using System.Collections.Generic;

namespace nbRecruitment.Models;

public partial class UserMenu
{
    public int Id { get; set; }

    public int MenuId { get; set; }

    public int UserId { get; set; }

    public sbyte? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
