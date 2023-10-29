using TMPro;
using UnityEngine;
using ZapiHan.Helpers;

namespace AirHockey
{
	public class InGameOverlay : MonoBehaviour
	{
		public TextMeshProUGUI playerScore;
		public TextMeshProUGUI enemyScore;
		public TextMeshProUGUI timer;

		//

		public void UpdateScore()
		{
			var game = AirHockeyGame.Instance;

			playerScore.text = game.PlayerScore.ToString();
			enemyScore.text = game.EnemyScore.ToString();
		}

		void UpdateTimer()
		{
			var delay = AirHockeyGame.Instance.RoundDelay;

			timer.gameObject.SetActive(delay > 0);
			if (delay > 0)
			{
				int seconds = Mathf.FloorToInt(delay);
				int ms = (int)((delay - (float)seconds) / 0.01f);

				timer.text = string.Format("{0}:{1}", seconds, ms);
			}
			
		}

		private void Update()
		{
			UpdateTimer();
		}
	}
}
