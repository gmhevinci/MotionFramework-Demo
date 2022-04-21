using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public class CanvasWindowAdapter : CrossBindingAdaptor
{
	public override Type BaseCLRType
	{
		get
		{
			return typeof(global::CanvasWindow);
		}
	}
	public override Type AdaptorType
	{
		get
		{
			return typeof(Adapter);
		}
	}
	public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
	{
		return new Adapter(appdomain, instance);
	}

	public class Adapter : global::CanvasWindow, CrossBindingAdaptorType
	{
		public ILTypeInstance ILInstance { get; }
		private readonly AppDomain _appDomain;
		private readonly AdaptMethod mOnCreate;
		private readonly AdaptMethod mOnDestroy;
		private readonly AdaptMethod mOnRefresh;
		private readonly AdaptMethod mOnUpdate;
		private readonly AdaptMethod mOnSortDepth;
		private readonly AdaptMethod mOnSetVisible;

		public Adapter(AppDomain appdomain, ILTypeInstance instance)
		{
			_appDomain = appdomain;
			ILInstance = instance;

			mOnCreate = new AdaptMethod(_appDomain, ILInstance, "OnCreate", 0, false);
			mOnDestroy = new AdaptMethod(_appDomain, ILInstance, "OnDestroy", 0, false);
			mOnRefresh = new AdaptMethod(_appDomain, ILInstance, "OnRefresh", 0, false);
			mOnUpdate = new AdaptMethod(_appDomain, ILInstance, "OnUpdate", 0, false);
			mOnSortDepth = new AdaptMethod(_appDomain, ILInstance, "OnSortDepth", 1, true);
			mOnSetVisible = new AdaptMethod(_appDomain, ILInstance, "OnSetVisible", 1, true);
		}

		public override void OnCreate()
		{
			mOnCreate.Invoke();
		}
		public override void OnDestroy()
		{
			mOnDestroy.Invoke();
		}
		public override void OnRefresh()
		{
			mOnRefresh.Invoke();
		}
		public override void OnUpdate()
		{
			mOnUpdate.Invoke();
		}
		protected override void OnSortDepth(int depth)
		{
			mOnSortDepth.Invoke(depth);
			if (mOnSortDepth.ShouldInvokeBase())
				base.OnSortDepth(depth);
		}
		protected override void OnSetVisible(bool visible)
		{
			mOnSetVisible.Invoke(visible);
			if (mOnSetVisible.ShouldInvokeBase())
				base.OnSetVisible(visible);
		}

		public override string ToString()
		{
			IMethod m = _appDomain.ObjectType.GetMethod("ToString", 0);
			m = ILInstance.Type.GetVirtualMethod(m);
			if (m == null || m is ILMethod)
			{
				return ILInstance.ToString();
			}
			else
				return ILInstance.Type.FullName;
		}
	}
}