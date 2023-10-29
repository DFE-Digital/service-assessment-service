using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ServiceAssessmentService.WebApp.Controllers;


public class DebugController : Controller
{
    private readonly IActionDescriptorCollectionProvider _provider;
    private readonly IEnumerable<EndpointDataSource> _endpointSources;

    public DebugController(IActionDescriptorCollectionProvider provider, IEnumerable<EndpointDataSource> endpointSources)
    {
        _provider = provider;
        _endpointSources = endpointSources;
    }

    public IActionResult RouteDebug()
    {
        // var urls = _provider.ActionDescriptors.Items
        //     .Select(descriptor => '/' + string.Join('/', descriptor.RouteValues.Values
        //         .Where(v => v != null)
        //         .Reverse()))
        //     .Distinct()
        //     .ToList();
        //
        // return Ok(urls);

        var objs = new List<object>();

        var sb = new StringBuilder();
        var endpoints = _endpointSources.SelectMany(es => es.Endpoints);
        foreach (var endpoint in endpoints)
        {
            if (endpoint is RouteEndpoint routeEndpoint)
            {
                _ = routeEndpoint.RoutePattern.RawText;
                _ = routeEndpoint.RoutePattern.PathSegments;
                _ = routeEndpoint.RoutePattern.Parameters;
                _ = routeEndpoint.RoutePattern.InboundPrecedence;
                _ = routeEndpoint.RoutePattern.OutboundPrecedence;

                var routePatternRequiredValues = routeEndpoint.RoutePattern.RequiredValues;
                // to string of key/value
                var str = routePatternRequiredValues.Select(pair => pair.Key + ": " + (pair.Value ?? "<not specified>"));
                sb.Append($"{string.Join("\t\t\t", str)} -- ");

                var area = routePatternRequiredValues.FirstOrDefault(kv => kv.Key.Equals("area", StringComparison.Ordinal)).Value;
                var controller = routePatternRequiredValues.FirstOrDefault(kv => kv.Key.Equals("controller", StringComparison.Ordinal)).Value;
                var action = routePatternRequiredValues.FirstOrDefault(kv => kv.Key.Equals("action", StringComparison.Ordinal)).Value;
                var uri = "";
                if (area is not null) uri += $"{area}/";
                if (controller is not null) uri += $"{controller}/";
                if (action is not null) uri += $"{action}/";
                objs.Add(new
                {
                    RawText = routeEndpoint.RoutePattern.RawText,
                    Area = area,
                    Controller = controller,
                    Action = action,
                    Methods = endpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods,
                    DisplayName = endpoint.DisplayName,
                    RouteNameMetadata = endpoint.Metadata.OfType<Microsoft.AspNetCore.Routing.RouteNameMetadata>().FirstOrDefault(),
                    RouteUrl = uri,

                }
                );
            }

            var routeNameMetadata = endpoint.Metadata.OfType<Microsoft.AspNetCore.Routing.RouteNameMetadata>().FirstOrDefault();
            _ = routeNameMetadata?.RouteName;

            var httpMethodsMetadata = endpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault();
            //_ = httpMethodsMetadata?.HttpMethods; // [GET, POST, ...]
            var methods = string.Join(", ", httpMethodsMetadata?.HttpMethods ?? Array.Empty<string>());

            // There are many more metadata types available...

            sb.AppendLine($"{endpoint.DisplayName}");
        }

        //return Ok(sb.ToString());
        return Ok(objs);
        //return Ok(endpoints);

    }
}
