using GLib;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Lab_1_Comp_Graf
{
    public static class Program
    {
        private static readonly int Height = 1000;
        private static readonly int Width = 1000;

        private class Game : GameWindow
        {
            private float _frameTime = 0.0f;
            private int _fps = 0;
            private float _xPosition = 0.0f;
            private float _yPosition = 0.0f;
            private int _mode = 0;
            public List<Vector2> Points = new List<Vector2>();
            private readonly Color4 _standartColor4 = Color4.Black;
            private readonly Color4 _selectionColor4 = Color4.Red;
            private readonly Color4 _selectionPoinColor4 = Color4.Blue;

            private readonly float _standartSize = 3.0f;
            private readonly float _selectionSize = 6.0f;
            private readonly float _selectionPoinSize = 5.0f;

            public static readonly NativeWindowSettings NativeWindowSettings = new NativeWindowSettings()
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

            private class MyPoint
            {
                private Color4 _color = Color4.White;
                public bool Selection = false;
                public Vector2 Coordinates;

                public MyPoint(Color4 color4, Vector2 vector2, bool selection)
                {
                    Coordinates = vector2;
                    _color = color4;
                    this.Selection = selection;
                }

                public MyPoint(Vector2 vector2, bool selection)
                {
                    Coordinates = vector2;
                    this.Selection = selection;
                }

                public MyPoint()
                {
                }
            }

            private class Primitive
            {
                public readonly int Type = 0;
                public readonly Color4 Color;
                public bool Selection = false;
                public readonly List<MyPoint> Coordinates;

                public Primitive(int type, Color4 color, bool selection, List<MyPoint> coordinates)
                {
                    this.Type = type;
                    this.Color = color;
                    this.Selection = selection;
                    Coordinates = coordinates;
                }

                public Primitive(int type, List<MyPoint> coordinates, Color4 color, bool selection)
                {
                    this.Type = type;
                    this.Coordinates = coordinates;
                    this.Color = color;
                    this.Selection = selection;
                }

                public void Primitive_add(MyPoint myPoint)
                {
                    Coordinates.Add(myPoint);
                }
            }

            private class Primitives
            {
                public readonly List<Primitive> ListPrimitive = new List<Primitive>();

                public void Del_Primitive(Primitive p)
                {
                }

            }

            private readonly Primitives _primitives = new Primitives();
            private readonly List<MyPoint> _temp = new List<MyPoint>();

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
                _frameTime += (float)args.Time;
                _fps++;
                if (_frameTime >= 1.0f)
                {
                    Title = $"Lab_1 FPS-{_fps}";
                    _frameTime = 0.0f;
                    _fps = 0;
                }

                var key = KeyboardState;

                if (key.IsKeyReleased(Keys.D0))
                {
                    if (_mode != 4 && _primitives.ListPrimitive.Count != 0)
                    {
                        _primitives.ListPrimitive[^1].Selection = false;
                    }

                    if (_mode == 4 && _primitives.ListPrimitive[^1].Coordinates.Count != 0)
                    {
                        _primitives.ListPrimitive[^1].Selection = false;
                        _primitives.ListPrimitive.Add(new Primitive(_mode, new List<MyPoint>(), _standartColor4, true));
                    }

                    _mode = 0;
                    Console.WriteLine("Click 0");
                }

                if (key.IsKeyReleased(Keys.D1))
                {
                    _mode = 1;
                    Console.WriteLine("Click 1");
                }

                if (key.IsKeyReleased(Keys.D2))
                {
                    _mode = 2;
                    Console.WriteLine("Click 2");
                }

                if (key.IsKeyReleased(Keys.D3))
                {
                    _mode = 3;
                    Console.WriteLine("Click 3");
                }

                if (key.IsKeyReleased(Keys.D4))
                {
                    _mode = 4;
                    Console.WriteLine("Click 4");
                }
                
                if (key.IsKeyReleased(Keys.D9))
                {
                    _mode = 9;
                    Console.WriteLine("Click 9");
                }

                if (key.IsKeyReleased(Keys.Space))
                {
                    Console.WriteLine("Spase");
                    if (_mode == 4 && _primitives.ListPrimitive[^1].Coordinates.Count != 0)
                    {
                        _primitives.ListPrimitive.Add(new Primitive(_mode, new List<MyPoint>(), _standartColor4, true));
                    }
                }

                if (key.IsKeyReleased(Keys.Delete))
                {
                    Console.WriteLine("Delete");
                    for (var i = _primitives.ListPrimitive.Count-1; i >= 0; i--)
                    {
                        if (_primitives.ListPrimitive[i].Selection)
                        {
                            _primitives.ListPrimitive.RemoveAt(i);
                        }
                    }
                }

                
                if (key.IsKeyReleased(Keys.Z))
                {
                    Console.WriteLine("Click Z");
                    if (_primitives.ListPrimitive.Count != 0)
                    {
                        _primitives.ListPrimitive.Remove(_primitives.ListPrimitive.Last());
                    }
                }

                if (key.IsKeyReleased(Keys.Backspace))
                {
                    Console.WriteLine("Backspace");
                    
                    for (var i = _primitives.ListPrimitive.Count-1; i >= 0; i--)
                    {
                        if (_primitives.ListPrimitive[i].Type == _mode)
                        {
                            _primitives.ListPrimitive.RemoveAt(i);
                        }
                    }
                }

                _xPosition = MousePosition.X - Height / 2;
                _yPosition = -MousePosition.Y + Width / 2;
                base.OnUpdateFrame(args);
            }

            protected override void OnRenderFrame(FrameEventArgs args)
            {
                GL.ClearColor(Color4.White);
                GL.Clear(ClearBufferMask.ColorBufferBit);
                GL.Enable(EnableCap.PointSmooth);
                foreach (var p in _primitives.ListPrimitive)
                {
                    if (p.Coordinates.Count != 0)
                    {
                        GL.Color4(p.Color);
                        switch (p.Type)
                        {
                            case 1:
                                GL.PointSize(_standartSize);
                                GL.Begin(PrimitiveType.Points);
                                break;
                            case 2:
                                GL.LineWidth(_standartSize);
                                GL.Begin(PrimitiveType.Lines);
                                break;
                            case 3:
                                GL.Begin(PrimitiveType.Triangles);
                                break;
                            case 4:
                                GL.LineWidth(_standartSize);
                                GL.Begin(PrimitiveType.LineStrip);
                                break;
                        }

                        foreach (var point in p.Coordinates)
                        {
                            GL.Vertex2(2 * point.Coordinates.X / Height, 2 * point.Coordinates.Y / Width);
                        }

                        GL.End();
                        if (p.Selection)
                        {
                            GL.PointSize(_selectionSize);
                            GL.Color4(_selectionColor4);
                            GL.Begin(PrimitiveType.Points);
                            foreach (var point in p.Coordinates)
                            {
                                GL.Vertex2(2 * point.Coordinates.X / Height, 2 * point.Coordinates.Y / Width);
                            }

                            GL.End();
                        }

                        foreach (var point in p.Coordinates)
                        {
                            if (point.Selection)
                            {
                                GL.PointSize(_selectionPoinSize);
                                GL.Color4(_selectionPoinColor4);
                                GL.Begin(PrimitiveType.Points);
                                GL.Vertex2(2 * point.Coordinates.X / Height, 2 * point.Coordinates.Y / Width);
                                GL.End();
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
                if (e.Button == MouseButton.Left && _mode == 0)
                {
                    var lUp = new Vector2(_xPosition - 5, _yPosition + 5);

                    var rDown = new Vector2(_xPosition + 5, _yPosition - 5);
                    if (_primitives.ListPrimitive.Count != 0)
                    {
                        foreach (var p in _primitives.ListPrimitive)
                        {
                            foreach (var coord in p.Coordinates)
                            {
                                if (coord.Coordinates.X > lUp.X && coord.Coordinates.Y < lUp.Y)
                                {
                                    if (coord.Coordinates.X < rDown.X && coord.Coordinates.Y > rDown.Y)
                                    {
                                        p.Selection = !p.Selection;
                                    }
                                }
                            }
                        }
                    }
                }
                
                if (e.Button == MouseButton.Left && _mode == 9)
                {
                    var lUp = new Vector2(_xPosition - 5, _yPosition + 5);
                    var rDown = new Vector2(_xPosition + 5, _yPosition - 5);
                    if (_primitives.ListPrimitive.Count != 0)
                    {
                        foreach (var p in _primitives.ListPrimitive)
                        {
                            if (p.Selection)
                            {
                                foreach (var coord in p.Coordinates)
                                {
                                    if (coord.Coordinates.X > lUp.X && coord.Coordinates.Y < lUp.Y)
                                    {
                                        if (coord.Coordinates.X < rDown.X && coord.Coordinates.Y > rDown.Y)
                                        {
                                            coord.Selection = !coord.Selection;
                                        }
                                        else
                                            coord.Selection = false;
                                    }
                                    else
                                        coord.Selection = false;
                                }
                            }
                        }
                    }
                }

                if (e.Button == MouseButton.Left && _mode == 1)
                {
                    if (_primitives.ListPrimitive.Count != 0)
                    {
                        _primitives.ListPrimitive[^1].Selection = false;
                    }

                    var dot = new Vector2(_xPosition, _yPosition);
                    _temp.Add(new MyPoint(dot, false));
                    var point = new List<MyPoint>(_temp);
                    _primitives.ListPrimitive.Add(new Primitive(_mode, point, _standartColor4, true));
                    _temp.Clear();
                }

                if (e.Button == MouseButton.Left && _mode == 2)
                {
                    if (_primitives.ListPrimitive.Count != 0)
                    {
                        _primitives.ListPrimitive[^1].Selection = false;
                    }

                    var dot = new Vector2(_xPosition, _yPosition);
                    _temp.Add(new MyPoint(dot, false));
                    _primitives.ListPrimitive.Add(new Primitive(1, _temp, _standartColor4, true));

                    if (_temp.Count == 2)
                    {
                        List<MyPoint> point = new List<MyPoint>(_temp);
                        _primitives.ListPrimitive.Add(new Primitive(_mode, point, _standartColor4, true));
                        _primitives.ListPrimitive.RemoveAt(_primitives.ListPrimitive.Count - 2);
                        _primitives.ListPrimitive.RemoveAt(_primitives.ListPrimitive.Count - 2);
                        _temp.Clear();
                    }
                }

                if (e.Button == MouseButton.Left && _mode == 3)
                {
                    if (_primitives.ListPrimitive.Count != 0)
                    {
                        _primitives.ListPrimitive[^1].Selection = false;
                    }

                    var dot = new Vector2(_xPosition, _yPosition);
                    _temp.Add(new MyPoint(dot, false));
                    _primitives.ListPrimitive.Add(new Primitive(1, _temp, _standartColor4, true));
                    if (_temp.Count == 2)
                    {
                        _primitives.ListPrimitive.Add(new Primitive(2, _temp, _standartColor4, true));
                    }

                    if (_temp.Count == 3)
                    {
                        var point = new List<MyPoint>(_temp);
                        _primitives.ListPrimitive.Add(new Primitive(_mode, point, _standartColor4, true));
                        _primitives.ListPrimitive.RemoveAt(_primitives.ListPrimitive.Count - 2);
                        _primitives.ListPrimitive.RemoveAt(_primitives.ListPrimitive.Count - 2);
                        _primitives.ListPrimitive.RemoveAt(_primitives.ListPrimitive.Count - 2);
                        _primitives.ListPrimitive.RemoveAt(_primitives.ListPrimitive.Count - 2);
                        _temp.Clear();
                    }
                }

                if (e.Button == MouseButton.Left && _mode == 4)
                {
                    if (_primitives.ListPrimitive.Count != 1)
                    {
                        _primitives.ListPrimitive[^2].Selection = false;
                    }

                    if (_primitives.ListPrimitive[^1].Type == 4)
                    {
                        var coord = new Vector2(_xPosition, _yPosition);
                        var point = new MyPoint(coord, false);
                        _primitives.ListPrimitive[^1].Primitive_add(point);
                    }
                    else
                    {
                        _primitives.ListPrimitive.Add(new Primitive(_mode, new List<MyPoint>(), _standartColor4, true));
                        if (_primitives.ListPrimitive.Count != 0)
                        {
                            _primitives.ListPrimitive[^2].Selection = false;
                        }

                        var coord = new Vector2(_xPosition, _yPosition);
                        var point = new MyPoint(coord, false);
                        _primitives.ListPrimitive[^1].Primitive_add(point);
                    }
                }

                if (e.Button == MouseButton.Right)
                {
                }
            }

            protected override void OnMouseUp(MouseButtonEventArgs e)
            {
                base.OnMouseDown(e);
                
                if (e.Button == MouseButton.Left && _mode == 9)
                {
                    var lUp = new Vector2(_xPosition - 5, _yPosition + 5);
                    var rDown = new Vector2(_xPosition + 5, _yPosition - 5);
                    if (_primitives.ListPrimitive.Count != 0)
                    {
                        foreach (var p in _primitives.ListPrimitive)
                        {
                            if (p.Selection)
                            {
                                foreach (var coord in p.Coordinates)
                                {
                                    if (coord.Selection)
                                    {
                                        coord.Coordinates.X = _xPosition;
                                        coord.Coordinates.Y = _yPosition;
                                        coord.Selection = false;
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
            using var game = new Game(GameWindowSettings.Default, Program.Game.NativeWindowSettings);
            game.Run();
        }
    }
}