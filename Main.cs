using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ParticleSimulator { 
    public class Main : Game {

        private GraphicsDeviceManager graphics;
        private SpriteBatch b;

        //define screen dimensions
        public const int screenWidth = 1280;
        public const int screenHeight = 720;

        //content manager for loading font
        private ContentManager contentManager;

        //set the particle speed (this is just FPS)
        public float runSpeed = 60.0f;
        private float minRunSpeed = 10f; //dont set this to lower than 10
        private float maxRunSpeed = 500f;
        private float speedChange = 10f; //how much the speed would change when you click the increase/decrease speed buttons

        //defining textures
        private Texture2D pixel;

        //defining particles
        private const int particleSize = 10;
        private Particle Block;
        private Particle Sand;
        private Particle Water;
        private List<Particle> Particles = new List<Particle>();

        //defining particle manager
        private Manager Manager;

        //current block type selected
        private int currentBlock = 0;

        //buttons
        public const int buttonSize = 40;
        private Rectangle eraseButton;
        private Rectangle clearButton;
        private Rectangle increaseSpeedButton;
        private Rectangle decreaseSpeedButton;

        //indicator for displaying which button is chosen
        public static Rectangle selectedButton = new Rectangle();

        //name of chosen particle
        private string chosenParticle = "None";

        //get the mouse state
        private MouseState currentMouseState;
        private MouseState prevMouseState;

        //create font for displaying text
        private SpriteFont Font;

        public static Random random = new Random();

        public Main() {

            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.SynchronizeWithVerticalRetrace = true;

            this.Window.AllowAltF4 = true;
            this.Window.AllowUserResizing = false;

            this.Content.RootDirectory = "Content";
            this.IsFixedTimeStep = true;
            this.IsMouseVisible = true;

            //set running speed (FPS)
            this.TargetElapsedTime = TimeSpan.FromSeconds(1 / this.runSpeed);
        }

        protected override void Initialize() {

            //set window size
            this.graphics.PreferredBackBufferWidth = Main.screenWidth;
            this.graphics.PreferredBackBufferHeight = Main.screenHeight;
            this.graphics.ApplyChanges();

            //initiliaze content manager
            this.contentManager = new ContentManager(this.Content.ServiceProvider, "Content");

            base.Initialize();
        }

        protected override void LoadContent() {

            this.b = new SpriteBatch(this.GraphicsDevice);

            //create an empty white pixel
            this.pixel = new Texture2D(this.GraphicsDevice, 1, 1);
            this.pixel.SetData(new[] { Color.White });

            //initializing particles
            this.Block = new Particle(Particle.ParticleType.Block, 1, Color.Gray, 0); //since block needs to stay still, its directly created from Particle object
            this.Sand = new Sand(Particle.ParticleType.Sand, 2, Color.Gold, Main.buttonSize);
            this.Water = new Water(Particle.ParticleType.Water, 3, Color.Blue, Main.buttonSize * 2);

            //add particles to particles list
            this.Particles.Add(this.Block);
            this.Particles.Add(this.Sand);
            this.Particles.Add(this.Water);

            //initialize manager and buttons
            this.Manager = new Manager(Main.particleSize, this.Particles);

            this.eraseButton = new Rectangle(Main.buttonSize * 7, 0, Main.buttonSize, Main.buttonSize);
            this.clearButton = new Rectangle(Main.buttonSize * 8, 0, Main.buttonSize, Main.buttonSize);

            this.decreaseSpeedButton = new Rectangle(Main.buttonSize * 30, 0, Main.buttonSize, Main.buttonSize);
            this.increaseSpeedButton = new Rectangle(Main.buttonSize * 31, 0, Main.buttonSize, Main.buttonSize);

            //load font
            this.Font = this.contentManager.Load<SpriteFont>("Font");
        }

        private void HandleButtonClick() {

            this.prevMouseState = this.currentMouseState; //set the previous mouse state to current one
            this.currentMouseState = Mouse.GetState(); //set current mouse state

            bool buttonClicked;

            //check if the mouse is within window bounds and if it is clicked
            if (this.prevMouseState.LeftButton == ButtonState.Pressed &&
                this.currentMouseState.X >= 0 && this.currentMouseState.X < Main.screenWidth &&
                this.currentMouseState.Y >= 0 && this.currentMouseState.Y < Main.screenHeight) {

                //create mouse hitbox
                Rectangle mouseClickArea = new Rectangle(this.currentMouseState.X, this.currentMouseState.Y, 1, 1);

                //checking if a button is clicked
                buttonClicked = false;

                //iterate through particles do check button clicks
                foreach (Particle p in Particles) {

                    if (mouseClickArea.Intersects(p.Button)) {

                        this.currentBlock = p.Index;
                        buttonClicked = true;

                        Main.selectedButton = p.Button;

                        //check pressed button's block and set the text to its name
                        switch (p.Type) {

                            case Particle.ParticleType.Block:

                                this.chosenParticle = "Block";
                                break;

                            case Particle.ParticleType.Sand:

                                this.chosenParticle = "Sand";
                                break;

                            case Particle.ParticleType.Water:

                                this.chosenParticle = "Water";
                                break;

                            default:

                                this.chosenParticle = "None";
                                break;
                        }
                    }
                }

                //checking for click on erase button
                if (mouseClickArea.Intersects(this.eraseButton)) {

                    this.currentBlock = 0;
                    buttonClicked = true;

                    Main.selectedButton = this.eraseButton;
                    this.chosenParticle = "Erase";
                }

                //checking for click on clear button
                else if (mouseClickArea.Intersects(this.clearButton)) {

                    this.currentBlock = 0;
                    this.Manager.ClearGrid();
                    buttonClicked = true;

                    Main.selectedButton = new Rectangle();
                    this.chosenParticle = "None";
                }

                //checking for click on decrease speed button
                else if (mouseClickArea.Intersects(this.decreaseSpeedButton) && this.currentMouseState.LeftButton == ButtonState.Released) {

                    buttonClicked = true;
                    this.runSpeed -= this.speedChange;
                }

                //checking for click on increase speed button
                else if (mouseClickArea.Intersects(this.increaseSpeedButton) && this.currentMouseState.LeftButton == ButtonState.Released) {

                    buttonClicked = true;
                    this.runSpeed += this.speedChange;
                }

                //clamp the simulation speed between chosen values (dont enter a value smaller than 0)
                this.runSpeed = MathHelper.Clamp(this.runSpeed, this.minRunSpeed, this.maxRunSpeed);

                //if a button hasnt been pressed, place particle at mouse position (if last clicked erase button, particles placed will be empty spaces)
                if (buttonClicked == false && this.currentMouseState.Y > Main.buttonSize) {

                    //spawn the particle and the clicked position
                    this.Manager.SetParticle(this.currentMouseState.X, this.currentMouseState.Y, this.currentBlock);
                }
            }
        }

        protected override void Update(GameTime dt) {

            //if you delete the following if statement, the simulation will keep running even minimize the window or switch to another tab
            if (this.IsActive == true) {

                //set the simulation speed
                this.TargetElapsedTime = TimeSpan.FromSeconds(1 / this.runSpeed);

                //get button clicks
                this.HandleButtonClick();

                //update all particles
                this.Manager.MoveParticles();
            }

            base.Update(dt);
        }

        protected override void Draw(GameTime dt) {

            //set the background color
            this.GraphicsDevice.Clear(Color.Black);

            this.b.Begin(); //begin drawing

            //draw particles and their buttons
            this.Manager.DrawGrid(this.b, this.pixel);

            //draw top panel
            this.DrawPanel();

            this.b.End(); //end drawing

            base.Draw(dt);
        }

        private void DrawPanel() {

            //draw line
            this.b.Draw(this.pixel, new Rectangle(0, Main.buttonSize, Main.screenWidth, Main.particleSize), Color.DarkSlateGray);

            //draw erase and clear buttons
            this.b.Draw(this.pixel, new Vector2(this.eraseButton.X, this.eraseButton.Y), this.eraseButton, Color.White);
            this.b.Draw(this.pixel, new Vector2(this.clearButton.X, this.clearButton.Y), this.clearButton, Color.Red);

            //draw decrease and increase speed buttons
            this.b.Draw(this.pixel, new Vector2(this.decreaseSpeedButton.X, this.decreaseSpeedButton.Y), this.decreaseSpeedButton, Color.LightGray);
            this.b.Draw(this.pixel, new Vector2(this.increaseSpeedButton.X, this.increaseSpeedButton.Y), this.increaseSpeedButton, Color.DarkGray);

            //draw the indicator for current selected button
            this.DrawRectangularOutline(b, Main.selectedButton.X, Main.selectedButton.Y, Main.selectedButton.Width, Main.selectedButton.Height, 5, Color.DarkSeaGreen);

            //draw chosen block text
            this.b.DrawString(this.Font, "Chosen Block:" + this.chosenParticle, new Vector2(this.clearButton.X + Main.buttonSize + 8, 12), Color.Lime);

            //draw the simulation speed
            this.b.DrawString(this.Font, "-", new Vector2(this.decreaseSpeedButton.X + this.decreaseSpeedButton.Width / 3, this.decreaseSpeedButton.Y + this.decreaseSpeedButton.Height / 3), Color.Black);
            this.b.DrawString(this.Font, "+", new Vector2(this.increaseSpeedButton.X + this.increaseSpeedButton.Width / 3, this.increaseSpeedButton.Y + this.increaseSpeedButton.Height / 3), Color.Black);
            this.b.DrawString(this.Font, "Simulation Speed:" + this.runSpeed, new Vector2(Main.screenWidth - 400, 12), Color.Orange);
        }

        //method for drawing an rectangular outline
        private void DrawRectangularOutline(SpriteBatch b, int posX, int posY, int width, int height, int stroke, Color color) {

            //the edges are basically very thin rectangles
            Rectangle edge1 = new Rectangle(posX, posY, width, stroke); //top edge
            Rectangle edge2 = new Rectangle(posX, posY, stroke, height); //left edge
            Rectangle edge3 = new Rectangle(posX + width - stroke, posY, stroke, height); //right edge
            Rectangle edge4 = new Rectangle(posX, posY + height - stroke, width, stroke); //bottom edge

            List<Rectangle> edges = new List<Rectangle> { edge1, edge2, edge3, edge4 }; //add them to a list

            //iterate through all 4 of them and draw 
            foreach (Rectangle edge in edges) {

                b.Draw(this.pixel, edge, color);
            }
        }
    }
}