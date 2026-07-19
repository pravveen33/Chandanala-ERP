using Chandanala.ERP.Web.Data; // This tells the file where to find AppDbContext
using Chandanala.ERP.Web.Models; // This ensures it can see the Bill class

namespace Chandanala.ERP.Web.Models
{
    public class BillService
    {
        private readonly AppDbContext _context;

        public BillService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveBillAsync(Bill bill)
        {
            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();
        }
    }
}