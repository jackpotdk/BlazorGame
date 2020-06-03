using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.WebGL;
using Blazor.Extensions;
using System.Numerics;

namespace BlazorGame.Engine
{
    public class Renderer
    {
        public SpriteEngine spriteEngine;

        private BECanvasComponent _canvasReference;

        WebGLContext _context;

        public long canvasWidth = 0;
        public long canvasHeight = 0;

        byte[] byteArr;

        byte[] textureData = new byte[4] { 0, 0, 255, 0 };

        WebGLUniformLocation u_matrix_location;

        private const string VS_SOURCE = 
            "attribute vec3 aPos;" +
            "attribute vec2 aTex;" +
            "varying vec2 vTex;" +
            "uniform mat4 u_matrix;" +

            "void main() {" +
                "gl_Position = u_matrix * vec4(aPos, 1.0);" +
                "vTex = aTex;" +
            "}";

        private const string FS_SOURCE = "precision mediump float;" +
                                         "varying vec2 vTex;" +
                                         "uniform sampler2D u_texture;" +

                                         "void main() {" +
                                            "gl_FragColor = texture2D(u_texture, vTex);" +
                                         "}";

        Vector3 transVector = new Vector3((float)-1, (float)-1, 0);

        WebGLBuffer vertexBuffer;

        public Renderer(BECanvasComponent CanvasReference)
        {
            _canvasReference = CanvasReference;

            spriteEngine = new SpriteEngine();
            byteArr = new byte[spriteEngine.vertices.Length * System.Runtime.InteropServices.Marshal.SizeOf<float>()];
        }


        public async Task Initialize()
        {
            _context = await _canvasReference.CreateWebGLAsync(new WebGLContextAttributes
            {
                PowerPreference = WebGLContextAttributes.POWER_PREFERENCE_HIGH_PERFORMANCE
            });

            vertexBuffer = await _context.CreateBufferAsync();
            await _context.BindBufferAsync(BufferType.ARRAY_BUFFER, vertexBuffer);

            var program = await this.InitProgramAsync(this._context, VS_SOURCE, FS_SOURCE);

            var positionLocation = await _context.GetAttribLocationAsync(program, "aPos");
            var texcoordLocation = await _context.GetAttribLocationAsync(program, "aTex");

            await _context.VertexAttribPointerAsync((uint)positionLocation, 3, DataType.FLOAT, false, 6 * sizeof(float), 0);
            await _context.VertexAttribPointerAsync((uint)texcoordLocation, 2, DataType.FLOAT, false, 6 * sizeof(float), 3 * sizeof(float));
            await _context.EnableVertexAttribArrayAsync((uint)positionLocation);
            await _context.EnableVertexAttribArrayAsync((uint)texcoordLocation);

            await _context.UseProgramAsync(program);

            var texture = await _context.CreateTextureAsync();
            await _context.BindTextureAsync(TextureType.TEXTURE_2D, texture);

        }


        private async Task<WebGLProgram> InitProgramAsync(WebGLContext gl, string vsSource, string fsSource)
        {
            var vertexShader = await this.LoadShaderAsync(gl, ShaderType.VERTEX_SHADER, vsSource);
            var fragmentShader = await this.LoadShaderAsync(gl, ShaderType.FRAGMENT_SHADER, fsSource);

            var program = await gl.CreateProgramAsync();
            await gl.AttachShaderAsync(program, vertexShader);
            await gl.AttachShaderAsync(program, fragmentShader);
            await gl.LinkProgramAsync(program);

            await gl.DeleteShaderAsync(vertexShader);
            await gl.DeleteShaderAsync(fragmentShader);

            if (!await gl.GetProgramParameterAsync<bool>(program, ProgramParameter.LINK_STATUS))
            {
                string info = await gl.GetProgramInfoLogAsync(program);
                throw new Exception("An error occured while linking the program: " + info);
            }

            u_matrix_location = await _context.GetUniformLocationAsync(program, "u_matrix");

            return program;
        }



        private async Task<WebGLShader> LoadShaderAsync(WebGLContext gl, ShaderType type, string source)
        {
            var shader = await gl.CreateShaderAsync(type);

            await gl.ShaderSourceAsync(shader, source);
            await gl.CompileShaderAsync(shader);

            if (!await gl.GetShaderParameterAsync<bool>(shader, ShaderParameter.COMPILE_STATUS))
            {
                string info = await gl.GetShaderInfoLogAsync(shader);
                await gl.DeleteShaderAsync(shader);
                throw new Exception("An error occured while compiling the shader: " + info);
            }

            return shader;
        }

        public async Task RenderFrame()
        {
            await _context.ClearColorAsync(0, 0, 0, 1);

            await _context.ClearAsync(BufferBits.COLOR_BUFFER_BIT);


            Buffer.BlockCopy(spriteEngine.vertices, 0, byteArr, 0, byteArr.Length);
            await _context.BufferDataAsync(BufferType.ARRAY_BUFFER, byteArr, BufferUsageHint.DYNAMIC_DRAW);

            // this matrix will convert from pixels to clip space
            Matrix4x4 myMatrix = Matrix4x4.CreateOrthographic(_canvasReference.Width, _canvasReference.Height, 1, -1);

            // this matrix will translate our quad to dstX, dstY
            Matrix4x4 transMatrix = Matrix4x4.CreateTranslation(transVector);
            myMatrix = Matrix4x4.Multiply(myMatrix, transMatrix);

            // Set the matrix.
            await _context.UniformMatrixAsync(u_matrix_location, false, new float[16] { myMatrix.M11, myMatrix.M12, myMatrix.M13, myMatrix.M14, myMatrix.M21, myMatrix.M22, myMatrix.M23, myMatrix.M24, myMatrix.M31, myMatrix.M32, myMatrix.M33, myMatrix.M34, myMatrix.M41, myMatrix.M42, myMatrix.M43, myMatrix.M44 });

            await _context.DrawArraysAsync(Primitive.TRIANGLES, 0, 6 * spriteEngine.spriteCount);


            spriteEngine.spriteCount = 0;

        }
    }
}
