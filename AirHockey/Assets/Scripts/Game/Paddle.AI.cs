using UnityEngine;
using ZapiHan.Extensions;
using ZapiHan.Helpers;

namespace AirHockey
{
	public enum EPaddleAIState
	{
		None,
		Defense,
		Between,
		Behind,
		Hit,
	}

	public partial class Paddle
	{
		struct AIData
		{
			public float angle;
			public SyncTimer timer;

			public float mPaddleToGate;
			public float mPuckToGate;
			public Vector3 dPuckToGate;
			public Vector3 dPaddleToGate;

			public Vector3 pEnemyGate;
			public Vector3 pMineGate;
			public Vector3 pPuck;
			public Vector3 pPaddle;

			public EPaddleAIState state;
		}
		
		AIData ai;
		
		//

		bool IsAIEnabled()
		{
			var game = AirHockeyGame.Instance;

			if (game.InterfaceMode != EInterfaceMode.Session)
				return false;

			if (game.RoundDelay > 0)
				return false;

			return PaddleKind == EPaddleActor.Enemy;
		}

		void InitAI()
		{
			ai.timer = new SyncTimer(0.03f);
		}

		void UpdateAI()
		{
			if (!IsAIEnabled())
				return;

			if (!ai.timer.TimeToUpdate())
				return;

			// collect geometry data
			UpdateAIData();

			if (DefendGate())
				ai.state = EPaddleAIState.Defense;
			else if (TakePointBetweenPuckAndGate())
				ai.state = EPaddleAIState.Between;
			else if (TakePointBehindPuck())
				ai.state = EPaddleAIState.Behind;
			else if (HitPuck())
				ai.state = EPaddleAIState.Hit;
			else
				ai.state = EPaddleAIState.None;
		}

		//

		void UpdateAIData()
		{
			ai.pEnemyGate = OffenseGate.position.ToXZ();
			ai.pMineGate = DefenseGate.position.ToXZ();

			ai.pPuck = AirHockeyGame.Instance.puck.Position;
			ai.pPaddle = transform.position.ToXZ();

			ai.dPuckToGate = ai.pEnemyGate - ai.pPuck;
			ai.dPuckToGate = ai.dPuckToGate.ToNormalizedXZ(out ai.mPuckToGate);

			ai.dPaddleToGate = ai.pEnemyGate - ai.pPaddle;
			ai.dPaddleToGate = ai.dPaddleToGate.ToNormalizedXZ(out ai.mPaddleToGate);

			ai.angle = Vector3.Angle(ai.dPuckToGate, ai.dPaddleToGate);
		}

		// state machine

		bool DefendGate()
		{
			var puck = AirHockeyGame.Instance.puck;
			bool isOffense = FieldSideBounds.Contains(puck.Position);

			if (isOffense)
				return false;

			var pMyGate = DefenseGate.position;
			var pPuddle = AirHockeyGame.Instance.puck.transform.position;

			var toPuddle = (pPuddle - pMyGate).normalized;

			var pProtectPoint = pMyGate + toPuddle * 15;

			TargetPoint = pProtectPoint;

			return true;
		}

		bool TakePointBetweenPuckAndGate()
		{
			bool isDirectLineToGate = ai.angle < 5;
			bool isPaddleInFrontOfPuck = ai.mPaddleToGate < ai.mPuckToGate;

			bool condition = (isPaddleInFrontOfPuck && !isDirectLineToGate);

			if (!condition)
				return false;

			var puck = AirHockeyGame.Instance.puck;

			TargetPoint = 0.5f * (ai.pMineGate + ai.pPuck) + ai.dPuckToGate * 0.5f * puck.Radius;

			return true;
		}

		bool TakePointBehindPuck()
		{
			bool isDirectLineToGate = ai.angle < 5;

			if (!isDirectLineToGate)
			{
				TargetPoint = ai.pPuck - ai.dPuckToGate * 20;
				return true;
			}

			return false;
		}

		bool HitPuck()
		{
			var puck = AirHockeyGame.Instance.puck;

			TargetPoint = ai.pPuck - ai.dPaddleToGate * 0.1f * puck.Radius;

			return true;
		}

		//

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (TargetPoint != null)
			{
				Gizmos.color = new Color(1, 1, 1, 0.5f);
				Gizmos.DrawLine(transform.position, TargetPoint.Value);
			}
		}
#endif
	}
}
