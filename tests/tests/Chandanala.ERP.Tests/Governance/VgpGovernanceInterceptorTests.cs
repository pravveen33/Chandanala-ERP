using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Chandanala.ERP.Core.Governance;
using Chandanala.ERP.Core.Intelligence;
using Moq;
using Xunit;

namespace Chandanala.ERP.Tests.Governance;

// Sample domain command for testing
public record InventoryWriteDownCommand : IDomainCommand
{
    public required string ItemId { get; init; }
    public required int Quantity { get; init; }
    public required decimal EstimatedLossAmount { get; init; }
}

public class VgpGovernanceInterceptorTests
{
    private readonly Mock<IVgpPolicyEvaluator> _policyEvaluatorMock;
    private readonly Mock<ISecurityAuditLogger> _auditLoggerMock;
    private readonly VgpGovernanceInterceptor _interceptor;

    public VgpGovernanceInterceptorTests()
    {
        _policyEvaluatorMock = new Mock<IVgpPolicyEvaluator>();
        _auditLoggerMock = new Mock<ISecurityAuditLogger>();
        _interceptor = new VgpGovernanceInterceptor(
            _policyEvaluatorMock.Object, 
            _auditLoggerMock.Object);
    }

    private static GovernanceContext CreateTestContext(bool hasHumanApproval = false) => new()
    {
        TenantId = "tenant-warangal-01",
        UserId = "usr-mgr-01",
        CapabilityId = "cap-inv-analysis",
        HasHumanApproval = hasHumanApproval
    };

    [Fact]
    public async Task InterceptAndValidate_EmptyPayload_RejectsAndLogsViolation()
    {
        // Arrange
        var context = CreateTestContext();
        var emptyResult = new CapabilityResult { RawJson = "", IsAuthoritative = false };

        // Act
        var result = await _interceptor.InterceptAndValidateAsync<InventoryWriteDownCommand>(emptyResult, context);

        // Assert
        Assert.False(result.IsAuthorized);
        Assert.Contains("empty payload", result.RejectionReason, StringComparison.OrdinalIgnoreCase);

        _auditLoggerMock.Verify(a => a.LogViolationAsync(It.Is<GovernanceViolationRecord>(
            v => v.ViolationType == "EMPTY_PAYLOAD" && v.TenantId == context.TenantId)), Times.Once);
    }

    [Fact]
    public async Task InterceptAndValidate_MalformedJson_RejectsAndLogsSchemaMismatch()
    {
        // Arrange
        var context = CreateTestContext();
        var malformedResult = new CapabilityResult { RawJson = "{ invalid_json: ", IsAuthoritative = false };

        // Act
        var result = await _interceptor.InterceptAndValidateAsync<InventoryWriteDownCommand>(malformedResult, context);

        // Assert
        Assert.False(result.IsAuthorized);
        Assert.Contains("Payload schema mismatch", result.RejectionReason);

        _auditLoggerMock.Verify(a => a.LogViolationAsync(It.Is<GovernanceViolationRecord>(
            v => v.ViolationType == "MALFORMED_JSON")), Times.Once);
    }

    [Fact]
    public async Task InterceptAndValidate_PolicyEvaluatorRejects_BlocksExecution()
    {
        // Arrange
        var context = CreateTestContext();
        var validJson = JsonSerializer.Serialize(new InventoryWriteDownCommand
        {
            ItemId = "MED-PAR-500",
            Quantity = 100,
            EstimatedLossAmount = 2500.00m
        });
        var capabilityResult = new CapabilityResult { RawJson = validJson, IsAuthoritative = false };

        _policyEvaluatorMock
            .Setup(p => p.EvaluatePolicyAsync(It.IsAny<InventoryWriteDownCommand>(), context, It.IsAny<CancellationToken>()))
            .ReturnsAsync(PolicyEvaluationResult.Deny("User lacks scope for inventory adjustment."));

        // Act
        var result = await _interceptor.InterceptAndValidateAsync<InventoryWriteDownCommand>(capabilityResult, context);

        // Assert
        Assert.False(result.IsAuthorized);
        Assert.Contains("User lacks scope", result.RejectionReason);

        _auditLoggerMock.Verify(a => a.LogViolationAsync(It.Is<GovernanceViolationRecord>(
            v => v.ViolationType == "AUTHORIZATION_VIOLATION")), Times.Once);
    }

    [Fact]
    public async Task InterceptAndValidate_HighRiskWithoutHumanApproval_RequiresApproval()
    {
        // Arrange
        var context = CreateTestContext(hasHumanApproval: false);
        var validJson = JsonSerializer.Serialize(new InventoryWriteDownCommand
        {
            ItemId = "MED-EXP-100",
            Quantity = 500,
            EstimatedLossAmount = 75000.00m
        });
        var capabilityResult = new CapabilityResult { RawJson = validJson, IsAuthoritative = false };

        _policyEvaluatorMock
            .Setup(p => p.EvaluatePolicyAsync(It.IsAny<InventoryWriteDownCommand>(), context, It.IsAny<CancellationToken>()))
            .ReturnsAsync(PolicyEvaluationResult.Allow(RiskClass.HighRiskTransaction));

        // Act
        var result = await _interceptor.InterceptAndValidateAsync<InventoryWriteDownCommand>(capabilityResult, context);

        // Assert
        Assert.False(result.IsAuthorized);
        Assert.True(result.RequiresHumanApproval);
        Assert.NotNull(result.Command);
        Assert.Equal("MED-EXP-100", result.Command.ItemId);

        _auditLoggerMock.Verify(a => a.LogViolationAsync(It.Is<GovernanceViolationRecord>(
            v => v.ViolationType == "APPROVAL_REQUIRED")), Times.Once);
    }

    [Fact]
    public async Task InterceptAndValidate_ValidStandardCommand_AuthorizesForExecution()
    {
        // Arrange
        var context = CreateTestContext(hasHumanApproval: false);
        var validJson = JsonSerializer.Serialize(new InventoryWriteDownCommand
        {
            ItemId = "MED-VIT-200",
            Quantity = 10,
            EstimatedLossAmount = 150.00m
        });
        var capabilityResult = new CapabilityResult { RawJson = validJson, IsAuthoritative = false };

        _policyEvaluatorMock
            .Setup(p => p.EvaluatePolicyAsync(It.IsAny<InventoryWriteDownCommand>(), context, It.IsAny<CancellationToken>()))
            .ReturnsAsync(PolicyEvaluationResult.Allow(RiskClass.StandardOperational));

        // Act
        var result = await _interceptor.InterceptAndValidateAsync<InventoryWriteDownCommand>(capabilityResult, context);

        // Assert
        Assert.True(result.IsAuthorized);
        Assert.False(result.RequiresHumanApproval);
        Assert.NotNull(result.Command);
        Assert.Equal("MED-VIT-200", result.Command.ItemId);

        _auditLoggerMock.Verify(a => a.LogGovernancePassAsync(context, result.Command), Times.Once);
    }
}
