
namespace Hotfix
{
	public interface IHotfixNetMessage : Google.Protobuf.IMessage
	{
	}

	public interface IRequest : IHotfixNetMessage
	{
		int RpcId { get; set; }
	}

	public interface IResponse : IHotfixNetMessage
	{
		int RpcId { get; set; }
		int ErrorID { get; set; }
		string ErrorInfo { get; set; }
	}
}