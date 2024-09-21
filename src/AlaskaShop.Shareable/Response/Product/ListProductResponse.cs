using AlaskaShop.Shareable.Vos;
using AlaskaShop.Shareable.Vos.Product;

namespace AlaskaShop.Shareable.Response.Product;

public record ListProductResponse(ListProductVo[] List, PaginationVo PageInfo);
