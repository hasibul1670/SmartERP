using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SmartERP.Api.Conventions;

public sealed class GlobalRoutePrefixConvention : IApplicationModelConvention
{
    private readonly AttributeRouteModel _prefix;

    public GlobalRoutePrefixConvention(string prefix)
    {
        _prefix = new AttributeRouteModel(new RouteAttribute(prefix));
    }

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        foreach (var selector in controller.Selectors)
            selector.AttributeRouteModel =
                selector.AttributeRouteModel == null
                    ? _prefix
                    : AttributeRouteModel.CombineAttributeRouteModel(
                        _prefix,
                        selector.AttributeRouteModel);
    }
}