using Ucommerce.Extensions.InProcess.Abstractions.Common;
using Ucommerce.Web.Common.Extensions;

namespace CustomUITest;

internal class ContextParser : IContextParser{
    public Task<RequestContext> ParseRequestContext(CancellationToken token)
    {
        return new RequestContext().InTask();
    }
}