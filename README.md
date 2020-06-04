# BlazorGame
BlazorGame is a small proof-of-concept project to investigate the feasability of Blazor-based C# game development.

A quick build is hosted [here](https://blazorgametestbuild.azurewebsites.net/). Once the black square appears, click inside the square to spawn a new sprite. 

This project relies heavily on the work done in [Blazor Canvas](https://github.com/BlazorExtensions/Canvas). However, as this has not been updated for a while, I have included (an updated version of) it as part of this project.

Once/if BlazorCanvas gets an update, this project will be updated to support the NuGet.

# What it does

Not much, but some basics and essentials that are nice to have running for anyone looking to do games:

- Opens a re-sizable Canvas.
- Demonstrates calling into JS from WASM, and calling back to C# from WASM.
- Hooks up requestAnimationFrame
- Hooks up eventlistener for mouse clicks
- Renders an object (using WebGL) when you click inside the Canvas.

# Notes

- Builds against the release version of Blazor WebAssembly (3.2.0) and .NET Core SDK (3.1.300 or later).
- "Sprites" are rendered by updating the vertices of all sprites each frame. I realize this is not ideal, but it works for this test.
- texImage2D support in BlazorCanvas is broken, so the textures is loaded through a JS interop call to loadTexture.
- The GarbageCollector is very busy. Either I may be doing something wrong, or there is room for improvement.

# Contributions and feedback

Please feel free to use / learn from this code, open issues, fix bugs or provide feedback.

# Contributors

If you want to get in touch, reach me at thomasn@vanillapp.com

