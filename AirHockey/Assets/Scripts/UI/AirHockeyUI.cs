using UnityEngine;
using ZapiHan.Helpers;

namespace AirHockey
{
	public enum EInterfaceMode
	{
		None,
		MainMenu,
		Session,
		FinalScore,
	}

	public class AirHockeyUI : MonoBehaviourSingleton<AirHockeyUI>
	{
		[SerializeField]
		MainMenu mainMenu;

		[SerializeField]
		InGameOverlay inGameOverlay;

		[SerializeField]
		FinalScore finalScore;

		//

		public MainMenu MainMenu => mainMenu;
		public InGameOverlay InGameOverlay => inGameOverlay;
		public FinalScore FinalScore => finalScore;

		EInterfaceMode mode = EInterfaceMode.None;

		//

		public EInterfaceMode Mode 
		{ 
			get => mode;
			set 
			{
				if (mode == value)
					return;

				mode = value;

				if (mode == EInterfaceMode.MainMenu)
					mainMenu.UpdateContent();
				mainMenu.gameObject.SetActive(mode == EInterfaceMode.MainMenu);

				inGameOverlay.gameObject.SetActive(mode == EInterfaceMode.Session);

				if (mode == EInterfaceMode.FinalScore)
					finalScore.UpdateContent();
				finalScore.gameObject.SetActive(mode == EInterfaceMode.FinalScore);
			}
		}

		//
	}
}
