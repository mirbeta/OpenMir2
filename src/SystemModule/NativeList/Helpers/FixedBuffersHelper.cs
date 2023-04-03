using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace SystemModule.NativeList.Helpers;

public static unsafe class FixedBuffersHelper
{
    private const String BuffersSizeError = "Length of the array and cell size is not the same.";

    /// <inheritdoc cref="GetString(Span{byte}, Encoding)"/>
    /// <exception cref="ArgumentNullException">If pointer is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If the size is less than zero.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static String GetString(byte* pointer, int size, Encoding encoding)
    {
        NativeGuard.ThrowIfNull(pointer);
        ArgumentsGuard.ThrowIfLessZero(size);

        return GetString(new Span<byte>(pointer, size), encoding);
    }

    /// <inheritdoc cref="GetString(Span{byte}, Encoding)"/>
    /// <exception cref="ArgumentNullException">If buffer is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static String GetString(byte[] buffer, Encoding encoding)
    {
        ArgumentsGuard.ThrowIfNull(buffer);

        return GetString(new Span<byte>(buffer), encoding);
    }

    /// <summary>
    /// Gets string from the buffer from start to the first null terminator.
    /// </summary>
    /// <remarks>
    /// This method doesn't check that the input buffer is valid.
    /// <br/>Returns empty string if buffer length is zero.
    /// </remarks>
    /// <exception cref="ArgumentNullException">If encoding is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static String GetString(Span<byte> span, Encoding encoding)
    {
        ArgumentsGuard.ThrowIfNull(encoding);

        if (span.Length == 0)
        {
            return String.Empty;
        }

        int realSize = 0;
        int step = encoding.GetByteCount("\0");

        if (span.Length % step != 0)
        {
            throw new ArgumentException("Buffer size does not correspond to the char size.");
        }

        for (; realSize < span.Length; realSize += step)
        {
            int zerosCount = 0;

            for (int i = realSize; i < realSize + step; i++)
            {
                if (span[i] == 0)
                {
                    zerosCount++;
                }
            }

            if (zerosCount == step)
            {
                break;
            }
        }

        if (realSize == 0)
        {
            return string.Empty;
        }

        return encoding.GetString(span[..realSize]);
    }

    /// <inheritdoc cref="SetString(Span{byte}, Encoding, string)"/>
    /// <exception cref="ArgumentNullException">If pointer is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If the size is less than zero.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetString(byte* pointer, int size, Encoding encoding, String value)
    {
        NativeGuard.ThrowIfNull(pointer);
        ArgumentsGuard.ThrowIfLessZero(size);

        SetString(new Span<byte>(pointer, size), encoding, value);
    }

    /// <inheritdoc cref="SetString(Span{byte}, Encoding, string)"/>
    /// <exception cref="ArgumentNullException">If buffer is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetString(byte[] buffer, Encoding encoding, String value)
    {
        ArgumentsGuard.ThrowIfNull(buffer);

        SetString(new Span<byte>(buffer), encoding, value);
    }

    /// <summary>
    /// Sets the string value to the given buffer.
    /// </summary>
    /// <remarks>
    /// This method doesn't check that the input buffer is valid.
    /// <br/>Can accept null or empty value.
    /// <br/>Will set zero value to whole buffer in any case.
    /// <br/>If the string is longer than the buffer capacity will cut the string.
    /// </remarks>
    /// <exception cref="ArgumentNullException">If encoding is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetString(Span<byte> span, Encoding encoding, String value)
    {
        ArgumentsGuard.ThrowIfNull(encoding);

        if (span.Length == 0)
        {
            return;
        }

        span.Fill(0);

        if (String.IsNullOrEmpty(value))
        {
            return;
        }

        int step = encoding.GetByteCount("\0");
        int length = span.Length / step;

        if (value.Length > length)
        {
            encoding.GetBytes(value.AsSpan()[..length], span);
        }
        else
        {
            encoding.GetBytes(value.AsSpan(), span);
        }
    }

    /// <summary>
    /// Returns array copy of the given native buffer.
    /// </summary>
    /// <exception cref="ArgumentNullException">If pointer is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If size is less than zero.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TItem[] GetArray<TItem>(TItem* pointer, int length) where TItem : unmanaged
    {
        NativeGuard.ThrowIfNull(pointer);
        ArgumentsGuard.ThrowIfLessZero(length);

        if (length == 0)
        {
            return Array.Empty<TItem>();
        }

        TItem[] result = new TItem[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = pointer[i];
        }

        return result;
    }

    /// <summary>
    /// Copies input array to the given native buffer.
    /// </summary>
    /// <exception cref="ArgumentException">Length of the array doesn't not correspond to the size of the buffer.</exception>
    /// <exception cref="ArgumentNullException">If pointer is null.</exception>
    /// <exception cref="ArgumentNullException">If array is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If size is less than zero.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetArray<TItem>(TItem* pointer, int length, TItem[] array) where TItem : unmanaged
    {
        NativeGuard.ThrowIfNull(pointer);
        ArgumentsGuard.ThrowIfLessZero(length);
        ArgumentsGuard.ThrowIfNull(array);

        if (array.Length != length)
        {
            throw new ArgumentException(BuffersSizeError, nameof(array));
        }

        if (length == 0)
        {
            return;
        }

        for (int i = 0; i < array.Length; i++)
        {
            pointer[i] = array[i];
        }
    }
}