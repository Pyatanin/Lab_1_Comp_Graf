using GLib;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Lab_1_Comp_Graf
{
    public class Program
    {
        public static int Height = 1000;
        public static int Width = 1000;

        public class Game : GameWindow
        {
            public float FrameTime = 0.0f;
            public int Fps = 0;
            public float XPosition = 0.0f;
            public float YPosition = 0.0f;
            public int mode = 0;
            public List<Vector2> points = new List<Vector2>();
            public Color4 StandartColor4 = Color4.Black;
            public Color4 SelectionColor4 = Color4.Red;
            public Color4 SelectionPoinColor4 = Color4.Blue;

            public float StandartSize = 3.0f;
            public float SelectionSize = 6.0f;
            public float SelectionPoinSize = 5.0f;

            public static NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(Height, Width),
                Location = new Vector2i(0, 30),
                WindowBorder = WindowBorder.Resizable,
                WindowState = WindowState.Normal,
                StartVisible = true,
                StartFocused = true,
                Title = "Lab_1",
                APIVersion = new Version(3, 3),
                Flags = ContextFlags.Default,
                Profile = ContextProfile.Compatability,
                API = ContextAPI.OpenGL,
                IsFullscreen = true,
                NumberOfSamples = 0
            };
            public class MyPoint
            {
                public Color4 color = Color4.White;
                public bool selection = false;
                public Vector2 coordinates;

                public MyPoint(Color4 color4, Vector2 vector2, bool Selection)
                {
                    coordinates = vector2;
                    color = color4;
                    selection = Selection;
                }

                public MyPoint(Vector2 vector2, bool Selection)
                {
                    coordinates = vector2;
                    selection = Selection;
                }

                public MyPoint()
                {
                }
            }

            public class Primitive
            {
                public int type = 0;
                public Color4 color = Color4.White;
                public bool selection = false;
                public List<MyPoint> coordinates;

                public Primitive(int Type, Color4 Color, bool Selection)
                {
                    type = Type;
                    color = Color;
                    selection = Selection;
                }

                public Primitive(int Type, List<MyPoint> Coordinates, Color4 Color, bool Selection)
                {
                    type = Type;
                    coordinates = Coordinates;
                    color = Color;
                    selection = Selection;
                }

                public void Primitive_add(MyPoint myPoint)
                {
                    coordinates.Add(myPoint);
                }
            }

            public class Primitives
            {
                public List<Primitive> listPrimitive = new List<Primitive>();

                public void Del_Primitive(Primitive p)
                {
                }

            }

            public Primitives primitives = new Primitives();
            public List<MyPoint> temp = new List<MyPoint>();

            public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(
                gameWindowSettings, nativeWindowSettings)
            {
                Console.WriteLine("Start");
                VSync = VSyncMode.On;
                Cursor = MouseCursor.Crosshair;
            }

            protected override void OnLoad()
            {
                base.OnLoad();
                Console.WriteLine("Vendor: " + GL.GetString(StringName.Vendor));
                Console.WriteLine("OpenGL version: " + GL.GetString(StringName.Version));
                GL.Enable(EnableCap.ProgramPointSize);
            }

            protected override void OnResize(ResizeEventArgs e)
            {
                base.OnResize(e);
            }

            protected override void OnUpdateFrame(FrameEventArgs args)
            {
                FrameTime += (float)args.Time;
                Fps++;
                if (FrameTime >= 1.0f)
                {
                    Title = $"Lab_1 FPS-{Fps}";
                    FrameTime = 0.0f;
                    Fps = 0;
                }

                var key = KeyboardState;

                if (key.IsKeyReleased(Keys.D0))
                {
                    if (mode != 4 && primitives.listPrimitive.Count != 0)
                    {
                        primitives.listPrimitive[^1].selection = false;
                    }

                    if (mode == 4 && primitives.listPrimitive[^1].coordinates.Count != 0)
                    {
                        primitives.listPrimitive[^1].selection = false;
                        primitives.listPrimitive.Add(new Primitive(mode, new List<MyPoint>(), StandartColor4, true));
                    }

                    mode = 0;
                    Console.WriteLine("Click 0");
                }

                if (key.IsKeyReleased(Keys.D1))
                {
                    mode = 1;
                    Console.WriteLine("Click 1");
                }

                if (key.IsKeyReleased(Keys.D2))
                {
                    mode = 2;
                    Console.WriteLine("Click 2");
                }

                if (key.IsKeyReleased(Keys.D3))
                {
                    mode = 3;
                    Console.WriteLine("Click 3");
                }

                if (key.IsKeyReleased(Keys.D4))
                {
                    mode = 4;
                    Console.WriteLine("Click 4");
                }
                
                if (key.IsKeyReleased(Keys.D9))
                {
                    mode = 9;
                    Console.WriteLine("Click 9");
                }

                if (key.IsKeyReleased(Keys.Space))
                {
                    Console.WriteLine("Spase");
                    if (mode == 4 && primitives.listPrimitive[^1].coordinates.Count != 0)
                    {
                        primitives.listPrimitive.Add(new Primitive(mode, new List<MyPoint>(), StandartColor4, true));
                    }
                }

                if (key.IsKeyReleased(Keys.Delete))
                {
                    Console.WriteLine("Delete");
                    for (int i = primitives.listPrimitive.Count-1; i >= 0; i--)
                    {
                        if (primitives.listPrimitive[i].selection)
                        {
                            primitives.listPrimitive.RemoveAt(i);
                        }
                    }
                }

                
                if (key.IsKeyReleased(Keys.Z))
                {
                    Console.WriteLine("Click Z");
                    if (primitives.listPrimitive.Count != 0)
                    {
                        primitives.listPrimitive.Remove(primitives.listPrimitive.Last());
                    }
                }

                if (key.IsKeyReleased(Keys.Backspace))
                {
                    Console.WriteLine("Backspace");
                    
                    for (int i = primitives.listPrimitive.Count-1; i >= 0; i--)
                    {
                        if (primitives.listPrimitive[i].type == mode)
                        {
                            primitives.listPrimitive.RemoveAt(i);
                        }
                    }
                }

                XPosition = MousePosition.X - Height / 2;
                YPosition = -MousePosition.Y + Width / 2;
                base.OnUpdateFrame(args);
            }

            protected override void OnRenderFrame(FrameEventArgs args)
            {
                GL.ClearColor(Color4.White);
                GL.Clear(ClearBufferMask.ColorBufferBit);
                GL.Enable(EnableCap.PointSmooth);
                if (primitives != null)
                {
                    foreach (var p in primitives.listPrimitive)
                    {
                        if (p.coordinates.Count != 0)
                        {
                            GL.Color4(p.color);
                            switch (p.type)
                            {
                                case 1:
                                    GL.PointSize(StandartSize);
                                    GL.Begin(PrimitiveType.Points);
                                    break;
                                case 2:
                                    GL.LineWidth(StandartSize);
                                    GL.Begin(PrimitiveType.Lines);
                                    break;
                                case 3:
                                    GL.Begin(PrimitiveType.Triangles);
                                    break;
                                case 4:
                                    GL.LineWidth(StandartSize);
                                    GL.Begin(PrimitiveType.LineStrip);
                                    break;
                            }

                            foreach (var point in p.coordinates)
                            {
                                GL.Vertex2(2 * point.coordinates.X / Height, 2 * point.coordinates.Y / Width);
                            }

                            GL.End();
                            if (p.selection)
                            {
                                GL.PointSize(SelectionSize);
                                GL.Color4(SelectionColor4);
                                GL.Begin(PrimitiveType.Points);
                                foreach (var point in p.coordinates)
                                {
                                    GL.Vertex2(2 * point.coordinates.X / Height, 2 * point.coordinates.Y / Width);
                                }

                                GL.End();
                            }

                            foreach (var point in p.coordinates)
                            {
                                if (point.selection)
                                {
                                    GL.PointSize(SelectionPoinSize);
                                    GL.Color4(SelectionPoinColor4);
                                    GL.Begin(PrimitiveType.Points);
                                    GL.Vertex2(2 * point.coordinates.X / Height, 2 * point.coordinates.Y / Width);
                                    GL.End();
                                }
                            }
                        }
                    }
                }

                SwapBuffers();
                base.OnRenderFrame(args);
            }

            protected override void OnMouseDown(MouseButtonEventArgs e)
            {
                base.OnMouseDown(e);
                Console.WriteLine("Click");
                if (e.Button == MouseButton.Left && mode == 0)
                {
                    var lUp = new Vector2(XPosition - 5, YPosition + 5);

                    var rDown = new Vector2(XPosition + 5, YPosition - 5);
                    if (primitives.listPrimitive.Count != 0)
                    {
                        foreach (var p in primitives.listPrimitive)
                        {
                            foreach (var coord in p.coordinates)
                            {
                                if (coord.coordinates.X > lUp.X && coord.coordinates.Y < lUp.Y)
                                {
                                    if (coord.coordinates.X < rDown.X && coord.coordinates.Y > rDown.Y)
                                    {
                                        if (p.selection)
                                            p.selection = false;
                                        else
                                            p.selection = true;
                                    }
                                }
                            }
                        }
                    }
                }
                
                if (e.Button == MouseButton.Left && mode == 9)
                {
                    var lUp = new Vector2(XPosition - 5, YPosition + 5);
                    var rDown = new Vector2(XPosition + 5, YPosition - 5);
                    if (primitives.listPrimitive.Count != 0)
                    {
                        foreach (var p in primitives.listPrimitive)
                        {
                            if (p.selection)
                            {
                                foreach (var coord in p.coordinates)
                                {
                                    if (coord.coordinates.X > lUp.X && coord.coordinates.Y < lUp.Y)
                                    {
                                        if (coord.coordinates.X < rDown.X && coord.coordinates.Y > rDown.Y)
                                        {
                                            if (coord.selection)
                                                coord.selection = false;
                                            else
                                                coord.selection = true;
                                        }
                                        else
                                            coord.selection = false;
                                    }
                                    else
                                        coord.selection = false;
                                }
                            }
                        }
                    }
                }

                if (e.Button == MouseButton.Left && mode == 1)
                {
                    if (primitives.listPrimitive.Count != 0)
                    {
                        primitives.listPrimitive[^1].selection = false;
                    }

                    var dot = new Vector2(XPosition, YPosition);
                    temp.Add(new MyPoint(dot, false));
                    List<MyPoint> point = new List<MyPoint>(temp);
                    primitives.listPrimitive.Add(new Primitive(mode, point, StandartColor4, true));
                    temp.Clear();
                }

                if (e.Button == MouseButton.Left && mode == 2)
                {
                    if (primitives.listPrimitive.Count != 0)
                    {
                        primitives.listPrimitive[^1].selection = false;
                    }

                    var dot = new Vector2(XPosition, YPosition);
                    temp.Add(new MyPoint(dot, false));
                    primitives.listPrimitive.Add(new Primitive(1, temp, StandartColor4, true));

                    if (temp.Count == 2)
                    {
                        List<MyPoint> point = new List<MyPoint>(temp);
                        primitives.listPrimitive.Add(new Primitive(mode, point, StandartColor4, true));
                        primitives.listPrimitive.RemoveAt(primitives.listPrimitive.Count - 2);
                        primitives.listPrimitive.RemoveAt(primitives.listPrimitive.Count - 2);
                        temp.Clear();
                    }
                }

                if (e.Button == MouseButton.Left && mode == 3)
                {
                    if (primitives.listPrimitive.Count != 0)
                    {
                        primitives.listPrimitive[^1].selection = false;
                    }

                    var dot = new Vector2(XPosition, YPosition);
                    temp.Add(new MyPoint(dot, false));
                    primitives.listPrimitive.Add(new Primitive(1, temp, StandartColor4, true));
                    if (temp.Count == 2)
                    {
                        primitives.listPrimitive.Add(new Primitive(2, temp, StandartColor4, true));
                    }

                    if (temp.Count == 3)
                    {
                        List<MyPoint> point = new List<MyPoint>(temp);
                        primitives.listPrimitive.Add(new Primitive(mode, point, StandartColor4, true));
                        primitives.listPrimitive.RemoveAt(primitives.listPrimitive.Count - 2);
                        primitives.listPrimitive.RemoveAt(primitives.listPrimitive.Count - 2);
                        primitives.listPrimitive.RemoveAt(primitives.listPrimitive.Count - 2);
                        primitives.listPrimitive.RemoveAt(primitives.listPrimitive.Count - 2);
                        temp.Clear();
                    }
                }

                if (e.Button == MouseButton.Left && mode == 4)
                {
                    if (primitives.listPrimitive.Count != 1)
                    {
                        primitives.listPrimitive[^2].selection = false;
                    }

                    if (primitives.listPrimitive[^1].type == 4)
                    {
                        Vector2 coord = new Vector2(XPosition, YPosition);
                        MyPoint point = new MyPoint(coord, false);
                        primitives.listPrimitive[^1].Primitive_add(point);
                    }
                    else
                    {
                        primitives.listPrimitive.Add(new Primitive(mode, new List<MyPoint>(), StandartColor4, true));
                        if (primitives.listPrimitive.Count != 0)
                        {
                            primitives.listPrimitive[^2].selection = false;
                        }

                        Vector2 coord = new Vector2(XPosition, YPosition);
                        MyPoint point = new MyPoint(coord, false);
                        primitives.listPrimitive[^1].Primitive_add(point);
                    }
                }

                if (e.Button == MouseButton.Right)
                {
                }
            }

            protected override void OnMouseUp(MouseButtonEventArgs e)
            {
                base.OnMouseDown(e);
                
                if (e.Button == MouseButton.Left && mode == 9)
                {
                    var lUp = new Vector2(XPosition - 5, YPosition + 5);
                    var rDown = new Vector2(XPosition + 5, YPosition - 5);
                    if (primitives.listPrimitive.Count != 0)
                    {
                        foreach (var p in primitives.listPrimitive)
                        {
                            if (p.selection)
                            {
                                foreach (var coord in p.coordinates)
                                {
                                    if (coord.selection)
                                    {
                                        coord.coordinates.X = XPosition;
                                        coord.coordinates.Y = YPosition;
                                        coord.selection = false;
                                    }
                                }
                            }
                        }
                    }
                }

            }

            protected override void OnUnload()
            {
                base.OnUnload();
            }
        }

        public static void Main(string[] args)
        {
            using (Game game = new Game(GameWindowSettings.Default, Program.Game.nativeWindowSettings))
            {
                game.Run();
            }
        }
    }
}