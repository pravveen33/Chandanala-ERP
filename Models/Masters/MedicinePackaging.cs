using Chandanala.ERP.Web.Models.Enums;

namespace Chandanala.ERP.Web.Models.Masters;

public class MedicinePackaging
{
    public int Id { get; set; }

    public PackType PackType { get; set; }

    public decimal PackSize { get; set; }

    public UnitOfMeasure Unit { get; set; }
}
