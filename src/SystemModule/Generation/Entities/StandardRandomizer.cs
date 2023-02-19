using System;
using SystemModule.Generation.Interfaces.Entities;

namespace SystemModule.Generation.Entities
{
    /// <summary>
    /// Copy paste from standard library.
    /// Just to make it compatible with the interface.
    /// </summary>
    public class StandardRandomizer : IRandomizer
    {
        private const int MBIG = Int32.MaxValue;
        private const int MSEED = 161803398;

        private int _inext;
        private int _inextp;
        private int[] _seedArray;

        public StandardRandomizer() : this(Environment.TickCount)
        { }

        public StandardRandomizer(int seed)
        {
            _seedArray = new int[56];

            int ii;
            int mj, mk;

            //Initialize our Seed array.
            //This algorithm comes from Numerical Recipes in C (2nd Ed.)
            int subtraction = (seed == Int32.MinValue) ? Int32.MaxValue : Math.Abs(seed);

            Seed = seed;

            mj = MSEED - subtraction;

            _seedArray[55] = mj;

            mk = 1;

            for (int i = 1; i < 55; i++)
            {  //Apparently the range [1..55] is special (Knuth) and so we're wasting the 0'th position.
                ii = (21 * i) % 55;

                _seedArray[ii] = mk;

                mk = mj - mk;

                if (mk < 0) 
                    mk += MBIG;

                mj = _seedArray[ii];
            }

            for (int k = 1; k < 5; k++)
            {
                for (int i = 1; i < 56; i++)
                {
                    _seedArray[i] -= _seedArray[1 + (i + 30) % 55];

                    if (_seedArray[i] < 0) 
                        _seedArray[i] += MBIG;
                }
            }

            _inext = 0;
            _inextp = 21;
        } 

        /// <inheritdoc/>
        public long Seed { get; private set; }

        /// <inheritdoc/>
        public int NextInteger()
        {
            int retVal;
            int locINext = _inext;
            int locINextp = _inextp;

            if (++locINext >= 56) 
                locINext = 1;
            
            if (++locINextp >= 56) 
                locINextp = 1;

            retVal = _seedArray[locINext] - _seedArray[locINextp];

            if (retVal == MBIG) 
                retVal--;

            if (retVal < 0) 
                retVal += MBIG;

            _seedArray[locINext] = retVal;

            _inext = locINext;
            _inextp = locINextp;

            return retVal;
        }
        
        public int NextInteger(int maxValue)
        {
            return NextInteger() * maxValue;
        }

        /// <inheritdoc/>
        public double NextNormalized()
        {
            return NextInteger() * (1.0 / MBIG);
        }
    }
}
