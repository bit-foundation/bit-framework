﻿using Bit.Test;
using Bit.Tests.Api.ApiControllers;
using Bit.Tests.Core.Contracts;
using Bit.Tests.Model.DomainModels;
using FakeItEasy;
using IdentityModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simple.OData.Client;
using System;
using System.Threading.Tasks;

namespace Bit.Tests.Api.Middlewares.WebApi.Tests
{
    [TestClass]
    public class ODataSqlBuilderTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public virtual async Task ValidateODataSqlBuilderResults()
        {
            IValueChecker valueChecker = A.Fake<IValueChecker>();

            using (BitOwinTestEnvironment testEnvironment = new BitOwinTestEnvironment(new TestEnvironmentArgs
            {
                AdditionalDependencies = (manager, services) => manager.RegisterInstance(valueChecker)
            }))
            {
                TokenResponse token = await testEnvironment.Server.Login("ValidUserName", "ValidPassword", clientId: "TestResOwner");

                IODataClient client = testEnvironment.Server.BuildODataClient(token: token);

                await client.Controller<TestModelsController, TestModel>()
                    .Function(nameof(TestModelsController.TestSqlBuilder))
                    .Filter(t => (t.Id == 1 && t.StringProperty.IndexOf("Test", StringComparison.OrdinalIgnoreCase) >= 0) || t.Id == 3)
                    .OrderBy(t => t.Id)
                    .ThenByDescending(t => t.StringProperty)
                    .Top(10)
                    .Skip(7)
                    .FindEntriesAsync();

                A.CallTo(() => valueChecker.CheckValue((long?)10))
                    .MustHaveHappened(Repeated.Exactly.Once);

                A.CallTo(() => valueChecker.CheckValue((long?)7))
                    .MustHaveHappened(Repeated.Exactly.Once);

                A.CallTo(() => valueChecker.CheckValue("(([TestModel].[Id] = @Param1 AND [TestModel].[StringProperty] LIKE @Param2) OR [TestModel].[Id] = @Param3)"))
                    .MustHaveHappened(Repeated.Exactly.Once);

                A.CallTo(() => valueChecker.CheckValue("[TestModel].[Id],[TestModel].[StringProperty] DESC"))
                    .MustHaveHappened(Repeated.Exactly.Once);

                A.CallTo(() => valueChecker.CheckValue(A<object[]>.That.Matches(parameters => (long)parameters[0] == 1 && (string)parameters[1] == "%Test%" && (long)parameters[2] == 3)))
                    .MustHaveHappened(Repeated.Exactly.Once);

                A.CallTo(() => valueChecker.CheckValue("select  * from Test.TestModels as [TestModel] where (([TestModel].[Id] = @Param1 AND [TestModel].[StringProperty] LIKE @Param2) OR [TestModel].[Id] = @Param3) order by [TestModel].[Id],[TestModel].[StringProperty] DESC offset 7 rows fetch next 10 rows only"))
                    .MustHaveHappened(Repeated.Exactly.Once);

                A.CallTo(() => valueChecker.CheckValue("select count_big(1) from Test.TestModels as [TestModel] where (([TestModel].[Id] = @Param1 AND [TestModel].[StringProperty] LIKE @Param2) OR [TestModel].[Id] = @Param3)"))
                    .MustHaveHappened(Repeated.Exactly.Once);

                A.CallTo(() => valueChecker.CheckValue(false))
                    .MustHaveHappened(Repeated.Exactly.Once);
            }
        }
    }
}
