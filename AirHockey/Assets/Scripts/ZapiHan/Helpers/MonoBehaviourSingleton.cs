using System;
using UnityEngine;

namespace ZapiHan.Helpers
{
	/// <summary>
	/// Singleton for single object, added to scene.
	/// Can be null on GetInstance() if it is not present on scene.
	/// It does not add itself on GetInstance(), so you can check if it exist.
	/// </summary>
	public abstract class MonoBehaviourSingleton<TChild> : MonoBehaviour where TChild : class
	{
		static TChild instanсе;

		public static TChild GetInstance() // can be null
		{
			//if (instanсе == null)
			//	throw new Exception("Instance not found: " + typeof(T));

			return instanсе;
		}

		void Init()
		{
			if (instanсе == null)
				instanсе = this as TChild;
			else if (instanсе != this as TChild)
				throw new Exception("Multiple instance objects: " + typeof(TChild));
		}

		void CleanupInstance()
		{
			if (instanсе == this as TChild)
				instanсе = null;
		}

		public static TChild Instance
		{
			get => instanсе;
		}

		protected virtual void OnEnable()
		{
			// on enable for refresh

			Init();
		}

		protected virtual void OnDisable()
		{
			CleanupInstance();
		}

		protected virtual void OnDestroy()
		{
			CleanupInstance();
		}
	}
}
