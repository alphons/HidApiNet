using System;
using HidApiNet;

namespace Weather
{
	class Station : IDisposable
	{
		private HidDevice device;

		private const byte EndMark			= 0x20;
		private const byte ReadCommand		= 0xA1;
		private const byte WriteCommand		= 0xA0;
		private const byte WriteCommandWord	= 0xA2;
		private const byte Zero				= 0x00;

		private const byte Acknowledge		= 0xA5;

		public Station(ushort vid, ushort pid)
		{
			this.device = new HidDevice(vid, pid);
		}

		public int SetNonBlocking()
		{
			return this.device.SetNonBlocking();
		}

		public bool IsOpen
		{
			get
			{
				return this.device.IsOpen;
			}
		}

		private int ReadTimeOut(byte[] buffer, int t)
		{
			return device.ReadTimeOut(buffer, t);
		}

		public void Dispose()
		{
			device.Dispose();
		}

		private int Write(byte[] buffer)
		{
			return device.Write(buffer);
		}

		private int Read(byte[] buffer)
		{
			int total = 0;
			var smallbuffer = new byte[8];
			while (total < buffer.Length)
			{
				int intToRead = Math.Min(buffer.Length - total, smallbuffer.Length);
				int len = device.Read(smallbuffer, intToRead);
				if (len == 0)
					break;
				Array.Copy(smallbuffer, 0, buffer, total, len);
				total += len;
			}
			return total;
		}

		public int ReadBlock(ushort intAdres, byte[] buffer)
		{
			var send = new byte[] { ReadCommand, (byte)(intAdres >> 8), (byte)(intAdres & 0xff), EndMark, ReadCommand, (byte)(intAdres >> 8), (byte)(intAdres & 0xff), EndMark };

			var len = Write(send);
			if (len != send.Length)
				return 0;
			return Read(buffer);
		}

		public bool WriteByte(ushort adres, byte b)
		{
			var send = new byte[] { WriteCommand, (byte)(adres >> 8), (byte)(adres & 0xff), EndMark, WriteCommandWord, b, Zero, EndMark };

			var len = Write(send);
			if (len != send.Length)
				return false;

			var buffer = new byte[8];
			var intRead = Read(buffer);
			if (intRead == 0)
				return false;

			for (int intI = 0; intI < buffer.Length; intI++)
			{
				if (buffer[intI] != Acknowledge)
					return false;
			}
			return true;
		}

	}
}
