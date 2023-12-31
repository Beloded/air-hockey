﻿using UnityEngine;
using ZapiHan.Extensions;
using ZapiHan.Helpers;

namespace AirHockey
{
	public partial class AirHockeyGame
	{
		SyncTimer goalCheckTimer = new SyncTimer(0.1f, ESyncMode.Fixed);

		//

		void CheckGoal()
		{
			// check puck position on the game field

			Bounds playerBounds = levelSettings.playerArea.bounds;
			playerBounds.Expand(0.1f);

			Bounds enemyBounds = levelSettings.enemyArea.bounds;
			enemyBounds.Expand(0.1f);

			if (playerBounds.Contains(puck.Position) || enemyBounds.Contains(puck.Position))
				// puck is still inside game field
				return;

			Vector3 playerGate = levelSettings.playerGate.transform.position.ToXZ();
			Vector3 enemyGate = levelSettings.enemyGate.transform.position.ToXZ();

			var toPlayer = playerGate - puck.Position;
			var toEnemy = enemyGate - puck.Position;

			//

			bool playerWins = (toPlayer.SqrMagnitudeXZ() > toEnemy.SqrMagnitudeXZ());

			EndRound(playerWins);
		}

		void UpdatePhysics()
		{
			if (InterfaceMode != EInterfaceMode.Session)
				return;

			if (!goalCheckTimer.TimeToUpdate())
				return;

			CheckGoal();
		}
	}
}
