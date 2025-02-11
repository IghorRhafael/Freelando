using Freelando.Api.Converters;
using Freelando.Api.Helpers;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ClienteExtension
{
    public static void AddEndPointClientes(this WebApplication app)
    {
        //Retorna lista de clientes
        app.MapGet("/clientes", async ([FromServices] ClienteConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            var clientes = converter.EntityListToResponseList(contexto.Clientes.ToList());
            var entries = contexto.ChangeTracker.Entries();
            return Results.Ok(await Task.FromResult(clientes));
        }).WithTags("Cliente").WithOpenApi();

        //retorna cliente por id
        app.MapGet("/cliente/{id}", async ([FromServices] ClienteConverter converter, [FromServices] FreelandoContext contexto, Guid id) =>
        {
            var cliente = await contexto.Clientes.FindAsync(id);
            if (cliente is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(converter.EntityToResponse(cliente));
        }).WithTags("Cliente").WithOpenApi();

        //Cria um cliente
        app.MapPost("/cliente", async ([FromServices] ClienteConverter converter, [FromServices] FreelandoContext contexto, ClienteRequest clienteRequest) =>
        {
            var cliente = converter.RequestToEntity(clienteRequest);
            await contexto.Clientes.AddAsync(cliente);
            await contexto.SaveChangesAsync();
            return Results.Created($"/cliente/{cliente.Id}", cliente);
        }).WithTags("Cliente").WithOpenApi();

        //Atualiza um cliente
        app.MapPut("/cliente/{id}", async ([FromServices] ClienteConverter converter, [FromServices] FreelandoContext contexto, Guid id, ClienteRequest clienteRequest) =>
        {
            var cliente = await contexto.Clientes.FindAsync(id);
            if (cliente is null)
            {
                return Results.NotFound();
            }
            var clienteAtualizado = converter.RequestToEntity(clienteRequest);

            cliente.Nome = Helper.GetValue(cliente.Nome, clienteAtualizado.Nome);
            cliente.Email = Helper.GetValue(cliente.Email, clienteAtualizado.Email);
            cliente.Telefone = Helper.GetValue(cliente.Telefone, clienteAtualizado.Telefone);
            cliente.Cpf = Helper.GetValue(cliente.Cpf, clienteAtualizado.Cpf);

            cliente.Projetos = clienteAtualizado.Projetos;

            await contexto.SaveChangesAsync();
            return Results.Ok(cliente);
        }).WithTags("Cliente").WithOpenApi();

        //Deleta um cliente
        app.MapDelete("/cliente/{id}", async ([FromServices] FreelandoContext contexto, Guid id) =>
        {
            var cliente = await contexto.Clientes.FindAsync(id);
            if (cliente is null)
            {
                return Results.NotFound();
            }
            contexto.Clientes.Remove(cliente);
            await contexto.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Cliente").WithOpenApi();
    }

}
