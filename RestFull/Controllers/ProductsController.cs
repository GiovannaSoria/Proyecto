using System.Web.Http;
using Entities;
using BLL;
using System;

namespace Service.Controllers
{
    public class ProductsController : ApiController
    {
        // Crear producto
        [HttpPost]
        [Route("api/Products")]
        public IHttpActionResult CreateProduct(Products products)
        {
            if (products == null)
                return BadRequest("El producto no puede ser nulo.");

            var _productsLogic = new ProductsLogic();

            try
            {
                var createdProduct = _productsLogic.Create(products);
                return Created($"api/Products/{createdProduct.ProductID}", createdProduct);
            }
            catch (Exception ex)
            {
                // En caso de error, se captura la excepción y se devuelve un BadRequest
                return BadRequest($"Error al crear el producto: {ex.Message}");
            }
        }

        // Eliminar producto
        [HttpDelete]
        [Route("api/Products/{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var _productsLogic = new ProductsLogic();
            try
            {
                var result = _productsLogic.Delete(id);

                if (result)
                    return Ok("Producto eliminado correctamente.");
                else
                    return BadRequest("El producto no pudo ser eliminado. Verifica que no tenga stock o que exista.");
            }
            catch (Exception ex)
            {
                // En caso de error, se captura la excepción y se responde con un InternalServerError
                return InternalServerError(new Exception($"Error al eliminar el producto: {ex.Message}"));
            }
        }

        // Obtener todos los productos
        [HttpGet]
        [Route("api/Products")]
        public IHttpActionResult GetAll()
        {
            var _productsLogic = new ProductsLogic();
            try
            {
                var products = _productsLogic.RetrieveAll();
                if (products == null || products.Count == 0)
                    return Content(System.Net.HttpStatusCode.NotFound, "No se encontraron productos.");

                return Ok(products);
            }
            catch (Exception ex)
            {
                // En caso de error, se captura la excepción y se responde con un InternalServerError
                return InternalServerError(new Exception($"Error al obtener los productos: {ex.Message}"));
            }
        }

        // Obtener producto por ID
        [HttpGet]
        [Route("api/Products/{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var _productsLogic = new ProductsLogic();
            try
            {
                var product = _productsLogic.RetrieveById(id);
                if (product == null)
                    return Content(System.Net.HttpStatusCode.NotFound, "El producto con el ID proporcionado no se encontró.");

                var response = new
                {
                    product.ProductID,
                    product.ProductName,
                    product.CategoryID,
                    product.UnitPrice,
                    product.UnitsInStock
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // En caso de error, se captura la excepción y se responde con un InternalServerError
                return InternalServerError(new Exception($"Error al obtener el producto: {ex.Message}"));
            }
        }

        // Actualizar producto
        [HttpPut]
        [Route("api/Products/{id:int}")]
        public IHttpActionResult UpdateProduct(int id, Products products)
        {
            if (products == null)
                return BadRequest("El producto no puede ser nulo.");

            if (id != products.ProductID)
                return BadRequest("El ID del producto no coincide con el ID de la URL.");

            var _productsLogic = new ProductsLogic();

            try
            {
                var updated = _productsLogic.Update(products);
                if (updated)
                    return Ok("Producto actualizado correctamente.");
                else
                    return Content(System.Net.HttpStatusCode.NotFound, "El producto con el ID proporcionado no se encontró.");
            }
            catch (Exception ex)
            {
                // En caso de error, se captura la excepción y se devuelve un BadRequest
                return BadRequest($"Error al actualizar el producto: {ex.Message}");
            }
        }
    }
}
