using Freelando.Api.Converters;
using Freelando.Api.Helpers;
using Freelando.Api.Requests;
using Freelando.Dados.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Freelando.Api.Endpoints;

public static class ServicoExtensions
{
    public static void AddEndPointServicos(this WebApplication app)
    {
        //Retorna lista de servicos
        app.MapGet("/servicos", async ([FromServices] ServicoConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var servico = converter.EntityListToResponseList(await unitOfWork.ServicoRepository.GetAll());
            return Results.Ok(await Task.FromResult(servico));
        }).WithTags("Servicos").WithOpenApi();

        //retorna servico por id
        app.MapGet("/servico/{id}", async ([FromServices] ServicoConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var servico = await unitOfWork.ServicoRepository.GetById(s => s.Id == id);
            if (servico is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(converter.EntityToResponse(servico));
        }).WithTags("Servicos").WithOpenApi();

        //Cria um servico
        app.MapPost("/servico", async ([FromServices] ServicoConverter converter, [FromServices] IUnitOfWork unitOfWork, ServicoRequest servicoRequest) =>
        {
            var servico = converter.RequestToEntity(servicoRequest);
            
            await unitOfWork.ServicoRepository.Add(servico);
            await unitOfWork.Commit();

            return Results.Created($"/servico/{servico.Id}", servico);
        }).WithTags("Servicos").WithOpenApi();

        //Atualiza um servico
        app.MapPut("/servico/{id}", async ([FromServices] ServicoConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id, ServicoRequest servicoRequest) =>
        {
            var servico = await unitOfWork.ServicoRepository.GetById(s => s.Id == id);
            if (servico is null)
            {
                return Results.NotFound();
            }

            var servicoAtualizado = converter.RequestToEntity(servicoRequest);
            servico.Titulo = Helper.GetValue(servico.Titulo, servicoAtualizado.Titulo);
            servico.Descricao = Helper.GetValue(servico.Descricao, servicoAtualizado.Descricao);
            servico.Status = servicoAtualizado.Status;
            
            await unitOfWork.ServicoRepository.Update(servico);
            await unitOfWork.Commit();

            return Results.Ok(servico);
        }).WithTags("Servicos").WithOpenApi();

        //Delete um servico
        app.MapDelete("/servico/{id}", async ([FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var servico = await unitOfWork.ServicoRepository.GetById(s => s.Id == id);
            if (servico is null)
            {
                return Results.NotFound();
            }

            await unitOfWork.ServicoRepository.Delete(servico);
            await unitOfWork.Commit();

            return Results.NoContent();
        }).WithTags("Servicos").WithOpenApi();
    }
}
