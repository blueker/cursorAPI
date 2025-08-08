namespace weather.WebApi.Middleware;

using System.Text;
using System.Text.Json;
using weather.Application.Interfaces;

public sealed class EncryptionMiddleware
{
    private readonly RequestDelegate _next;

    public EncryptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICryptoService crypto)
    {
        // bypass swagger and health endpoints
        var path = context.Request.Path.Value ?? string.Empty;
        if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) || path.Equals("/health", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        await DecryptRequestBodyIfNeededAsync(context, crypto);

        var originalBody = context.Response.Body;
        using var memStream = new MemoryStream();
        context.Response.Body = memStream;

        try
        {
            await _next(context);

            memStream.Position = 0;
            var responseText = await new StreamReader(memStream).ReadToEndAsync();
            context.Response.Body = originalBody;

            // Only encrypt JSON responses
            if (IsJsonResponse(context) && !string.IsNullOrEmpty(responseText))
            {
                var cipher = crypto.Encrypt(responseText);
                var envelope = JsonSerializer.Serialize(new { data = cipher });
                context.Response.ContentType = "application/json";
                context.Response.ContentLength = Encoding.UTF8.GetByteCount(envelope);
                await context.Response.WriteAsync(envelope);
            }
            else
            {
                // write raw
                await context.Response.WriteAsync(responseText);
            }
        }
        finally
        {
            context.Response.Body = originalBody;
        }
    }

    private static bool IsJsonResponse(HttpContext context)
    {
        var contentType = context.Response.ContentType ?? string.Empty;
        return contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase)
            || contentType.Contains("text/json", StringComparison.OrdinalIgnoreCase)
            || string.IsNullOrEmpty(contentType); // allow encrypt if not set yet
    }

    private static async Task DecryptRequestBodyIfNeededAsync(HttpContext context, ICryptoService crypto)
    {
        if (!HttpMethods.IsPost(context.Request.Method) && !HttpMethods.IsPut(context.Request.Method) && !HttpMethods.IsPatch(context.Request.Method))
        {
            return;
        }

        if (!context.Request.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) ?? true)
        {
            return;
        }

        context.Request.EnableBuffering();
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
        if (string.IsNullOrWhiteSpace(body)) return;

        try
        {
            using var doc = JsonDocument.Parse(body);
            if (!doc.RootElement.TryGetProperty("data", out var dataProp)) return;
            var cipher = dataProp.GetString();
            if (string.IsNullOrWhiteSpace(cipher)) return;
            var json = crypto.Decrypt(cipher);

            var bytes = Encoding.UTF8.GetBytes(json);
            context.Request.Body = new MemoryStream(bytes);
            context.Request.ContentLength = bytes.Length;
        }
        catch
        {
            // not a valid envelope; ignore
        }
    }
}