# Riemann-Mating

This project attempts to show the slow mating of two Mandelbrot Julia Sets in real time, based on "The Thurston Algorithm for quadratic matings" (https://arxiv.org/pdf/1706.04177v1.pdf) by Wolf Jung.

The algorithm used is largely taken from https://code.mathr.co.uk/mating: see this question for a more detailed explanation of the algorithm I'm using: https://math.stackexchange.com/questions/3747790


To change the values of the slow mating, simply go into Game.cs and adjust the desired values at the beginning of the file, and then build and run the project in Visual Studio (this project was made in VS 2019).

***CONTROLS:***

  **WASD, Space, L-Shift**: camera movement
  **Mouse**: rotate camera in space
  **Scroll**: zoom
  **N**: toggle lock camera to center of the world (so that it appears that the sphere itself is rotating rather than the camera)
  **B**: change camera type from FPS to FREE or vice versa

  **IJKL**: movement along the projected complex plane (initially, the origin is centered at the south pole)
  **O, U**: zoom in and out on the projected complex plane
   
  **Left, Right, Down Arrow Keys**: decrease, increase, or stop changing the mating frame (NOTE: this only can be done after all the frames have been generated)
  
  **F11**: toggle fullscreen
  

.

**PROBLEMS:**
  1. When the last 'n' is being displayed (going through all the 's' for "mating_iterations - 1"), the fractals have slightly different shapes - unsure of what's causing this problem	
