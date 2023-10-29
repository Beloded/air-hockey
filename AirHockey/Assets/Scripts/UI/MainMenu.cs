using System;
using UnityEngine;
using UnityEngine.UI;
using ZapiHan.Helpers;

namespace AirHockey
{
	public class MainMenu : MonoBehaviour
	{
		public Button startGame;
		public Button continueGame;
		public Button exitGame;

		//

		private void OnEnable()
		{
			startGame.onClick.AddListener(StartGame);
			continueGame.onClick.AddListener(ContinueGame);
			exitGame.onClick.AddListener(ExitGame);
		}

		private void OnDisable()
		{
			startGame.onClick.RemoveAllListeners();
			continueGame.onClick.RemoveAllListeners();
			exitGame.onClick.RemoveAllListeners();
		}

		public void UpdateContent()
		{
			continueGame.gameObject.SetActive(AirHockeyGame.Instance.IsPaused);
		}

		//

		private void StartGame()
		{
			AirHockeyGame.Instance.Restart();
		}

		private void ContinueGame()
		{
			AirHockeyGame.Instance.Continue();
		}

		private void ExitGame()
		{
			UnityEngine.Application.Quit();
		}

	}
}
