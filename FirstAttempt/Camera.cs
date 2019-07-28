using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstAttempt {
	public abstract class Camera {
		Matrix view, projection;

		public Matrix View {
			get { return view; }
			set {
				view = value;
				generateFrustrum();
			}
		}
		public Matrix Projection {
			get { return projection; }
			protected set {
				projection = value;
				generateFrustrum();
			}
		}

		protected GraphicsDevice GraphicsDevice;

		public BoundingFrustum Frustum { get; private set; }

		public Camera(GraphicsDevice graphicsDevice){
			this.GraphicsDevice = graphicsDevice;
			generatePerspectiveProjectionMatrix(MathHelper.Pi / 3f);
		}

		private void generatePerspectiveProjectionMatrix(float fieldOfView){
			PresentationParameters pp = GraphicsDevice.PresentationParameters;
			float aspectRatio = (float)pp.BackBufferWidth / (float)pp.BackBufferHeight;
			this.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, 0.1f, 1000000.0f);
		}

		private void generateFrustrum(){
			Matrix viewProjection = View * Projection;
			Frustum = new BoundingFrustum(viewProjection);
		}

		public bool BoundingVolumeIsInView(BoundingSphere sphere){
			return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
		}

		public bool BoundingVolumeIsInView(BoundingBox box) {
			return (Frustum.Contains(box) != ContainmentType.Disjoint);
		}

		public virtual void Update(){

		}
	}
}