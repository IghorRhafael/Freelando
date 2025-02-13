using Freelando.Api.Converters;
using Freelando.Api.Helpers;
using Freelando.Api.Requests;
using Freelando.Dados;
using Freelando.Dados.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ProjetoExtension
{
    public static void AddEndPointProjetos(this WebApplication app)
    {
        //Retorna lista de projetos
        app.MapGet("/projetos", async ([FromServices] ProjetoConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var projetos = converter.EntityListToResponseList(unitOfWork.contexto.Projetos.Include(p => p.Cliente).Include(p => p.Especialidades).ToList());
            return Results.Ok(await Task.FromResult(projetos));
        }).WithTags("Projeto").WithOpenApi();

        app.MapGet("/projetos/vigencia", async ([FromServices] ProjetoConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var projetos = unitOfWork.ProjetoRepository.GetAll();

            return Results.Ok(await Task.FromResult(projetos));
        }).WithTags("Projeto").WithOpenApi();

        //retorna projeto por id
        app.MapGet("/projeto/{id}", async ([FromServices] ProjetoConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var projeto = await unitOfWork.contexto.Projetos.Include(p => p.Cliente).Include(p => p.Especialidades).FirstOrDefaultAsync(p => p.Id == id);
            if (projeto is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(converter.EntityToResponse(projeto));
        }).WithTags("Projeto").WithOpenApi();

        //Cria um projeto
        app.MapPost("/projeto", async ([FromServices] ProjetoConverter converter, [FromServices] IUnitOfWork unitOfWork, ProjetoRequest projetoRequest) =>
        {
            var projeto = converter.RequestToEntity(projetoRequest);

            await unitOfWork.ProjetoRepository.Add(projeto);
            await unitOfWork.Commit();

            return Results.Created($"/projeto/{projeto.Id}", projeto);
        }).WithTags("Projeto").WithOpenApi();

        //Atualiza um projeto
        app.MapPut("/projeto/{id}", async ([FromServices] ProjetoConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id, ProjetoRequest projetoRequest) =>
        {
            var projeto = await unitOfWork.ProjetoRepository.GetById(p => p.Id == id);
            if (projeto is null)
            {
                return Results.NotFound();
            }
            var projetoAtualizado = converter.RequestToEntity(projetoRequest);
            projeto.Titulo = Helper.GetValue(projeto.Titulo, projetoAtualizado.Titulo);
            projeto.Descricao = Helper.GetValue(projeto.Descricao, projetoAtualizado.Descricao);
            projeto.Status = projetoAtualizado.Status;
            
            await unitOfWork.ProjetoRepository.Update(projeto);
            await unitOfWork.Commit();

            return Results.Ok(projeto);
        }).WithTags("Projeto").WithOpenApi();

        //Delete um projeto
        app.MapDelete("/projeto/{id}", async ([FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var projeto = await unitOfWork.ProjetoRepository.GetById(p => p.Id == id);
            if (projeto is null)
            {
                return Results.NotFound();
            }

            await unitOfWork.ProjetoRepository.Delete(projeto);
            await unitOfWork.Commit();

            return Results.NoContent();
        }).WithTags("Projeto").WithOpenApi();
    }
}
