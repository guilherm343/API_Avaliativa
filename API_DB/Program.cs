using API_DB.Models.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

/**** Configurando e registrando a conexão com o banco de dados ****/

// Obter a string de conexão do arquivo appsettings.json
string stringDeConexao = builder.Configuration.GetConnectionString("ConexaoSqlServer");

// Registrado a classe de conexão com o banco de dados
builder.Services.AddSingleton<IConexao>(new ConexaoSql(stringDeConexao));

builder.Services.AddScoped<AlunoDB, AlunoDB>();
builder.Services.AddScoped<CursoDB, CursoDB>();
builder.Services.AddScoped<MatriculaDB, MatriculaDB>();
builder.Services.AddScoped<TipoAlunoDB, TipoAlunoDB>();

/********************************************************************/


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
