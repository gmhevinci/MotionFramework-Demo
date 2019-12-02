
namespace Hotfix
{
	public interface IHotfixMessage : Google.Protobuf.IMessage
	{
	}

	public interface IRequest : IHotfixMessage
	{
		int RpcId { get; set; }
	}

	public interface IResponse : IHotfixMessage
	{
		int RpcId { get; set; }
		int ErrorID { get; set; }
		string ErrorInfo { get; set; }
	}
}