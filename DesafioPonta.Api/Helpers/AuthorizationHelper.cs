namespace DesafioPonta.Api.Helpers
{
    public static class AuthorizationHelper
    {
        public static string GetTokenFromHeader(HttpRequest request)
        {
            string? token = request.Headers["Authorization"];
            token = token?.Replace("Bearer ", "");
            return token;
        }
    }
}
