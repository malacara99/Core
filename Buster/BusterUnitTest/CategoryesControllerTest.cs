using Buster.Contexts;
using Buster.Controllers;
using Buster.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Sdk;

namespace BusterUnitTest
{
    [TestClass]
    public class CategoryesControllerTest
    {
        [TestMethod]
        public void Get_NotExistCategory_Return404()
        {

            try
            {

                var mockSet = new Mock<DbSet<Category>>();

                var mockCategory = new Category { Id = 2 };

                //Prepare
                var idCategory = 1;
                var mock = new Mock<ApplicationDbContext>();

                //mock.Setup(x => x.Categories).Returns(mockSet);
                var categoriesController = new CategoriesController(mock.Object);

                //Test
                var res = categoriesController.Get(idCategory);
                Assert.IsInstanceOfType(res.Result, typeof(NotFoundResult));
            }
            catch(Exception ex) 
            {
            
            }
        }
    }
}
