# Riemann-Mating

Algorithm largely copied from https://code.mathr.co.uk/mating

Controls:
  WASD, Space, L-Shift: camera movement
  Mouse: rotate camera in space
  Scroll: zoom
  N: toggle lock camera to center of the world (so that it appears that the sphere itself is rotating rather than the camera)
  B: change camera type from FPS to FREE or vice versa
     
  IJKL: movement along the projected complex plane (initially, the origin is centered at the south pole)
  O, U: zoom in and out on the projected complex plane


Problems:
  1. The closer 's' is to zero, the more infinite-points (represented as blue) there are - which obscures the outer 'q' Julia Set
  2. When 's' is zero, the entire sphere is somehow colored as if it's the outer 'q' Julia Set
  3. When the last 'n' is being displayed (going through all the 's' for that 'n'), the fractals have slightly different shapes - it would not be a smooth transition, even if problems 1 and 2 were not there
     Note: this can be easily seen if the projection is zoomed in, so that 'q' mates into 'p' on the north pole (rather than the original 'p' mating with 'q' on the south pole)


I largely suspect these errors have to do with my complex arithmetic in shader.frag regarding 'q', and/or the order in which I do the mating operations in Game.cs.