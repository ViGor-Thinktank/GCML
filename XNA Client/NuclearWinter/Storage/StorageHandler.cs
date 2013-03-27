using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NuclearWinter.Storage
{
    public abstract class StorageHandler
    {
        public abstract BinaryReader OpenRead( string _strFilename );
        public abstract BinaryWriter OpenWrite( string _strFilename );
    }
}
