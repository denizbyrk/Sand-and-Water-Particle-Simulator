using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ParticleSimulator {
    public class Water : Particle {

        public Water (ParticleType type, int index, Color color, int buttonPosition) : base(type, index, color, buttonPosition) { }

        public override int[,] ParticleMovement(int[,] grid, int gridWidth, int gridHeight, int x, int y, List<int> waterIndexes) {

            //if water has nothing below it move it down
            if (grid[x, y + 1] == 0) {

                grid[x, y] = 0;
                grid[x, y + 1] = this.Index;
            }

            //else move to a random direction (left or right) unless there is a screen border
            else {

                //get random direction
                int direction = Main.random.Next(2);

                if (direction == 0 && x > 0 && grid[x - 1, y] == 0) {

                    grid[x, y] = 0;
                    grid[x - 1, y] = this.Index;

                } else if (direction == 1 && x < gridWidth - 1 && grid[x + 1, y] == 0) {

                    grid[x, y] = 0;
                    grid[x + 1, y] = this.Index;
                }
            }

            return grid;
        }
    }
}