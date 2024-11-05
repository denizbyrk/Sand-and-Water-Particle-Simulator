using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ParticleSimulator {
    public class Manager {

        private int particleSize; //size of each particle
        private int[,] grid; //the 2D array in which the particle indexes are going to be stored
        private int gridWidth; //width of the grid
        private int gridHeight; //height of the grid

        //list of particle types
        private List<Particle> particles; 

        //list of all distinct water particle indexes (used for checking if sand collides with water)
        private List<int> waterIndexes = new List<int>(); 

        public Manager(int particleSize, List<Particle> p) {

            //defining particle size
            this.particleSize = particleSize;

            //defining the grid array of integers based on the size of the screen relative to the particle size
            this.gridWidth = Main.screenWidth / this.particleSize;
            this.gridHeight = Main.screenHeight / this.particleSize;
            this.grid = new int[this.gridWidth, this.gridHeight];

            this.particles = p;

            //adding all the indexes of any different types of water particles to the waterIndexes list
            foreach (Particle pr in this.particles) {

                if (pr.Type == Particle.ParticleType.Water) {

                    this.waterIndexes.Add(pr.Index);
                }
            }
        }

        public void SetParticle(int x, int y, int index) {

            //set the X and Y positions of the particle
            x /= this.particleSize;
            y /= this.particleSize;

            //only setting a particle if cell is 0 and the index is not 0, or the cell is not 0 and the index is
            if ((this.grid[x, y] == 0 && index != 0) || (this.grid[x, y] != 0 && index == 0)) {

                this.grid[x, y] = index;
            }
        }

        //method for clearing the grid
        public void ClearGrid() {

            this.grid = new int[this.gridWidth, this.gridHeight];
        }

        public void MoveParticles() {

            //iterating through each horizontal row in grid from bottom to top
            for (int y = this.gridHeight - 2; y > -1; y--) {

                //iterating through each cell in each row
                for (int x = 0; x < this.gridWidth; x++) {

                    //checking if current cell is a particle
                    foreach (Particle p in particles) {

                        if (this.grid[x, y] == p.Index) {

                            //calling the particles 'ParticleMovement' method
                            this.grid = p.ParticleMovement(this.grid, this.gridWidth, this.gridHeight, x, y, this.waterIndexes);
                        }
                    }
                }
            }
        }

        //method for drawing particles
        public void DrawGrid(SpriteBatch b, Texture2D pixel) {

            //iterate through columns
            for (int y = 0; y < this.gridHeight; y++) {

                //iterate through rows
                for (int x = 0; x < this.gridWidth; x++) {

                    //loop through particles
                    foreach (Particle p in particles) {

                        //if the index matches the cell on the grid, draw the particle
                        if (this.grid[x, y] == p.Index) {

                            Vector2 pos = new Vector2(x * particleSize, y * particleSize);
                            Rectangle rect = new Rectangle(x * particleSize, y * particleSize, particleSize, particleSize);

                            b.Draw(pixel, pos, rect, p.ParticleColor);
                        }
                    }
                }
            }

            //draw the buttons for the particles
            foreach (Particle p in particles) {

                b.Draw(pixel, new Vector2(p.Button.X, p.Button.Y), p.Button, p.ParticleColor);
            }
        }
    }
}