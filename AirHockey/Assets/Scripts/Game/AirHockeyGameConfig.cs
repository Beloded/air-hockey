using UnityEngine;

namespace AirHockey
{
	[CreateAssetMenu(fileName = "Air Hockey Config", menuName = "Air Hockey/Config")]
	public class AirHockeyGameConfig : ScriptableObject
	{
		[Header("Physics")]
		public AnimationCurve velocityToDistance;
		public float dynamicForce = 30;

		[Space]

		public int prepareTimer = 3;
		public int maxScore = 5;

		[Space]
		public float paddleRadius = 5;
		public float puckRadius = 5;


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
