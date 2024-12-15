using System;
using System.Web.Mvc;
using BLL;
using Entities;
using Security;

namespace YourNamespace.Controllers
{
    public class HomeController : Controller
    {
        private ProductsLogic _logic = new ProductsLogic();
        private CategoriesLogic _categoryLogic = new CategoriesLogic();

        // Verificar si el usuario ha iniciado sesión
        private bool IsAuthenticated()
        {
            return Session["UserId"] != null;
        }

        // Método privado para verificar roles
        private bool IsAuthorized(params string[] requiredRoles)
        {
            var userRole = Session["Role"]?.ToString();
            if (userRole == null || !Array.Exists(requiredRoles, role => role == userRole))
            {
                ViewBag.ErrorMessage = "No tienes permisos para acceder a esta acción.";
                return false;
            }
            return true;
        }

        // Redirigir al login si no está autenticado
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!IsAuthenticated() && filterContext.ActionDescriptor.ActionName != "Login")
            {
                filterContext.Result = RedirectToAction("Login", "Auth");
            }
            base.OnActionExecuting(filterContext);
        }

        // Página de inicio
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int id, string searchType)
        {
            if (!IsAuthorized("Cliente")) return RedirectToAction("Index");

            try
            {
                if (searchType == "product")
                {
                    var product = _logic.RetrieveById(id);
                    if (product == null)
                    {
                        ViewBag.ErrorMessage = "Producto no encontrado.";
                        return View();
                    }
                    return View("Details", product);
                }
                else if (searchType == "category")
                {
                    ViewBag.ErrorMessage = "No tienes permisos para buscar categorías.";
                    return View();
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // Detalles de producto
        public ActionResult Details(int id)
        {
            if (!IsAuthorized("Cliente")) return RedirectToAction("Index");

            var product = _logic.RetrieveById(id);
            if (product == null)
            {
                ViewBag.ErrorMessage = "Producto no encontrado.";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // Formulario CUD para productos
        public ActionResult CUD(int? id = null)
        {
            if (!IsAuthorized("Proveedor", "Admin")) return RedirectToAction("Index");

            Products model = id.HasValue ? _logic.RetrieveById(id.Value) : new Products();
            if (model == null && id.HasValue)
            {
                ViewBag.ErrorMessage = "Producto no encontrado.";
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_categoryLogic.RetrieveAll(), "CategoryID", "CategoryName", model.CategoryID);
            return View(model);
        }

        [HttpPost]
        public ActionResult CUD(Products model, string CreateBtn, string UpdateBtn, string DeleteBtn)
        {
            if (!IsAuthorized("Proveedor", "Admin")) return RedirectToAction("Index");

            try
            {
                if (!string.IsNullOrEmpty(CreateBtn))
                {
                    _logic.Create(model);
                    ViewBag.SuccessMessage = "Producto creado exitosamente.";
                    return RedirectToAction("Details", new { id = model.ProductID });
                }
                else if (!string.IsNullOrEmpty(UpdateBtn))
                {
                    _logic.Update(model);
                    ViewBag.SuccessMessage = "Producto actualizado exitosamente.";
                    return RedirectToAction("Details", new { id = model.ProductID });
                }
                else if (!string.IsNullOrEmpty(DeleteBtn))
                {
                    if (_logic.Delete(model.ProductID))
                    {
                        ViewBag.SuccessMessage = "Producto eliminado exitosamente.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se puede eliminar el producto. Asegúrate de que las unidades en existencia sean 0.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(model);
        }

        // Formulario CUD para categorías
        public ActionResult CategoryCUD(int? id = null)
        {
            if (!IsAuthorized("Admin")) return RedirectToAction("Index");

            Categories model = id.HasValue ? _categoryLogic.RetrieveById(id.Value) : new Categories();
            if (model == null && id.HasValue)
            {
                ViewBag.ErrorMessage = "Categoría no encontrada.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult CategoryCUD(Categories model, string CreateBtn, string UpdateBtn, string DeleteBtn)
        {
            if (!IsAuthorized("Admin")) return RedirectToAction("Index");

            try
            {
                if (!string.IsNullOrEmpty(CreateBtn))
                {
                    _categoryLogic.Create(model);
                    ViewBag.SuccessMessage = "Categoría creada exitosamente.";
                    return RedirectToAction("CategoryDetails", new { id = model.CategoryID });
                }
                else if (!string.IsNullOrEmpty(UpdateBtn))
                {
                    _categoryLogic.Update(model);
                    ViewBag.SuccessMessage = "Categoría actualizada exitosamente.";
                    return RedirectToAction("CategoryDetails", new { id = model.CategoryID });
                }
                else if (!string.IsNullOrEmpty(DeleteBtn))
                {
                    if (_categoryLogic.Delete(model.CategoryID))
                    {
                        ViewBag.SuccessMessage = "Categoría eliminada exitosamente.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se puede eliminar la categoría.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(model);
        }
    }
}
