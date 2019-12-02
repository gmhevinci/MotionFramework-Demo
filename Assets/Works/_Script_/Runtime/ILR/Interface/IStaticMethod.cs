
namespace MotionGame
{
	public interface IStaticMethod
	{
		object Invoke();
		object Invoke(object arg0);
		object Invoke(object arg0, object arg1);
		object Invoke(object arg0, object arg1, object arg2);
		object Invoke(params object[] args);
	}
}