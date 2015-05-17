using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem
{
    internal class CachedActivator
    {
        private delegate object ObjectActivator();

        private static Dictionary<Type, ObjectActivator> cache = new Dictionary<Type, ObjectActivator>();

        public static object CreateInstance(Type type)
        {
            ObjectActivator activator;
            if (!cache.TryGetValue(type, out activator))
            {
                activator = CreateCtor(type);
                cache[type] = activator;
            }

            return activator();
        }

        public static T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        // From http://stackoverflow.com/a/23433748
        private static ObjectActivator CreateCtor(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
            var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);

            return (ObjectActivator)dynamicMethod.CreateDelegate(typeof(ObjectActivator));
        }
    }
}
