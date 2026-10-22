using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Proyecto.WebAPI.Tests;

public sealed class TechnicianEndpointsTests
{
    [Fact]
    public async Task CatalogAndTechnicianEndpoints_ReturnSeededData()
    {
        using var factory = new ApiFactory();
        await factory.SeedAsync();
        using var client = factory.CreateClient();

        foreach (var path in new[]
                 {
                     "/api/locations",
                     "/api/maintenance-types",
                     "/api/specialties",
                     "/api/technicians"
                 })
        {
            using var response = await client.GetAsync(path);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            using var body = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            Assert.True(body.RootElement.GetProperty("data").GetArrayLength() > 0, path);
        }

        using var technicianResponse = await client.GetAsync($"/api/technicians/{ApiFactory.TechnicianId}");
        Assert.Equal(HttpStatusCode.OK, technicianResponse.StatusCode);
    }

    [Fact]
    public async Task TechnicianSpecialtyEndpoints_AssignReadAndRemoveSpecialty()
    {
        using var factory = new ApiFactory();
        await factory.SeedAsync();
        using var client = factory.CreateClient();
        var request = new
        {
            certificationLevel = "Advanced",
            certifiedSince = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1))
        };
        var assignmentPath =
            $"/api/technicians/{ApiFactory.TechnicianId}/specialties/{ApiFactory.SpecialtyId}";

        using var assignmentResponse = await client.PostAsJsonAsync(assignmentPath, request);
        Assert.Equal(HttpStatusCode.Created, assignmentResponse.StatusCode);

        using var technicianResponse =
            await client.GetAsync($"/api/technicians/{ApiFactory.TechnicianId}");
        Assert.Equal(HttpStatusCode.OK, technicianResponse.StatusCode);
        using (var technicianBody =
               await JsonDocument.ParseAsync(await technicianResponse.Content.ReadAsStreamAsync()))
        {
            var specialties = technicianBody.RootElement
                .GetProperty("data")
                .GetProperty("specialties");
            Assert.Contains(
                specialties.EnumerateArray(),
                specialty => specialty.GetProperty("id").GetInt32() == ApiFactory.SpecialtyId);
        }

        using var removalResponse = await client.DeleteAsync(assignmentPath);
        Assert.Equal(HttpStatusCode.NoContent, removalResponse.StatusCode);

        using var technicianAfterRemoval =
            await client.GetAsync($"/api/technicians/{ApiFactory.TechnicianId}");
        using var bodyAfterRemoval =
            await JsonDocument.ParseAsync(await technicianAfterRemoval.Content.ReadAsStreamAsync());
        Assert.Empty(bodyAfterRemoval.RootElement
            .GetProperty("data")
            .GetProperty("specialties")
            .EnumerateArray());
    }

    [Fact]
    public async Task TechnicianSpecialtyEndpoints_RejectInvalidDuplicateAndMissingAssignments()
    {
        using var factory = new ApiFactory();
        await factory.SeedAsync();
        using var client = factory.CreateClient();
        var assignmentPath =
            $"/api/technicians/{ApiFactory.TechnicianId}/specialties/{ApiFactory.SpecialtyId}";

        using var invalidLevelResponse = await client.PostAsJsonAsync(assignmentPath, new
        {
            certificationLevel = "Expert",
            certifiedSince = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1))
        });
        Assert.Equal(HttpStatusCode.BadRequest, invalidLevelResponse.StatusCode);

        using var futureDateResponse = await client.PostAsJsonAsync(assignmentPath, new
        {
            certificationLevel = "Advanced",
            certifiedSince = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1))
        });
        Assert.Equal(HttpStatusCode.BadRequest, futureDateResponse.StatusCode);

        var validRequest = new
        {
            certificationLevel = "Basic",
            certifiedSince = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1))
        };
        using var firstAssignment = await client.PostAsJsonAsync(assignmentPath, validRequest);
        Assert.Equal(HttpStatusCode.Created, firstAssignment.StatusCode);

        using var duplicateAssignment = await client.PostAsJsonAsync(assignmentPath, validRequest);
        Assert.Equal(HttpStatusCode.Conflict, duplicateAssignment.StatusCode);

        using var missingTechnician = await client.PostAsJsonAsync(
            $"/api/technicians/{ApiFactory.TechnicianId + 1}/specialties/{ApiFactory.SpecialtyId}",
            validRequest);
        Assert.Equal(HttpStatusCode.NotFound, missingTechnician.StatusCode);
    }
}
