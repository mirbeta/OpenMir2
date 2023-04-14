using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemModule.Dependency.Attribute;

namespace SystemModule.Dependency
{
    /// <summary>
    /// IOC容器
    /// </summary>
    public class Container : IContainer
    {
        private readonly ConcurrentDictionary<string, DependencyDescriptor> m_registrations = new ConcurrentDictionary<string, DependencyDescriptor>();

        /// <summary>
        /// 初始化一个IOC容器
        /// </summary>
        public Container()
        {
            this.RegisterSingleton<IContainer>(this);
        }

        /// <summary>
        /// 返回迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<DependencyDescriptor> GetEnumerator()
        {
            return m_registrations.Values.ToList().GetEnumerator();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsRegistered(Type fromType, string key = "")
        {
            return m_registrations.ContainsKey($"{fromType.FullName}{key}");
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="key"></param>
        public void Register(DependencyDescriptor descriptor, string key = "")
        {
            string k = $"{descriptor.FromType.FullName}{key}";
            m_registrations.AddOrUpdate(k, descriptor, (k, v) => { return descriptor; });
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="ps"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Resolve(Type fromType, object[] ps = null, string key = "")
        {
            if (fromType == typeof(IContainerProvider))
            {
                return GetScopedContainer();
            }
            string k;
            DependencyDescriptor descriptor;
            if (fromType.IsGenericType)
            {
                Type type = fromType.GetGenericTypeDefinition();
                k = $"{type.FullName}{key}";
                if (m_registrations.TryGetValue(k, out descriptor))
                {
                    if (descriptor.ImplementationFactory != null)
                    {
                        return descriptor.ImplementationFactory.Invoke(this);
                    }
                    if (descriptor.Lifetime == Lifetime.Singleton)
                    {
                        if (descriptor.ToInstance != null)
                        {
                            return descriptor.ToInstance;
                        }
                        lock (descriptor)
                        {
                            if (descriptor.ToInstance != null)
                            {
                                return descriptor.ToInstance;
                            }
                            if (descriptor.ToType.IsGenericType)
                            {
                                return descriptor.ToInstance = Create(descriptor.ToType.MakeGenericType(fromType.GetGenericArguments()), ps);
                            }
                            else
                            {
                                return descriptor.ToInstance = Create(descriptor.ToType, ps);
                            }
                        }
                    }
                    if (descriptor.ToType.IsGenericType)
                    {
                        return Create(descriptor.ToType.MakeGenericType(fromType.GetGenericArguments()), ps);
                    }
                    else
                    {
                        return Create(descriptor.ToType, ps);
                    }
                }
            }
            k = $"{fromType.FullName}{key}";
            if (m_registrations.TryGetValue(k, out descriptor))
            {
                if (descriptor.ImplementationFactory != null)
                {
                    return descriptor.ImplementationFactory.Invoke(this);
                }
                if (descriptor.Lifetime == Lifetime.Singleton)
                {
                    if (descriptor.ToInstance != null)
                    {
                        return descriptor.ToInstance;
                    }
                    lock (descriptor)
                    {
                        if (descriptor.ToInstance != null)
                        {
                            return descriptor.ToInstance;
                        }
                        return descriptor.ToInstance = Create(descriptor.ToType, ps);
                    }
                }
                return Create(descriptor.ToType, ps);
            }
            else if (fromType.IsPrimitive || fromType == typeof(string))
            {
                return default;
            }
            else if (fromType.IsClass && !fromType.IsAbstract)
            {
                if (fromType.GetCustomAttribute<DependencyInjectAttribute>() is DependencyInjectAttribute attribute)
                {
                    if (attribute.ResolveNullIfNoRegistered && !IsRegistered(fromType, key))
                    {
                        return default;
                    }
                }
                return Create(fromType, ps);
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="key"></param>
        public void Unregister(DependencyDescriptor descriptor, string key = "")
        {
            string k = $"{descriptor.FromType.FullName}{key}";
            m_registrations.TryRemove(k, out _);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="toType"></param>
        /// <param name="ops"></param>
        /// <returns></returns>
        private object Create(Type toType, object[] ops)
        {
            ConstructorInfo ctor = toType.GetConstructors().FirstOrDefault(x => x.IsDefined(typeof(DependencyInjectAttribute), true));
            if (ctor is null)
            {
                //如果没有被特性标记，那就取构造函数参数最多的作为注入目标
                if (toType.GetConstructors().Length == 0)
                {
                    throw new Exception($"没有找到类型{toType.FullName}的公共构造函数。");
                }
                ctor = toType.GetConstructors().OrderByDescending(x => x.GetParameters().Length).First();
            }
            else
            {
                if (ops == null)
                {
                    ops = ctor.GetCustomAttribute<DependencyInjectAttribute>().Ps;
                }
            }

            DependencyTypeAttribute dependencyTypeAttribute = null;
            if (toType.IsDefined(typeof(DependencyTypeAttribute), true))
            {
                dependencyTypeAttribute = toType.GetCustomAttribute<DependencyTypeAttribute>();
            }

            ParameterInfo[] parameters = ctor.GetParameters();
            object[] ps = new object[parameters.Length];

            if (dependencyTypeAttribute == null || dependencyTypeAttribute.Type.HasFlag(DependencyType.Constructor))
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (ops != null && ops.Length - 1 >= i)
                    {
                        ps[i] = ops[i];
                    }
                    else
                    {
                        if (parameters[i].ParameterType.IsPrimitive || parameters[i].ParameterType == typeof(string))
                        {
                            if (parameters[i].HasDefaultValue)
                            {
                                ps[i] = parameters[i].DefaultValue;
                            }
                            else
                            {
                                ps[i] = default;
                            }
                        }
                        else
                        {
                            if (parameters[i].IsDefined(typeof(DependencyParamterInjectAttribute), true))
                            {
                                DependencyParamterInjectAttribute attribute = parameters[i].GetCustomAttribute<DependencyParamterInjectAttribute>();
                                Type type = attribute.Type == null ? parameters[i].ParameterType : attribute.Type;

                                if (attribute.ResolveNullIfNoRegistered && !IsRegistered(type, attribute.Key))
                                {
                                    ps[i] = default;
                                }
                                else
                                {
                                    ps[i] = Resolve(type, attribute.Ps, attribute.Key);
                                }
                            }
                            else
                            {
                                ps[i] = Resolve(parameters[i].ParameterType, null);
                            }
                        }
                    }
                }
            }
            object instance = Activator.CreateInstance(toType, ps);

            if (dependencyTypeAttribute == null || dependencyTypeAttribute.Type.HasFlag(DependencyType.Property))
            {
                IEnumerable<PropertyInfo> propetys = toType.GetProperties().Where(x => x.IsDefined(typeof(DependencyInjectAttribute), true));
                foreach (PropertyInfo item in propetys)
                {
                    if (item.CanWrite)
                    {
                        object obj;
                        if (item.IsDefined(typeof(DependencyParamterInjectAttribute), true))
                        {
                            DependencyParamterInjectAttribute attribute = item.GetCustomAttribute<DependencyParamterInjectAttribute>();
                            Type type = attribute.Type == null ? item.PropertyType : attribute.Type;
                            if (attribute.ResolveNullIfNoRegistered && !IsRegistered(type, attribute.Key))
                            {
                                obj = null;
                            }
                            else
                            {
                                obj = Resolve(type, attribute.Ps, attribute.Key);
                            }
                        }
                        else
                        {
                            obj = Resolve(item.PropertyType, null);
                        }
                        item.SetValue(instance, obj);
                    }
                }
            }

            if (dependencyTypeAttribute == null || dependencyTypeAttribute.Type.HasFlag(DependencyType.Method))
            {
                List<MethodInfo> methods = toType.GetMethods(BindingFlags.Default | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(x => x.IsDefined(typeof(DependencyInjectAttribute), true)).ToList();
                foreach (MethodInfo item in methods)
                {
                    parameters = item.GetParameters();
                    ops = item.GetCustomAttribute<DependencyInjectAttribute>().Ps;
                    ps = new object[parameters.Length];
                    for (int i = 0; i < ps.Length; i++)
                    {
                        if (ops != null && ops.Length - 1 >= i)
                        {
                            ps[i] = ops[i];
                        }
                        else
                        {
                            if (parameters[i].ParameterType.IsPrimitive || parameters[i].ParameterType == typeof(string))
                            {
                                if (parameters[i].HasDefaultValue)
                                {
                                    ps[i] = parameters[i].DefaultValue;
                                }
                                else
                                {
                                    ps[i] = default;
                                }
                            }
                            else
                            {
                                if (parameters[i].IsDefined(typeof(DependencyParamterInjectAttribute), true))
                                {
                                    DependencyParamterInjectAttribute attribute = parameters[i].GetCustomAttribute<DependencyParamterInjectAttribute>();
                                    Type type = attribute.Type == null ? parameters[i].ParameterType : attribute.Type;

                                    if (attribute.ResolveNullIfNoRegistered && !IsRegistered(type, attribute.Key))
                                    {
                                        ps[i] = default;
                                    }
                                    else
                                    {
                                        ps[i] = Resolve(type, attribute.Ps, attribute.Key);
                                    }
                                }
                                else
                                {
                                    ps[i] = Resolve(parameters[i].ParameterType, null);
                                }
                            }
                        }
                    }
                    item.Invoke(instance, ps);
                }
            }
            return instance;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IContainerProvider GetScopedContainer()
        {
            Container container = new Container();
            foreach (KeyValuePair<string, DependencyDescriptor> item in m_registrations)
            {
                if (item.Value.Lifetime == Lifetime.Scoped)
                {
                    container.m_registrations.AddOrUpdate(item.Key, new DependencyDescriptor(item.Value.FromType, item.Value.ToType, Lifetime.Singleton), (k, v) => { return v; });
                }
                else
                {
                    container.m_registrations.AddOrUpdate(item.Key, item.Value, (k, v) => { return v; });
                }
            }
            return new ContainerProvider(container);
        }
    }
}