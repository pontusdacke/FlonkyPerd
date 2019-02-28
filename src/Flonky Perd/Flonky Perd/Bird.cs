using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Flonky_Perd
{
    class Bird : DrawableGameComponent
    {
        private float yPosition = 0;
        private float scale = 0.5f;
        private float rotation = 0.1f;
        private float gravity = 1;
        private float gravityIncr = 0.63f;
        private float animationTimer = 0;
        private float animationInterval = 80;
        private int frameCount = 2;
        private bool textureLoaded = false;

        // Initialized in Initialize() or LoadContent()
        private KeyboardState previousState;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private Vector2 size;
        private Vector2 position;
        private Rectangle rectangle;

        public Vector2 Position
        {
            get { return position; }
        }
        public Vector2 Size
        {
            get { return size; }
        }
        public Bird(Game game) : base(game) 
        {
            game.Components.Add(this);
        }

        public bool LoadTexture(string _texture)
        {
            try
            {
                texture = Game.Content.Load<Texture2D>(_texture);
                rectangle = new Rectangle(0, 0, texture.Width / frameCount, texture.Height);
                size = new Vector2(texture.Width / frameCount * scale, texture.Height * scale);
                textureLoaded = true;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public override void Initialize()
        {
            // Initial position
            position = new Vector2(100, 200);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice); 
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            if (textureLoaded)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
                {
                    rotation = -MathHelper.ToRadians(45);
                    gravity = -10;
                }
                yPosition += gravity;
                gravity += gravityIncr;
                if (rotation <= MathHelper.ToRadians(30))
                    rotation += MathHelper.ToRadians(3);
                position = new Vector2(100, yPosition);

                animationTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (animationTimer >= animationInterval)
                {
                    animationTimer = 0;
                    if (rectangle.X == 0) rectangle = new Rectangle(211, 0, 210, 130);
                    else rectangle = new Rectangle(0, 0, 210, 130);
                }

                previousState = Keyboard.GetState();
                base.Update(gameTime);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (textureLoaded)
                spriteBatch.Draw(texture, position, rectangle, Color.White, rotation, new Vector2(rectangle.Width * scale, rectangle.Height * scale), scale, SpriteEffects.None, 1.0f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
