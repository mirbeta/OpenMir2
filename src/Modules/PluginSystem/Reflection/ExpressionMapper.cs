using System.Linq.Expressions;

namespace PluginEngine.Reflection
{

    /// <summary>
    /// 表达式复制
    /// </summary>
    public class ExpressionMapper
    {
        private static readonly Dictionary<string, object> m_dic = new Dictionary<string, object>();

        /// <summary>
        /// 字典缓存表达式树
        /// </summary>
        public static TOut Trans<TIn, TOut>(TIn tIn)
        {
            string key = string.Format("funckey_{0}_{1}", typeof(TIn).FullName, typeof(TOut).FullName);
            if (!m_dic.ContainsKey(key))
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
                List<MemberBinding> memberBindingList = new List<MemberBinding>();
                foreach (System.Reflection.PropertyInfo item in typeof(TOut).GetProperties())
                {
                    MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                    MemberBinding memberBinding = Expression.Bind(item, property);
                    memberBindingList.Add(memberBinding);
                }
                foreach (System.Reflection.FieldInfo item in typeof(TOut).GetFields())
                {
                    MemberExpression property = Expression.Field(parameterExpression, typeof(TIn).GetField(item.Name));
                    MemberBinding memberBinding = Expression.Bind(item, property);
                    memberBindingList.Add(memberBinding);
                }
                MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
                Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, parameterExpression);
                Func<TIn, TOut> func = lambda.Compile();//拼装是一次性的
                m_dic[key] = func;
            }
            return ((Func<TIn, TOut>)m_dic[key]).Invoke(tIn);
        }
    }
}