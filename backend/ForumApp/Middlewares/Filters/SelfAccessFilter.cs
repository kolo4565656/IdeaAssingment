using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing.Template;
using System.Security.Claims;

namespace ForumApi.Middlewares.Filters
{
    public class SelfAccessFilter
    {
        private readonly string _type;

        private readonly string _route;

        private readonly string _refIdName;

        private readonly string _claimKey;

        public SelfAccessFilter(string type, string route, string refIdName, string claimKey)
        {
            _type = type;
            _route = route;
            _refIdName = refIdName;
            _claimKey = claimKey;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string text = CheckUserAccess(context.HttpContext);
            if (text != null)
            {
                context.Result = new ForbidResult("oidc");
            }
        }

        private string CheckUserAccess(HttpContext context)
        {
            ClaimsPrincipal user = context.User;
            Claim claim = user.Claims.FirstOrDefault((Claim x) => x.Type == _claimKey);
            if (claim != null)
            {
                RouteTemplate template = TemplateParser.Parse(_route);
                TemplateMatcher templateMatcher = new TemplateMatcher(template, null);
                PathString path = context.Request.Path;
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
                if (templateMatcher.TryMatch(path, routeValueDictionary))
                {
                    if (!routeValueDictionary.ContainsKey(_refIdName))
                    {
                        return _refIdName + " does not found in the route " + _route;
                    }

                    string text = routeValueDictionary[_refIdName].ToString();
                    if (string.Equals(text, claim.Value, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return null;
                    }

                    return $"{_type} does not have a valid access to {text} (expected {claim})";
                }

                return $"Can not match the template route {_route} in path {path}";
            }

            return "Can not find the " + _claimKey + " in the claim. ";
        }
    }
}
