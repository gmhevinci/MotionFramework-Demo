using MotionFramework.Network;

namespace Hotfix
{
	[NetworkMessage(ProtoMessageID.C2R_Login)]
	public partial class C2R_Login : IRequest { }

	[NetworkMessage(ProtoMessageID.R2C_Login)]
	public partial class R2C_Login : IResponse { }
}

namespace Hotfix
{
	public static class ProtoMessageID
	{
		public const ushort C2R_Login = 10001;
		public const ushort R2C_Login = 10002;
	}
}