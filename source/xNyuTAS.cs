using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;

public class xNyuTAS : MonoBehaviour
{
	[DllImport("user32.dll", EntryPoint = "MessageBox")]
	public static extern int ShowMessage(int hWnd, string text, string caption, uint type);

	public void Update()
	{
		try
		{
			if (ScriptIteration >= ScriptLines.Count)
			{
				ScriptEnd = true;
			}

			if (!ScriptEnd)
			{
				//TAS Routine

				//Clean Key Checkers
				for (int i = 0; i < AllInputsCheck.Length; i++) AllInputsCheck[i] = false;
				StickL = Vector3.zero;
				StickR = Vector3.zero;
				if (ScriptLines[ScriptIteration].Contains("repeat("))
				{
					RepeatFrames = int.Parse(ScriptLines[ScriptIteration].Replace("repeat(", "").Split(')')[0]);
				}

				GetAttack = false;
				GetAttackDown = false;
				GetAttackUp = false;
				GetBack = false;
				GetBackDown = false;
				GetBackUp = false;
				GetCancel = false;
				GetCancelDown = false;
				GetCancelUp = false;
				GetConfirm = false;
				GetConfirmDown = false;
				GetConfirmUp = false;
				GetDPADRight = false;
				GetDPADLeft = false;
				GetDPADUp = false;
				GetDPADDown = false;
				GetEndingSpam = false;
				GetEndingSpamDown = false;
				GetEndingSpamUp = false;
				GetEraseSaveFile = false;
				GetEraseSaveFileDown = false;
				GetEraseSaveFileUp = false;
				GetGlide = false;
				GetGlideDown = false;
				GetGlideUp = false;
				GetGraplou = false;
				GetGraplouDown = false;
				GetGraplouUp = false;
				GetHorizontalInputLeft = false;
				GetHorizontalInputLeftDown = false;
				GetHorizontalInputLeftUp = false;
				GetHorizontalInputRight = false;
				GetHorizontalInputRightDown = false;
				GetHorizontalInputRightUp = false;
				GetVerticalInputUp = false;
				GetVerticalInputUpDown = false;
				GetVerticalInputUpUp = false;
				GetVerticalInputDown = false;
				GetVerticalInputDownDown = false;
				GetVerticalInputDownUp = false;
				Getbooleract = false;
				GetbooleractDown = false;
				GetbooleractUp = false;
				GetInventory = false;
				GetInventoryDown = false;
				GetInventoryUp = false;
				GetJump = false;
				GetJumpDown = false;
				GetJumpUp = false;
				GetMap = false;
				GetMapDown = false;
				GetMapUp = false;
				GetShuriken = false;
				GetShurikenDown = false;
				GetShurikenUp = false;
				GetStart = false;
				GetStartDown = false;
				GetStartUp = false;
				GetInteractDown = false;
				GetInteractUp = false;
				GetInteract = false;

				if (WaitFrames <= 0)
				{
					if (ScriptLines[ScriptIteration].Contains("frame{"))
					{
						string[] frame = ScriptLines[ScriptIteration].Replace("frame{", "").Replace("}", "").Split(';');

						if (frame.Length > 0)
						{
							foreach (string action in frame)
							{
								//Detect what should be pessed
								if (action == "attackdown()") //GetAttack
								{
									GetAttackDown = true;
								}
								else if (action == "attackup()")
								{
									GetAttackUp = true;
								}
								else if (action == "attack()")
								{
									GetAttack = true;
								}
								else if (action == "backdown()") //GetBack
								{
									GetBackDown = true;
								}
								else if (action == "backup()")
								{
									GetBackUp = true;
								}
								else if (action == "back()")
								{
									GetBack = true;
								}
								else if (action == "canceldown()") //GetCancel
								{
									GetCancelDown = true;
								}
								else if (action == "cancelup()")
								{
									GetCancelUp = true;
								}
								else if (action == "cancel()")
								{
									GetCancel = true;
								}
								else if (action == "confirmdown()") //GetConfirm
								{
									GetConfirmDown = true;
								}
								else if (action == "confirmup()")
								{
									GetConfirmUp = true;
								}
								else if (action == "confirm()")
								{
									GetConfirm = true;
								}
								else if (action == "drightdown()") //GetDPADRight
								{
									GetDPADRightDown = true;
								}
								else if (action == "drightup()")
								{
									GetDPADRightUp = true;
								}
								else if (action == "dright()")
								{
									GetDPADRight = true;
								}
								else if (action == "dleftdown()") //GetDPADLeft
								{
									GetDPADLeftDown = true;
								}
								else if (action == "dleftup()")
								{
									GetDPADLeftUp = true;
								}
								else if (action == "dleft()")
								{
									GetDPADLeft = true;
								}
								else if (action == "dupdown()") //GetDPADUp
								{
									GetDPADUpDown = true;
								}
								else if (action == "dupup()")
								{
									GetDPADUpUp = true;
								}
								else if (action == "dup()")
								{
									GetDPADUp = true;
								}
								else if (action == "ddowndown()") //GetDPADDown
								{
									GetDPADDownDown = true;
								}
								else if (action == "ddownup()")
								{
									GetDPADDownUp = true;
								}
								else if (action == "ddown()")
								{
									GetDPADDown = true;
								}
								else if (action == "endingspamdown()") //GetEndingSpam
								{
									GetEndingSpamDown = true;
								}
								else if (action == "endingspamup()")
								{
									GetEndingSpamUp = true;
								}
								else if (action == "endingspam()")
								{
									GetEndingSpam = true;
								}
								else if (action == "erasesavefiledown()") //GetEraseSaveFile
								{
									GetEraseSaveFileDown = true;
								}
								else if (action == "erasesavefileup()")
								{
									GetEraseSaveFileUp = true;
								}
								else if (action == "erasesavefile()")
								{
									GetEraseSaveFile = true;
								}
								else if (action == "glidedown()") //GetGlide
								{
									GetGlideDown = true;
								}
								else if (action == "glideup()")
								{
									GetGlideUp = true;
								}
								else if (action == "glide()")
								{
									GetGlide = true;
								}
								else if (action == "graploudown()") //GetGraplou
								{
									GetGraplouDown = true;
								}
								else if (action == "graplouup()")
								{
									GetGraplouUp = true;
								}
								else if (action == "graplou()")
								{
									GetGraplou = true;
								}
								else if (action == "hinputleftdown()") //GetHorizontalInputLeft
								{
									GetHorizontalInputLeftDown = true;
								}
								else if (action == "hinputleftup()")
								{
									GetHorizontalInputLeftUp = true;
								}
								else if (action == "hinputleft()")
								{
									GetHorizontalInputLeft = true;
								}
								else if (action == "hinputrightdown()") //GetHorizontalInputRight
								{
									GetHorizontalInputRightDown = true;
								}
								else if (action == "hinputrightup()")
								{
									GetHorizontalInputRightUp = true;
								}
								else if (action == "hinputright()")
								{
									GetHorizontalInputRight = true;
								}
								else if (action == "vinputdowndown()") //GetVerticalInputDown
								{
									GetVerticalInputDownDown = true;
								}
								else if (action == "vinputdownup()")
								{
									GetVerticalInputDownUp = true;
								}
								else if (action == "vinputdown()")
								{
									GetVerticalInputDown = true;
								}
								else if (action == "vinputupdown()") //GetVerticalInputUp
								{
									GetVerticalInputUpDown = true;
								}
								else if (action == "vinputupup()")
								{
									GetVerticalInputUpUp = true;
								}
								else if (action == "vinputup()")
								{
									GetVerticalInputUp = true;
								}
								else if (action == "interactdown()") //GetInteract
								{
									GetInteractDown = true;
								}
								else if (action == "interactup()")
								{
									GetInteractUp = true;
								}
								else if (action == "interact()")
								{
									GetInteract = true;
								}
								else if (action == "inventorydown()") //GetInventory
								{
									GetInventoryDown = true;
								}
								else if (action == "inventoryup()")
								{
									GetInventoryUp = true;
								}
								else if (action == "inventory()")
								{
									GetInventory = true;
								}
								else if (action == "jumpdown()") //GetJump
								{
									GetJumpDown = true;
								}
								else if (action == "jumpup()")
								{
									GetJumpUp = true;
								}
								else if (action == "jump()")
								{
									GetJump = true;
								}
								else if (action == "mapdown()") //GetMap
								{
									GetMapDown = true;
								}
								else if (action == "mapup()")
								{
									GetMapUp = true;
								}
								else if (action == "map()")
								{
									GetMap = true;
								}
								else if (action == "shurikendown()") //GetShuriken
								{
									GetShurikenDown = true;
								}
								else if (action == "shurikenup()")
								{
									GetShurikenUp = true;
								}
								else if (action == "shuriken()")
								{
									GetShuriken = true;
								}
								else if (action == "startdown()") //GetStart
								{
									GetStartDown = true;
								}
								else if (action == "startup()")
								{
									GetStartUp = true;
								}
								else if (action == "start()")
								{
									GetStart = true;
								}
							}
						}
					}
					else
					{
						//Wait
						WaitFramesToSet = int.Parse(ScriptLines[ScriptIteration].Replace("wait{", "").Replace("}", "")) - 1;
						WaitExtraFramesResult += WaitFramesToSet;
					}
				}

				//Debug Message
				if (ScriptLines.Count > 10)
				{
					if (ScriptIteration > 0 && ScriptIteration % ((int)Math.Round((float)ScriptLines.Count / 10f)) == 0)
					{
						ScriptProgress++;
						WriteConsole("TAS Progress: " + (ScriptProgress * 10).ToString() + "%");
					}
				}

				//Increase Iterator
				if (WaitFrames > 0)
				{
					WaitFrames--;
				}
				else
				{
					ScriptIteration++;
					WaitFrames = WaitFramesToSet;
					WaitFramesToSet = 0;
				}

			}
			else
			{
				GetAttack = false;
				GetAttackDown = false;
				GetAttackUp = false;
				GetBack = false;
				GetBackDown = false;
				GetBackUp = false;
				GetCancel = false;
				GetCancelDown = false;
				GetCancelUp = false;
				GetConfirm = false;
				GetConfirmDown = false;
				GetConfirmUp = false;
				GetDPADRight = false;
				GetDPADLeft = false;
				GetDPADUp = false;
				GetDPADDown = false;
				GetEndingSpam = false;
				GetEndingSpamDown = false;
				GetEndingSpamUp = false;
				GetEraseSaveFile = false;
				GetEraseSaveFileDown = false;
				GetEraseSaveFileUp = false;
				GetGlide = false;
				GetGlideDown = false;
				GetGlideUp = false;
				GetGraplou = false;
				GetGraplouDown = false;
				GetGraplouUp = false;
				GetHorizontalInputLeft = false;
				GetHorizontalInputLeftDown = false;
				GetHorizontalInputLeftUp = false;
				GetHorizontalInputRight = false;
				GetHorizontalInputRightDown = false;
				GetHorizontalInputRightUp = false;
				GetVerticalInputUp = false;
				GetVerticalInputUpDown = false;
				GetVerticalInputUpUp = false;
				GetVerticalInputDown = false;
				GetVerticalInputDownDown = false;
				GetVerticalInputDownUp = false;
				Getbooleract = false;
				GetbooleractDown = false;
				GetbooleractUp = false;
				GetInventory = false;
				GetInventoryDown = false;
				GetInventoryUp = false;
				GetJump = false;
				GetJumpDown = false;
				GetJumpUp = false;
				GetMap = false;
				GetMapDown = false;
				GetMapUp = false;
				GetShuriken = false;
				GetShurikenDown = false;
				GetShurikenUp = false;
				GetStart = false;
				GetStartDown = false;
				GetStartUp = false;
				GetInteractDown = false;
				GetInteractUp = false;
				GetInteract = false;

				//Script End
				WriteConsole("\n--- TAS finished ---\n");
				//WriteConsole("Duration: " + (DateTime.Now - ScriptStartTime).ToString("hh:mm:ss.FFF"));
				WriteConsole("Frames: " + (ScriptIteration + 1 + WaitExtraFramesResult).ToString() + "\n");
				Destroy(this.gameObject);
			}
		}
		catch(Exception e)
        {
			WriteConsole("!!! ERROR ORCCURED !!!");
			WriteConsole(e.Message);
			WriteConsole(e.StackTrace);
			Destroy(this.gameObject);
        }

	}

	public void Init()
    {
        try {
			WriteConsole("TAS.Execute() called");

			//ShowMessage(0, "111", "", 0);

			//Get Script Path from DebugMenu
			xNyuDebug DebugMenu = FindObjectOfType<xNyuDebug>();
			ScriptPath = DebugMenu.ScriptPath;

			//Read Lines from Script
			if (!File.Exists(ScriptPath))
			{
				WriteConsole("--- ERROR ---");
				WriteConsole("File " + @ScriptPath + " was not found!\n");
				Destroy(this.gameObject);
			}
			else
			{
				ScriptLines = File.ReadAllLines(ScriptPath).ToList<string>();

				List<string> ScriptLinesProcess = new List<string>();
				for (int i = 0; i < ScriptLines.Count; i++)
				{
					List<string> lines = new List<string>();
					string line = ScriptLines[i].ToLower().Replace(" ", "");
					if (line.Length > 4)
					{
						if (line.Contains("#"))
						{
							lines = line.Split('#').ToList<string>();
						}
						else
						{
							lines.Add(line);
						}
					}
					foreach(string s in lines)
					{
						if (s.Contains("repeat"))
						{
							int its = int.Parse(s.Replace("repeat(", "").Split(')')[0]);
							string payload = s.Substring(0, s.Length - 1).Replace("repeat(" + its.ToString() + "){", "");
							for (int k = 0; k < its; k++)
							{
								ScriptLinesProcess.Add(payload);
							}
						}
						else
						{
							ScriptLinesProcess.Add(s);
						}
					}
				}

				if (ScriptLines.Count > 0) ScriptLines.Clear();
				ScriptLines = ScriptLinesProcess;

				//Start Message
				WriteConsole("\n--- TAS START: " + ScriptPath.Split('\\')[ScriptPath.Split('\\').Length - 1] + " ---\n");
				WriteConsole(ScriptLines[0].Contains("wait") ? "Wait " + ScriptLines[0].Replace("wait{", "").Replace("}", "") + " frames before start..." : "");

				//Start Settings
				bool ScriptEnd = false;
				int ScriptIteration = 0;
				ScriptStartTime = DateTime.Now;
				ScriptProgress = 0;
				WaitFrames = 0;
				WaitFramesToSet = 0;
				WaitExtraFramesResult = 0;
				RepeatFrames = 0;

				AllInputsCheck = new bool[23];
			}
		}
		catch (Exception e)
		{
			WriteConsole("!!! ERROR ORCCURED !!!");
			WriteConsole(e.Message);
			WriteConsole(e.StackTrace);
			Destroy(this.gameObject);
		}

	}

	public void WriteConsole(object text)
	{
		try { Console.WriteLine(text); } catch { }
	}

	//Setings File

	//Settings TAS
	public string ScriptPath = "";
	public List<string> ScriptLines = new List<string>();
	public bool ScriptEnd = false;
	public bool[] AllInputsCheck;
	public int ScriptIteration = 0;
	public int ScriptProgress;
	public int RepeatFrames;
	public int WaitFrames;
	public int WaitFramesToSet;
	public int WaitExtraFramesResult;
	public DateTime ScriptStartTime;
	public Vector3 StickL;
	public Vector3 StickR;



	public bool GetAttack = false;
	public bool GetAttackDown = false;
	public bool GetAttackUp = false;
	public bool GetBack = false;
	public bool GetBackDown = false;
	public bool GetBackUp = false;
	public bool GetCancel = false;
	public bool GetCancelDown = false;
	public bool GetCancelUp = false;
	public bool GetConfirm = false;
	public bool GetConfirmDown = false;
	public bool GetConfirmUp = false;
	public bool GetDPADRight = false;
	public bool GetDPADRightUp = false;
	public bool GetDPADRightDown = false;
	public bool GetDPADLeftUp = false;
	public bool GetDPADLeft = false;
	public bool GetDPADLeftDown = false;
	public bool GetDPADUp = false;
	public bool GetDPADUpDown = false;
	public bool GetDPADUpUp = false;
	public bool GetDPADDown = false;
	public bool GetDPADDownDown = false;
	public bool GetDPADDownUp = false;
	public bool GetEndingSpam = false;
	public bool GetEndingSpamDown = false;
	public bool GetEndingSpamUp = false;
	public bool GetEraseSaveFile = false;
	public bool GetEraseSaveFileDown = false;
	public bool GetEraseSaveFileUp = false;
	public bool GetGlide = false;
	public bool GetGlideDown = false;
	public bool GetGlideUp = false;
	public bool GetGraplou = false;
	public bool GetGraplouDown = false;
	public bool GetGraplouUp = false;
	public bool GetHorizontalInputLeft = false;
	public bool GetHorizontalInputLeftDown = false;
	public bool GetHorizontalInputLeftUp = false;
	public bool GetHorizontalInputRight = false;
	public bool GetHorizontalInputRightDown = false;
	public bool GetHorizontalInputRightUp = false;
	public bool GetVerticalInputUp = false;
	public bool GetVerticalInputUpDown = false;
	public bool GetVerticalInputUpUp = false;
	public bool GetVerticalInputDown = false;
	public bool GetVerticalInputDownDown = false;
	public bool GetVerticalInputDownUp = false;
	public bool GetInteractDown = false;
	public bool GetInteractUp = false;
	public bool GetInteract = false;
	public bool Getbooleract = false;
	public bool GetbooleractDown = false;
	public bool GetbooleractUp = false;
	public bool GetInventory = false;
	public bool GetInventoryDown = false;
	public bool GetInventoryUp = false;
	public bool GetJump = false;
	public bool GetJumpDown = false;
	public bool GetJumpUp = false;
	public bool GetMap = false;
	public bool GetMapDown = false;
	public bool GetMapUp = false;
	public bool GetShuriken = false;
	public bool GetShurikenDown = false;
	public bool GetShurikenUp = false;
	public bool GetStart = false;
	public bool GetStartDown = false;
	public bool GetStartUp = false;

}


