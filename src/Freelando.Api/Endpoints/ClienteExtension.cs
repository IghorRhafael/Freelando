using Freelando.Api.Converters;
using Freelando.Api.Helpers;
using Freelando.Api.Requests;
using Freelando.Dados;
using Freelando.Dados.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ClienteExtension
{
    public static void AddEndPointClientes(this WebApplication app)
    {
        //Retorna lista de clientes
        app.MapGet("/clientes", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var clientes = converter.EntityListToResponseList(await unitOfWork.ClienteRepository.GetAll());
            
            return Results.Ok(await Task.FromResult(clientes));
        }).WithTags("Cliente").WithOpenApi();

        app.MapGet("/clientes/IdentificadorNome", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var clientes = unitOfWork.contexto.Clientes.Select(c => new { Identificador = c.Id, Nome = c.Nome });
            return Results.Ok(await Task.FromResult(clientes));
        }).WithTags("Cliente").WithOpenApi();

        app.MapGet("/clientes/projeto-especialidade", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var clientes = unitOfWork.contexto.Clientes.Include(x => x.Projetos).ThenInclude(p => p.Especialidades).AsSplitQuery().ToList();

            return Results.Ok(await Task.FromResult(clientes));
        }).WithTags("Cliente").WithOpenApi();

        app.MapGet("/clientes/por-email", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork, string email) =>
        {
            var clientes = unitOfWork.contexto.Clientes.Where(c => c.Email!.Equals(email)).ToList();

            return Results.Ok(await Task.FromResult(clientes));
        }).WithTags("Cliente").WithOpenApi();

        //retorna cliente por id
        app.MapGet("/cliente/{id}", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var cliente = await unitOfWork.ClienteRepository.GetById(c => c.Id == id);
            if (cliente is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(converter.EntityToResponse(cliente));
        }).WithTags("Cliente").WithOpenApi();

        //Cria um cliente
        app.MapPost("/cliente", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork, ClienteRequest clienteRequest) =>
        {
            if(string.IsNullOrEmpty(clienteRequest.Email))
            {
                return Results.BadRequest("Email é obrigatório");
            }

            if (unitOfWork.contexto.Clientes.Any(c => c.Email!.Equals(clienteRequest.Email)))
            {
                return Results.BadRequest("Email já cadastrado");
            }

            var cliente = converter.RequestToEntity(clienteRequest);
            await unitOfWork.ClienteRepository.Add(cliente);
            await unitOfWork.Commit();

            return Results.Created($"/cliente/{cliente.Id}", cliente);
        }).WithTags("Cliente").WithOpenApi();

        //Atualiza um cliente
        app.MapPut("/cliente/{id}", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id, ClienteRequest clienteRequest) =>
        {
            var cliente = await unitOfWork.ClienteRepository.GetById(c => c.Id == id);
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

            await unitOfWork.ClienteRepository.Update(cliente);
            await unitOfWork.Commit();

            return Results.Ok(cliente);
        }).WithTags("Cliente").WithOpenApi();

        //Deleta um cliente
        app.MapDelete("/cliente/{id}", async ([FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var cliente = await unitOfWork.ClienteRepository.GetById(c => c.Id == id);
            if (cliente is null)
            {
                return Results.NotFound();
            }

            await unitOfWork.ClienteRepository.Delete(cliente);
            await unitOfWork.Commit();

            return Results.NoContent();
        }).WithTags("Cliente").WithOpenApi();
    }

}
