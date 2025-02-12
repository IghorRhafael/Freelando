using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ContratoExtension
{
    public static void AddEndPointContratos(this WebApplication app)
    {
        //Retorna lista de contratos
        app.MapGet("/contratos", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            var contrato = converter.EntityListToResponseList(contexto.Contratos.ToList());
            var entries = contexto.ChangeTracker.Entries();
            return Results.Ok(await Task.FromResult(contrato));
        }).WithTags("Contrato").WithOpenApi();

        //retorna contrato por id
        app.MapGet("/contrato/{id}", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext contexto, Guid id) =>
        {
            var contrato = await contexto.Contratos.FindAsync(id);
            if (contrato is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(converter.EntityToResponse(contrato));
        }).WithTags("Contrato").WithOpenApi();

        //Cria um contrato
        app.MapPost("/contrato", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext contexto, ContratoRequest contratoRequest) =>
        {
            using var transaction = await contexto.Database.BeginTransactionAsync();
            try
            {
                transaction.CreateSavepoint("SavePoint");

                var contrato = converter.RequestToEntity(contratoRequest);
                await contexto.Contratos.AddAsync(contrato);
                await contexto.SaveChangesAsync();

                await transaction.CommitAsync();

                return Results.Created($"/contrato/{contrato.Id}", contrato);
            }
            catch(DbUpdateConcurrencyException ex)
            {
                transaction.RollbackToSavepoint("SavePoint");
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                transaction.RollbackToSavepoint("SavePoint");
                return Results.BadRequest($"Problemas de simultaneidade {ex.Message}");
            }

            
        }).WithTags("Contrato").WithOpenApi();

        //Atualiza um contrato
        app.MapPut("/contrato/{id}", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext contexto, Guid id, ContratoRequest contratoRequest) =>
        {
            var contrato = await contexto.Contratos.FindAsync(id);
            if (contrato is null)
            {
                return Results.NotFound();
            }
            var contratoAtualizado = converter.RequestToEntity(contratoRequest);
            contrato.Valor = contratoAtualizado.Valor;
            contrato.Vigencia = contratoAtualizado.Vigencia;
       
            await contexto.SaveChangesAsync();
            return Results.Ok(contrato);
        }).WithTags("Contrato").WithOpenApi();

        //Deleta um contrato
        app.MapDelete("/contrato/{id}", async ([FromServices] FreelandoContext contexto, Guid id) =>
        {
            var contrato = await contexto.Contratos.FindAsync(id);
            if (contrato is null)
            {
                return Results.NotFound();
            }
            contexto.Contratos.Remove(contrato);
            await contexto.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Contrato").WithOpenApi();
    }
}
