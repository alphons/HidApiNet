using System.ServiceModel;

namespace Weather
{
	[ServiceContract]
	interface IContract
	{
		[OperationContract]
		string Info();

		[OperationContract]
		int ReadBlock(ushort vid, ushort pid, ushort adres, ref byte[] buffer);

		[OperationContract]
		bool WriteByte(ushort vid, ushort pid, ushort adres, byte b);
	}
}
