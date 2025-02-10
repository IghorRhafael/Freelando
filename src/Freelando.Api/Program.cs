using Freelando.Api.Converters;
using Freelando.Api.Endpoints;
using Freelando.Dados;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FreelandoContext>((options) =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});

builder.Services.AddTransient<FreelandoContext>();
builder.Services.AddTransient(typeof(CandidaturaConverter));
builder.Services.AddTransient(typeof(ClienteConverter));
builder.Services.AddTransient(typeof(ContratoConverter));
builder.Services.AddTransient(typeof(EspecialidadeConverter));
builder.Services.AddTransient(typeof(ProfissionalConverter));
builder.Services.AddTransient(typeof(ProjetoConverter));
builder.Services.AddTransient(typeof(ServicoConverter));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddEndPointCandidatura();
app.AddEndPointClientes();
app.AddEndPointContrato();
app.AddEndPointProfissional();
app.AddEndPointEspecialidade();
app.AddEndPointProjeto();
app.AddEndPointServico();
app.UseHttpsRedirection();

app.Run();
