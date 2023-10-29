using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using ZapiHan.Extensions;

namespace AirHockey
{
	public partial class Paddle
	{
		bool IsTargetHold { get; set; } = false;

		Vector3 dynamicForce = Vector3.zero;
		Vector3? targetPoint;

		//

		/// <summary>
		/// Physics will try to reach the target point
		/// </summary>
		public Vector3? TargetPoint
		{
			get => targetPoint;
			set
			{
				if (value != null)
					targetPoint = value.Value;
				else
					targetPoint = null;
			}
		}

		public void UpdateDynamicInput()
		{
			if (AirHockeyGame.Instance.InterfaceMode != EInterfaceMode.Session)
				return;

			float f = AirHockeyGameConfig.Default.dynamicForce;
			float vX = Input.GetAxis("Mouse X") * f;
			float vZ = Input.GetAxis("Mouse Y") * f;

			dynamicForce = new Vector3 (vX, 0, vZ);
		}

		public void UpdateTargetPointInput()
		{
			Vector3? mousePoint = null;
			Vector3 point = Input.mousePosition;
			var ray = Camera.main.ScreenPointToRay(point);

			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, float.MaxValue, 1 << LayerMask.NameToLayer("Floor")))
			{
				mousePoint = hit.point;
			}

			TargetPoint = mousePoint;
			IsTargetHold = Input.GetMouseButton(0);
		}
	}
}
