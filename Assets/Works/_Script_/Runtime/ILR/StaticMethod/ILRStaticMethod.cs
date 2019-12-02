using System;
using ILRuntime.CLR.Method;

namespace MotionGame
{
	public class ILRStaticMethod : IStaticMethod
	{
		private readonly ILRuntime.Runtime.Enviorment.AppDomain _appDomain;
		private readonly IMethod _method;
		private readonly object[] _params;

		public ILRStaticMethod(ILRuntime.Runtime.Enviorment.AppDomain appDomain, string typeName, string methodName, int paramsCount)
		{
			_appDomain = appDomain;
			_method = appDomain.GetType(typeName).GetMethod(methodName, paramsCount);
			_params = new object[paramsCount];
		}

		public object Invoke()
		{
			if (_params.Length != 0)
				throw new ArgumentOutOfRangeException();

			return _appDomain.Invoke(_method, null, null);
		}
		public object Invoke(object arg0)
		{
			if (_params.Length != 1)
				throw new ArgumentOutOfRangeException();

			_params[0] = arg0;
			return _appDomain.Invoke(_method, null, _params);
		}
		public object Invoke(object arg0, object arg1)
		{
			if (_params.Length != 2)
				throw new ArgumentOutOfRangeException();

			_params[0] = arg0;
			_params[1] = arg1;
			return _appDomain.Invoke(_method, null, _params);
		}
		public object Invoke(object arg0, object arg1, object arg2)
		{
			if (_params.Length != 3)
				throw new ArgumentOutOfRangeException();

			_params[0] = arg0;
			_params[1] = arg1;
			_params[2] = arg2;
			return _appDomain.Invoke(_method, null, _params);
		}
		public object Invoke(params object[] args)
		{
			if (_params.Length != args.Length)
				throw new ArgumentOutOfRangeException();

			return _appDomain.Invoke(_method, null, args);
		}
	}
}