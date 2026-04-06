using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMwise.Application.Interfaces;

namespace STEMwise.API.Controllers;

/// <summary>
/// Provides high-level ROI, Visa, and Loan calculations for simulation.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CalculationController : ControllerBase
{
    private readonly ICalculationService _calculationService;

    public CalculationController(ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }

    /// <summary>
    /// Calculates the 10-year ROI and Break-Even point for a given study scenario.
    /// </summary>
    /// <remarks>
    /// **Key Points:**
    /// - Includes Opportunity Cost (Current Salary * Duration).
    /// - Includes Automated Currency Conversion if Study vs Home currency differs.
    /// - Applies a 25% global tax bracket to net earnings.
    /// </remarks>
    [HttpPost("roi")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ROIResult>> CalculateROI([FromBody] ROIRequest request)
    {
        var result = await _calculationService.CalculateROIAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Calculates cumulative H-1B success probability across multiple lottery attempts.
    /// </summary>
    [HttpPost("visa")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<VisaResult>> CalculateVisa([FromBody] VisaRequest request)
    {
        var result = await _calculationService.CalculateVisaProbabilityAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Calculates standard loan amortization, monthly EMI, and total interest.
    /// </summary>
    [HttpPost("loan")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LoanResult>> CalculateLoan([FromBody] LoanRequest request)
    {
        var result = await _calculationService.CalculateLoanAmortizationAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Compares ROI results across multiple study scenarios.
    /// </summary>
    /// <param name="requests">List of ROI scenarios to compare.</param>
    [HttpPost("compare")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ROIResult>>> Compare([FromBody] IEnumerable<ROIRequest> requests)
    {
        var results = await _calculationService.CompareROIAsync(requests);
        return Ok(results);
    }
}
