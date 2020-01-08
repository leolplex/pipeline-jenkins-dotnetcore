﻿using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TDD.API.Controllers;
using TDD.API.Interfaces;
using TDD.API.Test.Fakes;

namespace TDD.API.Test
{
    [TestFixture]
    public class SumaTest
    {
        SumaController _controller;
        IOperacionesService _service;

        public SumaTest()
        {
            _service = new SumaServiceFake();
            _controller = new SumaController(_service);
        }

        [TestCase(0, 0)]
        public void SumaGet(double input1, double input2)
        {
            // Act
            var okResult = _controller.Get(input1, input2);

            // Assert
            Assert.AreEqual(typeof(OkObjectResult), okResult.GetType());
        }



        [TestCase(5, 300, 305)]
        [TestCase(5, 45, 50)]
        public void SumaGetValue(double input1, double input2, double expectedResult)
        {

            // Act
            var okResult = _controller.Get(input1, input2) as OkObjectResult;

            // Assert
            Assert.AreEqual(typeof(OkObjectResult), okResult.GetType());
            Assert.AreEqual(expectedResult, okResult.Value);

        }
    }
}
