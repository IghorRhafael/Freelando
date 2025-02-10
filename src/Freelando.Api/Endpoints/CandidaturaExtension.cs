using Freelando.Dados;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class CandidaturaExtension
{
    public static void AddEndPointCandidatura(this WebApplication app)
    {
        app.MapGet("/candidaturas", async (FreelandoContext context) =>
        {
            return Results.Ok(await context.Candidaturas.ToListAsync());
        });
    }
}
