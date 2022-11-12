using System;
using System.Collections.Generic;

namespace NESharp.NES
{

    public abstract class Mapper
    {
        public List<MemoryAccessHandler<ReadHandler >> CPUReadHandlers   = new List<MemoryAccessHandler<ReadHandler >>();
        public List<MemoryAccessHandler<WriteHandler>> CPUWriteHandlers  = new List<MemoryAccessHandler<WriteHandler>>();
        public List<MemoryAccessHandler<ReadHandler >> PPUReadHandlers   = new List<MemoryAccessHandler<ReadHandler >>();
        public List<MemoryAccessHandler<WriteHandler>> PPUWriteHandlers  = new List<MemoryAccessHandler<WriteHandler>>();

        public abstract void Initialize();
    }
}