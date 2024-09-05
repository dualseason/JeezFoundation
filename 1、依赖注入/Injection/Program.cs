using Injection;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ServiceCollection();

services.AddTransient<ProductService>();

var provider = services.BuildServiceProvider();

ProductService? productService = provider.GetService<ProductService>();

productService?.getProduct();
