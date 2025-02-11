
using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Freelando.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Freelando.Api.Endpoints;

public static class EspecialidadeExtension
{
    public static void AddEndPointEspecialidade(this WebApplication app)
    {
        //Retorna lista de especialidades
        app.MapGet("/especialidades", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            var especialidades = converter.EntityListToResponseList(contexto.Especialidades.ToList());

            return Results.Ok(await Task.FromResult(especialidades));

        }).WithTags("Especialidade").WithOpenApi();

        //retorna especialidade por id
        app.MapGet("/especialidadeByID/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto, Guid id) =>
        {
            var especialidade = await contexto.Especialidades.FindAsync(id);
            if (especialidade is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(converter.EntityToResponse(especialidade));
        }).WithTags("Especialidade").WithOpenApi();

        //retorna especialidade por id
        app.MapGet("/especialidade/{letraInicial}", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto, string letrainicial) =>
        {
            
            Expression<Func<Especialidade, bool>> filtro = null; // e => e.Descricao.StartsWith(letrainicial.ToUpper());

            if (letrainicial.Length == 1)
            {
                filtro = e => e.Descricao.StartsWith(letrainicial.ToUpper());
            }
            //else
            //{
            //    return Results.BadRequest("A letra inicial deve conter apenas um caractere");
            //}

            IQueryable<Especialidade> especialidades = contexto.Especialidades;
            if (filtro != null)
            {
                especialidades = contexto.Especialidades.Where(filtro);
            }
            

            return await especialidades.ToListAsync();


        }).WithTags("Especialidade").WithOpenApi();

        //Cria uma especialidade
        app.MapPost("/especialidade", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = converter.RequestToEntity(especialidadeRequest);

            //Validação da descrição da especialidade
            Func<Especialidade, bool> validarDescricao = e => !string.IsNullOrEmpty(e.Descricao) && char.IsUpper(e.Descricao[0]);
            if (!validarDescricao(especialidade))
            {
                return Results.BadRequest("A descrição da especialidade não pode estar em branco e deve começar com letra maiúscula");
            }

            await contexto.Especialidades.AddAsync(especialidade);
            await contexto.SaveChangesAsync();

            return Results.Created($"/especialidade/{especialidade.Id}", especialidade);

        }).WithTags("Especialidade").WithOpenApi();


        //Atualiza uma especialidade
        app.MapPut("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto, Guid id, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = await contexto.Especialidades.FindAsync(id);
            if (especialidade is null)
            {
                return Results.NotFound();
            }

            var especialidadeAtualizada = converter.RequestToEntity(especialidadeRequest);

            especialidade.Descricao = especialidadeAtualizada.Descricao;
            especialidade.Projetos = especialidadeAtualizada.Projetos;
            especialidade.Profissionais = especialidadeAtualizada.Profissionais;

            await contexto.SaveChangesAsync();

            return Results.Ok(especialidade);
        }).WithTags("Especialidade").WithOpenApi();

        //Deleta uma especialidade
        //app.MapDelete("/especialidade/{id}", async ([FromServices] FreelandoContext contexto, Guid id) =>
        //{
        //    var especialidade = await contexto.Especialidades.FindAsync(id);
        //    if (especialidade is null)
        //    {
        //        return Results.NotFound();
        //    }

        //    contexto.Especialidades.Remove(especialidade);
        //    await contexto.SaveChangesAsync();
        //    return Results.NoContent();

        //}).WithTags("Especialidade").WithOpenApi();

        //Deleta uma especialidade com transaction
        app.MapDelete("/especialidade/{id}", async ([FromServices] FreelandoContext contexto, Guid id) =>
        {
            using var transaction = await contexto.Database.BeginTransactionAsync();
            try
            {
                var especialidade = await contexto.Especialidades.FindAsync(id);
                if (especialidade is null)
                {
                    return Results.NotFound();
                }
                contexto.Especialidades.Remove(especialidade);
                await contexto.SaveChangesAsync();
                await transaction.CommitAsync();
                return Results.NoContent();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return Results.BadRequest();
            }
        }).WithTags("Especialidade").WithOpenApi();

    }
}
