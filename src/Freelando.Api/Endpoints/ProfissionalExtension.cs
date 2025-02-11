using Freelando.Api.Converters;
using Freelando.Api.Helpers;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ProfissionalExtension
{
    public static void AddEndPointProfissional(this WebApplication app)
    {
        //Retorna lista de profissionais
        app.MapGet("/profissionais", async ([FromServices] ProfissionalConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            var profissional = converter.EntityListToResponseList(contexto.Profissionais.Include(e => e.Especialidades).ToList());
            var entries = contexto.ChangeTracker.Entries();
            return Results.Ok(await Task.FromResult(profissional));
        }).WithTags("Profissional").WithOpenApi();

        //retorna profissional por id
        app.MapGet("/profissional/{id}", async ([FromServices] ProfissionalConverter converter, [FromServices] FreelandoContext contexto, Guid id) =>
        {
            var profissional = await contexto.Profissionais.Include(e => e.Especialidades).FirstOrDefaultAsync(e => e.Id == id);
            if (profissional is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(converter.EntityToResponse(profissional));
        }).WithTags("Profissional").WithOpenApi();

        //Cria um profissional
        app.MapPost("/profissional", async ([FromServices] ProfissionalConverter converter, [FromServices] FreelandoContext contexto, ProfissionalRequest profissionalRequest) =>
        {
            var profissional = converter.RequestToEntity(profissionalRequest);
            await contexto.Profissionais.AddAsync(profissional);
            await contexto.SaveChangesAsync();
            return Results.Created($"/profissional/{profissional.Id}", profissional);
        }).WithTags("Profissional").WithOpenApi();

        //Atualiza um profissional
        app.MapPut("/profissional/{id}", async ([FromServices] ProfissionalConverter converter, [FromServices] FreelandoContext contexto, Guid id, ProfissionalRequest profissionalRequest) =>
        {
            var profissional = await contexto.Profissionais.FindAsync(id);
            if (profissional is null)
            {
                return Results.NotFound();
            }
            var profissionalAtualizado = converter.RequestToEntity(profissionalRequest);
            
            profissional.Nome = Helper.GetValue(profissional.Nome,profissionalAtualizado.Nome);
            profissional.Cpf = Helper.GetValue(profissional.Cpf, profissionalAtualizado.Cpf);
            profissional.Email = Helper.GetValue(profissional.Email, profissionalAtualizado.Email);
            profissional.Telefone = Helper.GetValue(profissional.Telefone, profissionalAtualizado.Telefone);
            
            await contexto.SaveChangesAsync();
            return Results.Ok(profissional);
        }).WithTags("Profissional").WithOpenApi();

        //Deleta um profissional
        app.MapDelete("/profissional/{id}", async ([FromServices] FreelandoContext contexto, Guid id) =>
        {
            var profissional = await contexto.Profissionais.FindAsync(id);
            if (profissional is null)
            {
                return Results.NotFound();
            }
            contexto.Profissionais.Remove(profissional);
            await contexto.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Profissional").WithOpenApi();
    }
}
