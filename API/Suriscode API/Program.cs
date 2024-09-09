
using Microsoft.AspNetCore.Mvc;
using Suriscode_API.Helper;
using Suriscode_API.Models;
using System.Text.RegularExpressions;

namespace Suriscode_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Configuraci�n de CORS (me traigo las URLs permitidas desde el appsettings.json)
            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder2 =>
            {
                builder2.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(builder.Configuration.GetSection("AllowedHosts").Get<string[]>())
                    .AllowCredentials();
            }));

            var app = builder.Build();

            app.UseCors("CorsPolicy");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            #region controllers
            // Como es tan peque�o el proyecto, arm� los controladores ac� directamente (tipo minimal API), en proyectos m�s grandes usar�a una carpeta Controllers y servicios para intermediar con la BD.

            var articulosFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Data\articulos.json");
            var vendedoresFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Data\vendedores.json");

            // Trae todos los art�culos del Dep�sito 1
            app.MapGet("/articulos", (HttpContext httpContext) =>
            {
                List<ArticuloDto> articulos = JsonFileHelper.ReadFromJsonFile<ArticuloDto>(articulosFilePath, "articulos");
                return articulos.Where(articulo => articulo.Deposito == 1);
            }).WithName("GetArticulos");

            // El path de Mocky no funciona, por lo tanto cre� otro en https://mocki.io/v1/5d1bc82b-21dd-4ba5-9870-4063d45e8ab0, A�n as�, armo el controller por las dudas:
            app.MapGet("/vendedores", (HttpContext context) =>
            {
                return JsonFileHelper.ReadFromJsonFile<VendedorDto>(vendedoresFilePath, "vendedores");
            }).WithName("GetVendedores");

            // Recibe un pedido del tipo PedidoDto y lo valida. Devuelve un mensaje de �xito si es v�lido y excepciones con sus detalles, en caso de falla.
            app.MapPost("/pedido", (PedidoQueryDto pedido) =>
            {
                try
                {
                    if (pedido != null && pedido.ArticuloIds.Count > 0)
                    {
                        List<ArticuloDto> articulos = JsonFileHelper.ReadFromJsonFile<ArticuloDto>(articulosFilePath, "articulos");
                        foreach (var art in pedido.ArticuloIds)
                        {
                            ArticuloDto articulo = articulos.FirstOrDefault(a => a.Codigo == art);
                            if (!(articulo.Precio > 0) || !Regex.IsMatch(articulo.Descripcion, @"^[ A-Za-z0-9]+$"))
                            {
                                throw new Exception($"El art�culo {articulo.Descripcion} no es v�lido. No se pudo completar la operaci�n");
                            }
                        }
                        // Si el valor de vendedores viene solo de Mocky, esta validaci�n no tiene sentido. O deber�a ir a buscarlos al endpoint correspondiente.
                        List<VendedorDto> vendedores = JsonFileHelper.ReadFromJsonFile<VendedorDto>(vendedoresFilePath, "vendedores");
                        if (!vendedores.Any(vendedor => vendedor.Id == pedido.VendedorId))
                        {
                            throw new Exception($"El vendedor con Id: {pedido.VendedorId} no es un vendedor registrado");
                        }
                        return Results.Ok("�El pedido fue generado con �xito!");
                    }
                    else
                    {
                        throw new Exception("El pedido no es v�lido");
                    }
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: 400);
                }
            }).WithName("CrearPedido");

            #endregion
            app.Run();
        }
    }
}
