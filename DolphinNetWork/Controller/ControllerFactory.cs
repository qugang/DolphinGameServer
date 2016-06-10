using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DolphinNetWork
{
    public class ControllerFactory
    {

        private static Dictionary<int, Func<ControllerContext, ControllerBase>> _controllerInitCache = new Dictionary<int, Func<ControllerContext, ControllerBase>>();

        private static Func<ControllerContext, ControllerBase> CreateInstanceDelegate(Type type)
        {
            Type paramType = typeof(ControllerContext);
            var construtor = type.GetConstructor(new Type[] { paramType });
            var param = new ParameterExpression[] { Expression.Parameter(paramType, "arg") };

            NewExpression newExp = Expression.New(construtor, param);
            Expression<Func<ControllerContext, ControllerBase>> lambdaExp =
                Expression.Lambda<Func<ControllerContext, ControllerBase>>(newExp, param);
            Func<ControllerContext, ControllerBase> func = lambdaExp.Compile();
            return func;
        }

        public static void InitController(Assembly assembly)
        {
            foreach (var row in assembly.GetTypes())
            {
                if (row.BaseType == typeof(ControllerBase))
                {
                    ControllerProtocolAttribute ca = row.GetCustomAttribute<ControllerProtocolAttribute>();

                    if (ca != null)
                    {
                        _controllerInitCache.Add(ca.ProtocolNumber, CreateInstanceDelegate(row));
                    }
                }
            }
        }

        internal static ControllerBase CreateController(ControllerContext context)
        {
            return _controllerInitCache[context.ProtocolNumber](context);
        }



    }
}
