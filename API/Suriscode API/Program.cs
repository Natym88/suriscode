
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

            //Configuración de CORS (me traigo las URLs permitidas desde el appsettings.json)
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
            // Como es tan pequeño el proyecto, armé los controladores acá directamente (tipo minimal API), en proyectos más grandes usaría una carpeta Controllers y servicios para intermediar con la BD.

            var articulosFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Data\articulos.json");
            var vendedoresFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Data\vendedores.json");

            // Trae todos los artículos del Depósito 1
            app.MapGet("/articulos", (HttpContext httpContext) =>
            {
                List<ArticuloDto> articulos = JsonFileHelper.ReadFromJsonFile<ArticuloDto>(articulosFilePath, "articulos");
                return articulos.Where(articulo => articulo.Deposito == 1);
            }).WithName("GetArticulos");

            // El path de Mocky no funciona, por lo tanto creé otro en https://mocki.io/v1/5d1bc82b-21dd-4ba5-9870-4063d45e8ab0, Aún así, armo el controller por las dudas:
            app.MapGet("/vendedores", (HttpContext context) =>
            {
                return JsonFileHelper.ReadFromJsonFile<VendedorDto>(vendedoresFilePath, "vendedores");
            }).WithName("GetVendedores");

            // Recibe un pedido del tipo PedidoDto y lo valida. Devuelve un mensaje de éxito si es válido y excepciones con sus detalles, en caso de falla.
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
                                throw new Exception($"El artículo {articulo.Descripcion} no es válido. No se pudo completar la operación");
                            }
                        }
                        // Si el valor de vendedores viene solo de Mocky, esta validación no tiene sentido. O debería ir a buscarlos al endpoint correspondiente.
                        List<VendedorDto> vendedores = JsonFileHelper.ReadFromJsonFile<VendedorDto>(vendedoresFilePath, "vendedores");
                        if (!vendedores.Any(vendedor => vendedor.Id == pedido.VendedorId))
                        {
                            throw new Exception($"El vendedor con Id: {pedido.VendedorId} no es un vendedor registrado");
                        }
                        return Results.Ok("¡El pedido fue generado con éxito!");
                    }
                    else
                    {
                        throw new Exception("El pedido no es válido");
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
