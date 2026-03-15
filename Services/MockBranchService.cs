using System.Text.RegularExpressions;

namespace NorthEndOneEntrance.Services;

public class MockBranchService : IBranchService
{
    private readonly HttpClient _httpClient;
    private List<Branch>? _cachedBranches;

    public MockBranchService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Branch>> GetBranchesAsync()
    {
        // Return cached branches if available
        if (_cachedBranches != null && _cachedBranches.Any())
        {
            return _cachedBranches.ToList();
        }

        try
        {
            // Fetch from actual website
            var response = await _httpClient.GetStringAsync("https://www.northendcoffee.com/cafes");
            var branches = ParseBranches(response);
            
            if (branches.Any())
            {
                _cachedBranches = branches;
                return branches;
            }
        }
        catch (Exception)
        {
            // Fall back to mock data if fetch fails
        }

        // Fallback mock data based on actual cafes
        _cachedBranches = new List<Branch>
        {
            new Branch { Id = 1, Name = "Borak Mehnur", Location = "Banani", Code = "BAN" },
            new Branch { Id = 2, Name = "Concord Baksh Tower", Location = "Gulshan-2", Code = "GUL2" },
            new Branch { Id = 3, Name = "Lotus Kamal Tower 2", Location = "Gulshan-1", Code = "GUL1" },
            new Branch { Id = 4, Name = "Cityscape Tower", Location = "Gulshan Avenue", Code = "GUAV" },
            new Branch { Id = 5, Name = "Ahmed & Kazi Tower, 3rd Floor", Location = "Dhanmondi", Code = "DHN3" },
            new Branch { Id = 6, Name = "Shahabuddin Ahmed Park", Location = "Gulshan-2", Code = "GULP" },
            new Branch { Id = 7, Name = "Roastery & Café", Location = "Tejgaon", Code = "TEJ" },
            new Branch { Id = 8, Name = "City Bank Center", Location = "Gulshan-1", Code = "GULC" },
            new Branch { Id = 9, Name = "Ascott Palace", Location = "Baridhara", Code = "BAR" },
            new Branch { Id = 10, Name = "Evercare Hospital", Location = "Bashundhara", Code = "BAS" },
            new Branch { Id = 11, Name = "Alamin Bhaban", Location = "Ukhiya", Code = "UKH" },
            new Branch { Id = 12, Name = "Liberty Tower, Ground Floor", Location = "Uttara", Code = "UTG" },
            new Branch { Id = 13, Name = "Liberty Tower, 3rd Floor", Location = "Uttara", Code = "UT3" },
            new Branch { Id = 14, Name = "Ahmed & Kazi Tower, Ground Floor", Location = "Dhanmondi", Code = "DHNG" }
        };

        return _cachedBranches.ToList();
    }

    public async Task<Branch?> GetBranchByIdAsync(int branchId)
    {
        var branches = await GetBranchesAsync();
        return branches.FirstOrDefault(b => b.Id == branchId);
    }

    private List<Branch> ParseBranches(string html)
    {
        var branches = new List<Branch>();
        
        // Parse branch names and locations from the HTML
        // Pattern: [Name | Location description...]
        var pattern = @"\[([^\|]+)\s*\|\s*([^\]]+)";
        var matches = Regex.Matches(html, pattern);

        int id = 1;
        foreach (Match match in matches)
        {
            if (match.Groups.Count >= 3)
            {
                var name = match.Groups[1].Value.Trim();
                var location = match.Groups[2].Value.Trim();
                
                // Extract just the area name (first part before description)
                var locationParts = location.Split(new[] { ' ' }, 2);
                var areaName = locationParts[0];
                
                // Generate a simple code from the area name
                var code = GenerateCode(areaName, id);

                branches.Add(new Branch
                {
                    Id = id++,
                    Name = name,
                    Location = location,
                    Code = code
                });
            }
        }

        return branches;
    }

    private string GenerateCode(string areaName, int id)
    {
        // Remove special characters and take first 3 letters, uppercase
        var cleaned = Regex.Replace(areaName, @"[^a-zA-Z]", "");
        if (cleaned.Length >= 3)
            return cleaned.Substring(0, 3).ToUpper() + id;
        
        return "BR" + id.ToString("D3");
    }
}
