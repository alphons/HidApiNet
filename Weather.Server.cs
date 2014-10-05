using System;
using System.Text;

namespace Weather
{
	public class Server : IContract
	{
		public string Info()
		{
			var sb = new StringBuilder();
			var devs = new HidApiNet.HidDeviceInfoCollection();
			foreach (var cur_dev in devs)
			{
				sb.AppendFormat("Device Found"+Environment.NewLine);
				sb.AppendFormat("\ttype: {0:x4} {1:x4}" + Environment.NewLine, cur_dev.vendor_id, cur_dev.product_id);
				sb.AppendFormat("\tpath: {0}" + Environment.NewLine, cur_dev.path);
				sb.AppendFormat("\tserial_number: {0}" + Environment.NewLine, cur_dev.serial_number ?? "(null)");
				sb.AppendFormat("\tManufacturer: {0}" + Environment.NewLine, cur_dev.manufacturer_string ?? "(null)");
				sb.AppendFormat("\tProduct: {0}" + Environment.NewLine, cur_dev.product_string ?? "(null)");
				sb.AppendFormat("\tRelease: {0:x}" + Environment.NewLine, cur_dev.release_number);
				sb.AppendFormat("\tInterface: {0}" + Environment.NewLine, cur_dev.interface_number);
				sb.AppendLine();
			}
			devs.Dispose();
			return sb.ToString();
		}

		public int ReadBlock(ushort vid, ushort pid, ushort intAdres, ref byte[] buffer)
		{
			//Console.WriteLine("{0}\\{1} on {2}", Environment.UserDomainName, Environment.UserName, Environment.MachineName);
			var res = 0;
			var station = new Weather.Station(vid, pid);
			//Console.WriteLine("Openened {0:x4} {1:x4} {2} ", vid, pid, station.IsOpen);
			if (station.IsOpen)
				res = station.ReadBlock(intAdres, buffer);
			station.Dispose();
			//Console.WriteLine("Read " + res + " bytes");
			return res;
		}

		public bool WriteByte(ushort vid, ushort pid, ushort intAdres, byte b)
		{
			bool res = false;
			var station = new Weather.Station(vid, pid);
			if (station.IsOpen)
				res = station.WriteByte(intAdres, b);
			station.Dispose();
			//Console.WriteLine("Read " + res + " bytes");
			return res;
		}

	}
}
