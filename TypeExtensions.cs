using System;
using System.Reflection.Emit;

namespace Extensions
{
    public static class TypeExtensions
    {
        public static Func<T> GetCreateInstanceFn<T>(this Type type)
        {
            var ctor = type.GetConstructor(Type.EmptyTypes);
            var dynamic = new DynamicMethod(string.Empty, type, Type.EmptyTypes, type);
            var il = dynamic.GetILGenerator();

            il.DeclareLocal(type);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            return (Func<T>)dynamic.CreateDelegate(typeof(Func<T>));
        }
        
        public static bool IsPrimitiveValue(this Type type)
        {
            return type == typeof(String) ||
                type.IsValueType ||
                type == typeof(Enum);
        }
    }
}
