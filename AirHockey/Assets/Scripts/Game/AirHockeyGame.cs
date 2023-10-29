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

		public EInterfaceMode InterfaceMode
		{
			get => AirHockeyUI.Instance.Mode;
			set { AirHockeyUI.Instance.Mode = value; }
		}

		//

		protected override void OnEnable()
		{
			base.OnEnable();

			Time.timeScale = 0;

			if (InterfaceMode == EInterfaceMode.Session)
				Pause();
			else
				InterfaceMode = EInterfaceMode.MainMenu;

			//Cursor.visible = false;
		}

		//

		void Update()
		{
			UpdateInput();
		}

		void FixedUpdate()
		{
			UpdatePhysics();
		}
	}
}
