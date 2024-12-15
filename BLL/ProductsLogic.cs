// Lógica de negocio
using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Entities;

namespace BLL
{
    public class ProductsLogic
    {
        public Products Create(Products products)
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                var existingProduct = repository.Retrieve<Products>(p => p.ProductName == products.ProductName);

                if (existingProduct != null)
                    throw new Exception("El producto ya existe.");

                return repository.Create(products);
            }
        }

        public Products RetrieveById(int id)
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                return repository.Retrieve<Products>(p => p.ProductID == id);
            }
        }

        public bool Update(Products products)
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                var existingProduct = repository.Retrieve<Products>(p => p.ProductID == products.ProductID);

                if (existingProduct == null)
                    throw new Exception("El producto no existe.");

                existingProduct.ProductName = products.ProductName;
                existingProduct.UnitPrice = products.UnitPrice;
                existingProduct.UnitsInStock = products.UnitsInStock;
                existingProduct.CategoryID = products.CategoryID;

                return repository.Update(existingProduct);
            }
        }

        public bool Delete(int id)
        {
            var product = RetrieveById(id);

            if (product == null)
                throw new Exception("El producto no existe.");

            if (product.UnitsInStock > 0)
                throw new Exception("El producto no puede ser eliminado porque aún tiene stock.");

            using (var repository = RepositoryFactory.CreateRepository())
            {
                return repository.Delete(product);
            }
        }

        public List<object> RetrieveAll()
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                return repository.Filter<Products>(p => p.ProductID > 0)
                                 .Select(p => new
                                 {
                                     p.ProductID,
                                     p.ProductName,
                                     p.UnitPrice,
                                     p.UnitsInStock,
                                     CategoryName = p.Categories.CategoryName
                                 })
                                 .Cast<object>()
                                 .ToList();
            }
        }
    }
}
