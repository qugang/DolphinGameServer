using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Core
{
    public delegate object CtorDelegate();

    public delegate object MethodDelegate(object target, object[] args);

    public delegate object GetValueDelegate(object target);

    public delegate void SetValueDelegate(object target, object arg);

    public class RedisDynamicMethodEmit
    {
        public static Func<object> CreateInstanceDelegate(Type type)
        {
            var construtor = type.GetConstructor(new Type[] { });

            NewExpression newExp = Expression.New(construtor);
            Expression<Func<object>> lambdaExp =
                Expression.Lambda<Func<object>>(newExp);
            Func<object> func = lambdaExp.Compile();
            return func;
        }

        public RedisDynamicMethodEmit(GetValueDelegate getValue, SetValueDelegate setValue, RedisColumnType columnType)
        {
            this.SetValue = setValue;
            this.GetValue = getValue;
            this.ColumnType = columnType;
        }
        public GetValueDelegate GetValue { get; private set; }
        public SetValueDelegate SetValue { get; private set; }

        public RedisColumnType ColumnType { get; private set; }
        

        public static MethodDelegate CreateMethod(MethodInfo method)
        {
            ParameterInfo[] pi = method.GetParameters();

            DynamicMethod dm = new DynamicMethod("DynamicMethod", typeof(object),
                new Type[] { typeof(object), typeof(object[]) },
                typeof(RedisDynamicMethodEmit), true);

            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);

            for (int index = 0; index < pi.Length; index++)
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4, index);

                Type parameterType = pi[index].ParameterType;
                if (parameterType.IsByRef)
                {
                    parameterType = parameterType.GetElementType();
                    if (parameterType.IsValueType)
                    {
                        il.Emit(OpCodes.Ldelem_Ref);
                        il.Emit(OpCodes.Unbox, parameterType);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldelema, parameterType);
                    }
                }
                else
                {
                    il.Emit(OpCodes.Ldelem_Ref);

                    if (parameterType.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox, parameterType);
                        il.Emit(OpCodes.Ldobj, parameterType);
                    }
                }
            }

            if ((method.IsAbstract || method.IsVirtual)
                && !method.IsFinal && !method.DeclaringType.IsSealed)
            {
                il.Emit(OpCodes.Callvirt, method);
            }
            else
            {
                il.Emit(OpCodes.Call, method);
            }

            if (method.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (method.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, method.ReturnType);
            }
            il.Emit(OpCodes.Ret);

            return (MethodDelegate)dm.CreateDelegate(typeof(MethodDelegate));
        }

        public static GetValueDelegate CreatePropertyGetter(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (!property.CanRead)
                return null;

            MethodInfo getMethod = property.GetGetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertyGetter", typeof(object),
                new Type[] { typeof(object) },
                property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!getMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.EmitCall(OpCodes.Callvirt, getMethod, null);
            }
            else
                il.EmitCall(OpCodes.Call, getMethod, null);

            if (property.PropertyType.IsValueType)
                il.Emit(OpCodes.Box, property.PropertyType);

            il.Emit(OpCodes.Ret);

            return (GetValueDelegate)dm.CreateDelegate(typeof(GetValueDelegate));
        }

        public static SetValueDelegate CreatePropertySetter(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (!property.CanWrite)
                return null;

            MethodInfo setMethod = property.GetSetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertySetter", null,
                new Type[] { typeof(object), typeof(object) },
                property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!setMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, property.PropertyType);
            if (!setMethod.IsStatic && !property.DeclaringType.IsValueType)
            {
                il.EmitCall(OpCodes.Callvirt, setMethod, null);
            }
            else
                il.EmitCall(OpCodes.Call, setMethod, null);

            il.Emit(OpCodes.Ret);

            return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));
        }



        public static GetValueDelegate CreateFieldGetter(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            DynamicMethod dm = new DynamicMethod("FieldGetter", typeof(object),
                new Type[] { typeof(object) },
                field.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!field.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);

                EmitCastToReference(il, field.DeclaringType);  //to handle struct object

                il.Emit(OpCodes.Ldfld, field);
            }
            else
                il.Emit(OpCodes.Ldsfld, field);

            if (field.FieldType.IsValueType)
                il.Emit(OpCodes.Box, field.FieldType);

            il.Emit(OpCodes.Ret);

            return (GetValueDelegate)dm.CreateDelegate(typeof(GetValueDelegate));
        }

        public static SetValueDelegate CreateFieldSetter(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            DynamicMethod dm = new DynamicMethod("FieldSetter", null,
                new Type[] { typeof(object), typeof(object) },
                field.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!field.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, field.FieldType);

            if (!field.IsStatic)
                il.Emit(OpCodes.Stfld, field);
            else
                il.Emit(OpCodes.Stsfld, field);
            il.Emit(OpCodes.Ret);

            return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));
        }

        private static void EmitCastToReference(ILGenerator il, Type type)
        {
            if (type.IsValueType)
                il.Emit(OpCodes.Unbox_Any, type);
            else
                il.Emit(OpCodes.Castclass, type);
        }
    }
}
