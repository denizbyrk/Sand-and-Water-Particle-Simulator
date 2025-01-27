using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ParticleSimulator {
    public class Sand : Particle {

        public Sand(ParticleType type, int index, Color color, int buttonPosition) : base(type, index, color, buttonPosition) { }

        public override int[,] ParticleMovement(int[,] grid, int gridWidth, int gridHeight, int x, int y, List<int> waterIndexes) {

            //if the sand particle has nothing below it (except water) move it down
            if (grid[x, y + 1] == 0 || this.IsInIndexList(waterIndexes, grid[x, y + 1])) {

                grid[x, y] = 0;
                grid[x, y + 1] = this.Index;
            }

            //if the sand particle is not touching the left side of the screen, has a sand beneath it, and no sand particle to its left or bottom left (except water) move it down and left
            else if (x > 0 &&
                    (grid[x - 1, y + 1] == 0 || this.IsInIndexList(waterIndexes, grid[x - 1, y + 1])) &&
                    (grid[x - 1, y] == 0 || this.IsInIndexList(waterIndexes, grid[x - 1, y]))) {

                grid[x, y] = 0;
                grid[x - 1, y + 1] = this.Index;
            }

            //if the sand particle is not touching the right side of the screen, has a sand particle beneath it and no sand particle to its right or bottom right (except water) move it down and right
            else if (x < gridWidth - 1 &&
                    (grid[x + 1, y + 1] == 0 || this.IsInIndexList(waterIndexes, grid[x + 1, y + 1])) &&
                    (grid[x + 1, y] == 0 || this.IsInIndexList(waterIndexes, grid[x + 1, y]))) {

                grid[x, y] = 0;
                grid[x + 1, y + 1] = this.Index;
            }

            return grid;
        }

        //function to check if a specific index belongs to a specific type of particle (used for detecting if sand is on top of water)
        private bool IsInIndexList(List<int> waterIndexes, int index) {

            //iterate through all water indexes
            foreach (int wt in waterIndexes) {

                //if the water index matches with particle index, return true
                if (wt == index) {

                    return true;
                }
            }

            //else return false
            return false;
        }
    }
}