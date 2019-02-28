using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Flonky_Perd
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FlonkyPerd : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D pipe, bg;
        Bird bird;
        float timer;
        float timerHardness;
        List<Vector2> pipes = new List<Vector2>();
        List<Vector2> bgs = new List<Vector2>();
        float score = 0;
        bool play = false;
        SpriteFont font;
        static Random rand = new Random();

        public FlonkyPerd()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
        }

        protected override void Initialize()
        {
            bgs.Add(new Vector2(0, 0));
            bgs.Add(new Vector2(800, 0));

            // Initialize components
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bg = Content.Load<Texture2D>("bg");
            pipe = Content.Load<Texture2D>("pipe");
            font = Content.Load<SpriteFont>("font");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            timerHardness += gameTime.ElapsedGameTime.Milliseconds;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (play)
            {
                #region pipes
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer >= (2000 - timerHardness / 30))
                {
                    float offset = (float)rand.NextDouble() * 100;
                    pipes.Add(new Vector2(graphics.PreferredBackBufferWidth, - 50 + offset));
                    pipes.Add(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - pipe.Height + offset - 50));
                    timer = 0;
                }
                float speed = 4;
                for (int i = 0; i < pipes.Count; i++)
                {
                    pipes[i] = new Vector2(pipes[i].X - speed, pipes[i].Y);
                    if (pipes[i].X + pipe.Width <= bird.Position.X)
                    {
                        pipes.RemoveAt(i);
                        score += 0.5f;
                    }

                    // Collision
                    if (pipes[i].X <= bird.Position.X + bird.Size.X)
                    {
                        Rectangle REKT = new Rectangle((int)pipes[i].X, (int)pipes[i].Y, pipe.Width, pipe.Height);
                        Rectangle birdRekt = new Rectangle(
                            (int)(bird.Position.X), 
                            (int)(bird.Position.Y), 
                            (int)bird.Size.X, 
                            (int)bird.Size.Y);

                        if (REKT.Intersects(birdRekt))
                        {
                            play = false;
                            score = 0;
                            pipes.Clear();
                            timerHardness = 0;
                        }
                    }
                }
                #endregion pipes
                #region background
                for (int i = 0; i < bgs.Count; i++)
                {
                    bgs[i] = new Vector2(bgs[i].X - 3, bgs[i].Y);
                    if (bgs[i].X <= -800) bgs[i] = new Vector2(798, bgs[i].Y);
                }
                #endregion
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    bird = new Bird(this);
                    bird.LoadTexture("bird");
                    play = true;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!play)
            {
                float r = (float)rand.NextDouble();
                float g = (float)rand.NextDouble();
                float b = (float)rand.NextDouble();
                GraphicsDevice.Clear(new Color(r, g, b));
            }
            else
                GraphicsDevice.Clear(Color.DarkGreen);
            spriteBatch.Begin();
            if (play)
            {
                for (int i = 0; i < bgs.Count; i++)
                {
                    spriteBatch.Draw(bg, bgs[i], Color.White);
                }
                for (int i = 0; i < pipes.Count; i++)
                {
                    spriteBatch.Draw(pipe, pipes[i], Color.White);
                }
                spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(0, 0), Color.DarkGoldenrod);
            }
            else
            {
                spriteBatch.DrawString(font, "Press ENTER to flope", new Vector2(0, 250), Color.PapayaWhip);
                spriteBatch.DrawString(font, "flupp with space", new Vector2(0, 320), Color.PapayaWhip);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}