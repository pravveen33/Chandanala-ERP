// Register the mock parser for local testing
services.AddTransient<IOcrInvoiceParser, MockOcrInvoiceParser>();
