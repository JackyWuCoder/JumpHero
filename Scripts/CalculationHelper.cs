using Godot;
using System;

namespace JumpHero
{
	public static class CalculationHelper
	{		
		// Checks for specific direction
		public static bool IsValidCollider(KinematicCollision2D collision, float directionAngle)
		{
			float collisionAngle = collision.GetNormal().Angle();
			// Uses high tolerance value to allow more varied angles rather than perpendicular only
			float equalityTolerance = Mathf.DegToRad(Player.SLOPE_ANGLE_THRESHOLD);
			return Mathf.IsEqualApprox(collisionAngle, directionAngle, equalityTolerance);
		}

		// Checks for left, top, and right without needing to specify direction
		public static bool IsValidCollider(KinematicCollision2D collision)
		{
			float collisionAngle = collision.GetNormal().Angle();
			float equalityTolerance = Mathf.DegToRad(Player.SLOPE_ANGLE_THRESHOLD);
			return 	Mathf.IsEqualApprox(collisionAngle, 0, equalityTolerance) ||
					Mathf.IsEqualApprox(collisionAngle, Mathf.Pi / 2, equalityTolerance) ||
					Mathf.IsEqualApprox(collisionAngle, Mathf.Pi, equalityTolerance);
		}
	}
}
