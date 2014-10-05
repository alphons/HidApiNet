using System;
using System.Runtime.InteropServices;

//Wraps: libhidapi-libusb

namespace HidApiNet
{
	class Interop
	{
		[DllImport("libhidapi-libusb.so")]
		public static extern int hid_init();

		[DllImport("libhidapi-libusb.so")]
		public static extern int hid_exit();

		[DllImport("libhidapi-libusb.so")]
		public static extern IntPtr hid_enumerate(UInt16 vendorId, UInt16 productId);

		[DllImport("libhidapi-libusb.so")]
		public static extern void hid_free_enumeration(IntPtr devs);

		[DllImport("libhidapi-libusb.so", CharSet = CharSet.Auto)]
		public static extern IntPtr hid_open(UInt16 vendorId, UInt16 productId, string serialNumber);

		[DllImport("libhidapi-libusb.so")]
		public static extern void hid_close(IntPtr device);

		[DllImport("libhidapi-libusb.so", CharSet = CharSet.Ansi)]
		public static extern IntPtr hid_open_path(string path);

		[DllImport("libhidapi-libusb.so")]
		public static extern int hid_set_nonblocking(IntPtr device, int nonBlock);

		[DllImport("libhidapi-libusb.so")]
		public static extern int hid_read(IntPtr device, [Out] byte[] data, int length);

		[DllImport("libhidapi-libusb.so")]
		public static extern int hid_read_timeout(IntPtr device, [Out] byte[] data, int length, int milliseconds);

		[DllImport("libhidapi-libusb.so")]
		public static extern int hid_write(IntPtr device, [In] byte[] data, int length);

		[DllImport("libhidapi-libusb.so")]
		public static extern int hid_get_feature_report(IntPtr device, [Out] string buf, int length);

		[DllImport("libhidapi-libusb.so")]
		public static extern int hid_send_feature_report(IntPtr device, [In] byte[] buf, int length);

		[DllImport("libhidapi-libusb.so", CharSet = CharSet.Unicode)]
		public static extern int hid_get_manufacturer_string(IntPtr device, [Out] string buf, int len);

		[DllImport("libhidapi-libusb.so", CharSet = CharSet.Unicode)]
		public static extern int hid_get_product_string(IntPtr device, [Out] string buf, int len);

		[DllImport("libhidapi-libusb.so", CharSet = CharSet.Unicode)]
		public static extern int hid_get_serial_number_string(IntPtr device, [Out] string buf, int len);

		[DllImport("libhidapi-libusb.so", CharSet = CharSet.Unicode)]
		public static extern int hid_get_indexed_string(IntPtr device, int index, [Out] string buf, int len);

		[DllImport("libhidapi-libusb.so", CharSet = CharSet.Unicode)]
		public static extern String hid_error(IntPtr device);
	}
}
