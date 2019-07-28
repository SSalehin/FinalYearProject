using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace FirstAttempt
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game {
		GraphicsDeviceManager graphics;

		List<CModel> models = new List<CModel>();
		Camera camera;

		MouseState lastMouseState;

		public Game1() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize() {
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		protected override void LoadContent() {
			models.Add(new CModel(Content.Load<Model>("ZZZ"), new Vector3(0, 400, 0), new Vector3(0,0,0), new Vector3(0.4f), GraphicsDevice));
			models.Add(new CModel(Content.Load<Model>("ground"), Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice));

			camera = new ChaseCamera(new Vector3(0, 400, 1500),	new Vector3(0, 200, 0), new Vector3(0, 0, 0), GraphicsDevice);

			lastMouseState = Mouse.GetState();
		}

		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		protected override void Update(GameTime gameTime) {
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here
			updateModel(gameTime);
			updateCamera(gameTime);

			base.Update(gameTime);
		}
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.CornflowerBlue);

			foreach (CModel model in models)
				if (camera.BoundingVolumeIsInView(model.BoundingSphere))
					model.Draw(camera.View, camera.Projection);

			base.Draw(gameTime);
		}

		void updateModel(GameTime gameTime){
			KeyboardState keyState = Keyboard.GetState();
			Vector3 rotChange = new Vector3(0, 0, 0);

			//Determine on which axis the ship should be rotated on
			if (keyState.IsKeyDown(Keys.W)) rotChange += new Vector3(1, 0, 0);
			if (keyState.IsKeyDown(Keys.S)) rotChange += new Vector3(-1, 0, 0);
			if (keyState.IsKeyDown(Keys.A)) rotChange += new Vector3(0, 1, 0);
			if (keyState.IsKeyDown(Keys.D)) rotChange += new Vector3(0, -1, 0);

			models[0].Rotation += rotChange * 0.1f;

			//If space isn't down, the ship shpuldn't move
			if (!keyState.IsKeyDown(Keys.Space)) return;

			//Determine which direction to move in
			Matrix rotation = Matrix.CreateFromYawPitchRoll(models[0].Rotation.Y, models[0].Rotation.X, models[0].Rotation.Z);
			//Move in the direction dictated by our rotation matrix
			models[0].Position += Vector3.Transform(Vector3.Forward, rotation) * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;
		}

		void updateCamera(GameTime gameTime) {
			//Move the camera to the new model's position and orientation
			((ChaseCamera)camera).Move(models[0].Position, models[0].Rotation);
			camera.Update();
		}
	}
}