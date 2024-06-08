namespace NESharp.NES
{
    public delegate byte ReadHandler(ushort address);
    public delegate void WriteHandler(ushort address, byte value);

    public class MemoryAccessHandler<T>
    {
        public ushort StartAddress { get; private set; }
        public ushort EndAddress { get; private set; }
        public T Handler { get; private set; }
        public MemoryAccessHandler(ushort start, ushort end, T handler)
        {
            StartAddress = start;
            EndAddress = end;
            Handler = handler;
        }
    }

    public class BaseMemory
    {
        public int Length => _memory.Length;
        internal byte[] _memory;
        public BaseMemory(int length = 1024 * 64)
        {
            _memory = new byte[length];
        }

        public virtual byte this[int index]
        {
            get
            {
                return _memory[index];
            }
            set
            {
                _memory[index] = value;
            }
        }

        public virtual byte   ReadByte(int addr) => this[addr];
        public virtual ushort ReadWord(int addr) => (ushort)(this[addr] | (this[addr + 1] << 8));

        public static implicit operator byte[] (BaseMemory m) => m._memory;
    }
}
