using System;
using System.Reflection.Emit;

namespace Extensions
{
    public static class TypeExtensions
    {
        public static Func<object> GetCreateInstanceFn(this Type type)
        {
            var ctor = type.GetConstructor(Type.EmptyTypes);
            var dynamic = new DynamicMethod(string.Empty, type, Type.EmptyTypes, type);
            var il = dynamic.GetILGenerator();

            il.DeclareLocal(type);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            return (Func<object>)dynamic.CreateDelegate(typeof(Func<object>));
        }
    }
}
