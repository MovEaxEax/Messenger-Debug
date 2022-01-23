using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;

public class xNyuHitbox : MonoBehaviour
{
    public void OnGUI()
    {
        GUI.Box(new Rect(x, Screen.height - y, w, h), "", BoxColor);
    }

    public void Update()
    {
        if (after_init)
        {
            if (xNyuDebugMenu.xNyuPerformanceBoost)
            {
                frame_skip_factor = 2;
            }
            else
            {
                frame_skip_factor = 0;
            }

            if (frame_skip == 0)
            {
                center = collider.bounds.center;
                size = collider.bounds.extents;

                x = Camera.main.WorldToScreenPoint(new Vector3(center.x - size.x, center.y, center.z)).x;
                y = Camera.main.WorldToScreenPoint(new Vector3(center.x, center.y + size.y, center.z)).y;
                c_x = Camera.main.WorldToScreenPoint(new Vector3(center.x, center.y, center.z)).x;
                c_y = Camera.main.WorldToScreenPoint(new Vector3(center.x, center.y, center.z)).y;
                w = (c_x - x) * 2;
                h = (y - c_y) * 2;
            }
            frame_skip++;
            if (frame_skip >= frame_skip_factor) frame_skip = 0;

            show = false;
            if (Parent != null)
            {
                if (Parent.active)
                {
                    if (!xNyuDebugMenu.DebugMenuActivated && xNyuDebugMenu.InspectorModeActive)
                    {
                        if (xNyuDebugMenu.InspectorCheckboxes[layer])
                        {
                            if (collider != null)
                            {
                                show = true;
                            }
                        }
                    }
                }
            }
        }
    }

    public void Init()
    {
        xNyuDebugMenu = GameObject.FindObjectOfType<xNyuDebug>();

        Parent = this.gameObject;

        Texture2D MagentaBox = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.45f));
        Texture2D GreenBox = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.45f));
        Texture2D BlueBox = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.45f));

        StyleMagentaBox = new GUIStyle();
        StyleMagentaBox.normal.background = MagentaBox;

        StyleGreenBox = new GUIStyle();
        StyleGreenBox.normal.background = GreenBox;

        StyleBlueBox = new GUIStyle();
        StyleBlueBox.normal.background = BlueBox;

        if (layer == 0)
        {
            BoxColor = StyleMagentaBox;
        }
        else if (layer == 1)
        {
            BoxColor = StyleGreenBox;
        }
        else if (layer == 2)
        {
            BoxColor = StyleBlueBox;
        }

        collider = Parent.GetComponent<Collider2D>();

        after_init = true;
    }

    public Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    public GUIStyle StyleMagentaBox;
    public GUIStyle StyleBlueBox;
    public GUIStyle StyleGreenBox;

    public GUIStyle BoxColor = null;

    public GameObject Parent = null;

    public xNyuDebug xNyuDebugMenu = null;

    public int layer = -1;

    int frame_skip = 0;

    public Vector3 center = Vector3.zero;
    public Vector3 size = Vector3.zero;

    public bool show = false;

    public float x = 0;
    public float y = 0;
    public float c_x = 0;
    public float c_y = 0;
    public float w = 0;
    public float h = 0;
    public Collider2D collider = null;

    public bool after_init = false;

    public int frame_skip_factor = 0;
}


