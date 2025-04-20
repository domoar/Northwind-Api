using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Figgle;

namespace Api;

[ExcludeFromCodeCoverage(Justification = "Banner Class")]
[GenerateFiggleText("FIGLETSTRING", "doom", "Northwind")]
public sealed partial class Banner : IDisposable {
  private readonly ILogger<Banner> _logger;

  public Banner(ILogger<Banner> logger) {
    _logger = logger;
  }

  private readonly static string USERNAME = Environment.UserName;
  private static string? DATE => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
  private static string? DOTNETVERSION => Environment.Version.ToString();
  private static string? DEVICENAME => Environment.MachineName;
  private static string? PROFILE => Environment.UserName;
  private static string? OSTYPE => Environment.OSVersion.Platform.ToString();
  private static string? OSVERSION => Environment.OSVersion.VersionString;
  private static string? IP_ADDRESS =>
  Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault()?.ToString() ?? "unknown host";
  private static string? RUNTIME => RuntimeInformation.FrameworkDescription;
  private static string? DOMAIN => "DOMAIN_NAME";
  private static string? DATABASE_CONFIG => "DATABASE_PROVIDER";
  public static string FIGLET => FIGLETSTRING;

  private string TEXT =>
   $"""
(c) Northwind, domoar (Manuel Dausmann)
API VERSION       {"v1"}
ENVIRONMENT       {"ENV"}
Dotnet Version    {DOTNETVERSION}
Runtime           {RUNTIME}
OS                {OSTYPE}
OS Version        {OSVERSION}
Date              {DATE}
Device Name       {DEVICENAME}
Username          {USERNAME}
Profile           {PROFILE}
IP                {IP_ADDRESS}
Domain            {DOMAIN}
Service Host      {"HOST"}
Service Port      {"PORT"}
Database          {DATABASE_CONFIG}
OpenAPI           {""}
Swagger           {""}
""";

  public void LogBanner() {
    var lines = FIGLET.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    var count = lines.Max(s => s.Length);

    LogSplit(count);

    foreach (var line in lines) {
      if (!string.IsNullOrWhiteSpace(line)) {
        _logger?.LogInformation("{Line}", line);
      }
    }

    LogSplit(count);

    var infos = TEXT.Split(["\r\n", "\n"], StringSplitOptions.None);
    foreach (var info in infos) {
      _logger?.LogInformation("{Info}", info);
    }

    LogSplit(count);
  }

  private bool _disposed;

  private void LogSplit(int count) {
    _logger?.LogInformation("");
    _logger?.LogInformation("{Seperator}", new string('-', count));
    _logger?.LogInformation("");
  }

  private void Dispose(bool disposing) {
    if (!_disposed) {
      _disposed = true;
    }
  }

  ~Banner() {
    Dispose(disposing: false);
  }

  void IDisposable.Dispose() {
    Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }
}
