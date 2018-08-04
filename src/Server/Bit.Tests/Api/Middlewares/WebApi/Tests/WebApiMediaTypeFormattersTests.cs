﻿using IdentityModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bit.Tests.Api.Middlewares.WebApi.Tests
{
    [TestClass]
    public class WebApiMediaTypeFormattersTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public virtual async Task MediaTypeFormattersShouldReturnMinimalStreamedODataJsonWhenNoContentTypeIsDeclaredInRequest()
        {
            using (BitOwinTestEnvironment testEnvironment = new BitOwinTestEnvironment())
            {
                TokenResponse token = await testEnvironment.Server.Login("ValidUserName", "ValidPassword", clientId: "TestResOwner");

                HttpResponseMessage getTestModelsResponse = await testEnvironment.Server.BuildHttpClient(token)
                    .GetAsync("/odata/Test/TestModels");

                Assert.AreEqual("application/json; odata.metadata=minimal; odata.streaming=true; charset=utf-8", getTestModelsResponse.Content.Headers.ContentType.ToString());
            }
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public virtual async Task MediaTypeFormattersShouldReturnMinimalODataJsonWhenJsonContentTypeIsDeclaredFirstInRequest()
        {
            using (BitOwinTestEnvironment testEnvironment = new BitOwinTestEnvironment())
            {
                TokenResponse token = await testEnvironment.Server.Login("ValidUserName", "ValidPassword", clientId: "TestResOwner");

                HttpResponseMessage getTestModelsResponse = await testEnvironment.Server.BuildHttpClient(token)
                    .AddHeader("Accept", "application/json, text/javascript, */*; q=0.01")
                    .GetAsync("/odata/Test/TestModels");

                Assert.AreEqual("application/json; charset=utf-8; odata.metadata=minimal", getTestModelsResponse.Content.Headers.ContentType.ToString());
            }
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public virtual async Task MediaTypeFormattersShouldReturnMinimalODataJsonWhenStarContentTypeIsDeclaredInRequest()
        {
            using (BitOwinTestEnvironment testEnvironment = new BitOwinTestEnvironment())
            {
                TokenResponse token = await testEnvironment.Server.Login("ValidUserName", "ValidPassword", clientId: "TestResOwner");

                HttpResponseMessage getTestModelsResponse = await testEnvironment.Server.BuildHttpClient(token)
                    .AddHeader("Accept", "*/*; q=0.01")
                    .GetAsync("/odata/Test/TestModels");

                Assert.AreEqual("application/json; charset=utf-8; odata.metadata=minimal", getTestModelsResponse.Content.Headers.ContentType.ToString());
            }
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public virtual async Task MediaTypeFormattersShouldReturnMinimalStreamedODataJsonWhenInvalidContentTypeIsDeclaredInRequest()
        {
            using (BitOwinTestEnvironment testEnvironment = new BitOwinTestEnvironment())
            {
                TokenResponse token = await testEnvironment.Server.Login("ValidUserName", "ValidPassword", clientId: "TestResOwner");

                HttpResponseMessage getTestModelsResponse = await testEnvironment.Server.BuildHttpClient(token)
                    .AddHeader("Accept", "text/html; q=0.01")
                    .GetAsync("/odata/Test/TestModels");

                Assert.AreEqual("application/json; odata.metadata=minimal; odata.streaming=true; charset=utf-8", getTestModelsResponse.Content.Headers.ContentType.ToString());
            }
        }
    }
}
