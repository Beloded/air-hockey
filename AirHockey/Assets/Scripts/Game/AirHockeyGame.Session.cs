using System;
using UnityEngine;
using ZapiHan.Helpers;

namespace AirHockey
{
	public partial class AirHockeyGame
	{

		public void Restart()
		{
			gameTurn = EGameTurn.Start;
			playerScore = 0;
			enemyScore = 0;

			isPaused = false;
			InterfaceMode = EInterfaceMode.Session;

			Time.timeScale = 1;

			NextRound();
		}

		public void Continue()
		{
			isPaused = false;
			InterfaceMode = EInterfaceMode.Session;

			Time.timeScale = 1;
		}

		public void Pause()
		{
			Time.timeScale = 0; // to hold physics

			isPaused = true;
			InterfaceMode = EInterfaceMode.MainMenu;
		}

		void ShowFinalScore()
		{
			InterfaceMode = EInterfaceMode.FinalScore;
			Time.timeScale = 0; // to pause physics
		}

		//

		public bool IsPlayerVictory
		{
			get => playerScore >= AirHockeyGameConfig.Default.maxScore;
		}

		void EndRound(bool isPlayerScore)
		{
			if (isPlayerScore)
			{
				playerScore += 1;
				gameTurn = EGameTurn.Enemy;
			}
			else
			{
				enemyScore += 1;
				gameTurn = EGameTurn.Player;
			}

			if (Mathf.Max(playerScore, enemyScore) >= AirHockeyGameConfig.Default.maxScore)
			{
				ShowFinalScore();
			}
			else
			{
				NextRound();
			}
		}

		void NextRound()
		{
			playerPaddle.TargetPoint = null;
			enemyPaddle.TargetPoint = null;

			if (gameTurn == EGameTurn.Start)
				puck.Position = levelSettings.puckDefault.position;
			else if (gameTurn == EGameTurn.Player)
				puck.Position = levelSettings.puckPlayer.position;
			else if (gameTurn == EGameTurn.Enemy)
				puck.Position = levelSettings.puckEnemy.position;

			playerPaddle.Position = levelSettings.playerPaddle.position;
			enemyPaddle.Position = levelSettings.enemyPaddle.position;

			AirHockeyUI.Instance.InGameOverlay.UpdateScore();

			inputDelay = AirHockeyGameConfig.Default.prepareTimer;
		}


	}
}
