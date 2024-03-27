using System;
using System.Collections.Generic;

namespace nbRecruitment.Models;

public partial class AsignUser
{
    public int Id { get; set; }

    public int PostingId { get; set; }

    public int UserId { get; set; }

    public int? Status { get; set; }

    public int Count { get; set; }
}
