namespace Suriscode_API.Models
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public VendedorDto Vendedor { get; set; }
        public List<ArticuloDto> Articulos { get; set; }
    }

    public class PedidoQueryDto
    {
        public int VendedorId { get; set; }
        public List<string> ArticuloIds { get; set; }
    }
}
