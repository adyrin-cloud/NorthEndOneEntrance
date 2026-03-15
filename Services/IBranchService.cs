namespace NorthEndOneEntrance.Services;

public interface IBranchService
{
    Task<List<Branch>> GetBranchesAsync();
    Task<Branch?> GetBranchByIdAsync(int branchId);
}

public class Branch
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
