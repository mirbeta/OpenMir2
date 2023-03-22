using System.Buffers;
using System.Text;

namespace GameSrv.Network
{
    public class MemoryReader : TextReader
    {
        private readonly IMemoryOwner<byte> _memoryOwner;
        private static readonly SpanAction<char, (ReadOnlyMemory<byte>, Decoder)> StringCreator = Action;
        private readonly Decoder _decoder = Encoding.ASCII.GetDecoder();
        private ReadOnlyMemory<byte> _memory;
        private const byte Newline = (byte) '\n';
        private const byte CarriageReturn = (byte) '\r';

        private MemoryReader(ReadOnlyMemory<byte> memory)
        {
            _memory = memory;
        }

        private MemoryReader(IMemoryOwner<byte> memoryOwner, int sequenceLength)
        {
            _memoryOwner = memoryOwner;
            _memory = memoryOwner.Memory.Slice(0, sequenceLength);
        }

        public override ValueTask<int> ReadBlockAsync(Memory<char> buffer, CancellationToken cancellationToken = default)
        {
            return new ValueTask<int>(Read(buffer.Span));
        }

        public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
        {
            return Task.FromResult(Read(buffer.AsSpan(index, count)));
        }

        public override int Peek()
        {
            if (_memory.Length == 0) return -1;
            Span<char> chars = stackalloc char[1];
            _decoder.GetChars(_memory.Span, chars, false);
            return chars[0];
        }

        public override int Read()
        {
            if (_memory.Length == 0) return -1;
            Span<char> chars = stackalloc char[1];
            _decoder.GetChars(_memory.Span, chars, false);
            _memory = _memory.Slice(1);
            return chars[0];
        }

        public override int Read(Span<char> buffer)
        {
            if (_memory.Length == 0) return 0;
            _decoder.Convert(_memory.Span, buffer, false, out int bytesUsed, out int charsUsed, out bool completed);
            _memory = _memory.Slice(bytesUsed);
            return charsUsed;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            return Read(buffer.AsSpan(index, count));
        }

        public override Task<int> ReadAsync(char[] buffer, int index, int count)
        {
            return Task.FromResult(Read(buffer.AsSpan(index, count)));
        }

        public override ValueTask<int> ReadAsync(Memory<char> buffer, CancellationToken cancellationToken = default)
        {
            return new ValueTask<int>(Read(buffer.Span));
        }

        public override int ReadBlock(char[] buffer, int index, int count)
        {
            return Read(buffer.AsSpan(index, count));
        }

        public override int ReadBlock(Span<char> buffer)
        {
            return Read(buffer);
        }

        public override string ReadLine()
        {
            if (_memory.Length == 0) return null;

            int newLine = _memory.Span.IndexOf(Newline);
            ReadOnlyMemory<byte> chunk;
            switch (newLine)
            {
                case -1:
                    chunk = _memory;
                    _memory = default;
                    break;
                case 0:
                    return string.Empty;
                default:
                    chunk = _memory.Slice(0, newLine);
                    _memory = _memory.Slice(newLine + 1);
                    break;
            }

            var span = chunk.Span;
            if (span[span.Length - 1] == CarriageReturn)
            {
                chunk = chunk.Slice(0, span.Length - 1);
            }

            var count = _decoder.GetCharCount(chunk.Span, false);
            return string.Create(count, (chunk, _decoder), StringCreator);
        }

        public override Task<string> ReadLineAsync()
        {
            return Task.FromResult<string>(ReadLine());
        }

        public override string ReadToEnd()
        {
            var chunk = _memory;
            _memory = default;
            var count = _decoder.GetCharCount(chunk.Span, false);
            return string.Create(count, (chunk, _decoder), StringCreator);
        }

        public override Task<string> ReadToEndAsync()
        {
            return Task.FromResult<string>(ReadToEnd());
        }

        protected override void Dispose(bool disposing)
        {
            _memoryOwner?.Dispose();
        }

        private static void Action(Span<char> span, (ReadOnlyMemory<byte> chunk, Decoder decoder) arg)
        {
            var (chunk, decoder) = arg;
            decoder.GetChars(chunk.Span, span, false);
        }
        
        public static MemoryReader Create(Memory<byte> memory) => new MemoryReader(memory);

        public static MemoryReader Create(ReadOnlySequence<byte> sequence)
        {
            if (sequence.IsSingleSegment)
            {
                return new MemoryReader(sequence.First);
            }

            var memory = MemoryPool<byte>.Shared.Rent((int) sequence.Length);
            sequence.CopyTo(memory.Memory.Span);
            return new MemoryReader(memory, (int) sequence.Length);
        }
    }
}