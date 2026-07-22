using System;

namespace Chandanala.ERP.Web.Models;

public class Bill
{
    public int BillId { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }

    public decimal TotalSavings { get; set; }

    public string PaymentMode { get; set; } = string.Empty;
}