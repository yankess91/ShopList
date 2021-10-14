using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShopList.Controllers;
using ShopList.Infrastructure.DTOs;
using ShopList.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopList.Tests
{
    public class ShoppingListControllerTest
    {
        private Mock<IShoppingListService> _shoppingServiceMock;
        private ShoppingListController _controller;
        private GetAllShoppingListResponse _getAllShoppingListResponse;
        private List<ShoppingListDto> _shoppingListDto;

        [SetUp]
        public void Setup()
        {
            _shoppingServiceMock = new Mock<IShoppingListService>();
            _controller = new ShoppingListController(_shoppingServiceMock.Object);

            _shoppingListDto = new List<ShoppingListDto>
                {
                    new ShoppingListDto()
                    {
                        Id = 1,
                        Name = "NameTest1",
                        Products = new List<ProductDto>
                        {
                            new ProductDto()
                            {
                                Id = 1,
                                Name = "Test1",
                                Price = 1,
                                Type = "red"
                            }
                        }
                    }
                };
            _getAllShoppingListResponse = new GetAllShoppingListResponse()
            {
                ShoppingLists = _shoppingListDto
            };
        }

        [Test]
        public void Get_ValidReuest_ReturnsValidResponse()
        {
            //Arrange
            _shoppingServiceMock.Setup(s => s.GetAllShoppingList())
                .Returns(_getAllShoppingListResponse);

            //Act
            var result = _controller.Get();
            var okResult = result as OkObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.Value, _getAllShoppingListResponse);
            Assert.AreEqual(200, okResult.StatusCode);
            _shoppingServiceMock.Verify(sp => sp.GetAllShoppingList(), Times.Once);
        }

        [Test]
        public async Task Post_ValidReuest_ReturnsValidResponseAsync()
        {
            //Arrange
            var request = new CreateShoppingListRequest()
            {
                Name = "test"
            };
            _shoppingServiceMock.Setup(s => s.CreateShoppingList(request))
                .ReturnsAsync(new CreateShoppingListResponse()
                {
                    IsSuccess = true,
                    ShoppingList = _shoppingListDto[0]
                });

            //Act
            var result = await _controller.Post(request);
            var okResult = result as OkObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.Value is CreateShoppingListResponse, true);
            Assert.AreEqual((okResult.Value as CreateShoppingListResponse).IsSuccess, true);
            Assert.AreEqual((okResult.Value as CreateShoppingListResponse).ShoppingList, _shoppingListDto[0]);
            Assert.AreEqual(200, okResult.StatusCode);
            _shoppingServiceMock.Verify(sp => sp.CreateShoppingList(request), Times.Once);
        }


        [Test]
        public async Task Post_InValidReuest_ReturnsValidResponseAsync()
        {
            //Arrange
            var request = new CreateShoppingListRequest();
            _shoppingServiceMock.Setup(s => s.CreateShoppingList(request))
                .ReturnsAsync(new CreateShoppingListResponse()
                {
                    IsSuccess = false
                });

            //Act
            var result = await _controller.Post(request);
            var okResult = result as OkObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.Value is CreateShoppingListResponse, true);
            Assert.AreEqual((okResult.Value as CreateShoppingListResponse).IsSuccess, false);
            Assert.AreEqual(200, okResult.StatusCode);
            _shoppingServiceMock.Verify(sp => sp.CreateShoppingList(request), Times.Once);
        }

        [Test]
        public async Task Delete_ValidReuest_ReturnsValidResponseAsync()
        {
            //Arrange
            _shoppingServiceMock.Setup(s => s.DeleteShoppingList(It.IsAny<int>()))
                .ReturnsAsync(new DeletedShoppingListResponse()
                {
                    IsSuccess = true,
                    ShoppingListId = 1                   
                }); ;


            //Act
            var result = await _controller.Delete(1);
            var okResult = result as OkObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.Value is DeletedShoppingListResponse, true);
            Assert.AreEqual((okResult.Value as DeletedShoppingListResponse).IsSuccess, true);
            Assert.AreEqual((okResult.Value as DeletedShoppingListResponse).ShoppingListId, 1);
            Assert.AreEqual(200, okResult.StatusCode);
            _shoppingServiceMock.Verify(sp => sp.DeleteShoppingList(It.IsAny<int>()), Times.Once);
        }
    }
}