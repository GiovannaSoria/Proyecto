using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLC
{
    public interface IProductService
    {
        // Crear un producto
        Products CreateProduct(Products products);

        // Eliminar un producto por ID
        bool Delete(int id);

        // Obtener todos los productos
        List<Products> GetAll();

        // Obtener un producto por ID
        Products GetById(int id);

        // Actualizar un producto
        bool UpdateProduct(Products products);
    }
}
