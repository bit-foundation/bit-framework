﻿using Bit.Owin.Contracts.Metadata;
using Lambda2Js;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bit.Owin.Implementations.Metadata
{
    public class DefaultDtoMetadataBuilder<TDto> : DefaultMetadataBuilder, IDtoMetadataBuilder<TDto>
        where TDto : class
    {
        private DtoMetadata _dtoMetadata;

        public virtual IDtoMetadataBuilder<TDto> AddDtoMetadata(DtoMetadata metadata)
        {
            _dtoMetadata = metadata ?? throw new ArgumentNullException(nameof(metadata));

            _dtoMetadata.DtoType = typeof(TDto).GetTypeInfo().FullName;

            AllMetadata.Add(_dtoMetadata);

            return this;
        }

        public virtual IDtoMetadataBuilder<TDto> AddLookup<TLookupDto>(string memberName, string dataValueField, string dataTextField, Expression<Func<TLookupDto, bool>> baseFilter = null, string lookupName = null)
            where TLookupDto : class
        {
            if (memberName == null)
                throw new ArgumentNullException(nameof(memberName));

            if (dataTextField == null)
                throw new ArgumentNullException(nameof(dataTextField));

            if (dataValueField == null)
                throw new ArgumentNullException(nameof(dataValueField));

            if (_dtoMetadata == null)
                throw new InvalidOperationException($"{nameof(AddDtoMetadata)} must be called first");

            DtoMemberLookup lookup = new DtoMemberLookup
            {
                DtoMemberName = memberName,
                DataTextField = dataTextField,
                DataValueField = dataValueField,
                LookupDtoType = typeof(TLookupDto).GetTypeInfo().FullName
            };

            if (baseFilter != null)
            {
                lookup.BaseFilter_JS = BuildLookupJsFilterFromLambdaExpression(baseFilter, lookup);
            }

            _dtoMetadata.MembersLookups.Add(lookup);

            return this;
        }

        protected virtual JavascriptCompilationOptions JavascriptCompilationOptions => new JavascriptCompilationOptions(JsCompilationFlags.BodyOnly, scriptVersion: ScriptVersion.Es70, extensions: new JavascriptConversionExtension[] { new LinqMethods(), new StaticStringMethods(), new StaticMathMethods(), new EnumConversionExtension(EnumOptions.UseStrings) });

        protected virtual string BuildLookupJsFilterFromLambdaExpression<TLookupDto>(Expression<Func<TLookupDto, bool>> baseFilter, DtoMemberLookup lookup)
            where TLookupDto : class
        {
            if (baseFilter.Parameters.ExtendedSingle("Finding base filter parameters").Name != "it")
                throw new Exception("base filter's parameter name must be 'it'. For example it => it.Id == 1");

            return baseFilter.CompileToJavascript(JavascriptCompilationOptions);
        }

        public virtual IDtoMetadataBuilder<TDto> AddMemberMetadata(string memberName, DtoMemberMetadata metadata)
        {
            if (memberName == null)
                throw new ArgumentNullException(nameof(memberName));

            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));

            return AddMemberMetadata(typeof(TDto).GetTypeInfo().GetProperty(memberName), metadata);
        }

        public virtual IDtoMetadataBuilder<TDto> AddMemberMetadata(PropertyInfo member, DtoMemberMetadata metadata)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));

            if (_dtoMetadata == null)
                throw new InvalidOperationException($"{nameof(AddDtoMetadata)} must be called first");

            metadata.DtoMemberName = member.Name;

            if (!metadata.IsRequired)
                metadata.IsRequired = member.GetCustomAttribute<RequiredAttribute>() != null;

            if (metadata.Pattern == null)
                metadata.Pattern = member.GetCustomAttribute<RegularExpressionAttribute>()?.Pattern;

            _dtoMetadata.MembersMetadata.Add(metadata);

            return this;
        }
    }
}
