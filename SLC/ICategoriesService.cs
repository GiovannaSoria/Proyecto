using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLC
{
    public interface ICategoriesService
    {
        // Crear una categoria
        Categories CreateCategory(Categories categories);

        // Eliminar una categoria por ID
        bool Delete(int id);

        // Obtener todas las categoria
        List<Categories> GetAll();

        // Obtener una categoria por ID
        Categories GetById(int id);

        // Actualizar una categoria
        bool UpdateCategory(Categories categories);
    }
}
