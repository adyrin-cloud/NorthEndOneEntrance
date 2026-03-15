namespace NorthEndOneEntrance.Services;

public interface IEmployeeCheckInService
{
    Task<CheckInResult> ProcessCheckInAsync(string employeeId, string photoBase64);
    Task<EmployeeStatus> GetEmployeeStatusAsync(string employeeId);
    bool ValidateEmployeeId(string employeeId);
}

public class CheckInResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsCheckIn { get; set; }
    public DateTime Timestamp { get; set; }
    public string? ErrorMessage { get; set; }
}

public class EmployeeStatus
{
    public string EmployeeId { get; set; } = string.Empty;
    public bool IsCheckedIn { get; set; }
    public DateTime? LastCheckInTime { get; set; }
    public DateTime? LastCheckOutTime { get; set; }
}
