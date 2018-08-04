﻿using Bit.Core.Models;
using Bit.Owin.Contracts.Metadata;
using Bit.Owin.Implementations.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bit.Tests.Owin.Metadata
{
    public class TestMetadataBuilder : DefaultProjectMetadataBuilder
    {
        public static readonly string SomeError = nameof(SomeError);

        public override Task<IEnumerable<ObjectMetadata>> BuildMetadata()
        {
            AddProjectMetadata(new ProjectMetadata
            {
                ProjectName = "Bit",
                Messages = new List<EnvironmentCulture>
                {
                    new EnvironmentCulture
                    {
                        Name = "FaIr",
                        Values = new List<EnvironmentCultureValue>
                        {
                            new EnvironmentCultureValue { Name = SomeError, Title = "یک خطا" }
                        }
                    },
                    new EnvironmentCulture
                    {
                        Name = "EnUs",
                        Values = new List<EnvironmentCultureValue>
                        {
                            new EnvironmentCultureValue { Name = SomeError, Title = "Some error" }
                        }
                    }
                }
            });

            return base.BuildMetadata();
        }
    }
}
