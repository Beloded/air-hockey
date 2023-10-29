using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ZapiHan.Extensions;
using ZapiHan.Helpers;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.GraphicsBuffer;

namespace AirHockey
{
	public partial class Paddle
	{
		void FixedUpdate()
		{
			//if (paddleKind == EPaddleKind.Player)
			//	UpdateDynamicPhysics();
			//else
			UpdateTargetPhysics();
		}

		public void ResetDynamicPhysics()
		{
			dynamicForce = Vector3.zero;
			rigidbody.velocity = Vector3.zero;
		}

		void UpdateDynamicPhysics()
		{
			//rigidbody.velocity += dynamicForce;
			if (dynamicForce == Vector3.zero)
				return;

			Vector3 velocity = rigidbody.velocity;
			Vector3 newVelocity = velocity + dynamicForce;

			Vector3 pCurrent = rigidbody.position.ToXZ();
			Vector3 newPoint = (pCurrent + newVelocity * Time.fixedDeltaTime).ToXZ();

			var bounds = SafeBounds;
			if (!bounds.Contains(newPoint))
			{
				// recalculate speed for new point inside game field

				Vector3 pBounds = bounds.ClosestPoint(newPoint);
				rigidbody.position = pBounds;
			}
			else
			{
				rigidbody.AddForce(dynamicForce, ForceMode.VelocityChange);
			}
		}

		void UpdateTargetPhysics()
		{
			if (TargetPoint == null || IsTargetHold)
			{
				rigidbody.velocity = Vector3.zero;
				return;
			}

			Vector3 pCurrent = rigidbody.position.ToXZ();
			Vector3 pTarget = TargetPoint.Value.ToXZ();

			if (pTarget == pCurrent)
				return;

			float mDelta;
			Vector3 pDelta = (pTarget - pCurrent).ToNormalizedXZ(out mDelta);
			float vTarget = AirHockeyGameConfig.Default.velocityToDistance.Evaluate(mDelta);

			// clamp new point into area bounds

			Vector3 newVelocity = pDelta * vTarget;
			Vector3 newPoint = (pCurrent + newVelocity * Time.fixedDeltaTime).ToXZ();

			var bounds = SafeBounds;
			if (!bounds.Contains(newPoint))
			{
				// recalculate speed for new point inside game field

				pTarget = bounds.ClosestPoint(pTarget);

				pDelta = (pTarget - pCurrent).ToNormalizedXZ(out mDelta);
				vTarget = AirHockeyGameConfig.Default.velocityToDistance.Evaluate(mDelta);
				newVelocity = pDelta * vTarget;
			}

			// destination is reached

			if (mDelta < 0.1f)
			{
				rigidbody.velocity = Vector3.zero;
				return;
			}

			float mToNextFrame = vTarget * Time.fixedDeltaTime;
			if (mToNextFrame >= mDelta)
			{
				// fix last velocity so puddle will move right to the target point on next frame
				vTarget = mDelta / Time.fixedDeltaTime;
			}

			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce(newVelocity, ForceMode.VelocityChange);

			//Debug.Log(mDelta + " " + vTarget);
		}

	}
}
