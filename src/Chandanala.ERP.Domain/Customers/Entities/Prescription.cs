namespace Chandanala.ERP.Domain.Customers.Entities;

public enum PrescriptionStatus
{
    Active = 1,
    Completed = 2,
    Expired = 3,
    Cancelled = 4
}

public class Prescription
{
    public Guid Id { get; private set; }

    public string PrescriptionNumber { get; private set; }

    public string DoctorName { get; private set; }

    public DateOnly IssueDate { get; private set; }

    public DateOnly ExpiryDate { get; private set; }

    public PrescriptionStatus Status { get; private set; }

    public string? Notes { get; private set; }

    public Prescription(
        string prescriptionNumber,
        string doctorName,
        DateOnly issueDate,
        DateOnly expiryDate,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(prescriptionNumber))
            throw new ArgumentException("Prescription number is required.", nameof(prescriptionNumber));

        if (string.IsNullOrWhiteSpace(doctorName))
            throw new ArgumentException("Doctor name is required.", nameof(doctorName));

        if (expiryDate < issueDate)
            throw new ArgumentException("Expiry date cannot be earlier than issue date.");

        Id = Guid.NewGuid();
        PrescriptionNumber = prescriptionNumber;
        DoctorName = doctorName;
        IssueDate = issueDate;
        ExpiryDate = expiryDate;
        Notes = notes;
        Status = PrescriptionStatus.Active;
    }

    public bool IsExpired(DateOnly today)
    {
        return today > ExpiryDate;
    }

    public void MarkCompleted()
    {
        Status = PrescriptionStatus.Completed;
    }

    public void Cancel()
    {
        Status = PrescriptionStatus.Cancelled;
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
    }
}
