using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using System.Diagnostics;

public class xNyuDebug : MonoBehaviour
{
	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	public static extern bool AllocConsole();

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	public static extern bool FreeConsole();

	[DllImport("msvcrt.dll")]
	public static extern int system(string cmd);

	[DllImport("User32.dll")]
	public static extern short GetAsyncKeyState(int vKeys);

	[DllImport("user32.dll", EntryPoint = "MessageBox")]
	public static extern int ShowMessage(int hWnd, string text, string caption, uint type);



	public void OnGUI()
	{

		//Debug Menu is open
		if (DebugMenuActivated)
		{

			//Draw Black Background
			GUI.color = Color.black;
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.color = new Color(1f, 1f, 1f, 1f);

			if (!DebugMenuHotkey)
			{
				//Display Title
				GUI.Label(new Rect(1320 * scr_scale_w, 200 * scr_scale_h, 1500 * scr_scale_w, 800 * scr_scale_h), "Messenger Debugger v1.0", StyleTitle);
				GUI.Label(new Rect(1520 * scr_scale_w, 340 * scr_scale_h, 1500 * scr_scale_w, 800 * scr_scale_h), "Display values, call methods and more!", StyleAbout);
				GUI.Label(new Rect(1500 * scr_scale_w, 390 * scr_scale_h, 1500 * scr_scale_w, 800 * scr_scale_h), "Click on the options you want to activate", StyleAbout);
				GUI.Label(new Rect(3340 * scr_scale_w, 2080 * scr_scale_h, 2000 * scr_scale_w, 800 * scr_scale_h), "Developed by xNyu", StyleAbout);

				//Option Details
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h, 960 * scr_scale_w, 1300 * scr_scale_h), "Activate Details", (OptionDetailsActivated ? StyleNormalCyan : StyleNormalWhite));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 1) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Player Position", (OptionDetailsActivated ? (OptionDetailsToggles[0] ? StyleNormalGreen : StyleNormalRed) : StyleNormalGray));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 2) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Player Physics", (OptionDetailsActivated ? (OptionDetailsToggles[1] ? StyleNormalGreen : StyleNormalRed) : StyleNormalGray));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 3) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Player Data", (OptionDetailsActivated ? (OptionDetailsToggles[2] ? StyleNormalGreen : StyleNormalRed) : StyleNormalGray));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 4) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Player States", (OptionDetailsActivated ? (OptionDetailsToggles[3] ? StyleNormalGreen : StyleNormalRed) : StyleNormalGray));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 5) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Player Attacks", (OptionDetailsActivated ? (OptionDetailsToggles[4] ? StyleNormalGreen : StyleNormalRed) : StyleNormalGray));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 6) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Player MISC", (OptionDetailsActivated ? (OptionDetailsToggles[5] ? StyleNormalGreen : StyleNormalRed) : StyleNormalGray));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 7) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Camera Data", (OptionDetailsActivated ? (OptionDetailsToggles[6] ? StyleNormalGreen : StyleNormalRed) : StyleNormalGray));
				GUI.Label(new Rect(860 * scr_scale_w, 800 * scr_scale_h + ((50 * 8) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "World Data", (OptionDetailsActivated ? (OptionDetailsToggles[7] ? StyleNormalGreen : StyleNormalRed) : StyleNormalGray));

				//Option Modes
				GUI.Label(new Rect(1820 * scr_scale_w, 800 * scr_scale_h, 960 * scr_scale_w, 1300 * scr_scale_h), "Activate Modes", (OptionModesActivated ? StyleNormalCyan : StyleNormalWhite));
				GUI.Label(new Rect(1820 * scr_scale_w, 800 * scr_scale_h + ((50 * 1) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Inspector Mode", (OptionModesActivated ? (OptionModesToggles[0] ? StyleNormalGreen : StyleNormalRed) : StyleNormalGray));
				GUI.Label(new Rect(1820 * scr_scale_w, 800 * scr_scale_h + ((50 * 2) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "TAS Mode", (OptionModesActivated ? (OptionModesToggles[1] ? StyleNormalGreen : StyleNormalRed) : StyleNormalGray));

				//Option Hotkeys
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h, 960 * scr_scale_w, 40 * scr_scale_h), "Activate Hotkeys", (OptionHotkeysActivated ? StyleNormalCyan : StyleNormalWhite));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 1) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_1: " + OptionHotkeyToggles[0], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 2) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_2: " + OptionHotkeyToggles[1], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 3) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_3: " + OptionHotkeyToggles[2], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 4) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_4: " + OptionHotkeyToggles[3], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 5) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_5: " + OptionHotkeyToggles[4], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 6) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_6: " + OptionHotkeyToggles[5], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 7) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_7: " + OptionHotkeyToggles[6], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 8) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_8: " + OptionHotkeyToggles[7], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 9) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "NUM_9: " + OptionHotkeyToggles[8], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 10) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F1:    " + OptionHotkeyToggles[9], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 11) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F2:    " + OptionHotkeyToggles[10], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 12) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F3:    " + OptionHotkeyToggles[11], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 13) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F4:    " + OptionHotkeyToggles[12], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 14) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F5:    " + OptionHotkeyToggles[13], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 15) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F6:    " + OptionHotkeyToggles[14], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 16) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F7:    " + OptionHotkeyToggles[15], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 17) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "F8:    " + OptionHotkeyToggles[16], (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));
				GUI.Label(new Rect(2680 * scr_scale_w, 800 * scr_scale_h + ((50 * 19) * scr_scale_h), 960 * scr_scale_w, 40 * scr_scale_h), "Reload Hotkeysettings from file", (OptionHotkeysActivated ? StyleNormalPurple : StyleNormalGray));

				//Performance
				GUI.Box(new Rect(150f * scr_scale_w, 50f * scr_scale_h, 51f * scr_scale_w, 51f * scr_scale_h), "", StyleWhiteBox);
				if (xNyuPerformanceBoost) GUI.Label(new Rect(164f * scr_scale_w, 58f * scr_scale_h, 350f * scr_scale_w, 250f * scr_scale_h), "X", StyleNormalRed);
				GUI.Label(new Rect(214f * scr_scale_w, 58f * scr_scale_h, 350f * scr_scale_w, 250f * scr_scale_h), "Performance Boost", StyleNormalWhite);

			}
			else
			{
				//Hotkey Menu
				int d_row = 0;
				int d_line = 0;
				List<string> d_commands = new List<string>();

				d_commands = Funcs_Player;
				for (int i = 0; i < d_commands.Count; i++)
				{
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++; if (d_row == 4) { d_line++; d_row = 0; }

				d_commands = Funcs_Camera;
				for (int i = 0; i < d_commands.Count; i++)
				{
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++; if (d_row == 4) { d_line++; d_row = 0; }

				d_commands = Funcs_Game;
				for (int i = 0; i < d_commands.Count; i++)
				{
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++; if (d_row == 4) { d_line++; d_row = 0; }

				d_commands = Funcs_TAS;
				for (int i = 0; i < d_commands.Count; i++)
				{
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++; if (d_row == 4) { d_line++; d_row = 0; }

				d_commands = Funcs_Blank5;
				for (int i = 0; i < d_commands.Count; i++)
				{
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++; if (d_row == 4) { d_line++; d_row = 0; }

				d_commands = Funcs_Debug;
				for (int i = 0; i < d_commands.Count; i++)
				{
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++; if (d_row == 4) { d_line++; d_row = 0; }

				d_commands = Funcs_Special;
				for (int i = 0; i < d_commands.Count; i++)
				{
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++; if (d_row == 4) { d_line++; d_row = 0; }

				d_commands = Funcs_Scripts;
				for (int i = 0; i < d_commands.Count; i++)
				{
					GUI.Label(new Rect((300 + (d_row * 800)) * scr_scale_w, (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h), 500 * scr_scale_w, 30 * scr_scale_h), d_commands[i], StyleSmallYellow);
				}
				d_row++; if (d_row == 4) { d_line++; d_row = 0; }

			}

		}
		else
		{
            //Menu is Deactivated

            if (TASRecordScript)
            {
				//Draw scaled box background
				GUI.Box(new Rect(3300f * scr_scale_w, 1900f * scr_scale_h, 310f * scr_scale_w, 90f * scr_scale_h), "", StyleBoxBlack);
				GUI.Label(new Rect(3315f * scr_scale_w, 1920f * scr_scale_h, 300f * scr_scale_w, 100f * scr_scale_h), "TAS Recording", StyleNormalRed);
			}

			//Drawing details is activated
			if (OptionDetailsActivated)
			{

				float details_box_height = 25f;
				if (OptionDetailsData.Count > 24)
				{
					details_box_height = 25f;
				}
				else
				{
					details_box_height = (float)OptionDetailsData.Count;
				}
				float details_box_width = (float)Math.Floor(((float)OptionDetailsData.Count / 25f) - 0.0001f) + 1f;

				if (OptionDetailsData.Count > 0)
				{
					//Draw scaled box background
					GUI.color = new Color(1f, 1f, 1f, 0.85f);
					GUI.Box(new Rect(0f, 0f, 160f + ((500f * scr_scale_w) * details_box_width), 160f + ((40f * scr_scale_h) * details_box_height)), "");
					GUI.Box(new Rect(0f, 0f, 160f + ((500f * scr_scale_w) * details_box_width), 160f + ((40f * scr_scale_h) * details_box_height)), "");
					GUI.Box(new Rect(0f, 0f, 160f + ((500f * scr_scale_w) * details_box_width), 160f + ((40f * scr_scale_h) * details_box_height)), "");
					GUI.color = new Color(1f, 1f, 1f, 1f);

					//Draw details
					float row = 0f;
					float line = 0f;
					for (int i = 0; i < OptionDetailsData.Count; i++)
					{
						if (i > 0 && i % 25 == 0)
						{
							row = row + 1f;
							line = 0;
						}
						string color_tenery = OptionDetailsData[i];
						try { GUI.Label(new Rect(20f + ((500f * scr_scale_w) * row), 20f + ((40f * scr_scale_h) * line), 400f * scr_scale_w, 40f * scr_scale_h), OptionDetailsData[i], (color_tenery[0] == '-' && color_tenery[color_tenery.Length - 1] == '-') ? StyleSmallCyan : StyleSmallWhite); } catch { }
						line++;
					}

				}

			}

            //Inspector mode
            if (InspectorModeActive)
            {
				//if (InspectorRaycastHits.Count > 0) GUI.Label(new Rect(100f, 100f, 500f, 500f), "Hits: " + InspectorRaycastHits.Count.ToString(), StyleAbout);
				//if (InspectorRaycastHitsOld.Count > 0) GUI.Label(new Rect(100f, 200f, 500f, 500f), "Nyus: " + InspectorRaycastHitsOld.Count.ToString(), StyleAbout);

				//Checkbox 1
				GUI.Box(new Rect(2700f * scr_scale_w, 50f * scr_scale_h, 28f * scr_scale_w, 28f * scr_scale_h), "", StyleWhiteBox);
				if (InspectorCheckboxes[0]) GUI.Label(new Rect(2703f * scr_scale_w, 46f * scr_scale_h, 350f * scr_scale_w, 250f * scr_scale_h), "X", StyleSmallRed);
				GUI.Label(new Rect(2735f * scr_scale_w, 48f * scr_scale_h, 350f * scr_scale_w, 250f * scr_scale_h), "Hittable Objects", StyleSmallWhite);

				//Checkbox 2
				GUI.Box(new Rect(2700f * scr_scale_w, (50f * scr_scale_h) + ((35f * 1f) * scr_scale_h), 28f * scr_scale_w, 28f * scr_scale_h), "", StyleWhiteBox);
				if (InspectorCheckboxes[1]) GUI.Label(new Rect(2703f * scr_scale_w, (46f * scr_scale_h) + ((35f * 1f) * scr_scale_h), 350f * scr_scale_w, 250f * scr_scale_h), "X", StyleSmallRed);
				GUI.Label(new Rect(2735f * scr_scale_w, (48f * scr_scale_h) + ((35f * 1f) * scr_scale_h), 350f * scr_scale_w, 250f * scr_scale_h), "Environment", StyleSmallWhite);

				//Checkbox 3
				GUI.Box(new Rect(2700f * scr_scale_w, (50f * scr_scale_h) + ((35f * 2f) * scr_scale_h), 28f * scr_scale_w, 28f * scr_scale_h), "", StyleWhiteBox);
				if (InspectorCheckboxes[2]) GUI.Label(new Rect(2703f * scr_scale_w, (46f * scr_scale_h) + ((35f * 2f) * scr_scale_h), 350f * scr_scale_w, 250f * scr_scale_h), "X", StyleSmallRed);
				GUI.Label(new Rect(2735f * scr_scale_w, (48f * scr_scale_h) + ((35f * 2f) * scr_scale_h), 350f * scr_scale_w, 250f * scr_scale_h), "Zones", StyleSmallWhite);

				//Checkbox 4
				GUI.Box(new Rect(3000f * scr_scale_w, (50f * scr_scale_h) + ((35f * 2f) * scr_scale_h), 28f * scr_scale_w, 28f * scr_scale_h), "", StyleWhiteBox);
				if (InspectorMouseMode) GUI.Label(new Rect(3003f * scr_scale_w, (46f * scr_scale_h) + ((35f * 2f) * scr_scale_h), 350f * scr_scale_w, 250f * scr_scale_h), "X", StyleSmallRed);
				GUI.Label(new Rect(3035f * scr_scale_w, (48f * scr_scale_h) + ((35f * 2f) * scr_scale_h), 350f * scr_scale_w, 250f * scr_scale_h), "Mouse Inspection", StyleSmallWhite);

			}
		}

	}

	public void Update()
	{
        if (xNyuPerformanceBoost)
        {
			skip_1_frame_factor = 5;
			skip_2_frame_factor = 7;
			skip_3_frame_factor = 50;
		}
        else
        {
			skip_1_frame_factor = 3;
			skip_2_frame_factor = 4;
			skip_3_frame_factor = 35;
		}

		//Need to be Up-To-Date Aspect Ratio
		width = (float)Screen.width;
		height = (float)Screen.height;
		scr_scale_w = width / fixed_size_width;
		scr_scale_h = height / fixed_size_height;

		//Find Objects
		PlayerController player = null;
		Controller2D player_controller = null;
		SaveManager save_manager = null;

		try
		{
			//Find Objects
			player = GameObject.FindObjectOfType<PlayerController>();
			if (player != null) player_controller = player.Controller;
			save_manager = GameObject.FindObjectOfType<SaveManager>();
			//camera_main = Camera.main;
			//camera_retro = Camera.main.GetComponent<RetroCamera>();
		}
		catch (Exception e)
		{

		}

		//Activate Debug Menu
		if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.F10))
		{
			if (!DebugMenuHotkey)
			{
				if (DebugMenuActivated)
				{
					DebugMenuActivated = false;
					ResumeGame();
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;

				}
				else
				{
					DebugMenuActivated = true;
					SuspendGame();
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;

				}
			}
			else
			{
				DebugMenuHotkey = false;
			}

		}

		//If Debug Menu is active (Start Click Routine)
		if (DebugMenuActivated)
		{

			float mouse_x = Input.mousePosition.x;
			float mouse_y = height - Input.mousePosition.y;

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			//Mouse Click
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
			{

				if (!DebugMenuHotkey)
				{
					//Click Options Details
					if (mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h && mouse_y < 800 * scr_scale_h + ((50 * 1) * scr_scale_h)) { OptionDetailsActivated = !OptionDetailsActivated; }
					if (mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 1) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 2) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[0] = !OptionDetailsToggles[0];
					if (mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 2) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 3) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[1] = !OptionDetailsToggles[1];
					if (mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 3) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 4) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[2] = !OptionDetailsToggles[2];
					if (mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 4) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 5) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[3] = !OptionDetailsToggles[3];
					if (mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 5) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 6) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[4] = !OptionDetailsToggles[4];
					if (mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 6) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 7) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[5] = !OptionDetailsToggles[5];
					if (mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 7) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 8) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[6] = !OptionDetailsToggles[6];
					if (mouse_x > 860 * scr_scale_w && mouse_x < ((860 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 8) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 9) * scr_scale_h) && OptionDetailsActivated) OptionDetailsToggles[7] = !OptionDetailsToggles[7];

					//Click Modes
					if (mouse_x > 1820 * scr_scale_w && mouse_x < ((1820 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h && mouse_y < 800 * scr_scale_h + ((50 * 1) * scr_scale_h)) { OptionModesActivated = !OptionModesActivated; }
					if (mouse_x > 1820 * scr_scale_w && mouse_x < ((1820 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 1) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 2) * scr_scale_h) && OptionModesActivated) { OptionModesToggles[0] = !OptionModesToggles[0]; InspectorModeActive = OptionModesToggles[0]; }
					if (mouse_x > 1820 * scr_scale_w && mouse_x < ((1820 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 2) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 3) * scr_scale_h) && OptionModesActivated) { OptionModesToggles[1] = !OptionModesToggles[1]; TASModeActive = OptionModesToggles[1]; }

					//Click Hotkeys
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h && mouse_y < 800 * scr_scale_h + ((50 * 1) * scr_scale_h)) { OptionHotkeysActivated = !OptionHotkeysActivated; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 1) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 2) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 0; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 2) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 3) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 1; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 3) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 4) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 2; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 4) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 5) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 3; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 5) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 6) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 4; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 6) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 7) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 5; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 7) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 8) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 6; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 8) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 9) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 7; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 9) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 10) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 8; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 10) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 11) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 9; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 11) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 12) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 10; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 12) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 13) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 11; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 13) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 14) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 12; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 14) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 15) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 13; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 15) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 16) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 14; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 16) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 17) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 15; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 17) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 18) * scr_scale_h) && OptionHotkeysActivated) { OptionHotkeySlot = 16; LoadTASScripts(); DebugMenuHotkey = true; }
					if (mouse_x > 2680 * scr_scale_w && mouse_x < ((2680 * scr_scale_w) + 800 * scr_scale_w) && mouse_y > 800 * scr_scale_h + ((50 * 19) * scr_scale_h) && mouse_y < 800 * scr_scale_h + ((50 * 20) * scr_scale_h) && OptionHotkeysActivated)
					{
						//Reload Keysettings
						string[] key_settings_lines = File.ReadAllLines(settings_file);
						for (int i = 0; i < key_settings_lines.Length; i++)
						{
							OptionHotkeyToggles[i] = key_settings_lines[i].Split(':')[1];
						}
					}

					//Performance
					if (mouse_x > 150f * scr_scale_w && mouse_x < ((150f * scr_scale_w) + 300f * scr_scale_w) && mouse_y > 50f * scr_scale_h && mouse_y < 50f * scr_scale_h + ((50 * 1) * scr_scale_h)) { xNyuPerformanceBoost = !xNyuPerformanceBoost; }

				}
				else
				{
					//Hotkey Menu
					int d_row = 0;
					int d_line = 0;
					List<string> d_commands = new List<string>();

					d_commands = Funcs_Player;
					for (int i = 0; i < d_commands.Count; i++)
					{
						if (i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row + 1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i + 1)) * scr_scale_h)) { OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false; }
					}
					d_row++; if (d_row == 4) { d_line++; d_row = 0; }

					d_commands = Funcs_Camera;
					for (int i = 0; i < d_commands.Count; i++)
					{
						if (i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row + 1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i + 1)) * scr_scale_h)) { OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false; }
					}
					d_row++; if (d_row == 4) { d_line++; d_row = 0; }

					d_commands = Funcs_Game;
					for (int i = 0; i < d_commands.Count; i++)
					{
						if (i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row + 1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i + 1)) * scr_scale_h)) { OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false; }
					}
					d_row++; if (d_row == 4) { d_line++; d_row = 0; }

					d_commands = Funcs_TAS;
					for (int i = 0; i < d_commands.Count; i++)
					{
						if (i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row + 1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i + 1)) * scr_scale_h)) { OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false; }
					}
					d_row++; if (d_row == 4) { d_line++; d_row = 0; }

					d_commands = Funcs_Blank5;
					for (int i = 0; i < d_commands.Count; i++)
					{
						if (i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row + 1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i + 1)) * scr_scale_h)) { OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false; }
					}
					d_row++; if (d_row == 4) { d_line++; d_row = 0; }

					d_commands = Funcs_Debug;
					for (int i = 0; i < d_commands.Count; i++)
					{
						if (i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row + 1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i + 1)) * scr_scale_h)) { OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false; }
					}
					d_row++; if (d_row == 4) { d_line++; d_row = 0; }

					d_commands = Funcs_Special;
					for (int i = 0; i < d_commands.Count; i++)
					{
						if (i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row + 1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i + 1)) * scr_scale_h)) { OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false; }
					}
					d_row++; if (d_row == 4) { d_line++; d_row = 0; }

					d_commands = Funcs_Scripts;
					for (int i = 0; i < d_commands.Count; i++)
					{
						if (i > 0 && mouse_x > (300 + (d_row * 800)) * scr_scale_w && mouse_x < (300 + ((d_row + 1) * 800)) * scr_scale_w && mouse_y > (100 + (d_line * 1080)) * scr_scale_h + ((40 * i) * scr_scale_h) && mouse_y < (100 + (d_line * 1080)) * scr_scale_h + ((40 * (i + 1)) * scr_scale_h)) { OptionHotkeyToggles[OptionHotkeySlot] = d_commands[i]; DebugMenuHotkey = false; }
					}
					d_row++; if (d_row == 4) { d_line++; d_row = 0; }

					//Save new settings
					string[] key_settings_lines = new string[17];
					key_settings_lines[0] = "NUM_1:" + (OptionHotkeyToggles[0] != "" && OptionHotkeyToggles[0].Length > 4 ? OptionHotkeyToggles[0] : "None");
					key_settings_lines[1] = "NUM_2:" + (OptionHotkeyToggles[1] != "" && OptionHotkeyToggles[1].Length > 4 ? OptionHotkeyToggles[1] : "None");
					key_settings_lines[2] = "NUM_3:" + (OptionHotkeyToggles[2] != "" && OptionHotkeyToggles[2].Length > 4 ? OptionHotkeyToggles[2] : "None");
					key_settings_lines[3] = "NUM_4:" + (OptionHotkeyToggles[3] != "" && OptionHotkeyToggles[3].Length > 4 ? OptionHotkeyToggles[3] : "None");
					key_settings_lines[4] = "NUM_5:" + (OptionHotkeyToggles[4] != "" && OptionHotkeyToggles[4].Length > 4 ? OptionHotkeyToggles[4] : "None");
					key_settings_lines[5] = "NUM_6:" + (OptionHotkeyToggles[5] != "" && OptionHotkeyToggles[5].Length > 4 ? OptionHotkeyToggles[5] : "None");
					key_settings_lines[6] = "NUM_7:" + (OptionHotkeyToggles[6] != "" && OptionHotkeyToggles[6].Length > 4 ? OptionHotkeyToggles[6] : "None");
					key_settings_lines[7] = "NUM_8:" + (OptionHotkeyToggles[7] != "" && OptionHotkeyToggles[7].Length > 4 ? OptionHotkeyToggles[7] : "None");
					key_settings_lines[8] = "NUM_9:" + (OptionHotkeyToggles[8] != "" && OptionHotkeyToggles[8].Length > 4 ? OptionHotkeyToggles[8] : "None");
					key_settings_lines[9] = "F1:" + (OptionHotkeyToggles[9] != "" && OptionHotkeyToggles[9].Length > 4 ? OptionHotkeyToggles[9] : "None");
					key_settings_lines[10] = "F2:" + (OptionHotkeyToggles[10] != "" && OptionHotkeyToggles[10].Length > 4 ? OptionHotkeyToggles[10] : "None");
					key_settings_lines[11] = "F3:" + (OptionHotkeyToggles[11] != "" && OptionHotkeyToggles[11].Length > 4 ? OptionHotkeyToggles[11] : "None");
					key_settings_lines[12] = "F4:" + (OptionHotkeyToggles[12] != "" && OptionHotkeyToggles[12].Length > 4 ? OptionHotkeyToggles[12] : "None");
					key_settings_lines[13] = "F5:" + (OptionHotkeyToggles[13] != "" && OptionHotkeyToggles[13].Length > 4 ? OptionHotkeyToggles[13] : "None");
					key_settings_lines[14] = "F6:" + (OptionHotkeyToggles[14] != "" && OptionHotkeyToggles[14].Length > 4 ? OptionHotkeyToggles[14] : "None");
					key_settings_lines[15] = "F7:" + (OptionHotkeyToggles[15] != "" && OptionHotkeyToggles[15].Length > 4 ? OptionHotkeyToggles[15] : "None");
					key_settings_lines[16] = "F8:" + (OptionHotkeyToggles[16] != "" && OptionHotkeyToggles[16].Length > 4 ? OptionHotkeyToggles[16] : "None");
					File.WriteAllLines(settings_file, key_settings_lines);

					//Deactivate Menu, even if nothing is selected
					DebugMenuHotkey = false;

				}


			}






        }
        else
        {


			float mouse_x = Input.mousePosition.x;
			float mouse_y = height - Input.mousePosition.y;

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			//Mouse Click
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
			{
				//Inspector mode
				if (InspectorModeActive)
				{
					//Checkbox 1
					if (mouse_x > 2700f * scr_scale_w && mouse_x < ((2700f * scr_scale_w) + 150f * scr_scale_w) && mouse_y > 50f * scr_scale_h && mouse_y < (50f * scr_scale_h) + ((35f * 1) * scr_scale_h)) {
						InspectorCheckboxes[0] = !InspectorCheckboxes[0];
						if (!InspectorCheckboxes[0])
						{
							if (InspectorRaycastHitsOld.Count > 0)
							{
								List<int> to_remove = new List<int>();
								int offset = 0;
								for (int i = 0; i < InspectorRaycastHitsOld.Count; i++)
								{
									bool dest = false;

									if (InspectorRaycastHitsOld[i].gameObject.layer == 8 || InspectorRaycastHitsOld[i].gameObject.layer == 11)
									{
										dest = true;
									}

									if (dest)
									{
										Destroy(InspectorRaycastHitsOld[i]);
										to_remove.Add(i - offset);
										offset++;
									}
								}

								if (to_remove.Count > 0)
								{
									for (int i = 0; i < to_remove.Count; i++) InspectorRaycastHitsOld.RemoveAt(to_remove[i]);
								}
							}
						}

					}

					if (mouse_x > 2700f * scr_scale_w && mouse_x < ((2700f * scr_scale_w) + 150f * scr_scale_w) && mouse_y > (50f * scr_scale_h) + ((35f * 1) * scr_scale_h) && mouse_y < (50f * scr_scale_h) + ((35f * 2) * scr_scale_h)) {
						InspectorCheckboxes[1] = !InspectorCheckboxes[1];
						if (!InspectorCheckboxes[1])
						{
							if (InspectorRaycastHitsOld.Count > 0)
							{
								List<int> to_remove = new List<int>();
								int offset = 0;
								for (int i = 0; i < InspectorRaycastHitsOld.Count; i++)
								{
									bool dest = false;

									if (InspectorRaycastHitsOld[i].gameObject.layer == 9 || InspectorRaycastHitsOld[i].gameObject.layer == 10 || InspectorRaycastHitsOld[i].gameObject.layer == 12 || InspectorRaycastHitsOld[i].gameObject.layer == 21 || InspectorRaycastHitsOld[i].gameObject.layer == 22 || InspectorRaycastHitsOld[i].gameObject.layer == 23 || InspectorRaycastHitsOld[i].gameObject.layer == 24 || InspectorRaycastHitsOld[i].gameObject.layer == 25)
									{
										dest = true;
									}

									if (dest)
									{
										Destroy(InspectorRaycastHitsOld[i]);
										to_remove.Add(i - offset);
										offset++;
									}
								}

								if (to_remove.Count > 0)
								{
									for (int i = 0; i < to_remove.Count; i++) InspectorRaycastHitsOld.RemoveAt(to_remove[i]);
								}
							}
						}
					}

					if (mouse_x > 2700f * scr_scale_w && mouse_x < ((2700f * scr_scale_w) + 150f * scr_scale_w) && mouse_y > (50f * scr_scale_h) + ((35f * 2) * scr_scale_h) && mouse_y < (50f * scr_scale_h) + ((35f * 3) * scr_scale_h)) {
						InspectorCheckboxes[2] = !InspectorCheckboxes[2];
						if (!InspectorCheckboxes[2])
						{
							if (InspectorRaycastHitsOld.Count > 0)
							{
								List<int> to_remove = new List<int>();
								int offset = 0;
								for (int i = 0; i < InspectorRaycastHitsOld.Count; i++)
								{
									bool dest = false;

									if (InspectorRaycastHitsOld[i].gameObject.layer == 28 || InspectorRaycastHitsOld[i].gameObject.layer == 29 || InspectorRaycastHitsOld[i].gameObject.layer == 30 || InspectorRaycastHitsOld[i].gameObject.layer == 31)
									{
										dest = true;
									}

									if (dest)
									{
										Destroy(InspectorRaycastHitsOld[i]);
										to_remove.Add(i - offset);
										offset++;
									}
								}

								if (to_remove.Count > 0)
								{
									for (int i = 0; i < to_remove.Count; i++) InspectorRaycastHitsOld.RemoveAt(to_remove[i]);
								}
							}
						}
					}

					if (mouse_x > 3000f * scr_scale_w && mouse_x < ((3000f * scr_scale_w) + 150f * scr_scale_w) && mouse_y > (50f * scr_scale_h) + ((35f * 2) * scr_scale_h) && mouse_y < (50f * scr_scale_h) + ((35f * 3) * scr_scale_h))
					{
						InspectorMouseMode = !InspectorMouseMode;
						if (!InspectorMouseMode) InspectorMoveObject = null;
					}
				}
			}



			if (InspectorModeActive || TASModeActive)
            {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		//If Debug Menu isn't active (Start Routine)
		if (!DebugMenuActivated)
		{
			if (skip_3_frame == 0)
			{
				if (InspectorModeActive)
				{
					if (InspectorCheckboxes[0] || InspectorCheckboxes[1] || InspectorCheckboxes[2])
					{
						Vector2 Origin = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0)).y);
						Vector2 Size = new Vector2(Math.Abs(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x), Math.Abs(Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y));
						InspectorRaycastHits = Physics2D.BoxCastAll(Origin, Size, 0, Vector2.up).ToList<RaycastHit2D>();
						for (int i = 0; i < InspectorRaycastHits.Count; i++)
						{
							GameObject _Object = InspectorRaycastHits[i].transform.gameObject;

							if (_Object != null)
							{
								if (InspectorCheckboxes[0])
								{
									if (_Object.layer == 8 || _Object.layer == 11)
									{
										if (_Object.active)
										{
											xNyuHitbox ins = _Object.GetComponent<xNyuHitbox>();
											if (ins == null)
											{
												xNyuHitbox insn = _Object.AddComponent<xNyuHitbox>();
												insn.layer = 0;
												insn.Init();
												InspectorRaycastHitsOld.Add(insn);
											}
										}
									}
								}

								if (InspectorCheckboxes[1])
								{
									if (_Object.layer == 9 || _Object.layer == 10 || _Object.layer == 12 || _Object.layer == 21 || _Object.layer == 22 || _Object.layer == 23 || _Object.layer == 24 || _Object.layer == 25)
									{
										if (_Object.active)
										{
											xNyuHitbox ins = _Object.GetComponent<xNyuHitbox>();
											if (ins == null)
											{
												xNyuHitbox insn = _Object.AddComponent<xNyuHitbox>();
												insn.layer = 1;
												insn.Init();
												InspectorRaycastHitsOld.Add(insn);
											}
										}

									}
								}

								if (InspectorCheckboxes[2])
								{
									if (_Object.layer == 28 || _Object.layer == 29 || _Object.layer == 30 || _Object.layer == 31) //_Object.layer == 15
									{
										if (_Object.active)
										{
											xNyuHitbox ins = _Object.GetComponent<xNyuHitbox>();
											if (ins == null)
											{
												xNyuHitbox insn = _Object.AddComponent<xNyuHitbox>();
												insn.layer = 2;
												insn.Init();
												InspectorRaycastHitsOld.Add(insn);
											}
										}

									}
								}
							}
						}

						if (InspectorRaycastHits.Count > 0)
						{
							if (InspectorRaycastHitsOld.Count > 0)
							{
								List<int> to_remove = new List<int>();
								int offset = 0;
								for (int i = 0; i < InspectorRaycastHitsOld.Count; i++)
								{
									bool dest = true;

									if (InspectorRaycastHitsOld[i] != null)
									{
										if (InspectorRaycastHitsOld[i].gameObject.active)
										{
											foreach (RaycastHit2D hit in InspectorRaycastHits)
											{
												if (hit.transform.gameObject == InspectorRaycastHitsOld[i].gameObject)
												{
													dest = false;
													break;
												}
											}
										}
									}

									if (dest)
									{
										Destroy(InspectorRaycastHitsOld[i]);
										to_remove.Add(i - offset);
										offset++;
									}
								}

								if (to_remove.Count > 0)
								{
									for (int i = 0; i < to_remove.Count; i++) InspectorRaycastHitsOld.RemoveAt(to_remove[i]);
								}
							}
						}
					}
				}
			}

			if (skip_2_frame == 0)
            {
                //Inspector mode
                if (InspectorModeActive)
                {
					if (InspectorMouseMode)
					{
						if (Input.GetMouseButton(1))
						{
							if (!InspectorClicked)
							{
								RaycastHit2D hit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1.5f, Vector2.up, 0f);
								if (hit.collider != null)
								{
									if (hit.transform.gameObject.layer == 8 || hit.transform.gameObject.layer == 11)
									{
										if (hit.transform.gameObject.GetComponent<xNyuInspector>() == null)
										{
											hit.transform.gameObject.AddComponent<xNyuInspector>();
											hit.transform.gameObject.GetComponent<xNyuInspector>().Init();
										}
									}
								}
								InspectorClicked = true;
							}
						}
						else
						{
							InspectorClicked = false;
						}
					}
				}

				//Inspector mode
				if (InspectorModeActive)
				{
					if (InspectorMouseMode)
					{
						if (Input.GetMouseButton(0) && InspectorMoveObject == null)
						{
							RaycastHit2D hit = Physics2D.BoxCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.5f, 0.5f), 0f, Vector2.up);
							if (hit.collider != null)
							{
								xNyuInspector insp = hit.transform.gameObject.GetComponent<xNyuInspector>();
								if (insp != null)
								{
									float mouse_x = Input.mousePosition.x;
									float mouse_y = height - Input.mousePosition.y;
									Vector3 calc_pos = Camera.main.WorldToScreenPoint(insp.position);
									calc_pos.y = Screen.height - calc_pos.y;
									if (mouse_x > calc_pos.x + (10f * scr_scale_w) && mouse_x < calc_pos.x + (10f * scr_scale_w) + (180f * scr_scale_w) && mouse_y > calc_pos.y + ((25f * insp.Data.Count) * scr_scale_h) + (17f * scr_scale_h) && mouse_y < calc_pos.y + ((25f * insp.Data.Count) * scr_scale_h) + (42f * scr_scale_h))
									{

									}
									else
									{
										if (hit.transform.gameObject.layer == 8 || hit.transform.gameObject.layer == 11)
										{
											InspectorMoveObject = hit.transform.gameObject;
										}
									}
								}
								else
								{
									if (hit.transform.gameObject.layer == 8 || hit.transform.gameObject.layer == 11)
									{
										InspectorMoveObject = hit.transform.gameObject;
									}
								}
							}
						}
						else if (!Input.GetMouseButton(0))
						{
							InspectorMoveObject = null;
						}
					}

					if (InspectorMoveObject != null)
					{
						Vector3 calc_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						calc_pos.z = InspectorMoveObject.transform.position.z;
						xNyuInspector c_ins = InspectorMoveObject.GetComponent<xNyuInspector>();
						if (c_ins == null)
						{
							InspectorMoveObject.transform.position = calc_pos;
						}
						else
						{
							if (c_ins.locked)
							{
								c_ins.locked_x = calc_pos.x;
								c_ins.locked_y = calc_pos.y;
								c_ins.locked_z = calc_pos.z;
							}
							else
							{
								InspectorMoveObject.transform.position = calc_pos;
							}
						}
					}
				}

			}





			if (skip_1_frame == 0)
			{
				//Clear List
				if (OptionDetailsActivated)
				{
					if (OptionDetailsData.Count > 0) OptionDetailsData.Clear();

					if (OptionDetailsToggles[0])
					{
						//Player Coordinates
						try { OptionDetailsData.Add("-Player Position-"); } catch { }
						try { OptionDetailsData.Add("Position: " + player_controller.transform.position.ToString()); } catch { }
						try { OptionDetailsData.Add("Rotation: " + player_controller.transform.eulerAngles.ToString()); } catch { }
						try { OptionDetailsData.Add("Forward: " + player_controller.transform.forward.ToString()); } catch { }
					}

					if (OptionDetailsToggles[1])
					{
						//Player Physics
						try { OptionDetailsData.Add("-Player Physics-"); } catch { }
						try { OptionDetailsData.Add("Gravity: " + player.GetGravity(player.StateMachine.GetState<PlayerDefaultState>().jumpHeight, player.StateMachine.GetState<PlayerDefaultState>().timeToJumpApex).ToString()); } catch { }
						try { OptionDetailsData.Add("External Velocity: " + player.GetTotalExternalVelocity().ToString()); } catch { }
						try { OptionDetailsData.Add("Internal Velocity: " + player.GetTotalVelocity().ToString()); } catch { }
						try { OptionDetailsData.Add("Jump Velocity: " + player.JumpVelocity.ToString()); } catch { }
						try { OptionDetailsData.Add("Fall Distance: " + player.CurrentFallDistance.ToString()); } catch { }
						try { OptionDetailsData.Add("Max Fall Speed: " + player.MaxFallingSpeed.ToString()); } catch { }
						try { OptionDetailsData.Add("Run Speed Multiplier: " + player.RunSpeedMultiplier.ToString()); } catch { }
						try { OptionDetailsData.Add("Move X Multiplier: " + player.moveXMultiplier.ToString()); } catch { }
						try { OptionDetailsData.Add("Climb Up Speed: " + player.climbUpSpeed.ToString()); } catch { }
						try { OptionDetailsData.Add("Climb Down Speed: " + player.climbDownSpeed.ToString()); } catch { }
						try { OptionDetailsData.Add(string.Format("Remove Velocities: {0}", string.Join(",", player.velocitiesToRemove))); } catch { }
					}

					if (OptionDetailsToggles[2])
					{
						//Player Data
						try { OptionDetailsData.Add("-Player Data-"); } catch { }
						try { OptionDetailsData.Add("HP: " + player.CurrentHP.ToString() + "/" + player.GetMaxHP().ToString()); } catch { }
						//try { OptionDetailsData.Add("MP: " + player.CurrentFallDistance .ToString()); } catch { }
					}

					if (OptionDetailsToggles[3])
					{
						//Player Movement
						try { OptionDetailsData.Add("-Player States-"); } catch { }
						try { OptionDetailsData.Add("Has Dies: " + player.HasDied.ToString()); } catch { }
						try { OptionDetailsData.Add("Is Disoriented: " + player.IsDisoriented.ToString()); } catch { }
						try { OptionDetailsData.Add("Is Ducking: " + player.IsDucking.ToString()); } catch { }
						try { OptionDetailsData.Add("Is Knocked Back: " + player.IsKnockedBack.ToString()); } catch { }
						try { OptionDetailsData.Add("Is On Wall: " + player.IsOnWall.ToString()); } catch { }
						try { OptionDetailsData.Add("Is Running: " + player.IsRunning.ToString()); } catch { }
						try { OptionDetailsData.Add("Is Running: " + player.IsRunning.ToString()); } catch { }
						try { OptionDetailsData.Add("Jumped: " + player.Jumped.ToString()); } catch { }
						try { OptionDetailsData.Add("Paused: " + player.Paused.ToString()); } catch { }
						try { OptionDetailsData.Add("Running On Soft Ground: " + player.RunningOnSoftGround.ToString()); } catch { }
						try { OptionDetailsData.Add("Look Direction: " + player.LookDirection.ToString()); } catch { }
					}

					if (OptionDetailsToggles[4])
					{
						//Camera Position
						try { OptionDetailsData.Add("-Player Attacks-"); } catch { }
						try { OptionDetailsData.Add("Attack Cooldown Duration: " + player.attackCooldownDuration.ToString()); } catch { }
						try { OptionDetailsData.Add("Attack Combo Cooldown Duration: " + player.attackComboCountdownDuration.ToString()); } catch { }
						try { OptionDetailsData.Add("Windmill Charge Duration: " + player.windmillChargeDuration.ToString()); } catch { }
						try { OptionDetailsData.Add("Windmill Charge Reduction: " + player.windmillChargeDurationReductionPerUpgrade.ToString()); } catch { }
						try { OptionDetailsData.Add("Windmill Max: " + player.maxWindmill.ToString()); } catch { }
					}

					if (OptionDetailsToggles[5])
					{
						//Camera Position
						try { OptionDetailsData.Add("-Player MISC-"); } catch { }
						try { OptionDetailsData.Add("Last Frame Movement: " + player.LastFrameMovement.ToString()); } catch { }
						try { OptionDetailsData.Add("Last Hook Cloud Step Received: " + player.LastHookCloudStepReceived.ToString()); } catch { }
						try { OptionDetailsData.Add("Quicksand Suck Speed: " + player.quicksandSuckSpeed.ToString()); } catch { }
						try { OptionDetailsData.Add("Quicksand Fuck Up Jump: " + player.quicksandDeepnessToFuckupJump.ToString()); } catch { }

					}

					if (OptionDetailsToggles[6])
					{
						//Camera Data
						try { OptionDetailsData.Add("-Camera Data-"); } catch { }
						try { OptionDetailsData.Add("Position: " + Camera.main.transform.position.ToString()); } catch { }
						try { OptionDetailsData.Add("xNyu Camera: " + UnlockedCameraPosition.ToString()); } catch { }
						try { OptionDetailsData.Add("Euler Angles: " + Camera.main.transform.eulerAngles.ToString()); } catch { }
						try { OptionDetailsData.Add("Depth: " + Camera.main.depth.ToString()); } catch { }
						try { OptionDetailsData.Add("FOV: " + Camera.main.fov.ToString()); } catch { }
						try { OptionDetailsData.Add("Far Clip Pane: " + Camera.main.farClipPlane.ToString()); } catch { }
						try { OptionDetailsData.Add("Velocity: " + Camera.main.velocity.ToString()); } catch { }
					}

					if (OptionDetailsToggles[7])
					{
						//World Data
						try { OptionDetailsData.Add("-World Data-"); } catch { }

					}

				}
			}








            if (CameraUnlock)
            {
				//camera_retro.transform.position = CameraPosition;
            }









			//Keybindings Call Functions
			if (OptionHotkeysActivated)
			{
				if (Input.GetKey(KeyCode.Keypad1) && OptionHotkeyCanPress[0])
				{
					OptionHotkeyCanPress[0] = HotkeyToFunc(OptionHotkeyToggles[0]);
				}
				else if (Input.GetKey(KeyCode.Keypad2) && OptionHotkeyCanPress[1])
				{
					OptionHotkeyCanPress[1] = HotkeyToFunc(OptionHotkeyToggles[1]);
				}
				else if (Input.GetKey(KeyCode.Keypad3) && OptionHotkeyCanPress[2])
				{
					OptionHotkeyCanPress[2] = HotkeyToFunc(OptionHotkeyToggles[2]);
				}
				else if (Input.GetKey(KeyCode.Keypad4) && OptionHotkeyCanPress[3])
				{
					OptionHotkeyCanPress[3] = HotkeyToFunc(OptionHotkeyToggles[3]);
				}
				else if (Input.GetKey(KeyCode.Keypad5) && OptionHotkeyCanPress[4])
				{
					OptionHotkeyCanPress[4] = HotkeyToFunc(OptionHotkeyToggles[4]);
				}
				else if (Input.GetKey(KeyCode.Keypad6) && OptionHotkeyCanPress[5])
				{
					OptionHotkeyCanPress[5] = HotkeyToFunc(OptionHotkeyToggles[5]);
				}
				else if (Input.GetKey(KeyCode.Keypad7) && OptionHotkeyCanPress[6])
				{
					OptionHotkeyCanPress[6] = HotkeyToFunc(OptionHotkeyToggles[6]);
				}
				else if (Input.GetKey(KeyCode.Keypad8) && OptionHotkeyCanPress[7])
				{
					OptionHotkeyCanPress[7] = HotkeyToFunc(OptionHotkeyToggles[7]);
				}
				else if (Input.GetKey(KeyCode.Keypad9) && OptionHotkeyCanPress[8])
				{
					OptionHotkeyCanPress[8] = HotkeyToFunc(OptionHotkeyToggles[8]);
				}
				else if (Input.GetKey(KeyCode.F1) && OptionHotkeyCanPress[9])
				{
					OptionHotkeyCanPress[9] = HotkeyToFunc(OptionHotkeyToggles[9]);
				}
				else if (Input.GetKey(KeyCode.F2) && OptionHotkeyCanPress[10])
				{
					OptionHotkeyCanPress[10] = HotkeyToFunc(OptionHotkeyToggles[10]);
				}
				else if (Input.GetKey(KeyCode.F3) && OptionHotkeyCanPress[11])
				{
					OptionHotkeyCanPress[11] = HotkeyToFunc(OptionHotkeyToggles[11]);
				}
				else if (Input.GetKey(KeyCode.F4) && OptionHotkeyCanPress[12])
				{
					OptionHotkeyCanPress[12] = HotkeyToFunc(OptionHotkeyToggles[12]);
				}
				else if (Input.GetKey(KeyCode.F5) && OptionHotkeyCanPress[13])
				{
					OptionHotkeyCanPress[13] = HotkeyToFunc(OptionHotkeyToggles[13]);
				}
				else if (Input.GetKey(KeyCode.F6) && OptionHotkeyCanPress[14])
				{
					OptionHotkeyCanPress[14] = HotkeyToFunc(OptionHotkeyToggles[14]);
				}
				else if (Input.GetKey(KeyCode.F7) && OptionHotkeyCanPress[15])
				{
					OptionHotkeyCanPress[15] = HotkeyToFunc(OptionHotkeyToggles[15]);
				}
				else if (Input.GetKey(KeyCode.F8) && OptionHotkeyCanPress[16])
				{
					OptionHotkeyCanPress[16] = HotkeyToFunc(OptionHotkeyToggles[16]);
				}

				//Keybindings Release Detection
				if (Input.GetKeyUp(KeyCode.Keypad1))
				{
					OptionHotkeyCanPress[0] = true;
				}
				if (Input.GetKeyUp(KeyCode.Keypad2))
				{
					OptionHotkeyCanPress[1] = true;
				}
				if (Input.GetKeyUp(KeyCode.Keypad3))
				{
					OptionHotkeyCanPress[2] = true;
				}
				if (Input.GetKeyUp(KeyCode.Keypad4))
				{
					OptionHotkeyCanPress[3] = true;
				}
				if (Input.GetKeyUp(KeyCode.Keypad5))
				{
					OptionHotkeyCanPress[4] = true;
				}
				if (Input.GetKeyUp(KeyCode.Keypad6))
				{
					OptionHotkeyCanPress[5] = true;
				}
				if (Input.GetKeyUp(KeyCode.Keypad7))
				{
					OptionHotkeyCanPress[6] = true;
				}
				if (Input.GetKeyUp(KeyCode.Keypad8))
				{
					OptionHotkeyCanPress[7] = true;
				}
				if (Input.GetKeyUp(KeyCode.Keypad9))
				{
					OptionHotkeyCanPress[8] = true;
				}
				if (Input.GetKeyUp(KeyCode.F1))
				{
					OptionHotkeyCanPress[9] = true;
				}
				if (Input.GetKeyUp(KeyCode.F2))
				{
					OptionHotkeyCanPress[10] = true;
				}
				if (Input.GetKeyUp(KeyCode.F3))
				{
					OptionHotkeyCanPress[11] = true;
				}
				if (Input.GetKeyUp(KeyCode.F4))
				{
					OptionHotkeyCanPress[12] = true;
				}
				if (Input.GetKeyUp(KeyCode.F5))
				{
					OptionHotkeyCanPress[13] = true;
				}
				if (Input.GetKeyUp(KeyCode.F6))
				{
					OptionHotkeyCanPress[14] = true;
				}
				if (Input.GetKeyUp(KeyCode.F7))
				{
					OptionHotkeyCanPress[15] = true;
				}
				if (Input.GetKeyUp(KeyCode.F8))
				{
					OptionHotkeyCanPress[16] = true;
				}
			}












		}

		//Custom Scripts
		if (skip_2_frame == 0)
		{
			if (ScriptGodMode)
			{
				player.Heal(player.GetMaxHP());
			}
		}

		//Gather Prefabs
		if (skip_2_frame == 0)
        {
			if (!ObjectPrefabsHaveAll)
			{
				int AllDone = 0;
				List<GameObject> allObjects = (Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]).ToList<GameObject>();

				for (int i = 0; i < ObjectPrefabs.Count; i++)
				{
					if (ObjectPrefabs.ElementAt(i).Value == null)
					{
						GameObject _EnemyObject = allObjects.Find(obj => obj.name == ObjectPrefabs.ElementAt(i).Key);
						if (_EnemyObject != null)
						{
							_EnemyObject.SetActive(true);
							GameObject _ValueObject = UnityEngine.Object.Instantiate<GameObject>(_EnemyObject);
							_EnemyObject.SetActive(false);
							_ValueObject.SetActive(false);
							ObjectPrefabs[ObjectPrefabs.ElementAt(i).Key] = _ValueObject;
							AllDone++;
						}
                    }
                    else
                    {
						AllDone++;
                    }
				}

				if (AllDone >= ObjectPrefabs.Count) ObjectPrefabsHaveAll = true;
			}

		}










		if (TASRecordScript)
        {
            if (TASRecordScriptInit)
            {
				if (TASRecordedScript.Count > 0) TASRecordedScript.Clear();
				TASInputManager = GameObject.FindObjectOfType<InputManager>();
				TASRecordScriptInit = false;
			}

			if (TASRecordScriptDone)
			{
				if(TASRecordedScript.Count > 0)
                {
					if (TASRecordedScript.Count > 0)
					{
						List<string> TASRecordedScriptFormatted = new List<string>();
						string duplicate = "";
						int duplicate_amount = 0;
						for (int i = 0; i < TASRecordedScript.Count; i++)
                        {
							TASRecordedScriptFormatted.Add(TASRecordedScript[i]);
							/*
							if (i >= TASRecordedScript.Count - 1)
							{
								if (duplicate == "")
								{
									TASRecordedScriptFormatted.Add(TASRecordedScript[i]);
								}
								else
								{
									if (duplicate == TASRecordedScript[i])
									{
										duplicate_amount++;
										TASRecordedScriptFormatted.Add("repeat(" + duplicate_amount.ToString() + "){" + duplicate + "}");
									}
									else
									{
										if (duplicate_amount > 0)
										{
											TASRecordedScriptFormatted.Add("repeat(" + duplicate_amount.ToString() + "){" + duplicate + "}");
										}
										else
										{
											TASRecordedScriptFormatted.Add(duplicate);
										}
										TASRecordedScriptFormatted.Add(TASRecordedScript[i]);
									}
								}
							}
							else
							{
								if (duplicate == "")
								{
									duplicate = TASRecordedScript[i];
									duplicate_amount++;
								}
								else
								{
									if (duplicate == TASRecordedScript[i])
									{
										duplicate_amount++;
									}
									else
									{
										if (duplicate_amount > 0)
										{
											TASRecordedScriptFormatted.Add("repeat(" + duplicate_amount.ToString() + "){" + duplicate + "}");
										}
										else
										{
											TASRecordedScriptFormatted.Add(duplicate);
										}
										duplicate = TASRecordedScript[i];
										duplicate_amount = 0;
									}
								}
							}
							*/
						}

						string recorded_file = "TAS_Script_" + DateTime.Now.ToString("HH-mm-ss") + ".tmt";
						recorded_file = tas_dir + "\\" + recorded_file;
						File.WriteAllLines(recorded_file, TASRecordedScriptFormatted.ToArray());
					}
				}
				TASRecordScriptDone = false;
				TASRecordScript = false;
			}

			if (!TASRecordScriptInit && !TASRecordScriptDone)
            {
                if (TASInputManager == null)
                {
					TASInputManager = GameObject.FindObjectOfType<InputManager>();
				}
				if (TASInputManager == null)
                {
					WriteConsole("ERROR WHILE RECORDING: InputManager is null, add empty frame...");
					TASRecordedScript.Add("frame{ }");
				}
				else
                {
					string frame = "frame{ ";

					frame = frame + (TASInputManager.GetAttackDown() ? "AttackDown(); " : "");
					frame = frame + (TASInputManager.GetAttack() ? "Attack(); " : "");
					frame = frame + (TASInputManager.GetAttackUp() ? "AttackUp(); " : "");
					frame = frame + (TASInputManager.GetBackDown() ? "BackDown(); " : "");
					frame = frame + (TASInputManager.GetCancelDown() ? "CancelDown(); " : "");
					frame = frame + (TASInputManager.GetConfirmDown() ? "ConfirmDown(); " : "");
					frame = frame + (TASInputManager.GetConfirm() ? "Confirm(); " : "");
					frame = frame + (TASInputManager.GetRightDpadDown() ? "DRightDown(); " : "");
					frame = frame + (TASInputManager.GetLeftDpadDown() ? "DLeftDown(); " : "");
					frame = frame + (TASInputManager.GetUpDpadDown() ? "DUpDown(); " : "");
					frame = frame + (TASInputManager.GetDownDpadDown() ? "DDownDown(); " : "");
					frame = frame + (TASInputManager.GetEndingSpamDown() ? "EndingSpamDown(); " : "");
					frame = frame + (TASInputManager.GetEraseSaveFileDown() ? "EraseSaveFileDown(); " : "");
					frame = frame + (TASInputManager.GetEraseSaveFile() ? "EraseSaveFile(); " : "");
					frame = frame + (TASInputManager.GetGlide() ? "Glide(); " : "");
					frame = frame + (TASInputManager.GetGraplouDown() ? "GraplouDown(); " : "");
					frame = frame + (TASInputManager.GetGraplou() ? "Graplou(); " : "");

					if (TASInputManager.GetHorizontalInput() == 1f)
                    {
						frame = frame + "HInputRight(); ";
					}
					else if (TASInputManager.GetHorizontalInput() == -1f)
					{
						frame = frame + "HInputLeft(); ";
					}

					if (TASInputManager.GetVerticalInput() == 1f)
					{
						frame = frame + "VInputUp(); ";
					}
					else if (TASInputManager.GetVerticalInput() == -1f)
					{
						frame = frame + "VInputDown(); ";
					}

					frame = frame + (TASInputManager.GetInteractDown() ? "InteractDown(); " : "");
					frame = frame + (TASInputManager.GetInventoryDown() ? "InventoryDown(); " : "");

					frame = frame + (TASInputManager.GetJumpDown() ? "JumpDown(); " : "");
					frame = frame + (TASInputManager.GetJump() ? "Jump(); " : "");
					frame = frame + (TASInputManager.GetJumpUp() ? "JumpUp(); " : "");

					frame = frame + (TASInputManager.GetMapDown() ? "MapDown(); " : "");
					frame = frame + (TASInputManager.GetShurikenDown() ? "ShurikenDown(); " : "");
					frame = frame + (TASInputManager.GetShuriken() ? "Shuriken(); " : "");
					frame = frame + (TASInputManager.GetStartDown() ? "StartDown(); " : "");

					frame = frame + "}";
					TASRecordedScript.Add(frame);
				}
			}
        }






		//Font Settings
		if (skip_1_frame == 0)
		{
			int f_size_title = (int)(110f * scr_scale_w);
			int f_size_about = (int)(50f * scr_scale_w);
			int f_size_normal = (int)(40f * scr_scale_w);
			int f_size_small = (int)(30f * scr_scale_w);

			StyleTitle.fontSize = f_size_title;
			StyleAbout.fontSize = f_size_about;

			StyleNormalWhite.fontSize = f_size_normal;
			StyleNormalGray.fontSize = f_size_normal;
			StyleNormalRed.fontSize = f_size_normal;
			StyleNormalGreen.fontSize = f_size_normal;
			StyleNormalPurple.fontSize = f_size_normal;
			StyleNormalCyan.fontSize = f_size_normal;

			StyleSmallWhite.fontSize = f_size_small;
			StyleSmallGray.fontSize = f_size_small;
			StyleSmallRed.fontSize = f_size_small;
			StyleSmallGreen.fontSize = f_size_small;
			StyleSmallPurple.fontSize = f_size_small;
			StyleSmallCyan.fontSize = f_size_small;
			StyleSmallYellow.fontSize = f_size_small;
		}



		//Increase Performance Frameskip
		skip_1_frame++;
		skip_2_frame++;
		skip_3_frame++;
		if (skip_1_frame >= skip_1_frame_factor) skip_1_frame = 0;
		if (skip_2_frame >= skip_2_frame_factor) skip_2_frame = 0;
		if (skip_3_frame >= skip_3_frame_factor) skip_3_frame = 0;
	}



	public static void OpenConsole()
	{
		AllocConsole();
		Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
		Application.logMessageReceivedThreaded += (condition, stackTrace, type) => Console.WriteLine(condition + " " + stackTrace);
	}

	public static void ClearConsole()
	{
		system("CLS");
	}



	public bool HotkeyToFunc(string command)
	{
		//Set lowe case
		command = command.ToLower();

		//Allow Rapid Press
		bool allowPressMore = false;

		//Find Arguments
		List<string> hotkey_arguments = new List<string>();
		if (command[command.IndexOf("(") + 1] != ')')
		{
			string sub_arguments = command.Replace(command.Substring(0, command.IndexOf("(") + 1), "");
			sub_arguments = sub_arguments.Replace(")", "");
			string[] h_args = sub_arguments.Split(',');

			for (int i = 0; i < h_args.Length; i++)
			{
				hotkey_arguments.Add(h_args[i].Replace(" ", ""));
			}
		}

		//Find Objects
		PlayerController player = null;
		Controller2D player_controller = null;
		SaveManager save_manager = null;
		SaveLoadStandalone save_manager_native = null;
		Camera camera_main = null;
		RetroCamera camera_retro = null;

		try
		{
			//Find Objects
			player = GameObject.FindObjectOfType<PlayerController>();
			if (player != null) player_controller = player.Controller;
			save_manager = GameObject.FindObjectOfType<SaveManager>();
			save_manager_native = new SaveLoadStandalone();
			camera_main = Camera.main;
			camera_retro = Manager<Level>.Instance.RetroCamera;

		}
        catch (Exception e)
        {

        }

		












		try
		{
			if (command.Contains("player.changepositionx"))
			{
				Vector3 position = new Vector3(player_controller.transform.position.x + float.Parse(hotkey_arguments[0]), player_controller.transform.position.y, player_controller.transform.position.z);
				player_controller.transform.position = position;
				allowPressMore = true;
			}
			else if (command.Contains("player.changepositiony"))
			{
				Vector3 position = new Vector3(player_controller.transform.position.x, player_controller.transform.position.y + float.Parse(hotkey_arguments[0]), player_controller.transform.position.z);
				player_controller.transform.position = position;
				allowPressMore = true;
			}
			else if (command.Contains("player.changepositionz"))
			{
				Vector3 position = new Vector3(player_controller.transform.position.x, player_controller.transform.position.y, player_controller.transform.position.z + float.Parse(hotkey_arguments[0]));
				player_controller.transform.position = position;
				allowPressMore = true;
			}
			else if (command.Contains("player.setposition"))
			{
				Vector3 position = new Vector3(player_controller.transform.position.x + float.Parse(hotkey_arguments[0]), player_controller.transform.position.y + float.Parse(hotkey_arguments[1]), player_controller.transform.position.z + float.Parse(hotkey_arguments[2]));
				player_controller.transform.position = position;
				allowPressMore = true;
			}
			else if (command.Contains("player.activatewallcling"))
			{
				player.ActivateWallCling();
				allowPressMore = false;
			}
			else if (command.Contains("player.addscroll"))
			{
				if (hotkey_arguments.Count > 0)
				{
					player.AddScroll(bool.Parse(hotkey_arguments[0]));
				}
				allowPressMore = false;
			}
			else if (command.Contains("player.attack"))
			{
				player.Attack();
				allowPressMore = false;
			}
			else if (command.Contains("player.autoattachtowall"))
			{
				player.AutoAttachToWall(float.Parse(hotkey_arguments[0]));
				allowPressMore = false;
			}
			else if (command.Contains("player.beginflip"))
			{
				player.BeginFlip();
				allowPressMore = false;
			}
			else if (command.Contains("player.cancleattack"))
			{
				player.CancelAttack();
				allowPressMore = false;
			}
			else if (command.Contains("player.canclegraplou"))
			{
				if (hotkey_arguments.Count > 0)
				{
					player.CancelGraplou(bool.Parse(hotkey_arguments[0]));
				}
				allowPressMore = false;
			}
			else if (command.Contains("player.cancelinvincibilitycoroutine"))
			{
				player.CancelInvincibilityCoroutine();
				allowPressMore = false;
			}
			else if (command.Contains("player.canceljumpcoroutine"))
			{
				player.CancelJumpCoroutine();
				allowPressMore = false;
			}
			else if (command.Contains("player.cleanupexternalvelocities"))
			{
				player.CleanUpExternalVelocities();
				allowPressMore = false;
			}
			else if (command.Contains("player.clearholezones"))
			{
				player.ClearHoleZones();
				allowPressMore = false;
			}
			else if (command.Contains("player.climbdown"))
			{
				player.ClimbDown();
				allowPressMore = false;
			}
			else if (command.Contains("player.climbup"))
			{
				player.ClimbUp();
				allowPressMore = false;
			}
			else if (command.Contains("player.deactivatewallcling"))
			{
				player.DeactivateWallCling();
				allowPressMore = false;
			}
			else if (command.Contains("player.duck"))
			{
				player.Duck();
				allowPressMore = false;
			}
			else if (command.Contains("player.endflip"))
			{
				player.EndFlip();
				allowPressMore = false;
			}
			else if (command.Contains("player.enterholezone"))
			{
				player.EnterHoleZone();
				allowPressMore = false;
			}
			else if (command.Contains("player.leaveholezone"))
			{
				player.LeaveHoleZone();
				allowPressMore = false;
			}
			else if (command.Contains("player.forceidle"))
			{
				if (hotkey_arguments.Count > 0)
				{
					player.ForceIdle(bool.Parse(hotkey_arguments[0]));
				}
				allowPressMore = false;
			}
			else if (command.Contains("player.forceunblockinput"))
			{
				player.ForceUnblockInput();
				allowPressMore = false;
			}
			else if (command.Contains("player.giveairjump"))
			{
				player.GiveAirJump();
				allowPressMore = false;
			}
			else if (command.Contains("player.glideattack"))
			{
				player.GlideAttack();
				allowPressMore = false;
			}
			else if (command.Contains("player.ground"))
			{
				player.Ground();
				allowPressMore = false;
			}
			else if (command.Contains("player.hide"))
			{
				player.Hide();
				allowPressMore = false;
			}
			else if (command.Contains("player.show"))
			{
				player.Show(true);
				allowPressMore = false;
			}
			else if (command.Contains("player.hookattack"))
			{
				player.HookAttack();
				allowPressMore = false;
			}
			else if (command.Contains("player.receivehit"))
			{
				player.ReceiveHit(new HitData(1, 1f));
				allowPressMore = false;
			}
			else if (command.Contains("player.removeexternalvelocities"))
			{
				player.RemoveExternalVelocities();
				allowPressMore = false;
			}
			else if (command.Contains("player.removescroll"))
			{
				if (hotkey_arguments.Count > 0)
				{
					player.RemoveScroll(bool.Parse(hotkey_arguments[0]));
				}
				allowPressMore = false;
			}
			else if (command.Contains("player.resetboxcollider"))
			{
				player.ResetBoxCollider();
				allowPressMore = false;
			}
			else if (command.Contains("player.resetgravity"))
			{
				player.ResetGravity();
				allowPressMore = false;
			}
			else if (command.Contains("player.setlookdirection"))
			{
				player.SetLookDirection(int.Parse(hotkey_arguments[0]));
				allowPressMore = false;
			}
			else if (command.Contains("player.setvelocity"))
			{
				player.SetVelocity(float.Parse(hotkey_arguments[0]), float.Parse(hotkey_arguments[1]));
				allowPressMore = false;
			}
			else if (command.Contains("player.setwind"))
			{
				player.SetWind(float.Parse(hotkey_arguments[0]));
				allowPressMore = false;
			}
			else if (command.Contains("player.stopclimbing"))
			{
				player.StopClimbing();
				allowPressMore = false;
			}
			else if (command.Contains("player.stoprunning"))
			{
				player.StopRunning();
				allowPressMore = false;
			}
			else if (command.Contains("player.stopswimming"))
			{
				player.StopSwimming();
				allowPressMore = false;
			}
			else if (command.Contains("player.unlocklookdirection"))
			{
				player.UnlockLookDirection();
				allowPressMore = false;
			}
			else if (command.Contains("player.modifyhp"))
			{
				player.Heal(int.Parse(hotkey_arguments[0])); 
				allowPressMore = false;
			}
			else if (command.Contains("player.die"))
			{
				if(hotkey_arguments.Count > 0) player.Die((EDeathType) int.Parse(hotkey_arguments[0]), null);
				allowPressMore = false;
			}
			else if (command.Contains("player.jump"))
			{
				player.Jump();
				allowPressMore = true;
			}
			else if (command.Contains("player.freedialog"))
			{
                if (bool.Parse(hotkey_arguments[0]))
                {
					if (Manager<DialogManager>.Instance != null)
					{
						if (Manager<DialogManager>.Instance.GetDialogCharacter(ECharacter.THE_MESSENGER_NOSCROLL) != null) Manager<DialogManager>.Instance.UnregisterCharacter(ECharacter.THE_MESSENGER_NOSCROLL);
						if (Manager<DialogManager>.Instance.GetDialogCharacter(ECharacter.THE_MESSENGER) != null) Manager<DialogManager>.Instance.UnregisterCharacter(ECharacter.THE_MESSENGER);
						if (Manager<DialogManager>.Instance.GetDialogCharacter(ECharacter.PAST_MESSENGER_NOSCROLL) != null) Manager<DialogManager>.Instance.UnregisterCharacter(ECharacter.PAST_MESSENGER_NOSCROLL);
						if (Manager<DialogManager>.Instance.GetDialogCharacter(ECharacter.PAST_MESSENGER) != null) Manager<DialogManager>.Instance.UnregisterCharacter(ECharacter.PAST_MESSENGER);
					}
				}
                else
                {
					if (Manager<DialogManager>.Instance != null)
					{
						if (Manager<DialogManager>.Instance.GetDialogCharacter(ECharacter.THE_MESSENGER_NOSCROLL) != null) Manager<DialogManager>.Instance.RegisterCharacter(ECharacter.THE_MESSENGER_NOSCROLL, Manager<DialogManager>.Instance.GetDialogCharacter(ECharacter.THE_MESSENGER_NOSCROLL));
						if (Manager<DialogManager>.Instance.GetDialogCharacter(ECharacter.THE_MESSENGER) != null) Manager<DialogManager>.Instance.RegisterCharacter(ECharacter.THE_MESSENGER, Manager<DialogManager>.Instance.GetDialogCharacter(ECharacter.THE_MESSENGER));
						if (Manager<DialogManager>.Instance.GetDialogCharacter(ECharacter.PAST_MESSENGER_NOSCROLL) != null) Manager<DialogManager>.Instance.RegisterCharacter(ECharacter.PAST_MESSENGER_NOSCROLL, Manager<DialogManager>.Instance.GetDialogCharacter(ECharacter.PAST_MESSENGER_NOSCROLL));
						if (Manager<DialogManager>.Instance.GetDialogCharacter(ECharacter.PAST_MESSENGER) != null) Manager<DialogManager>.Instance.RegisterCharacter(ECharacter.PAST_MESSENGER, Manager<DialogManager>.Instance.GetDialogCharacter(ECharacter.PAST_MESSENGER));
					}
				}
				allowPressMore = false;
			}
			else if (command.Contains("player.additem"))
			{
				if(hotkey_arguments.Count > 1)
                {
					Manager<InventoryManager>.Instance.AddItem((EItems)int.Parse(hotkey_arguments[0]), int.Parse(hotkey_arguments[1]));
					if (int.Parse(hotkey_arguments[0]) == 0)
					{
						InGameHud view = Manager<UIManager>.Instance.GetView<InGameHud>();
						view.RefreshTimeshards();
					}
				}
				allowPressMore = false;
			}
			else if (command.Contains("player.removeitem"))
			{
				if (hotkey_arguments.Count > 1)
				{
					Manager<InventoryManager>.Instance.RemoveItem((EItems)int.Parse(hotkey_arguments[0]), int.Parse(hotkey_arguments[1]));
					if (int.Parse(hotkey_arguments[0]) == 0)
					{
						InGameHud view = Manager<UIManager>.Instance.GetView<InGameHud>();
						view.RefreshTimeshards();
					}
				}
				allowPressMore = false;
			}
			else if (command.Contains("game.pausegame"))
			{
				SuspendGame();
				allowPressMore = false;
			}
			else if (command.Contains("game.resumegame"))
			{
				ResumeGame();
				allowPressMore = false;
			}
			else if (command.Contains("game.loadscene"))
			{
				SceneLoader.LoadScene(hotkey_arguments[0], bool.Parse(hotkey_arguments[1]) ? UnityEngine.SceneManagement.LoadSceneMode.Additive : UnityEngine.SceneManagement.LoadSceneMode.Single, true);
				allowPressMore = false;
			}
			else if (command.Contains("game.savegame"))
			{
				save_manager_native = new SaveLoadStandalone();
				save_manager.SelectSaveGameSlot(int.Parse(hotkey_arguments[0]));
				save_manager.Save(false);
				save_manager_native.Save(save_manager.SaveFile);
				allowPressMore = false;
			}
			else if (command.Contains("game.loadgame"))
			{
				save_manager_native = new SaveLoadStandalone();
				save_manager.LoadSaveSlot(save_manager.GetSaveSlot(int.Parse(hotkey_arguments[0])));
				save_manager_native.Load();
				allowPressMore = false;
			}
			else if (command.Contains("game.newgame"))
			{
				save_manager.SelectSaveGameSlot(int.Parse(hotkey_arguments[0]));
				save_manager.NewGame();
				allowPressMore = false;
			}
			else if (command.Contains("game.selectlevel"))
			{
				if (hotkey_arguments.Count > 0) {
					int selectedLevel = int.Parse(hotkey_arguments[0]);
					int bits = 1;
					if (hotkey_arguments.Count > 1) bits = int.Parse(hotkey_arguments[0]);
					ELevel elevel = (ELevel)selectedLevel;
					string text = elevel.ToString();
					if (selectedLevel != 23)
					{
						text += "_Build";
					}

					AudioManager audio_manager = GameObject.FindObjectOfType<AudioManager>();
					if (audio_manager != null) audio_manager.StopMusic();
					LevelLoadingInfo levelLoadingInfo = new LevelLoadingInfo(text, true, true, (EBits)bits);
					levelLoadingInfo.showLevelIntro = true;
					levelLoadingInfo.positionPlayer = false;
					Manager<LevelManager>.Instance.LoadLevel(levelLoadingInfo);
				}
				allowPressMore = false;
			}
			else if (command.Contains("camera.changepositionx"))
			{
				UnlockedCameraPosition = camera_retro.transform.position;
				UnlockedCameraPosition.x += float.Parse(hotkey_arguments[0]);
				camera_retro.transform.position = UnlockedCameraPosition;
				allowPressMore = true;
			}
			else if (command.Contains("camera.changepositiony"))
			{
				UnlockedCameraPosition = camera_retro.transform.position;
				UnlockedCameraPosition.y += float.Parse(hotkey_arguments[0]);
				camera_retro.transform.position = UnlockedCameraPosition;
				allowPressMore = true;
			}
			else if (command.Contains("camera.changepositionz"))
			{
				UnlockedCameraPosition = camera_retro.transform.position;
				UnlockedCameraPosition.z += float.Parse(hotkey_arguments[0]);
				camera_retro.transform.position = UnlockedCameraPosition;
				allowPressMore = true;
			}
			else if (command.Contains("camera.setposition"))
			{
				UnlockedCameraPosition = new Vector3(float.Parse(hotkey_arguments[0]), float.Parse(hotkey_arguments[1]), float.Parse(hotkey_arguments[2]));
				camera_retro.transform.position = UnlockedCameraPosition;
				allowPressMore = true;
			}
			else if (command.Contains("camera.fov"))
			{
				Camera.main.fieldOfView = float.Parse(hotkey_arguments[0]);
				Camera.main.fov = float.Parse(hotkey_arguments[0]);
				allowPressMore = true;
			}
			else if (command.Contains("camera.unlock"))
			{
				CameraUnlocked = !CameraUnlocked;
                if (CameraUnlocked)
                {
					UnlockedCameraSizeInit = camera_retro.CameraSize;
					UnlockedCameraSize = 0f;
					UnlockedCameraPosition = camera_retro.transform.position;
                }
                else
                {
					camera_retro.CameraSize = UnlockedCameraSizeInit;
					camera_retro.DebugOffInit = true;
				}
				allowPressMore = false;
			}
			else if (command.Contains("camera.zoom"))
			{
				UnlockedCameraSize += float.Parse(hotkey_arguments[0]);
				allowPressMore = true;
			}
			else if (command.Contains("custom.godmode"))
			{
				bool to_set = !ScriptGodMode;
				if (hotkey_arguments.Count > 0) to_set = bool.Parse(hotkey_arguments[0]);
				ScriptGodMode = to_set;
				allowPressMore = false;
			}
			else if (command.Contains("game.enablecheatmenu"))
			{
				if (hotkey_arguments.Count > 0)
				{
					CheatButtonEnable = bool.Parse(hotkey_arguments[0]);
                }
                else
                {
					CheatButtonEnable = !CheatButtonEnable;

				}
				allowPressMore = false;
			}
			else if (command.Contains("game.showhud"))
			{
				if (hotkey_arguments.Count > 0)
				{
					InGameHud view = Manager<UIManager>.Instance.GetView<InGameHud>();
					if (bool.Parse(hotkey_arguments[0]))
                    {
						view.ShowHud();
                    }
                    else
                    {
						view.HideHud();
					}
				}
				allowPressMore = false;
			}
			else if (command.Contains("debug.dumpobjects"))
			{
				if (hotkey_arguments.Count > 0)
				{
					string end_string = "";
					if (hotkey_arguments.Count > 1) end_string = hotkey_arguments[1];
					List<string> stry = new List<string>();
					foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
					{
						stry.Add("------------------------------------");
						stry.Add("Name: " + go.name + end_string);
						stry.Add("ID: " + go.GetInstanceID().ToString() + end_string);
						stry.Add("Scene: " + go.scene.name + end_string);
						stry.Add("Tag: " + go.tag + end_string);
						stry.Add("Active: " + go.active.ToString() + end_string);
						stry.Add("IsNull: " + (go == null ? "true" : "false") + end_string);
					}
					string file = @hotkey_arguments[0];
					if (file.ToLower().Contains("%xnyudir%")) file = file.ToLower().Replace("%xnyudir%", Directory.GetCurrentDirectory() + @"\xNyuDebug");
					if (!Directory.Exists(file)) Directory.CreateDirectory(file);
					if (file[file.Length - 1] != '\\') file = file + "\\";
					file = file + "Object_Dump_" + DateTime.Now.ToString("mm-HH-ss") + ".txt";
					if (stry.Count > 0) File.WriteAllLines(file, stry.ToArray());
				}
				allowPressMore = false;
			}
			else if (command.Contains("debug.spawnenemy"))
			{
				if (hotkey_arguments.Count > 0)
				{
					string _enemy_name = hotkey_arguments[0];
					List<float> _SpawnParams = new List<float>();

					switch (_enemy_name)
					{
						case "turtleman":
							if (hotkey_arguments.Count > 1) _SpawnParams.Add(float.Parse(hotkey_arguments[1]));
							if (hotkey_arguments.Count > 2) _SpawnParams.Add(float.Parse(hotkey_arguments[2]));
							SpawnEnemy(_enemy_name, _SpawnParams.ToArray());
							break;
						case "bouncingdogo":
							if (hotkey_arguments.Count > 1) _SpawnParams.Add(float.Parse(hotkey_arguments[1]));
							if (hotkey_arguments.Count > 2) _SpawnParams.Add(float.Parse(hotkey_arguments[2]));
							if (hotkey_arguments.Count > 3) _SpawnParams.Add(float.Parse(hotkey_arguments[3]));
							if (hotkey_arguments.Count > 4) _SpawnParams.Add(float.Parse(hotkey_arguments[4]));
							if (hotkey_arguments.Count > 5) _SpawnParams.Add(float.Parse(hotkey_arguments[5]));
							SpawnEnemy(_enemy_name, _SpawnParams.ToArray());
							break;
						case "turtlemanranged":
							if (hotkey_arguments.Count > 1) _SpawnParams.Add(float.Parse(hotkey_arguments[1]));
							if (hotkey_arguments.Count > 2) _SpawnParams.Add(float.Parse(hotkey_arguments[2]));
							if (hotkey_arguments.Count > 3) _SpawnParams.Add(float.Parse(hotkey_arguments[2]));
							SpawnEnemy(_enemy_name, _SpawnParams.ToArray());
							break;
						case "skelouton":
							if (hotkey_arguments.Count > 1) _SpawnParams.Add(float.Parse(hotkey_arguments[1]));
							if (hotkey_arguments.Count > 2) _SpawnParams.Add(float.Parse(hotkey_arguments[2]));
							if (hotkey_arguments.Count > 3) _SpawnParams.Add(float.Parse(hotkey_arguments[3]));
							if (hotkey_arguments.Count > 4) _SpawnParams.Add(float.Parse(hotkey_arguments[4]));
							SpawnEnemy(_enemy_name, _SpawnParams.ToArray());
							break;
						case "birdy":
							if (hotkey_arguments.Count > 1) _SpawnParams.Add(float.Parse(hotkey_arguments[1]));
							if (hotkey_arguments.Count > 2) _SpawnParams.Add(float.Parse(hotkey_arguments[2]));
							SpawnEnemy(_enemy_name, _SpawnParams.ToArray());
							break;
					}
				}
				allowPressMore = false;
			}
			else if (command.Contains("tas.execute"))
			{
				if (GameObject.FindObjectOfType<xNyuTAS>() == null)
				{
					ScriptPath = "";
					if (hotkey_arguments.Count > 0)
					{
						ScriptPath = tas_dir + "\\" + hotkey_arguments[0];
                    }
                    else
                    {
						string[] _files = Directory.GetFiles(tas_dir);
						if (_files.Length > 0)
						{
							string _file_name = "";
							FileInfo _info = new FileInfo(_files[0]);
							DateTime _time = _info.LastWriteTime;
							_file_name = _files[0];
							if (_files.Length > 1) {
								for (int i = 1; i < _files.Length; i++)
								{
									FileInfo _iinfo = new FileInfo(_files[i]);
									if(_iinfo.LastWriteTime > _time)
                                    {
										_time = _iinfo.LastWriteTime;
										_file_name = _files[i];
									}
								}
							}
							ScriptPath = _file_name;
						}
					}
					if (ScriptPath != "")
					{
						var TASObject = new GameObject();
						xNyuTAS tas = TASObject.AddComponent<xNyuTAS>();
						GameObject.DontDestroyOnLoad(TASObject);
						TASObject.active = true;
						tas.Init();
					}
				}
				allowPressMore = false;
			}
			else if (command.Contains("tas.openconsole"))
			{
				OpenConsole();
				allowPressMore = false;
			}
			else if (command.Contains("tas.closeconsole"))
			{
				FreeConsole();
				allowPressMore = false;
			}
			else if (command.Contains("tas.clearconsole"))
			{
				ClearConsole();
				allowPressMore = false;
			}
			else if (command.Contains("tas.stopscript"))
			{
				if (TASRecordScript)
				{
					TASRecordScriptDone = true;
				}
				else
				{
					xNyuTAS gameObject = FindObjectOfType<xNyuTAS>();
					if (gameObject != null) Destroy(gameObject);
					allowPressMore = false;
				}
			}
			else if (command.Contains("tas.recordscript"))
			{
				if (!TASRecordScript && GameObject.FindObjectOfType<xNyuTAS>() == null)
				{
					TASRecordScriptDone = false;
					TASRecordScriptInit = true;
					TASRecordScript = true;
                }
                else
                {
					TASRecordScriptDone = true;
				}
				allowPressMore = false;
			}

		}
		catch (Exception e)
        {

        }







		return allowPressMore;

	}

	public void WriteConsole(object text)
	{
		try { Console.WriteLine(text); } catch { }
	}


	public void Start()
	{
		//First Init for Aspect Ratio
		width = (float)Screen.width;
		height = (float)Screen.height;
		scr_scale_w = width / fixed_size_width;
		scr_scale_h = height / fixed_size_height;

		//Set Font Styles
		StyleTitle = new GUIStyle();
		StyleTitle.normal.textColor = Color.blue;
		StyleAbout = new GUIStyle();
		StyleAbout.normal.textColor = Color.yellow;
		StyleNormalWhite = new GUIStyle();
		StyleNormalWhite.normal.textColor = Color.white;
		StyleNormalGray = new GUIStyle();
		StyleNormalGray.normal.textColor = Color.gray;
		StyleNormalRed = new GUIStyle();
		StyleNormalRed.normal.textColor = Color.red;
		StyleNormalGreen = new GUIStyle();
		StyleNormalGreen.normal.textColor = Color.green;
		StyleNormalPurple = new GUIStyle();
		StyleNormalPurple.normal.textColor = Color.magenta;
		StyleNormalCyan = new GUIStyle();
		StyleNormalCyan.normal.textColor = Color.cyan;
		StyleSmallWhite = new GUIStyle();
		StyleSmallWhite.normal.textColor = Color.white;
		StyleSmallGray = new GUIStyle();
		StyleSmallGray.normal.textColor = Color.gray;
		StyleSmallRed = new GUIStyle();
		StyleSmallRed.normal.textColor = Color.red;
		StyleSmallGreen = new GUIStyle();
		StyleSmallGreen.normal.textColor = Color.green;
		StyleSmallPurple = new GUIStyle();
		StyleSmallPurple.normal.textColor = Color.magenta;
		StyleSmallCyan = new GUIStyle();
		StyleSmallCyan.normal.textColor = Color.cyan;
		StyleSmallYellow = new GUIStyle();
		StyleSmallYellow.normal.textColor = Color.yellow;

		//Initialize Toggles
		for (int i = 0; i < OptionDetailsToggles.Length; i++) OptionDetailsToggles[i] = false;
		for (int i = 0; i < OptionModesToggles.Length; i++) OptionModesToggles[i] = false;
		for (int i = 0; i < OptionHotkeyToggles.Length; i++) OptionHotkeyToggles[i] = "";
		for (int i = 0; i < OptionHotkeyCanPress.Length; i++) OptionHotkeyCanPress[i] = true;

		//Set Box Styles
		StyleBoxBlack = new GUIStyle();
		StyleBoxBlack.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.7f));

		StyleWhiteBox = new GUIStyle();
		StyleWhiteBox.normal.background = MakeTex(2, 2, new Color(1f, 1f, 1f, 1f));




		//Directory and Settings create
		if (!Directory.Exists(tas_dir))
		{
			Directory.CreateDirectory(tas_dir);
			File.WriteAllLines(tas_dir + "\\TestScript.tmt", TestScriptSource.Split('#'));
		}
		if (!Directory.Exists(settings_dir)) Directory.CreateDirectory(settings_dir);
		if (!File.Exists(settings_file))
		{
			string[] lines = new string[17];

			lines[0] = "NUM_1:TAS.RecordScript()";
			lines[1] = "NUM_2:TAS.StopScript()";
			lines[2] = "NUM_3:TAS.Execute()";
			lines[3] = "NUM_4:TAS.OpenConsole()";
			lines[4] = "NUM_5:TAS.ClearConsole()";
			lines[5] = "NUM_6:TAS.CloseConsole()";
			lines[6] = "NUM_7:Camera.Zoom(-1)";
			lines[7] = "NUM_8:Camera.Unlock()";
			lines[8] = "NUM_9:Camera.Zoom(1)";
			lines[9] = "F1:Game.PauseGame()";
			lines[10] = "F2:Game.ResumeGame()";
			lines[11] = "F3:None";
			lines[12] = "F4:None";
			lines[13] = "F5:None";
			lines[14] = "F6:None";
			lines[15] = "F7:None";
			lines[16] = "F8:None";

			File.WriteAllLines(settings_file, lines);
		}

		string[] key_settings_lines = File.ReadAllLines(settings_file);

		//Read Key Settings
		for (int i = 0; i < key_settings_lines.Length; i++)
		{
			OptionHotkeyToggles[i] = key_settings_lines[i].Split(':')[1];
		}



		//Hotkey Functions List
		Funcs_Player.Add("-Player Functions-");
		Funcs_Player.Add("Player.ChangePositionX(10)");
		Funcs_Player.Add("Player.ChangePositionY(10)");
		Funcs_Player.Add("Player.ChangePositionZ(1)");
		Funcs_Player.Add("Player.SetPosition(0,0,0)");
		Funcs_Player.Add("Player.ActivateWallCling()");
		Funcs_Player.Add("Player.DeactivateWallCling()");
		Funcs_Player.Add("Player.AddScroll(false)");
		Funcs_Player.Add("Player.ModifyHP(1)");
		Funcs_Player.Add("Player.RemoveScroll(true)");
		Funcs_Player.Add("Player.BeginFlip(10)");
		Funcs_Player.Add("Player.CancelAttack()");
		Funcs_Player.Add("Player.CancelGraplou(true)");
		Funcs_Player.Add("Player.CancelInvincibilityCoroutine()");
		Funcs_Player.Add("Player.CancelJumpCoroutine()");
		Funcs_Player.Add("Player.CleanUpExternalVelocities()");
		Funcs_Player.Add("Player.ClimbDown()");
		Funcs_Player.Add("Player.ClimbUp()");
		Funcs_Player.Add("Player.StopClimbing()");
		Funcs_Player.Add("Player.StopRunning()");
		Funcs_Player.Add("Player.StopSwimming()");
		Funcs_Player.Add("Player.EndFlip()");
		Funcs_Player.Add("Player.Ground()");
		Funcs_Player.Add("Player.Duck()");
		Funcs_Player.Add("Player.EnterHoleZone()");
		Funcs_Player.Add("Player.LeaveHoleZone()");
		Funcs_Player.Add("Player.ClearHoleZones()");
		Funcs_Player.Add("Player.ForceIdle(true)");
		Funcs_Player.Add("Player.ForceUnblockInput()");
		Funcs_Player.Add("Player.GiveAirJump()");
		Funcs_Player.Add("Player.GlideAttack()");
		Funcs_Player.Add("Player.Hide()");
		Funcs_Player.Add("Player.Show()");
		Funcs_Player.Add("Player.HookAttack()");
		Funcs_Player.Add("Player.ReceiveHit()");
		Funcs_Player.Add("Player.RemoveExternalVelocities()");
		Funcs_Player.Add("Player.ResetBoxCollider()");
		Funcs_Player.Add("Player.ResetGravity()");
		Funcs_Player.Add("Player.SetLookDirection(1)");
		Funcs_Player.Add("Player.SetVelocity(5, 5)");
		Funcs_Player.Add("Player.SetWind(2)");
		Funcs_Player.Add("Player.UnlockLookDirection()");
		Funcs_Player.Add("Player.Die(1)");
		Funcs_Player.Add("Player.FreeDialog(true)");
		Funcs_Player.Add("Player.AddItem(0, 1000)");
		Funcs_Player.Add("Player.RemoveItem(0, 1000)");
		Funcs_Player.Add("Player.Jump()");

		Funcs_Camera.Add("-Camera Functions-");
		Funcs_Camera.Add("Camera.Unlock()");
		Funcs_Camera.Add("Camera.ChangePositionX(10)");
		Funcs_Camera.Add("Camera.ChangePositionY(10)");
		Funcs_Camera.Add("Camera.ChangePositionZ(1)");
		Funcs_Camera.Add("Camera.SetPosition(0,0,0)");
		Funcs_Camera.Add("Camera.Zoom(1)");
		Funcs_Camera.Add("Camera.FOV(55)");

		Funcs_Game.Add("-Game Functions-");
		Funcs_Game.Add("Game.PauseGame()");
		Funcs_Game.Add("Game.ResumeGame()");
		Funcs_Game.Add("Game.SaveGame(0)");
		Funcs_Game.Add("Game.LoadGame(0)");
		Funcs_Game.Add("Game.NewGame(0)");
		Funcs_Game.Add("Game.EnableCheatMenu(true)");
		Funcs_Game.Add("Game.ShowHUD(false)");
		Funcs_Game.Add("Game.SelectLevel(4)");

		Funcs_Debug.Add("-Debug Functions-");
		Funcs_Debug.Add("Debug.SpawnEnemy(0)");
		Funcs_Debug.Add(@"Debug.DumpObjects(%xNyuDir%\Dumps)");

		Funcs_Special.Add("-Special Functions-");
		Funcs_Special.Add("Special.LoadScene(Test_Scene,false)");

		Funcs_Scripts.Add("-Custom Scripts-");
		Funcs_Scripts.Add("Custom.Godmode()");



		//Add the Lists
		Key_functions_list.Add(Funcs_Player);
		Key_functions_list.Add(Funcs_Camera);
		Key_functions_list.Add(Funcs_Game);
		Key_functions_list.Add(Funcs_Blank5);
		Key_functions_list.Add(Funcs_Debug);
		Key_functions_list.Add(Funcs_Special);
		Key_functions_list.Add(Funcs_Scripts);








		ObjectPrefabs = new Dictionary<string, GameObject>();
		ObjectPrefabs.Add("TurtleMan", null);
		ObjectPrefabs.Add("BouncingDogo", null);
		ObjectPrefabs.Add("Skelouton", null);
		ObjectPrefabs.Add("Birdy", null);








	}


	public void SuspendGame()
	{
		PauseManager pause = GameObject.FindObjectOfType<PauseManager>();
		pause.Pause(true);
	}

	public void ResumeGame()
	{
		PauseManager pause = GameObject.FindObjectOfType<PauseManager>();
		pause.Resume();
	}

	public void LoadTASScripts()
	{
		if (Funcs_TAS.Count > 0) Funcs_TAS.Clear();

		Funcs_TAS.Add("-TAS Scripts-");
		Funcs_TAS.Add("TAS.OpenConsole()");
		Funcs_TAS.Add("TAS.CloseConsole()");
		Funcs_TAS.Add("TAS.ClearConsole()");
		Funcs_TAS.Add("TAS.StopScript()");
		Funcs_TAS.Add("TAS.RecordScript()");
		Funcs_TAS.Add("TAS.ExecuteScript()");

		string[] load_scripts = Directory.GetFiles(tas_dir);

		for (int i = 0; i < load_scripts.Length; i++)
		{
			string[] file_name = load_scripts[i].Split('\\');
			if (load_scripts[i].Contains(".tmt")) Funcs_TAS.Add("TAS.Execute(" + file_name[file_name.Length - 1] + ")");
		}
	}
























	public void SpawnEnemy(string enemy_name, float[] SpawnParams)
    {
		PlayerController player_controller = GameObject.FindObjectOfType<PlayerController>();

		Vector3 spawnPosition = Vector3.zero;
		SpawnableObjectParams paramsObject = null;
		Enemy spawnedObject = null;
		GameObject _EnemyObject = null;
		GameObject gameObject = null;

		if (SpawnParams.Length > 3)
		{
			spawnPosition = new Vector3(SpawnParams[0], SpawnParams[1], SpawnParams[2]);
		}
		else
		{
			spawnPosition = new Vector3(player_controller.transform.position.x, player_controller.transform.position.y, player_controller.transform.position.z);
		}

		switch (enemy_name)
		{
			case "turtleman":
				enemy_name = "TurtleMan";
				_EnemyObject = ObjectPrefabs[enemy_name];
				if (_EnemyObject != null)
				{
					_EnemyObject.SetActive(true);
					gameObject = UnityEngine.Object.Instantiate<GameObject>(_EnemyObject);
					_EnemyObject.SetActive(false);
					gameObject.name = enemy_name + "(Clone)";
					spawnedObject = gameObject.GetComponent<TurtleMan>();
					(spawnedObject as MonoBehaviour).transform.position = spawnPosition;
					if (SpawnParams.Length > 3)
					{
						paramsObject = new TurtleManParams() { startDirection = (EEnemyStartingDirection)SpawnParams[3], speed = SpawnParams[4], syncTransformOnMove = SpawnParams[5] > 0 ? true : false };
					}
					else
					{
						paramsObject = new TurtleManParams() { startDirection = (EEnemyStartingDirection)3, speed = 1f, syncTransformOnMove = true };
					}
					spawnedObject.OnSpawn(paramsObject);
					spawnedObject.StartBehaving();
                }
				break;
			case "bouncingdogo":
				enemy_name = "BouncingDogo";
				_EnemyObject = ObjectPrefabs[enemy_name];
				if (_EnemyObject != null)
				{
					_EnemyObject.SetActive(true); gameObject = UnityEngine.Object.Instantiate<GameObject>(_EnemyObject); _EnemyObject.SetActive(false);
					gameObject.name = enemy_name + "(Clone)";
					spawnedObject = gameObject.GetComponent<BouncingDogo>();
					(spawnedObject as MonoBehaviour).transform.position = spawnPosition;
					paramsObject = new BouncingDogoParams();
					if (SpawnParams.Length > 3)
					{
						paramsObject = new BouncingDogoParams() { startDirection = (EEnemyStartingDirection)SpawnParams[3], speed = SpawnParams[4], syncTransformOnMove = SpawnParams[5] > 0 ? true : false, jumpHeight = SpawnParams[6], timeToJumpApex = SpawnParams[7] };
					}
					else
					{
						paramsObject = new BouncingDogoParams() { startDirection = (EEnemyStartingDirection)3, speed = 3f, syncTransformOnMove = true, jumpHeight = 8f, timeToJumpApex = 2f };
					}
					spawnedObject.OnSpawn(paramsObject);
					spawnedObject.StartBehaving();
				}
				break;
			case "skelouton":
				enemy_name = "Skelouton";
				_EnemyObject = ObjectPrefabs[enemy_name];
				if (_EnemyObject != null)
				{
					_EnemyObject.SetActive(true); gameObject = UnityEngine.Object.Instantiate<GameObject>(_EnemyObject); _EnemyObject.SetActive(false);
					gameObject.name = enemy_name + "(Clone)";
					spawnedObject = gameObject.GetComponent<Skelouton>();
					(spawnedObject as MonoBehaviour).transform.position = spawnPosition;
					if (SpawnParams.Length > 3)
					{
						paramsObject = new SkeloutonParams() { startDirection = (EEnemyStartingDirection)SpawnParams[3], speed = SpawnParams[4], speedBoostMultiplier = SpawnParams[5], speedBoostRangeY = SpawnParams[6] };
					}
					else
					{
						paramsObject = new SkeloutonParams() { startDirection = (EEnemyStartingDirection)3, speed = 2f, speedBoostMultiplier = 2f, speedBoostRangeY = 2f };
					}
					spawnedObject.OnSpawn(paramsObject);
					spawnedObject.StartBehaving();
				}
				break;
			case "birdy":
				enemy_name = "Birdy";
				_EnemyObject = ObjectPrefabs[enemy_name];
				if (_EnemyObject != null)
				{
					_EnemyObject.SetActive(true); gameObject = UnityEngine.Object.Instantiate<GameObject>(_EnemyObject); _EnemyObject.SetActive(false);
					gameObject.name = enemy_name + "(Clone)";
					spawnedObject = gameObject.GetComponent<Birdy>();
					(spawnedObject as MonoBehaviour).transform.position = spawnPosition;
					if (SpawnParams.Length > 3)
					{
						paramsObject = new BirdyParams() { maxSpeed = SpawnParams[3], accelerationX = SpawnParams[4] };
					}
					else
					{
						paramsObject = new BirdyParams() { maxSpeed = 4f, accelerationX = 1.15f, activationZone = new EnemyActivationZone() };
					}
					spawnedObject.OnSpawn(paramsObject);
					spawnedObject.StartBehaving();
				}
				break;

		}

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





























	//Directory Settings
	public string settings_dir = Directory.GetCurrentDirectory() + @"\xNyuDebug";
	public string settings_file = Directory.GetCurrentDirectory() + @"\xNyuDebug\key_settings.txt";
	public string tas_dir = Directory.GetCurrentDirectory() + @"\xNyuDebug\TAS";

	//Menus
	public bool DebugMenuActivated = false;
	public bool DebugMenuHotkey = false;

	//Option Details
	public bool OptionDetailsActivated = false;
	public bool[] OptionDetailsToggles = new bool[10];
	public List<string> OptionDetailsData = new List<string>();

	//Option Details
	public bool OptionModesActivated = false;
	public bool[] OptionModesToggles = new bool[2];

	//Option Hotkeys
	public bool OptionHotkeysActivated = false;
	public string[] OptionHotkeyToggles = new string[17];
	public bool[] OptionHotkeyCanPress = new bool[17];
	public int OptionHotkeySlot = 0;
	public bool OptionHotkeyMenuActive = false;
	public List<List<string>> Key_functions_list = new List<List<string>>();
	public bool ExtraLook = false;
	public bool CameraUnlock = false;

	//Hotkey Lists
	List<string> Funcs_Player = new List<string>();
	List<string> Funcs_Camera = new List<string>();
	List<string> Funcs_Game = new List<string>();
	List<string> Funcs_TAS = new List<string>();
	List<string> Funcs_Blank5 = new List<string>();
	List<string> Funcs_Debug = new List<string>();
	List<string> Funcs_Special = new List<string>();
	List<string> Funcs_Scripts = new List<string>();

	//Custom Scripts
	public bool ScriptGodMode = false;
	public bool ScriptGodModeInit = false;
	public bool ScriptMaxSpeed = false;
	public bool ScriptMaxJump = false;

	//Styles
	public GUIStyle StyleBoxBlack;
	public GUIStyle StyleTitle;
	public GUIStyle StyleAbout;
	public GUIStyle StyleNormalWhite;
	public GUIStyle StyleNormalGray;
	public GUIStyle StyleNormalRed;
	public GUIStyle StyleNormalGreen;
	public GUIStyle StyleNormalPurple;
	public GUIStyle StyleNormalCyan;
	public GUIStyle StyleSmallWhite;
	public GUIStyle StyleSmallGray;
	public GUIStyle StyleSmallRed;
	public GUIStyle StyleSmallGreen;
	public GUIStyle StyleSmallPurple;
	public GUIStyle StyleSmallCyan;
	public GUIStyle StyleSmallYellow;

	public GUIStyle StyleWhiteBox;

	//Display Settings
	public float fixed_size_width = 3840;
	public float fixed_size_height = 2160;

	public float width = 0;
	public float height = 0;
	public float scr_scale_w = 0;
	public float scr_scale_h = 0;

	//Performance
	public int skip_1_frame = 0;
	public int skip_2_frame = 0;
	public int skip_3_frame = 0;
	public int skip_1_frame_factor = 0;
	public int skip_2_frame_factor = 0;
	public int skip_3_frame_factor = 0;
	public string debugstring = "";
	public bool debugbool = false;
	//TAS
	public string ScriptPath = "";
	public bool ConsoleActivate = false;

	public string TestScriptSource = "frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{VInputDown();}#frame{HInputLeft();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputLeft();}#frame{HInputLeft();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}#frame{JumpDown();}#frame{HInputRight();}#frame{HInputRight();}";

	public float[] CameraBoundsBackup = new float[4] { 0, 0, 0, 0 };
	public Vector3 CameraPosition = Vector3.zero;
	public bool CameraUnlocked = false;

	public RetroCamera camera_retro_old = null;
	public RetroCamera camera_retro_new = null;

	public bool CheatButtonEnable = false;

	public bool CheatLevelSelect = false;
	public int CheatLevelIndex = 0;

	public List<GameObject> ModEnemys;

	public Dictionary<string, GameObject> ObjectPrefabs;

	public bool ObjectPrefabsHaveAll = false;

	public bool InspectorModeActive = false;
	public bool InspectorClicked = false;
	public bool[] InspectorCheckboxes = new bool[3] { false, false, false };
	public bool InspectorMouseMode = false;

	public bool TASModeActive = false;

	public GameObject InspectorMoveObject = null;
	public List<xNyuHitbox> xNyuHitboxes = new List<xNyuHitbox>();

	public List<RaycastHit2D> InspectorRaycastHits = new List<RaycastHit2D>();
	public List<xNyuHitbox> InspectorRaycastHitsOld = new List<xNyuHitbox>();

	public bool xNyuPerformanceBoost = false;

	public Vector3 UnlockedCameraPosition = Vector3.zero;
	public float UnlockedCameraSize = 0f;
	public float UnlockedCameraSizeInit = 0f;

	public bool TASRecordScript = false;
	public bool TASRecordScriptInit = false;
	public bool TASRecordScriptDone = false;

	public List<string> TASRecordedScript = new List<string>();
	public InputManager TASInputManager = null;
}


