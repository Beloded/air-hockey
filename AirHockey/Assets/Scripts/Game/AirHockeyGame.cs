using System;
using UnityEngine;
using ZapiHan.Helpers;

namespace AirHockey
{
	public enum EGameTurn
	{
		Start,
		Player,
		Enemy
	}

	public partial class AirHockeyGame : MonoBehaviourSingleton<AirHockeyGame>
	{
		[Serializable]
		public class LevelSettings
		{
			public Collider playerArea;
			public Collider enemyArea;

			public float gateRadius;
			public Transform playerGate;
			public Transform enemyGate;

			public Transform playerPaddle;
			public Transform enemyPaddle;

			public Transform puckDefault;
			public Transform puckPlayer;
			public Transform puckEnemy;
		}

		public new Camera camera;

		public Paddle playerPaddle;
		public Paddle enemyPaddle;
		public Puck puck;

		public LevelSettings levelSettings = new LevelSettings();

		//

		bool isPaused = false;
		EGameTurn gameTurn = EGameTurn.Start;
		int playerScore = 0;
		int enemyScore = 0;
		float inputDelay;

		public int PlayerScore { get => playerScore; }
		public int EnemyScore { get => enemyScore; }
		public bool IsPaused { get => isPaused; }
		public float RoundDelay { get => inputDelay; }

		//

		protected override void OnEnable()
		{
			base.OnEnable();

			Time.timeScale = 0;

			if (InterfaceMode == EInterfaceMode.Session)
				Pause();
			else
				InterfaceMode = EInterfaceMode.MainMenu;

			Cursor.visible = false;
		}

		//

		void Update()
		{
			UpdateInput();
			UpdateTimers();
		}

		void FixedUpdate()
		{
			UpdatePhysics();
		}

		public EInterfaceMode InterfaceMode
		{
			get => AirHockeyUI.Instance.Mode;
			set { AirHockeyUI.Instance.Mode = value; }
		}

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
				InterfaceMode = EInterfaceMode.FinalScore;
				Time.timeScale = 0;
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
	}
}
