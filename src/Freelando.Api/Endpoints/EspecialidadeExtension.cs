using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados.UnitOfWork;
using Freelando.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace Freelando.Api.Endpoints;

public static class EspecialidadeExtension
{
    public static void AddEndPointEspecialidades(this WebApplication app)
    {
        //Retorna lista de especialidades
        app.MapGet("/especialidades", async ([FromServices] EspecialidadeConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var especialidades = unitOfWork.EspecialidadeRepository.GetAll();

            return Results.Ok(await Task.FromResult(especialidades));
        }).WithTags("Especialidade").WithOpenApi();

        //retorna especialidade por id
        app.MapGet("/especialidade/ID/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var especialidade = await unitOfWork.EspecialidadeRepository.GetById(e => e.Id == id);
            if (especialidade is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(converter.EntityToResponse(especialidade));
        }).WithTags("Especialidade").WithOpenApi();

        //retorna especialidade por descrição
        app.MapGet("/especialidade/{letraInicial}", async ([FromServices] EspecialidadeConverter converter, [FromServices] IUnitOfWork unitOfWork, string letrainicial) =>
        {
            if(string.IsNullOrEmpty(letrainicial) || letrainicial.Length > 1)
            {
                return Results.BadRequest("A letra inicial deve ser informada e conter apenas um caractere");
            }

            var especialidade = unitOfWork.contexto.Especialidades.Where(e => e.Descricao!.StartsWith(letrainicial)).ToList();

            if (especialidade is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(especialidade);

        }).WithTags("Especialidade").WithOpenApi();

        //Cria uma especialidade
        app.MapPost("/especialidade", async ([FromServices] EspecialidadeConverter converter, [FromServices] IUnitOfWork unitOfWork, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = converter.RequestToEntity(especialidadeRequest);

            //Validação da descrição da especialidade
            Func<Especialidade, bool> validarDescricao = e => !string.IsNullOrEmpty(e.Descricao) && char.IsUpper(e.Descricao[0]);
            if (!validarDescricao(especialidade))
            {
                return Results.BadRequest("A descrição da especialidade não pode estar em branco e deve começar com letra maiúscula");
            }

            await unitOfWork.EspecialidadeRepository.Add(especialidade);
            await unitOfWork.Commit();

            return Results.Created($"/especialidade/{especialidade.Id}", especialidade);

        }).WithTags("Especialidade").WithOpenApi();


        //Atualiza uma especialidade
        app.MapPut("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = await unitOfWork.EspecialidadeRepository.GetById(e => e.Id == id);
            if (especialidade is null)
            {
                return Results.NotFound();
            }

            var especialidadeAtualizada = converter.RequestToEntity(especialidadeRequest);

            especialidade.Descricao = especialidadeAtualizada.Descricao;
            especialidade.Projetos = especialidadeAtualizada.Projetos;
            especialidade.Profissionais = especialidadeAtualizada.Profissionais;

            await unitOfWork.EspecialidadeRepository.Update(especialidade);
            await unitOfWork.Commit();

            return Results.Ok(especialidade);
        }).WithTags("Especialidade").WithOpenApi();

        //Deleta uma especialidade com transaction
        app.MapDelete("/especialidade/{id}", async ([FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            using (var transaction = unitOfWork.contexto.Database.BeginTransaction())
            {
                try
                {
                    var especialidade = await unitOfWork.EspecialidadeRepository.GetById(e => e.Id == id);
                    if (especialidade is null)
                    {
                        return Results.NotFound();
                    }
                    await unitOfWork.EspecialidadeRepository.Delete(especialidade);
                    await unitOfWork.Commit();
                    await transaction.CommitAsync();
                    return Results.NoContent();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return Results.BadRequest();
                }
            }

        }).WithTags("Especialidade").WithOpenApi();

    }
}
