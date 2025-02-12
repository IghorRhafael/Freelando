using Freelando.Api.Converters;
using Freelando.Api.Helpers;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ServicoExtensions
{
    public static void AddEndPointServicos(this WebApplication app)
    {
        //Retorna lista de servicos
        app.MapGet("/servicos", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            var servico = converter.EntityListToResponseList(contexto.Servicos.ToList());
            return Results.Ok(await Task.FromResult(servico));
        }).WithTags("Servicos").WithOpenApi();

        //retorna servico por id
        app.MapGet("/servico/{id}", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext contexto, Guid id) =>
        {
            var servico = await contexto.Servicos.FindAsync(id);
            if (servico is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(converter.EntityToResponse(servico));
        }).WithTags("Servicos").WithOpenApi();

        //Cria um servico
        app.MapPost("/servico", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext contexto, ServicoRequest servicoRequest) =>
        {
            var servico = converter.RequestToEntity(servicoRequest);
            await contexto.Servicos.AddAsync(servico);
            await contexto.SaveChangesAsync();
            return Results.Created($"/servico/{servico.Id}", servico);
        }).WithTags("Servicos").WithOpenApi();

        //Atualiza um servico
        app.MapPut("/servico/{id}", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext contexto, Guid id, ServicoRequest servicoRequest) =>
        {
            var servico = await contexto.Servicos.FindAsync(id);
            if (servico is null)
            {
                return Results.NotFound();
            }

            var servicoAtualizado = converter.RequestToEntity(servicoRequest);
            servico.Titulo = Helper.GetValue(servico.Titulo, servicoAtualizado.Titulo);
            servico.Descricao = Helper.GetValue(servico.Descricao, servicoAtualizado.Descricao);
            servico.Status = servicoAtualizado.Status;
            
            await contexto.SaveChangesAsync();
            return Results.Ok(servico);
        }).WithTags("Servicos").WithOpenApi();

        //Delete um servico
        app.MapDelete("/servico/{id}", async ([FromServices] FreelandoContext contexto, Guid id) =>
        {
            var servico = await contexto.Servicos.FindAsync(id);
            if (servico is null)
            {
                return Results.NotFound();
            }
            contexto.Servicos.Remove(servico);
            await contexto.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Servicos").WithOpenApi();
    }
}
