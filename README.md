# unity3d-gravity-spheres
This repository contains recruitment task that focuses on implementation of gravity and object pooling.

## Task requirements:
* every quarter of a second, new sphere is spawned in the camera's field of view
* text in the upper right corner shows how many spheres have been spawned so far
* each sphere has it's own gravity field and attracts other spheres
* on collision, two spheres merge into one, that:
  * has proportionally greater mass and gravity field
  * has surface area that is equal to sum of surface areas of the two merged spheres
* when a sphere's mass exceeds mass of 50 original spheres, it breaks down into as many spheres as it contained (if a sphere collided with 5 other spheres, then it will break into 6 spheres)
  * spheres that were created as a result of the breakdown of a larger sphere should be fired at high speed in random directions
  * also, they should not collide with anything else for half a second
* when spheres amount reaches 250, spawning is stopped and spheres' gravity field should start to work in a opposite way - spheres should be repelled instead of attracted
* air resistance should be simulated, as spheres are not in vacuum
* no other gravitational forces interact with spheres

## Usage
Clone this repository, open Unity, open existing project and select GravitySpheres directory.
Project was created with Unity 2018.3.9f1

## License
[Apache 2.0](https://www.apache.org/licenses/LICENSE-2.0.txt)
