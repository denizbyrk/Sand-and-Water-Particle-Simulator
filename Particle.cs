using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ParticleSimulator {
    public class Particle {

        //enumerator for particle types
        public enum ParticleType {

            Block,
            Sand,
            Water
        }

        //type of the particle
        public ParticleType Type { get; set; } 

        //numerical index that represents this particle on the 2D array 'grid'
        public int Index { get; set; }

        //color of the particle and its toggle button
        public Color ParticleColor { get; set; }

        //the toggle button for this particle
        public Rectangle Button { get; set; } 

        public Particle(ParticleType type, int index, Color color, int buttonPosition) {

            this.Type = type;
            this.Index = index;
            this.ParticleColor = color;
            this.Button = new Rectangle(buttonPosition, 0, Main.buttonSize, Main.buttonSize);
        }

        public virtual int[,] ParticleMovement(int[,] grid, int gridWidth, int gridHeight, int x, int y, List<int> liquidTokens) => grid;
    }
}