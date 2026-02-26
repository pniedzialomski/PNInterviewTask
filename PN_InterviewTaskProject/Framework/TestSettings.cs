namespace PN_InterviewTaskProject.Framework;

using NUnit.Framework;

public static class TestSettings
{
    public static string BaseUrl =>
        TestContext.Parameters.Get(
            "baseUrl",
            Environment.GetEnvironmentVariable("BASE_URL")?.Trim() ?? "http://localhost:4200");
}
