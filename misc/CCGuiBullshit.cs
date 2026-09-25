using AsmResolver.IO;
using catclientv3.features.visual;
//using Il2Cpp;
using UnityEngine;
using UnityEngine.XR;

namespace catclientv3.misc;


internal struct DrawCall
    {
        public Vector3 Start;
        public Vector3 End;
        public Color Color;
    }

public static class CCGuiBullshit
{
    
    /*private static Mesh _lineMesh;
    private static Material _lineMaterial;

    public static void DrawLineImmediate(Vector3 start, Vector3 end, Color color)
    {
        if (_lineMesh == null) {
            _lineMesh = new Mesh();
            _lineMesh.vertices = new Vector3[] { Vector3.zero, Vector3.forward };
            _lineMesh.SetIndices(new int[] { 0, 1 }, MeshTopology.Lines, 0);
        }

        if (_lineMaterial == null) {
            Shader shader = Shader.Find("GUI/Text Shader"); //toon standard owo
            _lineMaterial = new Material(shader);
            _lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            _lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            _lineMaterial.SetInt("_ZWrite", 0); // disable depth writing
            _lineMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
            _lineMaterial.renderQueue = 3999; 
        }

        
        Vector3 dir = end - start;
        Quaternion rotation = Quaternion.LookRotation(dir);
        Matrix4x4 matrix = Matrix4x4.TRS(start, rotation, new Vector3(1, 1, dir.magnitude));
        
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        props.SetColor("_Color", color);

        // Render for this frame only
        Graphics.DrawMesh(_lineMesh, matrix, _lineMaterial, 0, null, 0, props);
    }*/
        
        private static Vector3[] offsetSigns = new Vector3[]
        {
            new Vector3( 1,  1,  1), // 0: FTR
            new Vector3( 1,  1, -1), // 1: BTR
            new Vector3( 1, -1,  1), // 2: FBR
            new Vector3( 1, -1, -1), // 3: BBR
            new Vector3(-1,  1,  1), // 4: FTL
            new Vector3(-1,  1, -1), // 5: BTL
            new Vector3(-1, -1,  1), // 6: FBL
            new Vector3(-1, -1, -1), // 7: BBL
        };
        
        // edging ohhhhh my goddd
        private static int[,] edges = new int[12, 2]
            {
                // front
                {0, 2}, {2, 6}, {6, 4}, {4, 0}, 
                // back
                {1, 3}, {3, 7}, {7, 5}, {5, 1}, 
                // between
                {0, 1}, {2, 3}, {6, 7}, {4, 5} 
            };
        
        private static List<DrawCall> drawQueue = new List<DrawCall>(); //store lines to draw bla bla
        //private static List<DrawCall> drawQueueCompat = new List<DrawCall>(); //store lines to draw bla bla
        
        private static Material lineMaterial;
        
        public static void DrawCompatGaySex(Vector3 start, Vector3 end, Color color, float width = 0.01f, float duration = 0.02f)
        {
            InitializeMaterial();
            // faggot
            GameObject lineObj = new GameObject("i hate life this shit SUCCKS");
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();

            // FUCK IOFF
            lr.material = lineMaterial;
        
            // AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
            lr.startColor = color;
            lr.endColor = color;
            lr.startWidth = width;
            lr.endWidth = width;
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);

            // die
            UnityEngine.Object.Destroy(lineObj, duration == 0 ? Time.deltaTime : duration);
        }
        
        private static void InitializeMaterial()
        {
            if (lineMaterial == null)
            {
                Shader shader = Shader.Find("Hidden/Internal-Colored"); //unlit internal
                //Shader shader = Shader.Find("GUI/Text Shader"); //toon standard owo
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                lineMaterial.SetInt("_ZWrite", 0); // disable depth writing
                lineMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
                lineMaterial.renderQueue = 3999; 
            }
        }
        
        public static void lineDrawinator(Vector3 a, Vector3 b, Color col)
        {
            //if (!CCCoolEspController.vrCompatMode)
                drawQueue.Add(new DrawCall { Start = a, End = b, Color = col}); //add to queue, this is here so I don't have to change the other code that uses it
            /*else
            {
                DrawCompatGaySex(a, b, col, duration: Time.deltaTime + 0.015f);
            }*/
        }
    
        
    
        public static void boxAroundBounds(Bounds bounds, Color col)
        {
            Vector3 center = bounds.center;
            Vector3 extents = bounds.extents;
            
            
            Vector3[] corners = new Vector3[8]; 
            for (int i = 0; i < 8; i++) //calc corners
            {
                corners[i] = center + Vector3.Scale(extents, offsetSigns[i]);
            }

            // queue edges
            for (int i = 0; i < 12; i++)
            {
                Vector3 p1 = corners[edges[i, 0]];
                Vector3 p2 = corners[edges[i, 1]];
                
                lineDrawinator(p1, p2, col);
            }
        }
        
        public static void RenderQueuedLines() // this is called to render the queue bla bla bla
        {
                if (drawQueue.Count == 0) return;
                InitializeMaterial();
                if (lineMaterial == null) return;

                lineMaterial.SetPass(0); //activate mat
                var cam = Camera.current;
                Matrix4x4 projectionMatrix = cam.stereoEnabled
                    ? cam.GetStereoProjectionMatrix((Camera.StereoscopicEye)cam.stereoActiveEye)
                    : cam.projectionMatrix;
                Matrix4x4 modelviewMatrix = cam.stereoEnabled
                    ? cam.GetStereoViewMatrix((Camera.StereoscopicEye)cam.stereoActiveEye)
                    : cam.worldToCameraMatrix;
                // begin drawing lines in world space
                GL.PushMatrix();
                GL.LoadProjectionMatrix(projectionMatrix);
                GL.modelview = modelviewMatrix;
                GL.Begin(GL.LINES);
                foreach (var call in drawQueue)
                {
                    //if(!CCCoolEspController.vrCompatMode)
                        DrawLineInternal(call);
                
                }

                GL.End();
                GL.PopMatrix();
        }

        public static void ClearQueue() => drawQueue.Clear(); //clear queue on late update so it renders in all active places (mirrors, camera, ect)


        //draw the line
        private static void DrawLineInternal(DrawCall call)
        {
            GL.Color(call.Color);
            GL.Vertex(call.Start);
            GL.Vertex(call.End);
        }
    }


//old screengui shit (ewwww)
/*public static class CCGuiBullshit
{
    public static GUIStyle gStyle = new GUIStyle
    {
        alignment = TextAnchor.MiddleCenter,
        fontSize = 16,
        normal = { textColor = Color.white }
    };
    
    public static void TextOutline(Rect rect, string text, Color outlineColor, Color textColor)
    {
        gStyle.normal.textColor = outlineColor;
        GUI.Label(new Rect(rect.x + 1, rect.y + 1, rect.width, rect.height), text, gStyle);

        gStyle.normal.textColor = textColor;
        GUI.Label(rect, text, gStyle);
    }

    public static void lineDrawinator(Vector3 a, Vector3 b, Color col, float thickness)
    {
        Matrix4x4 origM = GUI.matrix;
        Color origC = GUI.color;
        float ang = Vector3.Angle(b - a, Vector2.right);
        ang *= a.y > b.y ? -1 : 1;
        float l = (b - a).magnitude;
        GUIUtility.RotateAroundPivot(ang, a);
        GUI.color = col;
        GUI.DrawTexture(new Rect(a.x,a.y,l,thickness), Texture2D.whiteTexture);
        GUI.matrix = origM;
        GUI.color = origC;
        
    }

    public static void boxAroundBounds(Bounds bounds, Color col, float thickness)
    {
        //gemini code because I am unsure how to do this
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        // Define the 8 corner offsets relative to the center
        Vector3[] offsetSigns = new Vector3[]
        {
            new Vector3( 1,  1,  1), new Vector3( 1,  1, -1),
            new Vector3( 1, -1,  1), new Vector3( 1, -1, -1),
            new Vector3(-1,  1,  1), new Vector3(-1,  1, -1),
            new Vector3(-1, -1,  1), new Vector3(-1, -1, -1)
        };

        // Initialize min/max values based on the first projected corner
        Vector3 firstCorner3D = center + Vector3.Scale(extents, offsetSigns[0]);
        Vector3 firstScreenPoint = Camera.main.WorldToScreenPoint(firstCorner3D);
        Vector2 firstScreenGUI = new Vector2(firstScreenPoint.x, Screen.height - firstScreenPoint.y);

        float minX = firstScreenGUI.x;
        float maxX = firstScreenGUI.x;
        float minY = firstScreenGUI.y;
        float maxY = firstScreenGUI.y;

        // 1. Calculate 8 World Space Corners and find the 2D screen-space Min/Max extent
        for (int i = 1; i < 8; i++) // Start from the second index (i=1)
        {
            // Calculate 3D World Corner Position
            Vector3 corner3D = center + Vector3.Scale(extents, offsetSigns[i]);
            
            // Project 3D position to 2D Screen Position (bottom-left origin)
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(corner3D);

            // Skip drawing if the point is behind the camera (though the projection itself often handles this poorly)
            // For a true 2D bounding box, we must consider all projected points, even if off-screen.

            // Convert to GUI coordinate system (top-left origin: Y is inverted)
            float x = screenPoint.x;
            float y = Screen.height - screenPoint.y;

            // Update min/max screen coordinates
            minX = Mathf.Min(minX, x);
            maxX = Mathf.Max(maxX, x);
            minY = Mathf.Min(minY, y);
            maxY = Mathf.Max(maxY, y);
        }

        // 2. Define the 4 corners of the final 2D square (AABB)
        Vector2 topLeft     = new Vector2(minX, minY);
        Vector2 topRight    = new Vector2(maxX, minY);
        Vector2 bottomRight = new Vector2(maxX, maxY);
        Vector2 bottomLeft  = new Vector2(minX, maxY);

        // 3. Draw the 4 edges using lineDrawinator
        lineDrawinator(topLeft, topRight, col, thickness);      // Top
        lineDrawinator(topRight, bottomRight, col, thickness);  // Right
        lineDrawinator(bottomRight, bottomLeft, col, thickness); // Bottom
        lineDrawinator(bottomLeft, topLeft, col, thickness);    // Left
    }
}*/