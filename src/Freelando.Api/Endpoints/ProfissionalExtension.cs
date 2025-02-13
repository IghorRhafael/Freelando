using Freelando.Api.Converters;
using Freelando.Api.Helpers;
using Freelando.Api.Requests;
using Freelando.Dados;
using Freelando.Dados.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ProfissionalExtension
{
    public static void AddEndPointProfissionais(this WebApplication app)
    {
        //Retorna lista de profissionais
        app.MapGet("/profissionais", async ([FromServices] ProfissionalConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var profissional = converter.EntityListToResponseList(unitOfWork.contexto.Profissionais.Include(e => e.Especialidades).ToList());
            var entries = unitOfWork.contexto.ChangeTracker.Entries();
            return Results.Ok(await Task.FromResult(profissional));
        }).WithTags("Profissional").WithOpenApi();

        //retorna profissional por id
        app.MapGet("/profissional/{id}", async ([FromServices] ProfissionalConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var profissional = await unitOfWork.contexto.Profissionais.Include(e => e.Especialidades).FirstOrDefaultAsync(e => e.Id == id);
            if (profissional is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(converter.EntityToResponse(profissional));
        }).WithTags("Profissional").WithOpenApi();

        //Cria um profissional
        app.MapPost("/profissional", async ([FromServices] ProfissionalConverter converter, [FromServices] IUnitOfWork unitOfWork, ProfissionalRequest profissionalRequest) =>
        {
            var profissional = converter.RequestToEntity(profissionalRequest);

            await unitOfWork.ProfissionalRepository.Add(profissional);
            await unitOfWork.Commit();

            return Results.Created($"/profissional/{profissional.Id}", profissional);
        }).WithTags("Profissional").WithOpenApi();

        //Atualiza um profissional
        app.MapPut("/profissional/{id}", async ([FromServices] ProfissionalConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id, ProfissionalRequest profissionalRequest) =>
        {
            var profissional = await unitOfWork.ProfissionalRepository.GetById(p => p.Id == id);
            if (profissional is null)
            {
                return Results.NotFound();
            }
            var profissionalAtualizado = converter.RequestToEntity(profissionalRequest);
            
            profissional.Nome = Helper.GetValue(profissional.Nome,profissionalAtualizado.Nome);
            profissional.Cpf = Helper.GetValue(profissional.Cpf, profissionalAtualizado.Cpf);
            profissional.Email = Helper.GetValue(profissional.Email, profissionalAtualizado.Email);
            profissional.Telefone = Helper.GetValue(profissional.Telefone, profissionalAtualizado.Telefone);
            
            await unitOfWork.ProfissionalRepository.Update(profissional);
            await unitOfWork.Commit();

            return Results.Ok(profissional);
        }).WithTags("Profissional").WithOpenApi();

        //Deleta um profissional
        app.MapDelete("/profissional/{id}", async ([FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var profissional = await unitOfWork.ProfissionalRepository.GetById(p => p.Id == id);
            if (profissional is null)
            {
                return Results.NotFound();
            }

            await unitOfWork.ProfissionalRepository.Delete(profissional);
            await unitOfWork.Commit();

            return Results.NoContent();
        }).WithTags("Profissional").WithOpenApi();
    }
}
