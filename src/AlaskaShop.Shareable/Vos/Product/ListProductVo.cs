﻿namespace AlaskaShop.Shareable.Vos.Product;

public class ListProductVo
{
    public long Id { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; } = default!;
    public string Image { get; set; } = string.Empty;
}
