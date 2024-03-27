using System;
using System.Collections.Generic;

namespace nbRecruitment.ModelsERP;

public partial class Position
{
    public ulong Id { get; set; }

    public string? Date { get; set; }

    public ulong? UserId { get; set; }

    public ulong? ProfileId { get; set; }

    public string? Account { get; set; }

    public string? Team { get; set; }

    public string? Province { get; set; }

    public string? Position1 { get; set; }

    public string? Demand { get; set; }

    public long? Count { get; set; }

    public long? Amount { get; set; }

    public long? Total { get; set; }

    public long? TotalCost { get; set; }

    public long? Rate { get; set; }

    public long? TotalProfit { get; set; }

    public long? Flight { get; set; }

    public string? Lmia { get; set; }

    public string? JobBank { get; set; }

    public string? Recruitment { get; set; }

    public string? Notes { get; set; }

    public string? Notes1 { get; set; }

    public string? Message { get; set; }

    public string? Remarks { get; set; }

    public string? Charge { get; set; }

    public string? Invoice { get; set; }

    public string? InvoiceDate { get; set; }

    public string? InvoiceNumber { get; set; }

    public long? InvoiceAmount { get; set; }

    public string? InvoiceNotes { get; set; }

    public string? InvoiceDate1 { get; set; }

    public string? InvoiceNumber1 { get; set; }

    public long? InvoiceAmount1 { get; set; }

    public string? InvoiceNotes1 { get; set; }

    public string? InvoiceDate2 { get; set; }

    public string? InvoiceNumber2 { get; set; }

    public long? InvoiceAmount2 { get; set; }

    public string? InvoiceNotes2 { get; set; }

    public string? InvoiceDate3 { get; set; }

    public string? InvoiceNumber3 { get; set; }

    public long? InvoiceAmount3 { get; set; }

    public string? InvoiceNotes3 { get; set; }

    public string? LastUpdate { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual Profile? Profile { get; set; }

    public virtual User? User { get; set; }
}
