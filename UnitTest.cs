using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Termin8PrimerSaCasa.Controllers;
using Termin8PrimerSaCasa.Interface;
using Termin8PrimerSaCasa.Models;

namespace Termin8PrimerSaCasa.Tests

    Mapper.Initialize(cfg =>
            {
    cfg.CreateMap<Festival, FestivalDTO>()
    .ForMember(dest => dest.MestoNaziv, opt => opt.MapFrom(src => src.Mesto.Naziv));



});





    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        /*Ako akcija vraća 200 (OK) i objekat, prilikom
            testiranja povratni tip je OkNegotiatedContentResult*/
        public void GetReturnsProductWithSameId()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetById(42)).Returns(new Product { Id = 42 });

            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.GetById(42);
            var contentResult = actionResult as OkNegotiatedContentResult<Product>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(42, contentResult.Content.Id);
        }

        // --------------------------------------------------------------------------------------
        /*Ako akcija vraća 404 (Not Found), test proverava da li
            je povratni tip NotFoundResult*/
        [TestMethod]
        public void GetReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.GetById(10);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
        // --------------------------------------------------------------------------------------
        /*Ako akcija vraća 404 (Not Found), test proverava da li
            je povratni tip NotFoundResult*/
        [TestMethod]
        public void DeleteReturnsNotFound()
        {
            // Arrange 
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Delete(10);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        // --------------------------------------------------------------------------------------
        /*Ako akcija vraća 200 (OK) bez objekta, test proverava
            da li je povratni tip OkResult*/
        [TestMethod]
        public void DeleteReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetById(10)).Returns(new Product { Id = 10 });
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Delete(10);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        // --------------------------------------------------------------------------------------
        /*Ako akcija vraća 400 (BadRequest), test proverava da
             li je povratni tip BadRequestResult*/
        [TestMethod]
        public void PutReturnsBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Put(10, new Product { Id = 9, Name = "Product2" });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        // -------------------------------------------------------------------------------------
        /*Ako akcija vraća 201 (Created) sa URI-jem u zaglavlju
            (npr. POST), test proverava da li je akcija postavila
            pravilne vrednosti rutiranja*/
        [TestMethod]
        public void PostMethodSetsLocationHeader()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Post(new Product { Id = 10, Name = "Product1" });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Product>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(10, createdResult.RouteValues["id"]);
        }

        // ------------------------------------------------------------------------------------------
        /*Ako akcija vraća kolekciju objekata, test proverava da
            li je vraćena ispravna kolekcija*/
        [TestMethod]
        public void GetReturnsMultipleObjects()
        {
            // Arrange
            List<Product> products = new List<Product>();
            products.Add(new Product { Id = 1, Name = "Product1" });
            products.Add(new Product { Id = 2, Name = "Product2" });

            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(products.AsEnumerable());
            var controller = new ProductsController(mockRepository.Object);

            // Act
            IEnumerable<Product> result = controller.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(products.Count, result.ToList().Count);
            Assert.AreEqual(products.ElementAt(0), result.ElementAt(0));
            Assert.AreEqual(products.ElementAt(1), result.ElementAt(1));


        //----------------------ILI---------------------------------//
        [TestMethod]
        public void PostPretragaReturnsMultipleObjects()
        {
            Mapper.Initialize(cfg =>
            {

                cfg.CreateMap<Nekretnina, NekretninaDTO>();

            });
            // Arrange
            List<Nekretnina> nekretnine = new List<Nekretnina>();
            nekretnine.Add(new Nekretnina { Kvadratura = 50 });
            nekretnine.Add(new Nekretnina { Kvadratura = 70 });

            var mockRepository = new Mock<INekretninaRepository>();
            mockRepository.Setup(x => x.Pretraga(50, 70)).Returns(nekretnine.AsQueryable());
            var controller = new NekretnineController(mockRepository.Object);

            // Act
            IQueryable<NekretninaDTO> result = controller.PostPretraga(50, 70);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nekretnine.Count, result.ToList().Count);
            //  Assert.AreEqual(nekretnine.ElementAt(0), result.ElementAt(0));
            //Assert.AreEqual(nekretnine.ElementAt(1), result.ElementAt(1));
        }




    }
    }
}
