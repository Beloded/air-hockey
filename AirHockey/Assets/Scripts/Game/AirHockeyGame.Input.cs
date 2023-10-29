using UnityEngine;

namespace AirHockey
{
	public enum EInputMode
	{
		TargetPoint,
		Dynamic,
	}

	public partial class AirHockeyGame
	{
		public EInputMode InputMode => AirHockeyGameConfig.Default.inputMode;

		//

		void UpdateInput()
		{
			UpdateMouseInput();
			UpdateKeyInput();
			UpdateInputDelay();
		}

		void UpdateInputDelay()
		{
			if (inputDelay > 0)
			{
				inputDelay = Mathf.Max(0, inputDelay - Time.deltaTime);
			}
		}

		void UpdateMouseInput()
		{
			if (InterfaceMode != EInterfaceMode.Session || inputDelay > 0)
			{
				if (InputMode == EInputMode.Dynamic)
					playerPaddle.ResetDynamicPhysics();

				return;
			}

			if (InputMode == EInputMode.TargetPoint)
				playerPaddle.UpdateTargetPointInput();
			else
				playerPaddle.UpdateDynamicInput();
		}

		void UpdateKeyInput()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (InterfaceMode == EInterfaceMode.Session)
					Pause();
				else if (IsPaused && InterfaceMode == EInterfaceMode.MainMenu)
					Continue();
			}
		}
	}
}
