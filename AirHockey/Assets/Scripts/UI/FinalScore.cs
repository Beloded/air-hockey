using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZapiHan.Helpers;

namespace AirHockey
{
	public class FinalScore : MonoBehaviour
	{
		public Button restartGame;

		public TextMeshProUGUI victory;
		public TextMeshProUGUI loss;

		public TextMeshProUGUI playerScore;
		public TextMeshProUGUI enemyScore;

		//

		private void OnEnable()
		{
			restartGame.onClick.AddListener(RestartGame);
		}

		private void OnDisable()
		{
			restartGame.onClick.RemoveAllListeners();
		}

		private void RestartGame()
		{
			AirHockeyGame.Instance.Restart();
		}

		public void UpdateContent()
		{
			victory.gameObject.SetActive(AirHockeyGame.Instance.IsPlayerVictory);

			playerScore.text = AirHockeyGame.Instance.PlayerScore.ToString();
			enemyScore.text = AirHockeyGame.Instance.EnemyScore.ToString();
		}
		
	}
}
