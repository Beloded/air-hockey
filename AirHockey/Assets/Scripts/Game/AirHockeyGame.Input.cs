using UnityEngine;

namespace AirHockey
{
	public enum EUInputMode
	{
		TargetPoint,
		Dynamic,
	}

	public partial class AirHockeyGame
	{
		public EUInputMode inputMode = EUInputMode.Dynamic;
		const string kFloorLayer = "Floor";

		//

		void UpdateInput()
		{
			UpdateMouseInput();
			UpdateKeyInput();
		}

		bool IsInputEnabled()
		{
			return false;
		}

		void UpdateMouseInput()
		{
			if (InterfaceMode != EInterfaceMode.Session || inputDelay > 0)
			{
				//playerPaddle.ResetDynamicPhysics();
				return;
			}

			//playerPaddle.UpdateDynamicInput();

			//if (inputMode == EUInputMode.TargetPoint)
			playerPaddle.UpdateTargetPointInput();
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
