using System;
using UnityEngine;

namespace ZapiHan.Helpers
{
	public enum ESyncMode
	{
		Time,
		UnscaledTime,
		Fixed
	}

	/// <summary>
	/// Timer for synchronization different subsystems on game loop update.
	/// </summary>
	public class SyncTimer
	{
		float interval;
		float timer = float.NaN;
		float lastInterval;

		Func<float> DeltaTimeFunction = null;

		//

		public SyncTimer(float interval, ESyncMode syncMode = ESyncMode.Time)
		{
			this.interval = interval;

			SyncMode = syncMode;
		}

		public ESyncMode SyncMode
		{
			set
			{
				if (value == ESyncMode.Time)
					DeltaTimeFunction = DeltaTime;
				else if (value == ESyncMode.UnscaledTime)
					DeltaTimeFunction = UnscaledDeltaTime;
				else
					DeltaTimeFunction = FixedDeltaTime;

			}
		}

		public float LastInterval { get => lastInterval; }

		bool IsFirstFire
		{
			get => float.IsNaN(timer);
		}

		public bool TimeToUpdate()
		{
			if (IsFirstFire)
			{
				lastInterval = 0;
				// randomize first delay after immediate first fire
				timer = UnityEngine.Random.Range(0, interval);

				return true;
			}

			timer += DeltaTimeFunction();

			if (timer >= interval)
			{
				lastInterval = interval;
				timer -= interval;

				return true;
			}

			return false;
		}

		float UnscaledDeltaTime()
		{
			return Time.unscaledDeltaTime;
		}

		float DeltaTime()
		{
			return Time.deltaTime;
		}

		float FixedDeltaTime()
		{
			return Time.fixedDeltaTime;
		}
	}
}
