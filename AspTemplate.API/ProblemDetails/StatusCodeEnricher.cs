using Sstv.DomainExceptions.DebugViewer;
using System.Diagnostics.CodeAnalysis;

namespace AspTemplate.API.ProblemDetails;

[ExcludeFromCodeCoverage(Justification = "Not used in request processing")]
internal sealed class StatusCodeEnricher : IDomainExceptionDebugEnricher
{
    /// <summary>
    /// Erich <paramref name="domainExceptionCodeDebugVm"/>.
    /// </summary>
    /// <param name="domainExceptionCodeDebugVm">Domain exception debug view model.</param>
    public void Enrich(DomainExceptionCodeDebugVm domainExceptionCodeDebugVm)
    {
        domainExceptionCodeDebugVm.AdditionalData ??= [];
        domainExceptionCodeDebugVm.AdditionalData["HttpStatusCode"] =
            ErrorCodeMapping.MapToStatusCode(domainExceptionCodeDebugVm.Code).ToString();
    }
}