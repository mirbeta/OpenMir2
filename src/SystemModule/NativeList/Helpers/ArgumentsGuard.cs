using System;
using System.Runtime.CompilerServices;
using SystemModule.NativeList.Interfaces.Shared;
using SystemModule.NativeList.Utils;

namespace SystemModule.NativeList.Helpers
{
    public static class ArgumentsGuard
    {
        public const String WrongObjectType = "The input value has incorrect type.";

        public const String ValueIsEqualError = "The input value: {0} - cannot be equal to {1}.";
       
        public const String ValueIsLessError = "The input value: {0} - is less than {1}.";
        public const String ValueIsLessOrEqualError = "The input value: {0} - is less or equal to {1}.";
        
        public const String ValueIsGreaterError = "The input value: {0} - is greater than {1}.";
        public const String ValueIsGreaterOrEqualError = "The input value: {0} - is greater or equal to {1}.";
        
        public const double ApproximationValue = 0.000001;

        //=========================================================================//
        //CHECK OBJECT STATE

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the input object link is null.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(Object value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if the input object is disposed.
        /// </summary>
        /// <remarks>
        /// Method doesn't check object on null.
        /// <br/>Object has to implement <see cref="IDisposeIndication"/>
        /// </remarks>
        /// <exception cref="ObjectDisposedException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfDisposed(IDisposeIndication value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value.IsDisposed)
                throw new ObjectDisposedException(name, $"The object {value.GetType().Name} is already disposed.");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the input object has wrong type.
        /// </summary>
        /// <remarks>
        /// Method doesn't check object on null.
        /// </remarks>
        /// <exception cref="ObjectDisposedException"/>
        public static void ThrowIfNotType<TType>(Object value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (!(value is TType))
                throw new ArgumentException(WrongObjectType, name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the input object has wrong type.
        /// <br/> Can return casted object as out parameter
        /// </summary>
        /// <remarks>
        /// Method doesn't check object on null.
        /// </remarks>
        /// <exception cref="ObjectDisposedException"/>
        public static void ThrowIfNotType<TType>(Object value, out TType casted, [CallerArgumentExpression("value")] String name = "value")
        {
            casted = (TType)value;

            if (!(value is TType))
                throw new ArgumentException(WrongObjectType, name);
        }

        //=========================================================================//
        //CHECK VALUE EQUAL TO DESIRED

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input signed integer number is equal to input.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfEqual(long value, long input, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value == input)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsEqualError, value, input));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input unsigned integer number is equal to input.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfEqual(ulong value, ulong input, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value == input)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsEqualError, value, input));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input float number is equal to input.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfEqual(double value, double input, [CallerArgumentExpression("value")] String name = "value")
        {
            if (Math.Abs(value - input) < ApproximationValue)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsEqualError, value, input));
        }

        //=========================================================================//
        //CHECK VALUE LESS THAN DESIRED

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input signed integer number is less then desired.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLess(long value, long desired, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value < desired)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsLessError, value, desired));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input unsigned integer number is less then desired.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLess(ulong value, ulong desired, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value < desired)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsLessError, value, desired));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input float number is less then desired.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLess(double value, double desired, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value < desired)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsLessError, value, desired));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input signed integer number is less or equal to desired.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLessOrEqual(long value, long desired, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value <= desired)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsLessOrEqualError, value, desired));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input unsigned integer number is less or equal to desired.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLessOrEqual(ulong value, ulong desired, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value <= desired)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsLessOrEqualError, value, desired));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input float number is less or equal to desired.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="ApproximationValue"/> for equal comparison.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLessOrEqual(double value, double desired, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value < desired || Math.Abs(value - desired) < ApproximationValue)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsLessOrEqualError, value, desired));
        }

        //=========================================================================//
        //CHECK VALUE GREATER THAN DESIRED

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input signed integer number is greater than desired.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfGreater(long value, long desired, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value > desired)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsGreaterError, value, desired));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input unsigned integer number is greater than desired.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfGreater(ulong value, ulong desired, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value > desired)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsGreaterError, value, desired));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input float number is greater than desired.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfGreater(double value, double desired, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value > desired)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsGreaterError, value, desired));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input signed integer number is greater or equal to desired.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfGreaterOrEqual(long value, long desired, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value >= desired)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsGreaterOrEqualError, value, desired));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input unsigned integer number is greater or equal to desired.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfGreaterOrEqual(ulong value, ulong desired, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value >= desired)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsGreaterOrEqualError, value, desired));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input float number is greater or equal to desired.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="ApproximationValue"/> for equal comparison.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfGreaterOrEqual(double value, double desired, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value > desired || Math.Abs(value - desired) < ApproximationValue)
                throw new ArgumentOutOfRangeException(name, String.Format(ValueIsGreaterOrEqualError, value, desired));
        }

        //=========================================================================//
        //CHECK VALUE RELATIVELY TO ZERO

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input signed integer number is less 0.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLessZero(long value, [CallerArgumentExpression("value")] String name = "value")
        {
            ThrowIfLess(value, 0, name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input float number is less 0.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLessZero(double value, [CallerArgumentExpression("value")] String name = "value")
        {
            ThrowIfLess(value, 0, name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input signed integer number is less or equal to 0.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLessOrEqualZero(long value, [CallerArgumentExpression("value")] String name = "value")
        {
            ThrowIfLessOrEqual(value, 0, name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input float number is less or equal to 0.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="ApproximationValue"/> for equal comparison.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLessOrEqualZero(double value, [CallerArgumentExpression("value")] String name = "value")
        {
            ThrowIfLessOrEqual(value, 0, name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input signed integer number is less or equal to 0.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfEqualZero(long value, [CallerArgumentExpression("value")] String name = "value")
        {
            ThrowIfEqual(value, 0, name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input unsigned integer number is less or equal to 0.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfEqualZero(ulong value, [CallerArgumentExpression("value")] String name = "value")
        {
            ThrowIfEqual(value, 0, name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input float number is less or equal to 0.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="ApproximationValue"/> for equal comparison.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfEqualZero(double value, [CallerArgumentExpression("value")] String name = "value")
        {
            ThrowIfEqual(value, 0, name);
        }

        //=========================================================================//
        //CHECK RANGE

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input item is not in range.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotInRange<TItem>(TItem value, FlexibleRange<TItem> range, [CallerArgumentExpression("value")] String name = "value")
        {
            if (!range.IsInRange(value))
                throw new ArgumentOutOfRangeException(name);
        }

        //=========================================================================//
        //CHECK STRING STATE

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the input string is null or empty.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNullOrEmpty(String value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException(name, "String cannot be null or empty.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfObjectDisposed(IDisposeIndication value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value.IsDisposed)
                throw new ObjectDisposedException(name);
        }
    }
}
