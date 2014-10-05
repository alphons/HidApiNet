using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Text;

namespace HIDProject
{
	class Program
	{
		static void Main(string[] args)
		{
			const ushort vid = 0x1941;
			const ushort pid = 0x8021;

			if (args.Length == 0)
			{
				Console.WriteLine("commands: ... l(ist) r(ead) s(erver) d(dump) c(urrent)");
				return;
			}

			var command = args[0];

			if (command[0] == 'l')
			{
				var devs = new HidApiNet.HidDeviceInfoCollection();
				foreach (var cur_dev in devs)
				{
					Console.WriteLine("Device Found");
					Console.WriteLine("\ttype: {0:x4} {1:x4}", cur_dev.vendor_id, cur_dev.product_id);
					Console.WriteLine("\tpath: {0}", cur_dev.path);
					Console.WriteLine("\tserial_number: {0}", cur_dev.serial_number ?? "(null)");
					Console.WriteLine("\tManufacturer: {0}", cur_dev.manufacturer_string ?? "(null)");
					Console.WriteLine("\tProduct: {0}", cur_dev.product_string ?? "(null)");
					Console.WriteLine("\tRelease: {0:x}", cur_dev.release_number);
					Console.WriteLine("\tInterface: {0}", cur_dev.interface_number);
					Console.WriteLine();
				}
				devs.Dispose();
				return;
			}

			if (command[0] == 'r')
			{
				var station = new Weather.Station(vid, pid);
				if (station.IsOpen)
				{
					var buffer = new byte[32];
					for (ushort adres = 0x0000; adres < 0x100; adres += 0x20)
					{
						var len = station.ReadBlock(adres, buffer);
						var sb = new StringBuilder();
						sb.AppendFormat("{0:x4} ", adres);
						for (int intJ = 0; intJ < len; intJ++)
							sb.AppendFormat("{0:x2} ", buffer[intJ]);
						Console.WriteLine(sb.ToString());
					}
					station.Dispose();
				}
				else
				{
					Console.WriteLine("WeatherStation not found");
				}
			}

			if (command[0] == 'd')
			{
				var station = new Weather.Station(vid, pid);
				if (station.IsOpen)
				{
					var buffer = new byte[32];
					for (ushort adres = 0x0000; adres < 0x100; adres += 0x20)
					{
						var len = station.ReadBlock(adres, buffer);
						var sb = new StringBuilder();
						//sb.AppendFormat("{0:x4} ", adres);
						for (int intJ = 0; intJ < len; intJ++)
							sb.AppendFormat("{0:x2} ", buffer[intJ]);
						Console.Write(sb.ToString());
					}
					station.Dispose();
					Console.WriteLine();
				}
				else
				{
					Console.WriteLine("WeatherStation not found");
				}
			}

			if (command[0] == 's')
			{
				Console.WriteLine("Server: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
				Console.WriteLine("Starting host listening on port 20402");
				var host = new ServiceHost(typeof(Weather.Server),
					new Uri[] { new Uri("http://localhost:20402/Path1") });

				host.AddServiceEndpoint(typeof(Weather.IContract), new BasicHttpBinding(), "MyTest1");
				host.Open();

				Console.WriteLine("Hit return to stop server");
				Console.ReadLine();

				host.Close();

			}

			if (command[0] == 'c')
			{
				var weatherstation = new Weather.Station(vid, pid);
				if (weatherstation.IsOpen)
				{
					var total = new byte[256];
					var buffer = new byte[32];
					int intIndex = 0;
					for (ushort adres = 0x0000; adres < 0x100; adres += 0x20)
					{
						Debug.WriteLine("Reading block {0:x}", adres);
						var len = weatherstation.ReadBlock(adres, buffer);
						Array.Copy(buffer, 0, total, intIndex, buffer.Length);
						intIndex += buffer.Length;
						Debug.WriteLine("Done.");
						System.Threading.Thread.Sleep(100);
					}

					var stationData = Weather.Data.Helper.BytesToStruct<Weather.Data.StationData>(total);

					Debug.WriteLine("Getting CurrentData");

					var settings1 = stationData.settings_1.ToEnum<Weather.Data.Settings1Enum>();
					var settings2 = stationData.settings_2.ToEnum<Weather.Data.Settings2Enum>();

					ushort currentPosition = stationData.current_pos.ToUInt16();
					var lenLiveData = weatherstation.ReadBlock(currentPosition, buffer);

					weatherstation.Dispose();

					Debug.WriteLine("Read {0} bytes", lenLiveData);
					//var liveData = Helper.BytesToStruct<LiveData1080>(buffer);

					var sb = new StringBuilder();
					for (int intI = 0; intI < 16; intI++)
						sb.AppendFormat("{0:x2}", buffer[intI]);

					Console.WriteLine(""+sb);
				}
				else
				{
					Console.WriteLine("WeatherStation not found");
				}
			}

		}
	}
}
