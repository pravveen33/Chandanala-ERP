// Inside your AddApplicationServices extension method:

// Register the Pipeline Behavior
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AiProposalSecurityBehavior<,>));

// Register specific VGP Validators based on the Command Type
// In a real app, you would use Reflection/Scrutor to scan and register these automatically
services.AddTransient<IVgpValidator, AiInvoiceProposalVgpValidator>();
