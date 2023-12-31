﻿using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using ZapiHan.Extensions;

namespace AirHockey
{
	public enum EActor
	{
		Player,
		Enemy
	}

	public partial class Paddle : MonoBehaviour
	{
		[SerializeField]
		EActor actor;

		new Rigidbody rigidbody;
		new SphereCollider collider;
		new Renderer renderer;

		//

		public EActor PaddleKind => actor;
		
		//

		void OnEnable()
		{
			rigidbody = GetComponent<Rigidbody>();
			collider = GetComponentInChildren<SphereCollider>();

			//UpdateRadius();

			InitAI();
		}

		void UpdateRadius()
		{
			float radius = AirHockeyGameConfig.Default.paddleRadius;

			var c = GetComponentInChildren<SphereCollider>();
			if (c != null)
				c.radius = radius;

			var r = GetComponent<Renderer>();
			if (r != null)
				r.transform.localScale = new Vector3(2 * radius, 1, 2 * radius);
		}

		void Update()
		{
			UpdateAI();
		}

		//

		public Vector3 Position
		{
			set
			{
				rigidbody.position = value;
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
			}
		}

		public float Radius
		{
			get => collider.radius;
		}

		public Bounds FieldSideBounds
		{
			get
			{
				Bounds b;

				if (PaddleKind == EActor.Player)
					b = AirHockeyGame.Instance.levelSettings.playerArea.bounds;
				else
					b = AirHockeyGame.Instance.levelSettings.enemyArea.bounds;

				return b;
			}
		}

		public Bounds SafeBounds
		{
			get
			{
				Bounds b = FieldSideBounds;
				b.Expand(new Vector3(Radius * -2, 0, Radius * -2));
				return b;
			}
		}

		public Transform DefenseGate
		{
			get
			{
				if (PaddleKind == EActor.Player)
					return AirHockeyGame.Instance.levelSettings.playerGate;
				else
					return AirHockeyGame.Instance.levelSettings.enemyGate;
			}
		}

		public Transform OffenseGate
		{
			get
			{
				if (PaddleKind == EActor.Enemy)
					return AirHockeyGame.Instance.levelSettings.playerGate;
				else
					return AirHockeyGame.Instance.levelSettings.enemyGate;
			}
		}

		//

		Vector3 SafePoint(Vector3 p)
		{
			// bounds of safe zone, where puddle has no collisions with walls

			Bounds bounds = SafeBounds;
			bounds.Expand(Radius * -2);
			p = bounds.ClosestPoint(p);

			return p;
		}
		
	}
}
