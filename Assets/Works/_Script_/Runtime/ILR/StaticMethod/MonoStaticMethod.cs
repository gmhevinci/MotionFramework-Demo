using System;
using System.Reflection;

namespace MotionGame
{
	public class MonoStaticMethod : IStaticMethod
	{
		private readonly MethodInfo _methodInfo;
		private readonly object[] _params;

		public MonoStaticMethod(Type type, string methodName)
		{
			_methodInfo = type.GetMethod(methodName);
			_params = new object[_methodInfo.GetParameters().Length];
		}

		public object Invoke()
		{
			if (_params.Length != 0)
				throw new ArgumentOutOfRangeException();

			return _methodInfo.Invoke(null, null);
		}
		public object Invoke(object arg0)
		{
			if (_params.Length != 1)
				throw new ArgumentOutOfRangeException();

			_params[0] = arg0;
			return _methodInfo.Invoke(null, _params);
		}
		public object Invoke(object arg0, object arg1)
		{
			if (_params.Length != 2)
				throw new ArgumentOutOfRangeException();

			_params[0] = arg0;
			_params[1] = arg1;
			return _methodInfo.Invoke(null, _params);
		}
		public object Invoke(object arg0, object arg1, object arg2)
		{
			if (_params.Length != 3)
				throw new ArgumentOutOfRangeException();

			_params[0] = arg0;
			_params[1] = arg1;
			_params[2] = arg2;
			return _methodInfo.Invoke(null, _params);
		}
		public object Invoke(params object[] args)
		{
			if (_params.Length != args.Length)
				throw new ArgumentOutOfRangeException();

			return _methodInfo.Invoke(null, args);
		}
	}
}