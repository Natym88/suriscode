
export interface pedido {
    vendedorId: number,
    articuloIds: string[]
}

export class Pedido implements pedido {
    vendedorId = 0;
    articuloIds = []
}