# International Space Station - 3D

This will show you the current location of the ISS in 3D space as it flies over Earth. By clicking and dragging, you can see what the astronauts would be seeing right now!

![iss-animation](https://user-images.githubusercontent.com/3202211/30151242-80375a40-9363-11e7-8dd5-97894c4ecb31.gif)

## Getting Started

1. Pull everything into your `assets` folder
2. Create a folder called `packages` (this is where stuff from the asset store will go)
3. Add the following packages to the `packages` folder: 
  - Boomlagoon (for parsing JSON)
  - EarthRendering Free (for the model and maps of the earth)
  
## To Do
- Replace the cube with a 3D model of ISS
- Make ISS info visible to user (speed, altitude, longitude, etc.)
- Script day/night (right now it's always day and night in the same places)
- Add sun/moon/galaxy map
- Smooth animation (I only ping the API 1x/second so the motion is super choppy)
- Add a minimap of Earth so you can tell where you are (if you're above the ocean, it's currently impossible to tell)
- Create clickable areas on ISS for more info
