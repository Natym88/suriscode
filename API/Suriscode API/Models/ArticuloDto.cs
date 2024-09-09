namespace Suriscode_API.Models
{
    /// <summary>
    /// Representa a la entidad Artículo del Archivo Json provisto.
    /// </summary>
    public class ArticuloDto
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public int Deposito { get; set; }
    }
}
