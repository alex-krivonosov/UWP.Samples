using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.Samples.AsyncResponseFromSerialDevice
{
    /// <summary>
    /// Represents a package with request to serial device.
    /// </summary>
    public class RequestPackage
    {
        #region Properties

        /// <summary>
        /// Gets an unique identifier of package.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a data of package.
        /// </summary>
        public byte[] Data { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets bytes of the package.
        /// </summary>
        public byte[] GetBytes()
        {
            // For example, packages have a following format:
            // [0xfe][PackageId][Data][0xff]

            var bytes = new List<byte>();
            bytes.Add(0xfe);             
            bytes.AddRange(BitConverter.GetBytes(this.Id));
            bytes.AddRange(this.Data);
            bytes.Add(0xff); 
            return bytes.ToArray();
        }

        #endregion
    }
}
