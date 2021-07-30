# Playground
A place for me to experiment with ideas for systems and mechanics without needing to build a complete project

## List of Projects:
- [Galactic Pong](https://github.com/Harryonismyname/Playground#galactic-pong)
- [Neon Maze](https://github.com/Harryonismyname/Playground#neon-maze)
- [Economy Sim](https://github.com/Harryonismyname/Playground#economy-sim)
- [Engine Thing](https://github.com/Harryonismyname/Playground#engine-thing)

## Galactic Pong
[Play here](https://harryonismyname.itch.io/galactic-pong)

A Pong-clone, with a space aesthetic.

The goal was to attempt to build a complete project, and see which areas of development I enjoyed and which ones I didn't.

## Neon Maze
A Pac-Man clone with a Tron aesthetic

This was a second attempt to learn different areas of development. In this project I worked with pathfinding algorithms and working with Unity's Tilemap.
I also learned how to utilize some rudimentary coroutines to randomly select a path for the "ghosts"

## Economy Sim
This project was created because I had an idea for how to calculate a simulated value of trade goods based on supply and demand.

In this project I generated a 2D grid and populated it with random resource types, then performed a second pass where I used disc sampling to place settlements that could collect from the resource tiles nearby. I learned a little bit about how to generate custom meshes in this project as well as learned how to build a custom grid that accepted generic objects.

(I am still working on this one, sort of, I'm refining the grid class so that it can be more modular and eventually be used for 3D space in addition to 2D.)

## Engine Thing
This project was created as an expansion of Economy Sim, but with an emphasis on terrain generation.

Nothing in this project has been built, since I only recently started it, and got distracted by trying to fix my clunky grid system.

The goal for this project will be to generate terrain by simulating tectonic plates for landmass generation and then refining the landmasses via simulating a hydrologic cycle to erode the terrain for a more natural look. Then during post processing, run-off points where rain consistently fell, will be converted to a river type from the grid's perspective to save processing power that would be spent simulating water flow.
