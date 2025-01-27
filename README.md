# Sand and Water Particles

A 2D particle physics simulator coded in C# and uses Monogame Framework for graphics rendering.

## Installation

The following are the instructions for running the code:

- Download the project as a ZIP file or clone it using GitHub Desktop.
- Open the project in Visual Studio or VS Code.
- Make sure you have C# installed.
  - If you are using Visual Studio, Install .NET desktop development.
  - If you are using VS Code, run the following command:
    
  ```
  code --install-extension ms-dotnettools.csharp
  ```
  
  - To verify installation run the following command:
    
  ```
  dotnet --version
  ```
  
- Install Monogame
  - If you are using Visual Studio you can install Monogame through Extensions window.
  - If you are using VS Code, you can run the following command to install Monogame Templates:
    
  ```
  dotnet new --install MonoGame.Templates.CSharp
  ```

- Run the code

## How to Use

The project has no keyboard input, it only uses Mouse left click.

Select a particle or operation from the panel at the top by clicking on it.

- **Gray:** A solid block with no gravity or movement, only acting as a wall or obstacle.   
- **Yellow:** The sand particle.  
- **Blue:** The water particle.  
- **White:** It's the eraser, after selecting it use it to erase particles by clicking on them.  
- **Red:** It clears the whole screen.  

You can also change the simulation speed by using the minus and plus buttons at the top right.

## Code

Here is the brief explanation for what the files are responsible for.  

The explanation of the codes are written as comment lines in the code itself.

- **Main.cs** The code starts up from here. From here you can manage the screen size, running speed, controls and other management controls of the project.

- **Manager.cs** Responsible for management of particles and grid.

- **Particle.cs** The parent class of other particles.

- **Sand.cs** Controls the behavior of sand particles.

- **Water.cs** Controls the behavior of water particles.
