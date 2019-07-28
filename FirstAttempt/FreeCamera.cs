using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstAttempt {
	public class FreeCamera : Camera {
		public float Yaw { get; set; }
		public float Pitch { get; set; }

		public Vector3 Position { get; set; }
		public Vector3 Target { get; set; }

		public Vector3 translation;

		public FreeCamera(Vector3 Position, float Yaw, float Pitch, GraphicsDevice graphicsDevice) : base(graphicsDevice){
			this.Position = Position;
			this.Yaw = Yaw;
			this.Pitch = Pitch;

			translation = Vector3.Zero;
		}

		public void Rotate(float YawChange, float PitchChange){
			Yaw += YawChange;
			Pitch += PitchChange;
		}

		public void Move(Vector3 Translation){
			translation += Translation;
		}

		public override void Update() {
			//Calculate the rotation Matrix
			Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);

			//Offset the position and reset the translation
			translation = Vector3.Transform(translation, rotation);
			Position += translation;
			translation = Vector3.Zero;

			//Calculate the new target
			Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
			Target = Position + forward;

			//Calculate the up vector
			Vector3 up = Vector3.Transform(Vector3.Up, rotation);

			//calculate the new view matrix
			base.View = Matrix.CreateLookAt(Position, Target, up);
		}
	}
}
