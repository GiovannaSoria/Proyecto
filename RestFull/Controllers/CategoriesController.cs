using System;
using System.Web.Http;
using Entities;
using BLL;
using System.Linq;

namespace Service.Controllers
{
    public class CategoriesController : ApiController
    {
        // Crear categoría
        [HttpPost]
        [Route("api/Categories")]
        public IHttpActionResult CreateCategory(Categories categories)
        {
            if (categories == null)
                return BadRequest("La categoría no puede ser nula.");

            var _categoriesLogic = new CategoriesLogic();

            try
            {
                var createdCategory = _categoriesLogic.Create(categories);
                return Created($"api/Categories/{createdCategory.CategoryID}", createdCategory);
            }
            catch (Exception ex)
            {
                // Devolver un BadRequest con el mensaje de error
                return BadRequest($"Error al crear la categoría: {ex.Message}");
            }
        }

        // Eliminar categoría
        [HttpDelete]
        [Route("api/Categories/{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var _categoriesLogic = new CategoriesLogic();
            try
            {
                var result = _categoriesLogic.Delete(id);

                if (result)
                    return Ok("Categoría eliminada correctamente.");
                else
                    return Content(System.Net.HttpStatusCode.NotFound, "La categoría con el ID proporcionado no se encontró.");
            }
            catch (Exception ex)
            {
                // En caso de error, se captura la excepción y se responde con un BadRequest
                return BadRequest($"Error al eliminar la categoría: {ex.Message}");
            }
        }

        // Obtener todas las categorías
        [HttpGet]
        [Route("api/Categories")]
        public IHttpActionResult GetAll()
        {
            var _categoriesLogic = new CategoriesLogic();
            try
            {
                var categories = _categoriesLogic.RetrieveAll(); // Cambiado a RetrieveAll
                if (categories == null || categories.Count == 0)
                    return Content(System.Net.HttpStatusCode.NotFound, "No se encontraron categorías.");

                return Ok(categories);
            }
            catch (Exception ex)
            {
                // Devolver un error con el mensaje en caso de fallo
                return InternalServerError(new Exception($"Error al obtener las categorías: {ex.Message}"));
            }
        }

        // Obtener categoría por ID
        [HttpGet]
        [Route("api/Categories/{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var _categoriesLogic = new CategoriesLogic();
            try
            {
                var category = _categoriesLogic.RetrieveById(id);
                if (category == null)
                    return Content(System.Net.HttpStatusCode.NotFound, "La categoría con el ID proporcionado no se encontró.");

                var response = new
                {
                    category.CategoryID,
                    category.CategoryName,
                    category.Description
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Devolver un error con el mensaje en caso de fallo
                return InternalServerError(new Exception($"Error al obtener la categoría: {ex.Message}"));
            }
        }

        // Actualizar categoría
        [HttpPut]
        [Route("api/Categories/{id:int}")]
        public IHttpActionResult UpdateCategory(int id, Categories categories)
        {
            if (categories == null)
                return BadRequest("La categoría no puede ser nula.");

            if (id != categories.CategoryID)
                return BadRequest("El ID de la categoría no coincide con el ID de la URL.");

            var _categoriesLogic = new CategoriesLogic();

            try
            {
                var updated = _categoriesLogic.Update(categories);
                if (updated)
                    return Ok("Categoría actualizada correctamente.");
                else
                    return Content(System.Net.HttpStatusCode.NotFound, "La categoría con el ID proporcionado no se encontró.");
            }
            catch (Exception ex)
            {
                // En caso de error, se captura la excepción y se responde con un BadRequest
                return BadRequest($"Error al actualizar la categoría: {ex.Message}");
            }
        }
    }
}
