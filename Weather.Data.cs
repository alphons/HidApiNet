using System;
using System.Text;
using System.Runtime.InteropServices;

// From: https://jim-easterbrook.github.io/pywws/doc/en/html/_modules/pywws/WeatherStation.html

namespace Weather.Data
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct _date_time
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		private byte[] buffer;
		public override string ToString()
		{
			return ToDateTime.ToString();
		}

		private int BCD2INT(byte b)
		{
			return (10 * (b >> 4)) + (b & 0x0f);
		}

		public DateTime ToDateTime
		{
			get
			{
				return new DateTime(2000 + BCD2INT(buffer[0]), BCD2INT(buffer[1]), BCD2INT(buffer[2]), BCD2INT(buffer[3]), BCD2INT(buffer[4]), 0);
			}
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct _time
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		private byte[] buffer;
		public override string ToString()
		{
			return ToTimeSpan.ToString();
		}

		public TimeSpan ToTimeSpan
		{
			get
			{
				return new TimeSpan(buffer[0], buffer[1], 0);
			}
		}
	}


	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct _unsigned_int3
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		private byte[] buffer;

		public override string ToString()
		{
			return ToInt.ToString();
		}

		public int ToInt
		{
			get
			{
				return (buffer[2] << 16) + (buffer[1] << 8) + buffer[0];
			}
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct _signed_byte
	{
		private byte buffer;

		public override string ToString()
		{
			return ToInt.ToString();
		}

		public int ToInt
		{
			get
			{
				if (buffer >= 128)
					return 128 - buffer;
				else
					return buffer;
			}
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct _unsigned_short
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		private byte[] buffer;

		public override string ToString()
		{
			return ToUInt16().ToString();
		}

		public ushort ToUInt16()
		{
			return Convert.ToUInt16((buffer[1] << 8) + buffer[0]);
		}
	}


	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct _signed_short
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		private byte[] buffer;

		public override string ToString()
		{
			return ToInt16().ToString();
		}

		public short ToInt16()
		{
			if (buffer[1] >= 128)
				return Convert.ToInt16(((128 - buffer[1]) << 8) - buffer[0]);
			else
				return Convert.ToInt16((buffer[1] << 8) + buffer[0]);
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]

	public struct _wind
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] buffer;

		// wind average - 12 bits split across a byte and a nibble
		public int Average
		{
			get
			{
				return buffer[0] + ((buffer[2] & 0x0F) << 8);
			}
		}
		public int Gust
		{
			get
			{
				return buffer[0] + ((buffer[1] & 0xF0) << 4);
			}
		}
		public int Direction
		{
			get
			{
				return buffer[3];
			}
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct _wind_gust
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		private byte[] buffer;

		// wind gust - 12 bits split across a byte and a nibble
		public int ToInt
		{
			get
			{
				return buffer[0] + ((buffer[1] & 0xF0) << 4);
			}
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct _bit_field
	{
		private byte buffer;

		public override string ToString()
		{
			var sb = new StringBuilder();
			int intMask = 0x80;
			for (int intI = 0; intI < 8; intI++)
			{
				if ((buffer & intMask) != 0)
					sb.Append("1");
				else
					sb.Append("0");
				intMask = intMask >> 1;
			}
			return sb.ToString();
		}

		public T ToEnum<T>()
		{
			return (T)Enum.ToObject(typeof(T), buffer);
		}
	}

	[Flags]
	public enum Settings1Enum : uint
	{
		temp_in_F = 0x01,
		temp_out_F = 0x02,
		rain_in = 0x04,
		bit3 = 0x08,
		bit4 = 0x10,
		pressure_hPa = 0x20,
		pressure_inHg = 0x40,
		pressure_mmHg = 0x80
	}

	[Flags]
	public enum Settings2Enum : uint
	{
		wind_mps = 0x01,
		wind_kmph = 0x02,
		wind_knot = 0x04,
		wind_mph = 0x08,
		wind_bft = 0x10,
		bit5 = 0x20,
		bit6 = 0x40,
		bit7 = 0x80
	}

	[Flags]
	public enum Display1Enum : uint
	  {
		  pressure_rel = 0x01,
		  wind_gust = 0x02,
		  clock_12hr = 0x04,
		  date_mdy = 0x08,
		  time_scale_24 = 0x10,
		  show_year = 0x20,
		  show_day_name = 0x40,
		  alarm_time = 0x80
	  }

	[Flags]
	public enum Display2Enum : uint
	{
		temp_out_temp = 0x01,
		temp_out_chill = 0x02,
		temp_out_dew = 0x04,
		rain_hour = 0x08,
		rain_day = 0x10,
		rain_week = 0x20,
		rain_month = 0x40,
		rain_total = 0x80
	}

    [Flags]
	public enum Alarm1Enum : uint 
	{
		bit0, time = 0x01,
		wind_dir = 0x02,
		bit3 = 0x04,
		hum_in_lo = 0x08,
		hum_in_hi = 0x10,
		hum_out_lo = 0x20,
		hum_out_hi = 0x40
	}

    [Flags]
	public enum Alarm2Enum : uint
	{
		wind_ave = 0x01,
		wind_gust = 0x02,
		rain_hour = 0x04,
		rain_day = 0x08,
		pressure_abs_lo = 0x10,
		pressure_abs_hi = 0x20
	}

    [Flags]
	public enum Alarm3Enum : uint 
	{
		temp_in_lo = 0x01,
		temp_in_hi = 0x02,
		temp_out_lo = 0x04,
		temp_out_hi = 0x08,
		wind_chill_lo = 0x10,
		wind_chill_hi = 0x20,
		dew_point_lo = 0x40,
		dew_point_hi = 0x80
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LiveData1080
	{
		public byte delay;
		public byte hum_in;
		public _signed_short temp_in;
		public byte hum_out;
		public _signed_short temp_out;
		public _unsigned_short abs_pressure;
		public _wind wind;
		public _unsigned_short rain;
		public byte status;
	}


	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct StationData
	{
		public ushort MagicNumber;						// 00
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
		public byte[] FF;								// 02..15
		public byte read_period;						// 16
		public _bit_field settings_1;					// 17
		public _bit_field settings_2;					// 18
		public _bit_field display_1;					// 19
		public _bit_field display_2;					// 20
		public _bit_field alarm_1;						// 21
		public _bit_field alarm_2;						// 22
		public _bit_field alarm_3;						// 23
		public _signed_byte timezone;					// 24
		public byte unknown_01;							// 25
		public byte data_changed;						// 26
		public _unsigned_short data_count;				// 27
		public _bit_field display_3;					// 29
		public _unsigned_short current_pos;				// 30
		public _unsigned_short rel_pressure;			// 32
		public _unsigned_short abs_pressure;			// 34
		public _unsigned_short lux_wm2_coeff;			// 36
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public byte[] unknown1;							// 38
		public _date_time date_time;					// 43

		// Alarm block
		public byte alarm_hum_in_hi;					// 48
		public byte alarm_hum_in_low;					// 49
		public _signed_short alarm_temp_in_hi;			// 50
		public _signed_short alarm_temp_in_low;			// 52
		public byte alarm_hum_out_hi;					// 54
		public byte alarm_hum_out_low;					// 55
		public _signed_short alarm_temp_out_hi;			// 56
		public _signed_short alarm_temp_out_lo;			// 58
		public _signed_short alarm_windchill_hi;		// 60
		public _signed_short alarm_windchill_lo;		// 62
		public _signed_short alarm_dewpoint_hi;			// 64
		public _signed_short alarm_dewpoint_lo;			// 66
		public _unsigned_short alarm_abs_pressure_hi;	// 68
		public _unsigned_short alarm_abs_pressure_lo;	// 70
		public _unsigned_short alarm_rel_pressure_hi;	// 72
		public _unsigned_short alarm_rel_pressure_lo;	// 74
		public byte alarm_wind_ave_bft;					// 76
		public byte alarm_wind_ave_ms;					// 77
		public byte alarm_wind_gust_bft;				// 79
		public byte alarm_wind_gust_ms;					// 80
		public byte alarm_wind_dir;						// 82
		public _unsigned_short alarm_rain_hour;			// 83 
		public _unsigned_short alarm_rain_day;			// 85
		public _time alarm_time;						// 87
		public _unsigned_int3 alarm_illuminance;		// 89
		public byte alarm_uv;							// 92

		// Max block
		public byte max_uv_val;							// 93
		public _unsigned_int3 max_illuminance_val;		// 94
		public byte unknown_18;							// 97
		public byte max_hum_val;						// 98
		public byte min_hum_in_val;						// 99
		public byte max_hum_out_val;					// 100
		public byte min_hum_out_val;					// 101
		public _signed_short max_temp_in_val;			// 102
		public _signed_short min_temp_in_val;			// 104
		public _signed_short max_temp_out_val;			// 106
		public _signed_short min_temp_out_val;			// 108
		public _signed_short max_windchill_val;			// 110
		public _signed_short min_windchill_val;			// 112
		public _signed_short max_dewpoint_val;			// 114
		public _signed_short min_dewpoint_val;			// 116
		public _unsigned_short max_abs_pressure_val;	// 118
		public _unsigned_short min_abs_pressure_val;	// 120
		public _unsigned_short max_rel_pressure_val;	// 122
		public _unsigned_short min_rel_pressure_val;	// 124
		public _unsigned_short max_wind_ave_val;		// 126
		public _unsigned_short max_wind_gust_val;		// 128

		// Rain

		public _unsigned_short max_rain_hour_val;		// 130
		public _unsigned_short max_rain_day_val;		// 132
		public _unsigned_short max_rain_week_val;		// 134
		public _unsigned_short max_rain_month_val;		// 136
		public _unsigned_short max_rain_total_val;		// 138

		public byte TODO;								// 140

		public _date_time max_hum_date;					// 141
		public _date_time min_hum_in_date;				// 146
		public _date_time max_hum_out_date;				// 151
		public _date_time min_hum_out_date;				// 156
		public _date_time max_temp_in_date;				// 161
		public _date_time min_temp_in_date;				// 166
		public _date_time max_temp_out_date;			// 171 
		public _date_time min_temp_out_date;			// 176 
		public _date_time max_windchill_date;			// 181
		public _date_time min_windchill_date;			// 186
		public _date_time max_dewpoint_date;			// 191
		public _date_time min_dewpoint_date;			// 196
		public _date_time max_abs_pressure_date;		// 201
		public _date_time min_abs_pressure_date;		// 206
		public _date_time max_rel_pressure_date;		// 211
		public _date_time min_rel_pressure_date;		// 216
		public _date_time max_wind_ave_date;			// 221
		public _date_time max_wind_gust_date;			// 226
		public _date_time max_rain_hour_date;			// 231
		public _date_time max_rain_day_date;			// 236
		public _date_time max_rain_week_date;			// 241
		public _date_time max_rain_month_date;			// 246
		public _date_time max_rain_total_date;			// 251

		public _unsigned_short LastOne;					// 254 en 255
	}

	public class Helper
	{
		public static T BytesToStruct<T>(byte[] rawData) where T : struct
		{
			T result = default(T);
			GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
			try
			{
				result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
			}
			finally
			{
				handle.Free();
			}
			return result;
		}
	}

}

