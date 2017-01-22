using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;

namespace UWP.Samples.AsyncResponseFromSerialDevice
{
    /// <summary>
    /// Shows how to use this sample. 
    /// </summary>
    /// <remarks>
    /// You can use this code if you send 'command' to a serial device and 
    /// the response to this command can be received after several other 
    /// commands sent to device.
    /// </remarks>
    public class HowToUse
    {
        /// <summary>
        /// Runs sammple.
        /// </summary>
        public static async void Run()
        {
            string aqs = SerialDevice.GetDeviceSelector();
            var myDevices = await DeviceInformation.FindAllAsync(aqs);
            if (myDevices.Count == 0) return;
            var device = await SerialDevice.FromIdAsync(myDevices.First().Id);
            if (device == null) return;

            device.BaudRate = 9600;
            device.DataBits = 8;
            device.StopBits = SerialStopBitCount.One;
            device.Parity = SerialParity.None;
            device.Handshake = SerialHandshake.None;

            var packageManager = new PackageManager(device);

            var command1 = packageManager.SendAsync(new byte[] { /* bytes of command 1 */});
            var command2 = packageManager.SendAsync(new byte[] { /* bytes of command 2 */});
            var command3 = packageManager.SendAsync(new byte[] { /* bytes of command 3 */});
            var command4 = packageManager.SendAsync(new byte[] { /* bytes of command 4 */});

            var result1 = await command1;
            var result2 = await command2;
            var result3 = await command3;
            var result4 = await command4;
        }
    }
}
