using System;
using System.Collections.Generic;
using System.Linq;
using SystemModule.NativeList.Utils;

namespace SystemModule.NativeList.Helpers;

public static class NativeLinqExtensions
{
    /// <summary>
    /// Allocates new <see cref="NativeBuffer{TItem}"/> and fill it with input items.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    public static NativeBuffer<TItem> ToNativeBuffer<TItem>(this IEnumerable<TItem> items) where TItem : unmanaged
    {
        ArgumentsGuard.ThrowIfNull(items);

        int length = items.Count();
        NativeBuffer<TItem> result = new NativeBuffer<TItem>(length);
        int index = 0;

        foreach (TItem item in items)
            result[index++] = item;

        return result;
    }
}