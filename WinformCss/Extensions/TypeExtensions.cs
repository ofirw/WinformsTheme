using System;

namespace WinformsTheme.Extensions
{
    public static class TypeExtensions
    {
        public static bool TypeOrBaseTypesNameIs(this Type type, string name, string untilBaseTypeName)
        {
            if (type.Name == name) return true;
            if (type.Name == untilBaseTypeName) return false;

            return type.BaseType.TypeOrBaseTypesNameIs(name, untilBaseTypeName);
        }
    }
}