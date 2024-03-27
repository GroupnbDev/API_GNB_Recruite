using System;
using System.Collections.Generic;

namespace nbRecruitment.ModelsERP;

public partial class Candidate
{
    public ulong Id { get; set; }

    public string? Date { get; set; }

    public ulong? UserId { get; set; }

    public string? Admin { get; set; }

    public string? Sex { get; set; }

    public string? Name { get; set; }

    public ulong? ProfileId { get; set; }

    public ulong? PositionId { get; set; }

    public string? Nationality { get; set; }

    public string? Dob { get; set; }

    public string? Country { get; set; }

    public string? Employer { get; set; }

    public string? Number { get; set; }

    public string? Email { get; set; }

    public string? Permit { get; set; }

    public string? Resident { get; set; }

    public string? BCertificate { get; set; }

    public string? Marriage { get; set; }

    public string? Funds { get; set; }

    public string? Passport { get; set; }

    public string? Resume { get; set; }

    public string? WCertificate { get; set; }

    public string? Employment { get; set; }

    public string? Paystub { get; set; }

    public string? Diplomas { get; set; }

    public string? Transcript { get; set; }

    public string? Visa { get; set; }

    public string? PCertificate { get; set; }

    public string? Retainer { get; set; }

    public string? JobOffer { get; set; }

    public string? Foreign { get; set; }

    public string? Caq { get; set; }

    public string? Status { get; set; }

    public string? Bio { get; set; }

    public string? Med { get; set; }

    public string? VisaStat { get; set; }

    public string? Driver { get; set; }

    public string? Expiration { get; set; }

    public string? Settle { get; set; }

    public string? Image { get; set; }

    public string? LastUpdate { get; set; }

    public DateTime? Deleted { get; set; }

    public string? SettlementBy { get; set; }

    public string? SettlementDate { get; set; }

    public string? Healthcard { get; set; }

    public string? Canadian { get; set; }

    public string? Sin { get; set; }

    public string? Bank { get; set; }

    public string? License { get; set; }

    public string? Notes { get; set; }

    public string? Ticket { get; set; }

    public string? ArrivalDate { get; set; }

    public string? Starting { get; set; }

    public string? Airport { get; set; }

    public string? Housing { get; set; }

    public string? Remarks { get; set; }

    public string? PreArrival { get; set; }

    public string? Handbook { get; set; }

    public string? TeamsLink { get; set; }

    public virtual Position? Position { get; set; }

    public virtual Profile? Profile { get; set; }

    public virtual User? User { get; set; }
}
