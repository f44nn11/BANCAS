<%@ Application Language="VB" %>
<%@ Import Namespace="System.Net.Http.Headers" %>
<%@ Import Namespace="System.Net.Http.Formatting" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Web.Routing" %>
<script runat="server">
    Sub Application_Start(sender As Object, e As EventArgs)
        'format json
        GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear()
        GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(New QueryStringMapping("type", "json", New MediaTypeHeaderValue("application/json")))
        
        'format xml
        'GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedMediaTypes.Clear()
        'GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(New QueryStringMapping("type", "xml", New MediaTypeHeaderValue("application/xml")))
        
        
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)

        
        'RouteTable.Routes.MapHttpRoute("DefaultApi",
        '                              "api/{action}/{controller}/{id}",
        '                              defaults:=New With {.id = System.Web.Http.RouteParameter.Optional})
        RouteTable.Routes.MapHttpRoute("DefaultApi",
                                      "{action}/{controller}/{id}",
                                      defaults:=New With {.id = System.Web.Http.RouteParameter.Optional})
    End Sub
    
</script>