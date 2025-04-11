using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
    /// <summary>
    /// Orthographical Camera Class
    /// Purpose: An 2D Camera System that acts like an perspective camera.
    ///          Allows easily moving the viewable area of the world.
    /// </summary>
    internal class OrthographicCamera
    {
        //---------------------------------------------------------------
        // Fields
        //---------------------------------------------------------------

        // This matrix is referenced by the SpriteBatch when drawing.
        // Used to position, scale, and rotate all sprites attached to that SpriteBatch
        private Matrix cameraMatrix;

        // Position for the Matrix to use (For bounding the camera;i.e. clamping)
        private Vector2 camPosition;

        // Zoom scale for the Matrix
        private float zoom;

        // Two integer fields to store Screen Width and Height
        private int screenWidth;
        private int screenHeight;

        // A rectangle for the camera bounds (if needed)
        private Rectangle cameraBound;


        //---------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------
        public Matrix CameraMatrix
        {
            get { return cameraMatrix; }
        }

        public Vector2 CamPosition
        {
            get { return camPosition; }
        }

        public Rectangle CameraBound
        {
            get { return cameraBound; }
        }


        //---------------------------------------------------------------
        // Constructor
        //---------------------------------------------------------------
        /// <summary>
        /// Initializes the fields of the Orthographic Camera to function.
        /// </summary>
        /// <param name="viewport">Pass in the current viewport of the game</param>
        public OrthographicCamera(Viewport viewport)
        {
            // No initial translation to the world.
            cameraMatrix = Matrix.CreateTranslation(0, 0, 0);

            // Camera's position starts at (0,0).
            camPosition = Vector2.Zero;

            // Zoom value is currently 1.0f (basic)
            zoom = 1.0f;

            // Initialize screen width and height
            screenWidth = viewport.Width;
            screenHeight = viewport.Height;

            // The Camera bounds rectangle.
            cameraBound = new Rectangle((int)camPosition.X,
                                        (int)camPosition.Y,
                                        screenWidth,
                                        screenHeight);
        }


        //---------------------------------------------------------------
        // Class Methods (Behaviors)
        //---------------------------------------------------------------

        /// <summary>
        /// The Orthographic Camera has its own Update method. 
        /// This will be the only method of the  that will be able to accessed.
        /// </summary>
        /// <param name="worldToScreenOffSet">World to Screen Offset
        /// (i.e. worldPosition + this.OffSet = screenPosition)</param>
        /// <param name="worldPosition">
        /// The world position of the object to follow(e.g. usually player)</param>
        /// <param name="worldWidth">World Width; Num of columns * Size of a tile</param>
        /// <param name="worldHeight">World Height; Num of rows * Size of a tile</param>
        public void Update(Vector2 worldToScreenOffSet, Rectangle worldPosition, int worldWidth, int worldHeight)
        {
            // Update the Camera Matrix
            UpdateCameraMatrix(worldToScreenOffSet);

            // Ensure that the camera only rendering the drawn world
            CameraRendering(worldPosition, worldWidth, worldHeight);
        }


        /// <summary>
        /// Update the Camera's Matrix each frame.
        /// </summary>
        /// <param name="worldToScreenOffSet">World to Screen Offset
        /// (i.e. worldPosition + this.OffSet = screenPosition)</param>
        private void UpdateCameraMatrix(Vector2 worldToScreenOffSet)
        {
            cameraMatrix = Matrix.CreateTranslation(worldToScreenOffSet.X, worldToScreenOffSet.Y, 0);
        }


        /// <summary>
        /// Keeps the camera bounds to render only the drawn world,
        /// centering the camera position using the player's position.
        /// </summary>
        /// <param name="worldPosition">
        /// The world position of the object to follow(e.g. usually player)</param>
        /// <param name="worldWidth">World Width; Num of columns * Size of a tile</param>
        /// <param name="worldHeight">World Height; Num of rows * Size of a tile</param>
        private void CameraRendering(Rectangle worldPosition, int worldWidth, int worldHeight)
        {
            // Get the current Camera Position in relation to the world
            // (Which works as the offset;
            //  how much of the outside of the world the camera could be showing)
            camPosition.X = worldPosition.X - screenWidth / 2;
            camPosition.Y = worldPosition.Y - screenHeight / 2;

            // Clamp the camera position to be in the world bounds
            camPosition.X = MathHelper.Clamp(camPosition.X, 0, worldWidth - screenWidth);
            camPosition.Y = MathHelper.Clamp(camPosition.Y, 0, worldHeight - screenHeight);

            // Build the matrix to offset the world by the camera position
            cameraMatrix = Matrix.CreateTranslation(-camPosition.X, -camPosition.Y, 0);

            // Additionally, update the camera's bound defined by the new camera position.
            cameraBound.X = (int)camPosition.X;
            cameraBound.Y = (int)camPosition.Y;
        }
    }
}
