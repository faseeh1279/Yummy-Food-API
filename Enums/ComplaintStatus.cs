namespace Yummy_Food_API.Enums
{
    public enum ComplaintStatus
    {
        Pending,        // Newly created, not yet reviewed
        InProgress,     // Being looked into
        Resolved,       // Issue resolved
        Rejected,       // Found invalid or inappropriate
        Blocked,        // Blocked by admin for violations
        Closed        // Complaint closed (may be after resolution or inactivity)
    }
}
