using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;

public class xNyuInspector : MonoBehaviour
{
    public void OnGUI()
    {
        if (!xNyuDebugMenu.DebugMenuActivated && xNyuDebugMenu.InspectorModeActive)
        {
            if (menu_open)
            {


                Vector3 calc_pos = Camera.main.WorldToScreenPoint(position);
                calc_pos.y = Screen.height - calc_pos.y;
                if (calc_pos.x > 0 && calc_pos.x < Screen.width && calc_pos.y > 0 && calc_pos.y < Screen.height)
                {
                    GUI.Box(new Rect(calc_pos.x, calc_pos.y, box_width * scr_scale_w, (30f * scr_scale_h) + (((Data.Count + 1) * 25f) * scr_scale_h)), "", StyleBlackBox);
                    for (int i = 0; i < Data.Count; i++)
                    {
                        GUI.Label(new Rect(calc_pos.x + (10f * scr_scale_w), calc_pos.y + (10f * scr_scale_h) + (i * (25f * scr_scale_h)), 200f, 300f), Data.ElementAt(i).Key + ": " + Data.ElementAt(i).Value, StyleSmallRed);
                    }
                    GUI.Box(new Rect(calc_pos.x + (10f * scr_scale_w), calc_pos.y + ((25f * Data.Count) * scr_scale_h) + (17f * scr_scale_h), 18f * scr_scale_w, 18f * scr_scale_h), "", StyleWhiteBox);
                    if (locked) GUI.Label(new Rect(calc_pos.x + (12f * scr_scale_w), calc_pos.y + ((25f * Data.Count) * scr_scale_h) + (17f * scr_scale_h), 200f * scr_scale_w, 300f * scr_scale_h), "X", StyleSmallRed);
                    GUI.Label(new Rect(calc_pos.x + (31f * scr_scale_w), calc_pos.y + ((25f * Data.Count) * scr_scale_h) + (17f * scr_scale_h), 200f * scr_scale_w, 300f * scr_scale_h), "Lock Position", StyleSmallRed);

                }
            }
        }
    }

    public void Update()
    {
        if (!xNyuDebugMenu.InspectorModeActive)
        {
            locked = false;
            menu_open = false;
        }
        if (!xNyuDebugMenu.DebugMenuActivated && xNyuDebugMenu.InspectorModeActive)
        {
            if (locked)
            {
                if (locked_init)
                {
                    locked_x = position.x;
                    locked_y = position.y;
                    locked_z = position.z;
                    locked_init = false;
                }
                Parent.transform.position = new Vector3(locked_x, locked_y, locked_z);
            }

            float mouse_x = Input.mousePosition.x;
            float mouse_y = height - Input.mousePosition.y;

            Vector3 calc_pos = Camera.main.WorldToScreenPoint(position);
            calc_pos.y = Screen.height - calc_pos.y;

            //Mouse Click
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2))
            {
                //Checkbox 1
                if (mouse_x > calc_pos.x + (10f * scr_scale_w) && mouse_x < calc_pos.x + (10f * scr_scale_w) + (180f * scr_scale_w) && mouse_y > calc_pos.y + ((25f * Data.Count) * scr_scale_h) + (17f * scr_scale_h) && mouse_y < calc_pos.y + ((25f * Data.Count) * scr_scale_h) + (42f * scr_scale_h)) { if (locked) { locked = false; } else { locked_init = true; locked = true; } }
            }

            if (frame_skip >= 2)
            {
                width = (float)Screen.width;
                height = (float)Screen.height;
                scr_scale_w = width / fixed_size_width;
                scr_scale_h = height / fixed_size_height;

                int f_size_small = (int)(22 * scr_scale_w);

                StyleSmallRed.fontSize = f_size_small;


                //Data
                position = Parent.transform.position;

                Data["Name"] = Parent.name;
                Data["Position"] = position.ToString();

                Collider2D collider = Parent.GetComponent<Collider2D>();
                if (collider != null)
                {
                    var center = collider.bounds.center;
                    var size = collider.bounds.extents;
                    Data["Center"] = center.ToString();
                    Data["Size"] = size.ToString();
                }



                float chars = 0;
                foreach (string s in Data.Values)
                {
                    if ((float)s.Length > chars) chars = (float)s.Length;
                }
                box_width = chars * 16f;



                frame_skip = 0;

            }
            else
            {
                frame_skip++;
            }

            if (Input.GetMouseButton(1))
            {
                if (after_init)
                {
                    if (!InspectorClicked)
                    {
                        RaycastHit2D hit = Physics2D.BoxCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.5f, 0.5f), 0f, Vector2.up);
                        if (hit.collider != null)
                        {
                            if (hit.transform.gameObject == Parent)
                            {
                                if (hit.transform.gameObject.layer == 8 || hit.transform.gameObject.layer == 11)
                                {
                                    menu_open = !menu_open;
                                }
                            }
                        }
                        InspectorClicked = true;
                    }
                }
            }
            else
            {
                InspectorClicked = false;
                if (!after_init) after_init = true;
            }
        }
    }

    public void Init()
    {
        xNyuDebugMenu = GameObject.FindObjectOfType<xNyuDebug>();

        menu_open = true;
        Parent = this.gameObject;
        Data.Add("Name", Parent.name);
        Data.Add("Position", position.ToString());

        Collider2D collider = Parent.GetComponent<Collider2D>();
        if (collider != null)
        {
            var center = collider.bounds.center;
            var size = collider.bounds.extents;
            Data.Add("Center", center.ToString());
            Data.Add("Size", size.ToString());
        }

        StyleSmallRed = new GUIStyle();
        StyleSmallRed.normal.textColor = Color.white;

        StyleWhiteBox = new GUIStyle();
        StyleWhiteBox.normal.background = MakeTex(2, 2, new Color(1f, 1f, 1f, 1f));

        StyleBlackBox = new GUIStyle();
        StyleBlackBox.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.7f));

        StyleMagentaBox = new GUIStyle();
        StyleMagentaBox.normal.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.45f));

        width = (float)Screen.width;
        height = (float)Screen.height;
        scr_scale_w = width / fixed_size_width;
        scr_scale_h = height / fixed_size_height;
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

    //Display Settings
    public float fixed_size_width = 3840;
    public float fixed_size_height = 2160;

    public float width = 0;
    public float height = 0;
    public float scr_scale_w = 0;
    public float scr_scale_h = 0;

    public GUIStyle StyleSmallRed;
    public GUIStyle StyleWhiteBox;
    public GUIStyle StyleBlackBox;
    public GUIStyle StyleMagentaBox;

    public int frame_skip = 0;
    public Vector3 position = Vector3.zero;
    public GameObject Parent = null;
    public Dictionary<string, string> Data = new Dictionary<string, string>();

    public float locked_x = 0;
    public float locked_y = 0;
    public float locked_z = 0;

    public bool locked = false;
    public bool locked_init = false;

    public float box_width = 250f;

    public bool menu_open = false;
    public bool InspectorClicked = false;

    public bool after_init = false;

    public xNyuDebug xNyuDebugMenu = null;

}


