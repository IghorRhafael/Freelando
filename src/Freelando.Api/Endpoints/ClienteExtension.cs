using Freelando.Dados;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ClienteExtension
{
    public static void AddEndPointClientes(this WebApplication app)
    {
        app.MapGet("/clientes", async (FreelandoContext context) =>
        {
            return Results.Ok(await context.Clientes.ToListAsync());
        });
    }
}
