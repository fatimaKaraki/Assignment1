using ServerSide.Services;
using SharedObjects.RequestsAndResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Support
{

    

    internal class RoutingTableContainer
    {
        public readonly static Dictionary<string, MethodInfo> RoutingTable;

        static RoutingTableContainer()
        {

            RoutingTable= new Dictionary<String, MethodInfo>();
            Assembly assembly = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name);
            Type serviceType = typeof(Service);
            IEnumerable<Type> serviceDerivedTypes = assembly.GetTypes()
                .Where(type => serviceType.IsAssignableFrom(type) && !type.IsAbstract);

            foreach (Type derivedType in serviceDerivedTypes)
            {
                MethodInfo[] methods = derivedType.GetMethods()
                    .ToArray();
                foreach(MethodInfo method in methods)
                {
                    string key = $"{derivedType.Name}/{method.Name}";
                    RoutingTableContainer.RoutingTable[key] = method;

                }
            }


        }

    }
}
