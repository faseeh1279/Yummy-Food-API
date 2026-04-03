 using Microsoft.EntityFrameworkCore;
using Yummy_Food_API;
using Yummy_Food_API.Enums;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Repositories;
public class ComplaintsRepository
{
    private readonly ApplicationDBContext _dbContext;
    public ComplaintsRepository(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext; 
    }
    public async Task<List<Complaint>?> FetchAllComplaintsAsync()
    {
        var result = await _dbContext.Complaints.ToListAsync();
        if (result != null)
            return result;
        return null;
    }
    public async Task<Complaint?> UpdateComplaintStatus(Guid complaintID, ComplaintStatus complaintStatus)
    {
        var result = await _dbContext.Complaints.FirstOrDefaultAsync(c => c.Id == complaintID);
        if (result != null)
        {
            result.ComplaintStatus = complaintStatus;
            result.UpdatedAt = DateTime.UtcNow;
            _dbContext.Complaints.Update(result);
            await _dbContext.SaveChangesAsync();
        }
        return null;
    }
    
}
