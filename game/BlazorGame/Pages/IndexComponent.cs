using System;
using System.Threading.Tasks;
using System.Timers;
using Blazor.Extensions.Canvas.WebGL;
using Microsoft.AspNetCore.Components;
using Blazor.Extensions;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorGame
{
    public class IndexComponent : ComponentBase
    {
        protected BECanvasComponent _canvasReference;

        Game gameInstance;

        [Inject]
        internal IJSRuntime JSRuntime { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var tempdata = new byte[6] { 1, 255, 255, 0, 0, 0 };

            if (firstRender)
            {
                gameInstance = new Game(_canvasReference);
                await gameInstance.Initialize();

                var dotNetReference = DotNetObjectReference.Create(this);


                await JSRuntime.InvokeVoidAsync("anim.loadTexture", "assets/sprite.png");


                /// Hook animationloop
                await JSRuntime.InvokeVoidAsync("anim.start", dotNetReference);

                /// Hook MouseEvent on the client                
                await JSRuntime.InvokeVoidAsync("hookMouseEvents", dotNetReference);
            }
        }

        [JSInvokable("eventMouseDown")]
        public async void eventMouseDown(int x, int y)
        {
            await gameInstance.DoSpecial(x, y);
        }

        [JSInvokable("eventMouseUp")]
        public void eventMouseUp(int x, int y)
        {
//            await gameInstance.DoSpecial(x, y);
        }


        [JSInvokable("eventRequestAnimationFrame")]
        public async void eventRequestAnimationFrame(int x, int y)
        {
            gameInstance.rendererInstance.canvasWidth = x;
            gameInstance.rendererInstance.canvasHeight = y;
            gameInstance.rendererInstance.spriteEngine.canvasHeight = y;

            gameInstance.DoWorld();
            await gameInstance.DoRender();

        }

    }
}
