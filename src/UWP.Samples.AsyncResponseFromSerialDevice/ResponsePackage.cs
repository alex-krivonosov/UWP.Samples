using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.Samples.AsyncResponseFromSerialDevice
{
    /// <summary>
    /// Represents a package with response from serial device.
    /// </summary>
    public class ResponsePackage
    {
        #region Properties

        /// <summary>
        /// Gets an unique identifier of package.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets a data of package.
        /// </summary>
        public byte[] Data { get; private set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parses bytes of the package.
        /// </summary>
        public void ParseBytes(byte[] bytes)
        {
            // For example, packages have a following format:
            // [0xfe][PackageId][Data][0xff]

            this.Id = BitConverter.ToInt32(bytes.Skip(1).Take(4).ToArray(), 0);
            this.Data = bytes.Skip(5).Take(bytes.Count() - 6).ToArray();
        }

        #endregion
    }
}
