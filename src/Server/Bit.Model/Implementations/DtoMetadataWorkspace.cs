﻿using Bit.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Bit.Model.Implementations
{
    public class DtoMetadataWorkspace
    {
        private static DtoMetadataWorkspace _current;

        public static DtoMetadataWorkspace Current
        {
            get => _current ?? (_current = new DtoMetadataWorkspace());
            set => _current = value;
        }

        public virtual bool IsDto(TypeInfo type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsClass && type.GetInterface(nameof(IDto)) != null;
        }

        public virtual TypeInfo GetFinalDtoType(TypeInfo type)
        {
            if (!type.IsGenericParameter || type.GetGenericParameterConstraints().Length <= 0)
                return type;

            Type finalDtoType = type.GetGenericParameterConstraints().ExtendedSingleOrDefault($"Finding dto of {type.Name}", t => IsDto(t?.GetTypeInfo()));
            if (finalDtoType != null)
                return finalDtoType.GetTypeInfo();
            return null;
        }

        public virtual PropertyInfo[] GetKeyColums(TypeInfo typeInfo)
        {
            PropertyInfo[] props = typeInfo.GetProperties();

            PropertyInfo[] keys = props
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null)
                .ToArray();

            if (keys.Length > 0)
                return keys;
            return props.Where(p => p.Name == "Id" || p.Name == $"{typeInfo.Name}Id").ToArray();
        }

        public virtual object[] GetKeys(IDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            TypeInfo dtoType = dto.GetType().GetTypeInfo();

            PropertyInfo[] props = GetKeyColums(dtoType);

            return props.Select(p => p.GetValue(dto)).ToArray();
        }
    }
}
