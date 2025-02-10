using Freelando.Dados;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ContratoExtension
{
    public static void AddEndPointContrato(this WebApplication app)
    {
        app.MapGet("/contratos", async (FreelandoContext context) =>
        {
            return Results.Ok(await context.Contratos.ToListAsync());
        });
    }
}
