using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace UWP.Samples.AsyncResponseFromSerialDevice
{
    /// <summary>
    /// Package manager for async request-response to serial device. 
    /// </summary>
    public class PackageManager
    {
        #region Private fields

        /// <summary>
        /// A package counter.
        /// </summary>
        private int packageCounter;

        /// <summary>
        /// A data reader for connected serial device.
        /// </summary>
        private DataReader dataReader;

        /// <summary>
        /// A data writer for connected serial device.
        /// </summary>
        private DataWriter dataWriter;

        /// <summary>
        /// A list of packages which waiting for response.
        /// </summary>
        private Dictionary<int, TaskCompletionSource<byte[]>> waitingForResponse;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="PackageManager"/> class.
        /// </summary>
        public PackageManager(SerialDevice serialDevice)
        {
            this.dataWriter = new DataWriter(serialDevice.OutputStream);
            this.dataReader = new DataReader(serialDevice.InputStream);
            this.dataReader.InputStreamOptions = InputStreamOptions.None;
            this.waitingForResponse = new Dictionary<int, TaskCompletionSource<byte[]>>();
            this.Listen();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Sends a request to serial device.
        /// </summary>
        public async Task<byte[]> SendAsync(byte[] data)
        {
            var package = new RequestPackage();
            package.Data = data;
            package.Id = this.packageCounter++;
            var sendCompletionSource = new TaskCompletionSource<byte[]>();
            this.waitingForResponse.Add(package.Id, sendCompletionSource);
            dataWriter.WriteBytes(package.GetBytes());
            await dataWriter.StoreAsync();
            return await sendCompletionSource.Task;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Listens the serial port for input packages.
        /// </summary>
        private async void Listen()
        {
            // For example, packages have a following format:
            // [0xfe][PackageId][Data][0xff]

            var packageBytes = new List<byte>();

            while (true)
            {                 
                await this.dataReader.LoadAsync(1);
                byte currentByte = this.dataReader.ReadByte();
                if (currentByte == 0xfe) packageBytes.Clear();
                packageBytes.Add(currentByte);

                if (currentByte == 0xff)
                {
                    var responsePackage = new ResponsePackage();
                    responsePackage.ParseBytes(packageBytes.ToArray());
                    if (!this.waitingForResponse.ContainsKey(responsePackage.Id)) continue;
                    var completionSource = this.waitingForResponse[responsePackage.Id];
                    completionSource.SetResult(responsePackage.Data);
                    this.waitingForResponse.Remove(responsePackage.Id);
                }
            }
        }

        #endregion
    }
}
