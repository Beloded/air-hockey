using UnityEngine;
using ZapiHan.Extensions;
using ZapiHan.Helpers;

namespace AirHockey
{
	public partial class Paddle
	{	
		enum AIState
		{
			Defense,
			Between,
			Behind,
			Hit
		}

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
		}

		AIData ai;
		

		bool IsAIEnabled()
		{
			var game = AirHockeyGame.Instance;

			if (game.InterfaceMode != EInterfaceMode.Session)
				return false;

			if (game.RoundDelay > 0)
				return false;

			return PaddleKind == EPaddleKind.Enemy;
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

			var puck = AirHockeyGame.Instance.puck;
			bool isOffense = FieldSideBounds.Contains(puck.Position);

			if (isOffense)
			{
				UpdateOffenseTactics();
			}
			else
			{
				UpdateDefenseTactics();
			}
		}

		void UpdateDefenseTactics()
		{
			var pMyGate = DefenseGate.position;
			var pPuddle = AirHockeyGame.Instance.puck.transform.position;
			
			var toPuddle = (pPuddle - pMyGate).normalized;

			var pProtectPoint = pMyGate + toPuddle * 15;

			TargetPoint = pProtectPoint;
		}

		//

		void UpdateOffenseTactics()
		{
			// collect geometry data

			var puck = AirHockeyGame.Instance.puck;

			ai.pEnemyGate = OffenseGate.position.ToXZ();
			ai.pMineGate = DefenseGate.position.ToXZ();

			ai.pPuck = AirHockeyGame.Instance.puck.Position;
			ai.pPaddle = transform.position.ToXZ();

			ai.dPuckToGate = ai.pEnemyGate - ai.pPuck;
			ai.dPuckToGate = ai.dPuckToGate.ToNormalizedXZ(out ai.mPuckToGate);

			ai.dPaddleToGate = ai.pEnemyGate - ai.pPaddle;			
			ai.dPaddleToGate = ai.dPaddleToGate.ToNormalizedXZ(out ai.mPaddleToGate);

			ai.angle = Vector3.Angle(ai.dPuckToGate, ai.dPaddleToGate);

			bool isDirectLineToGate = ai.angle < 5;
			bool isPaddleInFrontOfPuck = ai.mPaddleToGate < ai.mPuckToGate;

			// ai state machine

			if (TakePointBetweenPuckAndGate()) { }
			else if (TakePointBehindPuck()) { }
			else 
			{
				HitPuck();
			}
		}

		// state machine

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
