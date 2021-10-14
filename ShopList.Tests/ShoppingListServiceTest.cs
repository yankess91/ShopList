using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using ShopList.Infrastructure.DTOs;
using ShopList.Infrastructure.Model;
using ShopList.Infrastructure.Repositories;
using ShopList.Logic.Hubs;
using ShopList.Logic.Mapper;
using ShopList.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ShopList.Tests
{
    public class ShoppingListServiceTest
    {
        private Mock<IShoppingListRepository> _shoppingListRepository;
        private Mock<IHubContext<ShoppingListHub>> _hub;
        private ShoppingListService _service;
        private List<ShoppingList> _shoppingList;
        private List<ShoppingListDto> _shoppingListDto;

        [SetUp]
        public void Setup()
        {
            _shoppingListRepository = new Mock<IShoppingListRepository>();
            _hub = new Mock<IHubContext<ShoppingListHub>>();
            _service = new ShoppingListService(_shoppingListRepository.Object, new ShoppingListMapper(), _hub.Object);

            var mockClientProxy = new Mock<IClientProxy>();

            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            _hub.Setup(x => x.Clients).Returns(() => mockClients.Object);


            _shoppingList = new List<ShoppingList>
            {
                new ShoppingList()
                    {
                        Id = 1,
                        Name = "NameTest1",
                        ProductList = new List<Product>
                        {
                            new Product()
                            {
                                Id = 1,
                                Name = "Test1",
                                Price = 1,
                                Type = "red"
                            }
                        }
                    },
                {
                new ShoppingList()
                    {
                        Id = 2,
                        Name = "NameTest2",
                        ProductList = new List<Product>
                        {
                            new Product()
                            {
                                Id = 1,
                                Name = "Test2",
                                Price = 1,
                                        Type = "red"
                            }
                         }
                    }
                }
            };

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
                    },
                {
                new ShoppingListDto()
                    {
                        Id = 2,
                        Name = "NameTest2",
                        Products = new List<ProductDto>
                        {
                            new ProductDto()
                            {
                                Id = 1,
                                Name = "Test2",
                                Price = 1,
                                        Type = "red"
                            }
                         }
                    }
                }
            };
        }

        [TestCase(false, "", false, true, 0)]
        [TestCase(false, null, false, true, 0)]
        [TestCase(false, "test", false, false, 1)]
        [TestCase(false, "test2", true, true, 1)]
        [TestCase(true, "", false, false, 0)]
        public async Task CreateShoppingList_ExpectedRequest_ReturnsValidResponseAsync(bool isRequestIsNull, string name, bool expectedResponse, bool isRepositoryReturnNull, int times)
        {
            //Arrange
            _shoppingListRepository.Setup(s => s.Get(It.IsAny<Expression<Func<ShoppingList, bool>>>(), It.IsAny<Func<IQueryable<ShoppingList>, IOrderedQueryable<ShoppingList>>>(), It.IsAny<string>()))
                .Returns(isRepositoryReturnNull ? null : _shoppingList);

            _shoppingListRepository.Setup(s => s.Insert(It.IsAny<ShoppingList>()))
                .ReturnsAsync(new ShoppingList());

            var request = isRequestIsNull ? null : new CreateShoppingListRequest()
            {
                Name = name
            };

            //Act
            var result = await _service.CreateShoppingList(request);

            //Assert
            Assert.AreEqual(result.IsSuccess, expectedResponse);
            _shoppingListRepository.Verify(sp => sp.Get(It.IsAny<Expression<Func<ShoppingList, bool>>>(), It.IsAny<Func<IQueryable<ShoppingList>, IOrderedQueryable<ShoppingList>>>(), It.IsAny<string>()), ToTimes(times));
        }

        [Test]
        public void GetAllShoppingList_ExpectedRequest_ReturnsValidResponseAsync()
        {
            //Arrange
            _shoppingListRepository.Setup(s => s.Get(It.IsAny<Expression<Func<ShoppingList, bool>>>(), It.IsAny<Func<IQueryable<ShoppingList>, IOrderedQueryable<ShoppingList>>>(), It.IsAny<string>()))
                .Returns(_shoppingList);

            //Act
            var result = _service.GetAllShoppingList();

            //Assert
            Assert.AreEqual(result.IsSuccess, true);
            Assert.AreEqual(result.ShoppingLists.Count(), _shoppingListDto.Count());
            Assert.AreEqual(result.ShoppingLists.ElementAt(0).Name, _shoppingListDto.ElementAt(0).Name);
            Assert.AreEqual(result.ShoppingLists.ElementAt(0).Id, _shoppingListDto.ElementAt(0).Id);
            Assert.AreEqual(result.ShoppingLists.ElementAt(1).Name, _shoppingListDto.ElementAt(1).Name);
            Assert.AreEqual(result.ShoppingLists.ElementAt(1).Id, _shoppingListDto.ElementAt(1).Id);
            _shoppingListRepository.Verify(sp => sp.Get(It.IsAny<Expression<Func<ShoppingList, bool>>>(), It.IsAny<Func<IQueryable<ShoppingList>, IOrderedQueryable<ShoppingList>>>(), It.IsAny<string>()), Times.Once);
        }

        [TestCase(1, true, true)]
        [TestCase(2, true, true)]
        [TestCase(5, false, false)]
        [TestCase(-15, false, false)]
        public async Task CreateShoppingList_ExpectedRequest_ReturnsValidResponseAsync(int requestId, bool expectedResponse, bool shouldCallDelete)
        {
            //Arrange 
            _shoppingListRepository.Setup(s => s.GetById(It.IsAny<int>()))
                .ReturnsAsync(_shoppingList.FirstOrDefault(x => x.Id == requestId));

            _shoppingListRepository.Setup(s => s.Delete(It.IsAny<int>()))
                .ReturnsAsync(new ShoppingList());


            //Act
            var result = await _service.DeleteShoppingList(requestId);

            //Assert
            Assert.AreEqual(result.IsSuccess, expectedResponse);
            _shoppingListRepository.Verify(sp => sp.GetById(It.IsAny<int>()), Times.Once);
            if(shouldCallDelete)
            {
                _shoppingListRepository.Verify(s => s.Delete(It.IsAny<int>()), Times.Once);
            }
        }


        private static Times ToTimes(int count) => count switch
        {
            0 => Times.Never(),
            1 => Times.Once(),
            _ => Times.Exactly(count)
        };
            
    }
}