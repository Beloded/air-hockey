using UnityEngine;
using UnityEngine.Serialization;

namespace AirHockey
{
	[CreateAssetMenu(fileName = "Air Hockey Config", menuName = "Air Hockey/Config")]
	public class AirHockeyGameConfig : ScriptableObject
	{
		[FormerlySerializedAs("velocityToDistance")]
		public AnimationCurve playerVelocityToDistance;
		public AnimationCurve enemyVelocityToDistance;
		public float dynamicForce = 30;
		public EInputMode inputMode = EInputMode.TargetPoint;

		[Space]

		public int prepareTimer = 3;
		public int maxScore = 5;

		[HideInInspector]
		public float paddleRadius = 5;
		[HideInInspector]
		public float puckRadius = 5;

		[Space]

		public float wallJumpTimer = 3;

		//

		static AirHockeyGameConfig instance;
		public static AirHockeyGameConfig Default
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load<AirHockeyGameConfig>("Config");
				}

				return instance;
			}
		}
	}
}
