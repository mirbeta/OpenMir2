using System;
using System.Collections.Generic;
using System.Linq;
using SystemModule.NativeList.Utils;

namespace SystemModule.NativeList.Helpers
{
    public static class NativeLinqExtensions
    {
        /// <summary>
        /// Allocates new <see cref="NativeBuffer{TItem}"/> and fill it with input items.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static NativeBuffer<TItem> ToNativeBuffer<TItem>(this IEnumerable<TItem> items) where TItem : unmanaged
        {
            ArgumentsGuard.ThrowIfNull(items);

            var length = items.Count();
            var result = new NativeBuffer<TItem>(length);
            var index = 0;

            foreach (var item in items)
                result[index++] = item;

            return result;
        }
    }
}
