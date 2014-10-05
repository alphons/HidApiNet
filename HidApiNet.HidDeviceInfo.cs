using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HidApiNet
{
	[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
	unsafe struct HidDeviceInfo
	{
		[MarshalAs(UnmanagedType.LPStr)]
		public readonly string path;
		public readonly ushort vendor_id;
		public readonly ushort product_id;
		[MarshalAs(UnmanagedType.LPWStr)]
		private readonly IntPtr _serial_number;
		public readonly ushort release_number;
		[MarshalAs(UnmanagedType.LPWStr)]
		private readonly IntPtr _manufacturer_string;
		[MarshalAs(UnmanagedType.LPWStr)]
		private readonly IntPtr _product_string;
		public readonly ushort usage_page;
		public readonly ushort usage;
		public readonly int interface_number;
		public readonly IntPtr next;

		private int StringLength(IntPtr ptr, int maxlen)
		{
			if (ptr == IntPtr.Zero)
				return 0;
			for (int intLen = 0; intLen < maxlen; intLen++)
			{
				int* endchar = (int*)ptr;
				if (*endchar == 0)
					return intLen;
				ptr = new IntPtr(ptr.ToInt32() + 4);
			}
			return 0;
		}
		private string GetString(IntPtr ptr)
		{
			int intLen = StringLength(ptr, 128);
			if (intLen == 0)
				return null;
			var buf = new byte[intLen * 4];
			Marshal.Copy(ptr, buf, 0, buf.Length);
			return Encoding.Unicode.GetString(buf);
		}

		public string product_string
		{
			get
			{
				return GetString(_product_string);
			}
		}
		public string manufacturer_string
		{
			get
			{
				return GetString(_manufacturer_string);
			}
		}
		public string serial_number
		{
			get
			{
				return GetString(_serial_number);
			}
		}
	}

	
}
