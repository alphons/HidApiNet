using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace HidApiNet
{
	class HidDeviceInfoCollection : List<HidDeviceInfo>, IDisposable
	{
		private IntPtr devs = IntPtr.Zero;

		public HidDeviceInfoCollection()
		{
			var res = Interop.hid_init();
			if (res != 0)
				return;

			this.devs = Interop.hid_enumerate(0x00, 0x00);
			var curdev = new IntPtr(this.devs.ToInt32());
			while (curdev != IntPtr.Zero)
			{
				var info = (HidDeviceInfo)Marshal.PtrToStructure(curdev, typeof(HidDeviceInfo));
				this.Add(info);
				curdev = info.next;
			}
		}

		public void Dispose()
		{
			if (this.devs != IntPtr.Zero)
				Interop.hid_free_enumeration(this.devs);
		}

	}

}
