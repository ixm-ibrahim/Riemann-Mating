using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace OpenTK_Reimann_Mating
{
	public class Camera
	{
		// Maps all usable keys
		void MapKeys()
		{
			// fullscreen mode
			KeyMap.Add("fullscreen", Key.F11);
			// Display info
			KeyMap.Add("displayInfo", Key.Slash);
			// Rotation Type change
			KeyMap.Add("cameraMode", Key.B);
			// Lock camera on target
			KeyMap.Add("cameraLock", Key.N);

			// Movement
			KeyMap.Add("moveForward", Key.W);
			KeyMap.Add("moveBackward", Key.S);
			KeyMap.Add("moveUp", Key.Space);
			KeyMap.Add("moveDown", Key.LShift);
			KeyMap.Add("moveLeft", Key.A);
			KeyMap.Add("moveRight", Key.D);

			// Zooming
			KeyMap.Add("zoomIn", Key.R);
			KeyMap.Add("zoomOut", Key.F);

			// Riemann sphere projection movement
			KeyMap.Add("fractal_left", Key.J);
			KeyMap.Add("fractal_down", Key.K);
			KeyMap.Add("fractal_up", Key.I);
			KeyMap.Add("fractal_right", Key.L);
			KeyMap.Add("fractal_in", Key.O);
			KeyMap.Add("fractal_out", Key.U);

			// Mating frame change (only once they all have been calculated)
			KeyMap.Add("left", Key.Left);
			KeyMap.Add("down", Key.Down);
			KeyMap.Add("right", Key.Right);
		}
		
		public enum Type
		{
			NONE = 0, FPS, FREE
		}

		// Directional unit vectors
		public readonly Vector3 unitFront = -Vector3.UnitZ;
		public readonly Vector3 unitUp = Vector3.UnitY;
		public readonly Vector3 unitRight = Vector3.UnitX;

		Vector3 right = Vector3.UnitX;
		Vector3 up = Vector3.UnitY;
		Vector3 direction = -Vector3.UnitZ;

		public Vector3 target = Vector3.Zero;
		float nearClipping = 0.1f;
		float farClipping = 1000f;

		public Type type = Type.FPS;

		// Rotation angles
		//		Pitch: rotation about the X axis
		//		Yaw: rotation about the Y axis
		//			Needs to start at -pi/2
		float pitch;
		float yaw = -MathHelper.PiOver2;

		// The field of view of the camera (set to 45 degrees)
		//float fov = MathHelper.PiOver2;
		float fov = MathHelper.DegreesToRadians(57);
		//float fov = MathHelper.DegreesToRadians(30);

		public float moveSpeed = 2f;
		public float zoomSpeed = .1f;
		public float turnSpeed = .2f;
		public float targetDistance;

		public Vector2 lastMousePos;
		public float lastScrollPos;

		public bool fullscreen = false;
		public bool cameraLock = false;

		public Camera(Vector3 position, float aspectRatio)
		{
			Position = position;
			AspectRatio = aspectRatio;

			UpdateTargetDistance();
			MapKeys();
		}

		public override string ToString()
		{
			return "Camera:\n\tPosition: " + Position + "\n\tDirection: " + direction + "\n\tRotation target: " + target + "\n\tHorizontal Axis: " + right + "\n\tVertical Axis: " + up + "\n\tDepth Axis: " + direction + "\n";
		}

		public Vector3 Direction => direction;
		public Vector3 Up => up;
		public Vector3 Right => right;

		// The position of the camera
		public Vector3 Position { get; set; }

		// The aspect ratio of the camera
		public float AspectRatio { get; set; }

		public Dictionary<string, Key> KeyMap { get; } = new Dictionary<string, Key>();

		public float Pitch
		{
			get => MathHelper.RadiansToDegrees(pitch);

			set
			{
				// We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and
				// to avoid gimbal lock
				pitch = MathHelper.DegreesToRadians(MathHelper.Clamp(value, -89f, 89f));

				RotateVectors();
			}
		}

		public float Yaw
		{
			get => MathHelper.RadiansToDegrees(yaw);

			set
			{
				yaw = MathHelper.DegreesToRadians(value);

				RotateVectors();
			}
		}

		// Changing this can simulate a zoom
		public float FOV
		{
			get => MathHelper.RadiansToDegrees(fov);

			set => fov = MathHelper.DegreesToRadians(MathHelper.Clamp(value, 1f, 45f));
		}

		// Gets the view matrix
		public Matrix4 GetViewMatrix()
		{
			return Matrix4.LookAt(Position, direction + Position, up);
		}

		// Gets the projection matrix
		public Matrix4 GetProjectionMatrix()
		{
			return Matrix4.CreatePerspectiveFieldOfView(fov, AspectRatio, nearClipping, farClipping);
			//return Matrix4.CreatePerspectiveFieldOfView(fov, AspectRatio, 0.0001f, targetDistance * 10);
		}

		// Keyboard Controller
		void KeyboardController(float frameTime)
		{
			var keyState = Keyboard.GetState();

			// Movement
			if (keyState.IsKeyDown(KeyMap["moveForward"]))
				MoveAlongAxis(Direction, frameTime);
			if (keyState.IsKeyDown(KeyMap["moveBackward"]))
				MoveAlongAxis(-Direction, frameTime);
			if (keyState.IsKeyDown(KeyMap["moveRight"]))
				MoveAlongAxis(Right, frameTime);
			if (keyState.IsKeyDown(KeyMap["moveLeft"]))
				MoveAlongAxis(-Right, frameTime);
			if (keyState.IsKeyDown(KeyMap["zoomIn"]))
				Zoom(frameTime);
			if (keyState.IsKeyDown(KeyMap["zoomOut"]))
				Zoom(frameTime);

			if (type == Type.FPS)
			{
				if (keyState.IsKeyDown(KeyMap["moveUp"]))
					MoveAlongAxis(unitUp, frameTime);
				if (keyState.IsKeyDown(KeyMap["moveDown"]))
					MoveAlongAxis(-unitUp, frameTime);
			}
			else
			{
				if (keyState.IsKeyDown(KeyMap["moveUp"]))
					MoveAlongAxis(Up, frameTime);
				if (keyState.IsKeyDown(KeyMap["moveDown"]))
					MoveAlongAxis(-Up, frameTime);
			}
		}

		// Mouse Controller
		void MouseController()
		{
			var mousePos = new Vector2(Mouse.GetCursorState().X, Mouse.GetCursorState().Y);
			var scrollPos = Mouse.GetState().WheelPrecise;

			// Calculate the offset of the mouse position
			var delta = lastMousePos - mousePos;

			if (cameraLock)
			{
				ArcBallYaw(delta.X, type, true, true);
				ArcBallPitch(delta.Y, type, true, true);
			}
			else
			{
				RotateYaw(delta.X, type, true);
				RotatePitch(delta.Y, type, true);
			}

			Zoom(.2f * (scrollPos - lastScrollPos));

			lastMousePos = mousePos;
			lastScrollPos = scrollPos;
		}

		public void Input(float frameTime)
		{
			KeyboardController(frameTime);
			MouseController();
		}

		public float GetRadius()
		{
			return Position.Length;
		}

		public void MoveAlongAxis(Vector3 axis, bool updateTarget = true)
		{
			MoveAlongAxis(axis, 1, updateTarget);
		}
		public void MoveAlongAxis(Vector3 axis, float distance, bool updateTarget = true)
		{
			UpdateTargetDistance();
			Position += axis * distance * moveSpeed;

			if (updateTarget)
				UpdateTarget();
		}
		public static Vector3 MoveAlongAxis(Vector3 position, Vector3 axis, float distance)
		{
			return position + axis * distance;
		}

		public void Zoom(float distance = 1)
		{
			MoveAlongAxis(direction, zoomSpeed * distance * targetDistance, false);
		}

		public void ZoomAlongAxis(Vector3 axis, float distance)
		{
			MoveAlongAxis(axis, zoomSpeed * distance * targetDistance, false);
		}

		public void ZoomTarget(Vector3 target, float distance, bool updateTarget = true)
		{
			UpdateTargetDistance();
			MoveAlongAxis((target - Position).Normalized(), zoomSpeed * distance * targetDistance, false);

			if (updateTarget)
				UpdateTarget();
		}

		public void RotateX(float angle, bool updateTarget = true)
		{
			UpdateTargetDistance();
			ArcBall(unitRight, angle);

			if (updateTarget)
				UpdateTarget();
		}

		public void RotateY(float angle, bool updateTarget = true)
		{
			UpdateTargetDistance();
			ArcBall(unitUp, angle);

			if (updateTarget)
				UpdateTarget();
		}

		public void RotateZ(float angle, bool updateTarget = true)
		{
			UpdateTargetDistance();
			ArcBall(unitFront, angle);

			if (updateTarget)
				UpdateTarget();
		}

		//@OpenTK.Quaternion error has to do something with FromAxisAngle()
		public void RotateYaw(float angle, Type type, bool updateTarget = true)
		{
			UpdateTargetDistance();

			if (type == Type.FPS)
				Yaw -= angle * turnSpeed;
			else
				RotateVectors(Quaternion.FromAxisAngle(MathHelper.DegreesToRadians(angle * turnSpeed), new Vector3D(up)));
			//RotateVectors(OpenTK.Quaternion.FromAxisAngle(up, MathHelper.DegreesToRadians(angle * turnSpeed)));

			if (updateTarget)
				UpdateTarget();
		}

		public void RotatePitch(float angle, Type type, bool updateTarget = true)
		{
			UpdateTargetDistance();

			if (type == Type.FPS)
				Pitch += angle * turnSpeed;
			else
				RotateVectors(Quaternion.FromAxisAngle(MathHelper.DegreesToRadians(angle * turnSpeed), new Vector3D(right)));
			//RotateVectors(OpenTK.Quaternion.FromAxisAngle(right, MathHelper.DegreesToRadians(angle * turnSpeed)));

			if (updateTarget)
				UpdateTarget();
		}

		public void RotateRoll(float angle, bool updateTarget = true)
		{
			UpdateTargetDistance();

			RotateVectors(Quaternion.FromAxisAngle(MathHelper.DegreesToRadians(angle * turnSpeed), new Vector3D(direction)));
			//RotateVectors(OpenTK.Quaternion.FromAxisAngle(direction, MathHelper.DegreesToRadians(angle * turnSpeed)));

			if (updateTarget)
				UpdateTarget();
		}

		public static Vector3 Rotate(Vector3 position, Vector3 axis, float angle)
		{
			/*Quaternion localRotation = Quaternion.FromAxisAngle(axis, MathHelper.DegreesToRadians(angle));

			return localRotation.Inverted() * (localRotation * position);*/
			Quaternion localRotation = Quaternion.FromAxisAngle(angle * Math.PI / 360, new Vector3D(axis));

			Vector3D v = localRotation.Inverse() * (localRotation * new Vector3D(position));

			position.X = (float)v.X;
			position.Y = (float)v.Y;
			position.Z = (float)v.Z;

			return position;
		}

		public void ArcBallYaw(float angle, Type type, bool withSpeed, bool aroundOrigin = false)
		{
			Position -= (aroundOrigin ? Vector3.Zero : target);

			if (type == Type.FPS)
				if (withSpeed)
					Yaw -= angle * turnSpeed;
				else
					Yaw -= turnSpeed;
			else
				if (withSpeed)
					RotateVectors(Quaternion.FromAxisAngle(MathHelper.DegreesToRadians(turnSpeed * angle), new Vector3D(up)));
				else
					RotateVectors(Quaternion.FromAxisAngle(MathHelper.DegreesToRadians(angle), new Vector3D(up)));

			//RotateVectors(OpenTK.Quaternion.FromAxisAngle(up, MathHelper.DegreesToRadians(turnSpeed * angle)));

			Position = (direction * -GetRadius()) + (aroundOrigin ? Vector3.Zero : target);

			if (aroundOrigin)
				UpdateTarget();
		}

		public void ArcBallPitch(float angle, Type type, bool withSpeed, bool aroundOrigin = false)
		{
			Position -= (aroundOrigin ? Vector3.Zero : target);

			if (type == Type.FPS)
				if (withSpeed)
					Pitch += angle * turnSpeed;
				else
					Pitch += turnSpeed;
			else
				if (withSpeed)
					RotateVectors(Quaternion.FromAxisAngle(MathHelper.DegreesToRadians(turnSpeed * angle), new Vector3D(right)));
				else
					RotateVectors(Quaternion.FromAxisAngle(MathHelper.DegreesToRadians(angle), new Vector3D(right)));

			//RotateVectors(OpenTK.Quaternion.FromAxisAngle(right, MathHelper.DegreesToRadians(turnSpeed * angle)));

			Position = (direction * -GetRadius()) + (aroundOrigin ? Vector3.Zero : target);

			if (aroundOrigin)
				UpdateTarget();
		}

		public void ArcBall(Vector3 axis, float angle, bool aroundOrigin = true)
		{
			RotateVectors(Quaternion.FromAxisAngle(turnSpeed * MathHelper.DegreesToRadians(angle), new Vector3D(axis)));
			//RotateVectors(OpenTK.Quaternion.FromAxisAngle(axis, turnSpeed * MathHelper.DegreesToRadians(angle)));

			Position = (unitFront * -GetRadius()) + (aroundOrigin ? Vector3.Zero : target);

			if (aroundOrigin)
				UpdateTarget();
		}

		// Updates direction vertices based on yaw and pitch
		void RotateVectors()
		{
			// First the direction matrix is calculated using some basic trigonometry
			direction.X = (float)Math.Cos(pitch) * (float)Math.Cos(yaw);
			direction.Y = (float)Math.Sin(pitch);
			direction.Z = (float)Math.Cos(pitch) * (float)Math.Sin(yaw);

			// We need to make sure the vectors are all normalized, as otherwise we would get some funky results
			direction = Vector3.Normalize(direction);

			// Calculate both the right and the up vector using cross product
			//		Note that we are calculating the right from the global up, this behaviour might
			//		not be what you need for all cameras so keep this in mind if you do not want a FPS camera
			right = Vector3.Normalize(Vector3.Cross(direction, Vector3.UnitY));
			up = Vector3.Normalize(Vector3.Cross(right, direction));
		}
		// Updates direction vertices based on quaternion
		void RotateVectors(Quaternion localRotation)
		{/*
			
			ApplyRotationToVector(localRotation, ref right);
			ApplyRotationToVector(localRotation, ref up);
			ApplyRotationToVector(localRotation, ref direction);
			*/
			var hAxis = new Vector3D(right);
			var vAxis = new Vector3D(up);
			var dir = new Vector3D(direction);

			ApplyRotationToVector(localRotation, ref hAxis);
			ApplyRotationToVector(localRotation, ref vAxis);
			ApplyRotationToVector(localRotation, ref dir);

			right.X = (float)hAxis.X;
			right.Y = (float)hAxis.Y;
			right.Z = (float)hAxis.Z;

			up.X = (float)vAxis.X;
			up.Y = (float)vAxis.Y;
			up.Z = (float)vAxis.Z;

			direction.X = (float)dir.X;
			direction.Y = (float)dir.Y;
			direction.Z = (float)dir.Z;

			yaw = (float)Math.Atan2(direction.X, -direction.Z) - MathHelper.PiOver2;
			pitch = (float)Math.Asin(direction.Y);
		}

		void ApplyRotationToVector(Quaternion rotation, ref Vector3D v)
		{
			//v = tmp * (rotation * v);
			v = rotation.Inverse() * (rotation * v);
		}

		public void UpdateTargetDistance()
		{
			targetDistance = (target - Position).Length;
		}
		public void UpdateTargetDistance(Vector3 target)
		{
			targetDistance = (target - Position).Length;
		}

		void UpdateTarget()
		{
			target = MoveAlongAxis(Position, direction, targetDistance);
		}
	}
}