using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Entities;

namespace BLL
{
    public class CategoriesLogic
    {
        public Categories Create(Categories categories)
        {
            Categories _categories = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                Categories _result = repository.Retrieve<Categories>
                    (c => c.CategoryName == categories.CategoryName);
                if (_result == null)
                {
                    _categories = repository.Create(categories);
                }
                else
                {
                    throw new Exception("Categoría ya existe.");
                }
            }
            return categories;
        }

        public Categories RetrieveById(int id)
        {
            Categories _categories = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                _categories = repository.Retrieve<Categories>(c => c.CategoryID == id);
            }
            return _categories;
        }

        public bool Update(Categories categories)
        {
            bool _updated = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                Categories _result = repository.Retrieve<Categories>
                    (c => c.CategoryName == categories.CategoryName);
                if (_result == null)
                {
                    _updated = repository.Update(categories);
                }
                else
                {
                    throw new Exception("La categoría no existe o ya está actualizada.");
                }
            }
            return _updated;
        }

        public bool Delete(int id)
        {
            bool _delete = false;
            var _category = RetrieveById(id);
            if (_category != null)
            {
                using (var repository = RepositoryFactory.CreateRepository())
                {
                    _delete = repository.Delete(_category);
                }
            }
            else
            {
                throw new Exception("La categoría con el ID proporcionado no se encontró.");
            }
            return _delete;
        }

        public List<object> RetrieveAll()
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                var categories = repository.Filter<Categories>(c => c.CategoryID > 0)
                    .Select(c => new
                    {
                        c.CategoryID,
                        c.CategoryName,
                        c.Description
                    }).ToList();

                return categories.Cast<object>().ToList(); // Devuelve un listado genérico compatible con JSON
            }
        }
    }
}
