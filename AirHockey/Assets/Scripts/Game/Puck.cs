using UnityEngine;
using ZapiHan.Extensions;
using ZapiHan.Helpers;

namespace AirHockey
{
	public class Puck : MonoBehaviour
	{
		new Rigidbody rigidbody;

		public SphereCollider defaultCollider;
		public Collider idleCollider;
		//

		float nonMovementTimer = 0;

		SyncTimer wallTimer = new SyncTimer(1.0f, ESyncMode.Time);

		//

		public Vector3 Position
		{
			get => transform.position.ToXZ();
			// use it only for instant teleportation
			set
			{
				transform.position = new Vector3(value.x, transform.position.y, value.z);
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
			}
		}

		public float Radius
		{
			get => defaultCollider.radius;
		}

		private void OnEnable()
		{
			rigidbody = GetComponent<Rigidbody>();

			nonMovementTimer = 0;
			idleCollider.gameObject.SetActive(false);
		}

		private void FixedUpdate()
		{
			UpdateIdleCollider();
		}

		void UpdateIdleCollider()
		{
			if (nonMovementTimer > 0)
			{
				nonMovementTimer -= Time.fixedDeltaTime;

				if (nonMovementTimer <= 0)
				{
					nonMovementTimer = 0;
					idleCollider.gameObject.SetActive(false);
				}
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.layer != LayerMask.NameToLayer("Wall"))
				return;

			idleCollider.gameObject.SetActive(true);
		}

		private void OnCollisionExit(Collision collision)
		{
			if (collision.gameObject.layer != LayerMask.NameToLayer("Wall"))
				return;

			nonMovementTimer = 0.25f;
		}
	}
}
