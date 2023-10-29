using System;
using UnityEngine;

namespace ZapiHan.Extensions
{
	public static class Vector3Ext
	{
		public static System.Numerics.Vector3 ToSystem(this Vector3 p)
		{
			return new System.Numerics.Vector3(p.x, p.y, p.z);
		}

		public static Vector3 ToUnity(this System.Numerics.Vector3 p)
		{
			return new Vector3(p.X, p.Y, p.Z);
		}

		//

		public static bool IsValid(this Vector3 v)
		{
			if (float.IsNaN(v.x) || float.IsInfinity(v.x))
				return false;

			if (float.IsNaN(v.y) || float.IsInfinity(v.y))
				return false;

			if (float.IsNaN(v.z) || float.IsInfinity(v.z))
				return false;

			return true;
		}

		public static Vector3 ToXZ(this Vector3 p)
		{
			return new Vector3(p.x, 0, p.z);
		}

		public static float SqrMagnitude(this Vector3 v)
		{
			return v.x * v.x + v.y * v.y + v.z * v.z;
		}

		public static float SqrMagnitudeXZ(this Vector3 v)
		{
			return v.x * v.x + v.z * v.z;
		}

		public static float MagnitudeXZ(this Vector3 v)
		{
			return Mathf.Sqrt(v.x * v.x + v.z * v.z);
		}

		public static float SqrDistanceXZ(Vector3 v0, Vector3 v1)
		{
			float dx = v1.x - v0.x;
			float dz = v1.z - v0.z;

			return dx * dx + dz * dz;
		}

		public static float DistanceXZ(Vector3 v0, Vector3 v1)
		{
			float dx = v1.x - v0.x;
			float dz = v1.z - v0.z;

			return Mathf.Sqrt(dx * dx + dz * dz);
		}

		//

		public static Vector3 ToNormalized(this Vector3 v, out float m)
		{
			m = v.magnitude;

			if (m == 0)
				throw new Exception("Vector3.Normalize error 0 vector magnitude");

			v /= m;

			return v;
		}

		public static Vector3 ToNormalizedXZ(this Vector3 v, out float m)
		{
			v.y = 0;

			m = v.MagnitudeXZ();

			if (m == 0)
				throw new Exception("Vector3.Normalize error 0 vector magnitude");

			v.x /= m;
			v.z /= m;

			return v;
		}

		public static Vector3 ToNormalizedXZ(this Vector3 v)
		{
			v.y = 0;

			float m = v.MagnitudeXZ();

			if (m == 0)
				throw new Exception("Vector3.Normalize error 0 vector magnitude");

			v.x /= m;
			v.z /= m;
			
			return v;
		}

		//

		public static bool IsAlmostEqual(this Vector3 v0, in Vector3 v1, float epsilon = 0.001f)
		{
			return SqrMagnitude(v1 - v0) < epsilon;
		}

		public static bool IsAlmostEqualXZ(this Vector3 v0, in Vector3 v1, float epsilon = 0.001f)
		{
			return SqrMagnitudeXZ(v1 - v0) < 0.001;
		}



		/// <summary>
		/// returns null if intersection is not possible
		/// directions should be normalized
		/// </summary>
		/// <returns></returns>
		public static Vector3? FindPlaneAndRayIntersection(Vector3 planePoint, Vector3 planeNormal, Vector3 linePoint, Vector3 lineDirection)
		{
			if (Vector3.Dot(planeNormal, lineDirection) == 0)
				return null;

			float t = (Vector3.Dot(planeNormal, planePoint) - Vector3.Dot(planeNormal, linePoint)) / Vector3.Dot(planeNormal, lineDirection);

			return linePoint + lineDirection * t;
		}

		/// <summary>
		/// Returns intersection of plane and segment if it exist.
		/// </summary>
		public static Vector3? FindPlaneAndSegmentIntersection(Vector3 planePoint, Vector3 planeNormal, Vector3 pSegment0, Vector3 pSegment1)
		{
			float mSegment;
			Vector3 dSegment = (pSegment1 - pSegment0).ToNormalized(out mSegment);

			// TODO: check both directions

			Vector3? pIntersection = FindPlaneAndRayIntersection(planePoint, planeNormal, pSegment0, dSegment);

			if (pIntersection == null)
				return null;

			var mToIntersection = 0.0f;

			return null;
		}

		public static float NormalsAngle(in Vector3 v0, in Vector3 v1)
		{
			float dot = Mathf.Clamp(Vector3.Dot(v0, v1), -1F, 1F);

			return ((float)Math.Acos(dot)) * Mathf.Rad2Deg;
		}

		//

		public static void ClampPointToRectangle(in Vector3 p, Transform rectTransform, float sizeX, float sizeZ)
		{

		}

		/// <summary>
		/// If points is outside of ellipse, function drops it to the edge.
		/// </summary>
		/// <param name="p">Point to clamp</param>
		/// <param name="ellipseTransform">Transform on which  ellipse is placed</param>
		/// <param name="rX">Local x radius.</param>
		/// <param name="rZ">Local y radius.</param>
		public static void ClampPointToEllipse(ref Vector3 p, Transform ellipseTransform, float rX, float rZ)
		{
			var pLocal = ellipseTransform.InverseTransformPoint(p);

			//var ellipseAX = 0.5f * ellipseRect.x;
			//var ellipseBZ = 0.5f * ellipseRect.z;

			// scale ellipse and point to circle with r = 1

			pLocal.x /= rX;
			pLocal.z /= rZ;

			var y = pLocal.y;

			if (pLocal.MagnitudeXZ() <= 1)
			{
				// is point inside
				return;
			}

			// drop point to circle

			pLocal = pLocal.ToNormalizedXZ();
			pLocal.y = y;

			// scale point back

			pLocal.x *= rX;
			pLocal.z *= rZ;

			p = ellipseTransform.transform.TransformPoint(pLocal);
		}
	}
}
