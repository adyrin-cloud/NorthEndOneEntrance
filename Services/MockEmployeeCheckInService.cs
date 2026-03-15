using System.Collections.Concurrent;

namespace NorthEndOneEntrance.Services;

public class MockEmployeeCheckInService : IEmployeeCheckInService
{
    private readonly ConcurrentDictionary<string, EmployeeStatus> _employeeStatuses = new();
    private readonly HashSet<string> _validEmployeeIds = new()
    {
        "1001", "1002", "1003", "1004", "1005",
        "2001", "2002", "2003", "2004", "2005",
        "3001", "3002", "3003", "3004", "3005"
    };

    public Task<CheckInResult> ProcessCheckInAsync(string employeeId, string photoBase64)
    {
        // Employee ID is already validated at UI level, but double-check for safety
        if (!ValidateEmployeeId(employeeId))
        {
            return Task.FromResult(new CheckInResult
            {
                Success = false,
                ErrorMessage = $"Employee ID {employeeId} not found in the system."
            });
        }

        var status = _employeeStatuses.GetOrAdd(employeeId, _ => new EmployeeStatus
        {
            EmployeeId = employeeId,
            IsCheckedIn = false
        });

        var now = DateTime.Now;
        bool isCheckIn = !status.IsCheckedIn;

        if (isCheckIn)
        {
            // Check-in
            status.IsCheckedIn = true;
            status.LastCheckInTime = now;

            return Task.FromResult(new CheckInResult
            {
                Success = true,
                Message = "Check-in successful!",
                IsCheckIn = true,
                Timestamp = now
            });
        }
        else
        {
            // Check-out
            status.IsCheckedIn = false;
            status.LastCheckOutTime = now;

            return Task.FromResult(new CheckInResult
            {
                Success = true,
                Message = "Check-out successful!",
                IsCheckIn = false,
                Timestamp = now
            });
        }
    }

    public Task<EmployeeStatus> GetEmployeeStatusAsync(string employeeId)
    {
        var status = _employeeStatuses.GetOrAdd(employeeId, _ => new EmployeeStatus
        {
            EmployeeId = employeeId,
            IsCheckedIn = false
        });

        return Task.FromResult(status);
    }

    // Validate employee ID
    public bool ValidateEmployeeId(string employeeId)
    {
        if (string.IsNullOrWhiteSpace(employeeId) || employeeId.Length < 4)
        {
            return false;
        }
        return _validEmployeeIds.Contains(employeeId);
    }
}
