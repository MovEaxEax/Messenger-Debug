using System;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;
using Rewired;
using UnityEngine;

// Token: 0x020008D4 RID: 2260
[AdvancedInspector(true, true)]
public class InputManager : Manager<global::InputManager>
{
	// Token: 0x06003CB1 RID: 15537 RVA: 0x00022504 File Offset: 0x00020704
	public InputManager()
	{
	}

	// Token: 0x1700097D RID: 2429
	// (get) Token: 0x06003CB2 RID: 15538 RVA: 0x00022530 File Offset: 0x00020730
	public float JumpTimeDown
	{
		get
		{
			return this.jumpTimeDown;
		}
	}

	// Token: 0x1700097E RID: 2430
	// (get) Token: 0x06003CB3 RID: 15539 RVA: 0x00022538 File Offset: 0x00020738
	public Player Player
	{
		get
		{
			return this.player;
		}
	}

	// Token: 0x1700097F RID: 2431
	// (get) Token: 0x06003CB4 RID: 15540 RVA: 0x00022540 File Offset: 0x00020740
	public bool Rumble
	{
		get
		{
			return this.rumble;
		}
	}

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x06003CB5 RID: 15541 RVA: 0x00022548 File Offset: 0x00020748
	public bool SeparateGraplouButton
	{
		get
		{
			return this.separateGraplouButton;
		}
	}

	// Token: 0x17000981 RID: 2433
	// (get) Token: 0x06003CB6 RID: 15542 RVA: 0x00022550 File Offset: 0x00020750
	public JoystickMap CurrentControllerMap
	{
		get
		{
			return this.currentControllerMap;
		}
	}

	// Token: 0x17000982 RID: 2434
	// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x00022558 File Offset: 0x00020758
	private Controller controller
	{
		get
		{
			if (this.player.controllers.GetLastActiveController() == null)
			{
				return this.lastConnectedController;
			}
			return this.player.controllers.GetLastActiveController();
		}
	}

	// Token: 0x17000983 RID: 2435
	// (get) Token: 0x06003CB8 RID: 15544 RVA: 0x0012FDEC File Offset: 0x0012DFEC
	private ControllerMap controllerMap
	{
		get
		{
			if (this.controller == null)
			{
				return null;
			}
			return this.player.controllers.maps.GetMap(this.controller.type, this.controller.id, "Default", "Default");
		}
	}

	// Token: 0x06003CB9 RID: 15545 RVA: 0x0000253E File Offset: 0x0000073E
	private void OnApplicationFocus(bool hasFocus)
	{
	}

	// Token: 0x06003CBA RID: 15546 RVA: 0x00022586 File Offset: 0x00020786
	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus && !Manager<GameManager>.Instance.IsCutscenePlaying() && !Manager<PauseManager>.Instance.IsPaused)
		{
			Manager<UIManager>.Instance.ShowView<PauseScreen>(EScreenLayers.PROMPT, null, true, AnimatorUpdateMode.Normal);
			Manager<PauseManager>.Instance.Pause(true);
		}
	}

	// Token: 0x06003CBB RID: 15547 RVA: 0x0012FE3C File Offset: 0x0012E03C
	private void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
		base.StartCoroutine(this.DisconnectectTimerCoroutine());
		ReInput.ControllerDisconnectedEvent += this.OnControllerDisconnected;
		ReInput.ControllerConnectedEvent += this.OnControllerConnected;
		ReInput.userDataStore.Load();
		base.StartCoroutine(this.GetCurrentJoystick());
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x0012FEA0 File Offset: 0x0012E0A0
	private IEnumerator GetCurrentJoystick()
	{
		while (this.player.controllers.joystickCount <= 0)
		{
			yield return null;
		}
		Controller controller = this.player.controllers.Joysticks[0];
		this.mappingId = this.player.controllers.maps.GetMap(controller, "Default", "Default").id;
		this.currentControllerMap = new JoystickMap(this.player.controllers.maps.GetMap(controller, "Default", "Default") as JoystickMap);
		yield break;
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x0000253E File Offset: 0x0000073E
	public void LoadMapping(List<ButtonMapping> controllerMapping)
	{
	}

	// Token: 0x06003CBE RID: 15550 RVA: 0x0012FEBC File Offset: 0x0012E0BC
	private IEnumerator LoadMappingCoroutine(List<ButtonMapping> controllerMapping)
	{
		while (this.player == null || this.player.controllers.joystickCount == 0)
		{
			yield return null;
		}
		if (controllerMapping == null)
		{
			yield break;
		}
		for (int i = 0; i < this.player.controllers.Joysticks.Count; i++)
		{
			Controller controller2 = this.player.controllers.Joysticks[i];
			ControllerMap map = this.player.controllers.maps.GetMap(controller2, "Default", "Default");
			List<ActionElementMap> list = new List<ActionElementMap>(map.AllMaps);
			if (list != null)
			{
				for (int j = list.Count - 1; j >= 0; j--)
				{
					ActionElementMap actionElementMap = list[j];
					ButtonMapping buttonMapping = null;
					for (int k = controllerMapping.Count - 1; k >= 0; k--)
					{
						if (controllerMapping[k].actionId == actionElementMap.actionId)
						{
							buttonMapping = controllerMapping[k];
							break;
						}
					}
					if (buttonMapping != null)
					{
						map.ReplaceOrCreateElementMap(new ElementAssignment(buttonMapping.elementIdentifierId, actionElementMap.actionId, false, actionElementMap.id)
						{
							type = ((buttonMapping.elementType != ControllerElementType.Button) ? ElementAssignmentType.FullAxis : ElementAssignmentType.Button)
						});
					}
				}
			}
		}
		Controller controller = this.player.controllers.Joysticks[0];
		JoystickMap controllerMap = this.player.controllers.maps.GetMap(controller, "Default", "Default") as JoystickMap;
		this.currentControllerMap = new JoystickMap(controllerMap);
		yield break;
	}

	// Token: 0x06003CBF RID: 15551 RVA: 0x000225C6 File Offset: 0x000207C6
	public void Shake()
	{
		this.Shake(1f, 1f, 0.25f);
	}

	// Token: 0x06003CC0 RID: 15552 RVA: 0x000225DD File Offset: 0x000207DD
	public void Shake(float duration)
	{
		this.Shake(1f, 1f, duration);
	}

	// Token: 0x06003CC1 RID: 15553 RVA: 0x0012FEE0 File Offset: 0x0012E0E0
	public void Shake(float leftMotorValue, float rightMotorValue, float duration)
	{
		foreach (Joystick joystick in this.player.controllers.Joysticks)
		{
			if (joystick.supportsVibration)
			{
				if (joystick.vibrationMotorCount > 0)
				{
					joystick.SetVibration(0, leftMotorValue, duration);
					joystick.SetVibration(1, rightMotorValue, duration);
				}
			}
		}
	}

	// Token: 0x06003CC2 RID: 15554 RVA: 0x0012FF6C File Offset: 0x0012E16C
	public float GetHorizontalInput(bool realInput = false)
	{
		if (this.blockAllInputs)
		{
			return 0f;
		}
		if(this.horizontalInputForced && !realInput)
		{
			return this.forcedHorizontalInput;
		}
		if(xNyuInstance == null){
			if (this.player.GetButton("Dpad Right"))
			{
				return 1f;
			}
			if (this.player.GetButton("Dpad Left"))
			{
				return -1f;
			}
			float axis = this.player.GetAxis("Move Horizontal");
			return (Mathf.Abs(axis) >= this.leftStickHorizontalDeadzone) ? axis : 0f;
		}
		else
		{
			if(xNyuInstance.GetHorizontalInputRight){
				if(xNyuInstance.GetHorizontalInputRight) return 1f;
			}
			if(xNyuInstance.GetHorizontalInputLeft){
				if(xNyuInstance.GetHorizontalInputLeft) return -1f;
			}
			return 0f;
		}
	}

	// Token: 0x06003CC3 RID: 15555 RVA: 0x000225F0 File Offset: 0x000207F0
	public void ForceHorizontalInput(float input)
	{
		this.forcedHorizontalInput = input;
		this.horizontalInputForced = true;
	}

	// Token: 0x06003CC4 RID: 15556 RVA: 0x00022600 File Offset: 0x00020800
	public void CancelForceHorizontalInput()
	{
		this.horizontalInputForced = false;
	}

	// Token: 0x06003CC5 RID: 15557 RVA: 0x00130008 File Offset: 0x0012E208
	public void Update()
	{
		if(xNyuInstance == null){
			xNyuInstance = GameObject.FindObjectOfType<xNyuTAS>();
		}

		if (this.GetJump(false))
		{
			this.jumpTimeDown += TimeVars.GetDeltaTime();
		}
		else if (this.GetJumpUp(false))
		{
			this.jumpTimeDown = 0f;
		}
		if (this.GetVerticalInput() > 0f)
		{
			if (!this.mLastInputUp)
			{
				this.mInputUpPressed = !this.mLastInputUp;
			}
			else
			{
				this.mInputUpPressed = false;
			}
			this.mLastInputUp = true;
		}
		else
		{
			this.mInputUpPressed = false;
			this.mLastInputUp = false;
		}
	}

	// Token: 0x06003CC6 RID: 15558 RVA: 0x00022609 File Offset: 0x00020809
	private void LateUpdate()
	{
		this.lastFrameHorizontalInput = this.GetHorizontalInput(false);
		this.lastFrameVerticalInput = this.GetVerticalInput();
	}

	// Token: 0x06003CC7 RID: 15559 RVA: 0x00022624 File Offset: 0x00020824
	public void CancelJumpTimeDown()
	{
		if (this.GetJump(false))
		{
			this.jumpTimeDown = float.MaxValue;
		}
	}

	// Token: 0x06003CC8 RID: 15560 RVA: 0x0002263D File Offset: 0x0002083D
	public bool GetRightDpadDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Dpad Right");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetDPADRightDown;
		}
	}

	// Token: 0x06003CC9 RID: 15561 RVA: 0x0002265C File Offset: 0x0002085C
	public bool GetLeftDpadDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Dpad Left");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetDPADLeftDown;
		}
	}

	// Token: 0x06003CCA RID: 15562 RVA: 0x0002267B File Offset: 0x0002087B
	public bool GetUpDpadDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Dpad Up");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetDPADUpDown;
		}
	}

	// Token: 0x06003CCB RID: 15563 RVA: 0x0002269A File Offset: 0x0002089A
	public bool GetDownDpadDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Dpad Down");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetDPADDownDown;
		}

	}

	// Token: 0x06003CCC RID: 15564 RVA: 0x000226B9 File Offset: 0x000208B9
	public bool GetAnyDpadDown()
	{
		return !this.blockAllInputs && (this.GetRightDpadDown() || this.GetLeftDpadDown() || this.GetDownDpadDown() || this.GetUpDpadDown());
	}

	// Token: 0x06003CCD RID: 15565 RVA: 0x000226F2 File Offset: 0x000208F2
	public bool GetHorizontalInputDown()
	{
		return !this.blockAllInputs && Mathf.Abs(this.lastFrameHorizontalInput) < 0.5f && Mathf.Abs(this.GetHorizontalInput(false)) >= 0.5f;
	}

	// Token: 0x06003CCE RID: 15566 RVA: 0x001300A0 File Offset: 0x0012E2A0
	public float GetVerticalInput()
	{
		if (this.blockAllInputs)
		{
			return 0f;
		}
		if(xNyuInstance == null){
			if (this.player.GetButton("Dpad Up"))
			{
				return 1f;
			}
			if (this.player.GetButton("Dpad Down"))
			{
				return -1f;
			}
			float axis = this.player.GetAxis("Move Vertical");
			return (Mathf.Abs(axis) >= this.leftStickVerticalDeadzone) ? axis : 0f;
		}
		else
		{
			if(xNyuInstance.GetVerticalInputUp){
				if(xNyuInstance.GetVerticalInputUp) return 1f;
			}
			if(xNyuInstance.GetVerticalInputDown){
				if(xNyuInstance.GetVerticalInputDown) return -1f;
			}
			return 0f;
		}
	}

	// Token: 0x06003CCF RID: 15567 RVA: 0x0002272F File Offset: 0x0002092F
	public bool InputUpPressed()
	{
		return !this.blockAllInputs && this.mInputUpPressed;
	}

	// Token: 0x06003CD0 RID: 15568 RVA: 0x00022744 File Offset: 0x00020944
	public bool InputUp()
	{
		return !this.blockAllInputs && this.GetVerticalInput() > 0f;
	}

	// Token: 0x06003CD1 RID: 15569 RVA: 0x00022760 File Offset: 0x00020960
	public bool InputRight()
	{
		return !this.blockAllInputs && this.GetHorizontalInput(false) > 0f;
	}

	// Token: 0x06003CD2 RID: 15570 RVA: 0x0002277D File Offset: 0x0002097D
	public bool InputDown()
	{
		return !this.blockAllInputs && this.GetVerticalInput() < 0f;
	}

	// Token: 0x06003CD3 RID: 15571 RVA: 0x00022799 File Offset: 0x00020999
	public bool InputLeft()
	{
		return !this.blockAllInputs && this.GetHorizontalInput(false) < 0f;
	}

	// Token: 0x06003CD4 RID: 15572 RVA: 0x000227B6 File Offset: 0x000209B6
	public bool InputLeftDown()
	{
		return !this.blockAllInputs && this.lastFrameHorizontalInput > -0.5f && this.GetHorizontalInput(false) <= -0.5f;
	}

	// Token: 0x06003CD5 RID: 15573 RVA: 0x000227E9 File Offset: 0x000209E9
	public bool InputRightDown()
	{
		return !this.blockAllInputs && this.lastFrameHorizontalInput < 0.5f && this.GetHorizontalInput(false) >= 0.5f;
	}

	// Token: 0x06003CD6 RID: 15574 RVA: 0x0002281C File Offset: 0x00020A1C
	public bool InputUpDown()
	{
		return !this.blockAllInputs && this.lastFrameVerticalInput < 0.5f && this.GetVerticalInput() >= 0.5f;
	}

	// Token: 0x06003CD7 RID: 15575 RVA: 0x0002284E File Offset: 0x00020A4E
	public bool InputDownDown()
	{
		return !this.blockAllInputs && this.lastFrameVerticalInput > -0.5f && this.GetVerticalInput() <= -0.5f;
	}

	// Token: 0x06003CD8 RID: 15576 RVA: 0x00022880 File Offset: 0x00020A80
	public bool GetAttack()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButton("Attack");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetAttack;
		}
	}

	// Token: 0x06003CD9 RID: 15577
	public bool GetAttackUp()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonUp("Attack");
		}
		else
		{
			return xNyuInstance.GetAttackUp;
		}
	}

	// Token: 0x06003CDA RID: 15578 RVA: 0x000228BE File Offset: 0x00020ABE
	public bool GetAttackDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Attack");
		}
		else
		{
			return xNyuInstance.GetAttackDown;
		}
	}

	// Token: 0x06003CDB RID: 15579 RVA: 0x00022880 File Offset: 0x00020A80
	public bool GetEraseSaveFile()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButton("Attack");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetEraseSaveFile;
		}
	}

	// Token: 0x06003CDC RID: 15580 RVA: 0x000228BE File Offset: 0x00020ABE
	public bool GetEraseSaveFileDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Attack");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetEraseSaveFileDown;
		}

	}

	// Token: 0x06003CDD RID: 15581 RVA: 0x000228DD File Offset: 0x00020ADD
	public bool GetBackDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Back");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetBackDown;
		}
	}

	// Token: 0x06003CDE RID: 15582 RVA: 0x000228FC File Offset: 0x00020AFC
	public bool GetShuriken()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButton("Shuriken");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetShuriken;
		}
	}

	// Token: 0x06003CDF RID: 15583 RVA: 0x0002291B File Offset: 0x00020B1B
	public bool GetShurikenDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Shuriken");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetShurikenDown;
		}
	}

	// Token: 0x06003CE0 RID: 15584 RVA: 0x0002293A File Offset: 0x00020B3A
	public bool GetInventoryDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Inventory");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetInventoryDown;
		}
	}

	// Token: 0x06003CE1 RID: 15585 RVA: 0x00022959 File Offset: 0x00020B59
	public bool GetMapDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Map");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetMapDown;
		}
	}

	// Token: 0x06003CE2 RID: 15586 RVA: 0x00022978 File Offset: 0x00020B78
	public bool GetJump(bool overrideBlockedInput = false)
	{
		if(xNyuInstance == null){
			return (!this.blockAllInputs || overrideBlockedInput) && this.player.GetButton("Jump");
		}
		else
		{
			return (!this.blockAllInputs || overrideBlockedInput) && xNyuInstance.GetJump;
		}
	}

	// Token: 0x06003CE3 RID: 15587 RVA: 0x0002299D File Offset: 0x00020B9D
	public bool GetJumpUp(bool overrideBlockedInput = false)
	{
		if(xNyuInstance == null){
			return (!this.blockAllInputs || overrideBlockedInput) && this.player.GetButtonUp("Jump");
		}
		else
		{
			return (!this.blockAllInputs || overrideBlockedInput) && xNyuInstance.GetJumpUp;
		}
	}

	// Token: 0x06003CE4 RID: 15588 RVA: 0x000229C2 File Offset: 0x00020BC2
	public bool GetJumpDown(bool overrideBlockedInput = false)
	{
		if(xNyuInstance == null){
			return (!this.blockAllInputs || overrideBlockedInput) && this.player.GetButtonDown("Jump");
		}
		else
		{
			return (!this.blockAllInputs || overrideBlockedInput) && xNyuInstance.GetJumpDown;
		}
	}

	// Token: 0x06003CE5 RID: 15589 RVA: 0x000229E7 File Offset: 0x00020BE7
	public bool GetConfirmDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Confirm");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetConfirmDown;
		}
	}

	// Token: 0x06003CE6 RID: 15590 RVA: 0x00022A06 File Offset: 0x00020C06
	public bool GetConfirm()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButton("Confirm");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetConfirm;
		}
	}

	// Token: 0x06003CE7 RID: 15591 RVA: 0x00022A25 File Offset: 0x00020C25
	public bool GetStartDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Start");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetStartDown;
		}

	}

	// Token: 0x06003CE8 RID: 15592 RVA: 0x00022A44 File Offset: 0x00020C44
	public void SetRumble(bool rumble)
	{
		this.rumble = rumble;
	}

	// Token: 0x06003CE9 RID: 15593 RVA: 0x00022A4D File Offset: 0x00020C4D
	public void SetSeparateGraplouButton(bool separateGraplouButton)
	{
		this.separateGraplouButton = separateGraplouButton;
	}

	// Token: 0x06003CEA RID: 15594 RVA: 0x00022A56 File Offset: 0x00020C56
	public bool GetInteractDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Jump");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetInteractDown;
		}
	}

	// Token: 0x06003CEB RID: 15595 RVA: 0x000228BE File Offset: 0x00020ABE
	public bool GetCancelDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Attack");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetCancelDown;
		}
	}

	// Token: 0x06003CEC RID: 15596 RVA: 0x00022A75 File Offset: 0x00020C75
	public bool GetRightTrigger()
	{
		return !this.blockAllInputs && this.player.GetAxis("MagicBoots") > 0.2f;
	}

	// Token: 0x06003CED RID: 15597 RVA: 0x00022A9B File Offset: 0x00020C9B
	public bool GetGraplou()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButton("Graplou");
		}
		else
		{
			return xNyuInstance.GetGraplou;
		}
	}

	// Token: 0x06003CEE RID: 15598 RVA: 0x00022ABA File Offset: 0x00020CBA
	public bool GetGlide()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetAxis("Glide") > 0.2f;
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetGlide;
		}
	}

	// Token: 0x06003CEF RID: 15599 RVA: 0x00022AE0 File Offset: 0x00020CE0
	public bool GetGraplouDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("Graplou");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetGraplouDown;
		}
	}

	// Token: 0x06003CF0 RID: 15600 RVA: 0x00022AFF File Offset: 0x00020CFF
	public bool GetHorizontalInputUp()
	{
		return !this.blockAllInputs && Mathf.Abs(this.lastFrameHorizontalInput) >= 0.5f && Mathf.Abs(this.GetHorizontalInput(false)) < 0.5f;
	}

	// Token: 0x06003CF1 RID: 15601 RVA: 0x00130124 File Offset: 0x0012E324
	public string GetTextButtonImageTag(string locTag)
	{
		EControllerType lastUsedControllerType = this.GetLastUsedControllerType();
		return this.GetTextButtonImageTag(locTag, lastUsedControllerType);
	}

	// Token: 0x06003CF2 RID: 15602 RVA: 0x00130140 File Offset: 0x0012E340
	public string GetTextButtonImageTag(string locTag, EControllerType controllerType)
	{
		EControllerType lastUsedControllerType = this.GetLastUsedControllerType();
		return this.GetTextButtonImageTag(locTag, lastUsedControllerType, this.GetLastActiveController().type);
	}

	// Token: 0x06003CF3 RID: 15603 RVA: 0x00130168 File Offset: 0x0012E368
	public Controller GetLastActiveController()
	{
		Controller controller = this.player.controllers.GetLastActiveController();
		if (controller == null && this.player.controllers.Joysticks.Count > 0)
		{
			controller = this.player.controllers.Joysticks[0];
		}
		if (controller == null && this.player.controllers.hasKeyboard)
		{
			controller = this.player.controllers.Keyboard;
		}
		if (controller == null && this.player.controllers.hasMouse)
		{
			controller = this.player.controllers.Mouse;
		}
		return controller;
	}

	// Token: 0x06003CF4 RID: 15604 RVA: 0x00130218 File Offset: 0x0012E418
	public string GetTextButtonImageTag(string locTag, EControllerType controllerType, ControllerType rewiredControllerType)
	{
		EControllerType lastUsedControllerType = this.GetLastUsedControllerType();
		return this.GetTextButtonImageTag(locTag, lastUsedControllerType, this.GetLastActiveController().type, this.GetLastActiveController().id);
	}

	// Token: 0x06003CF5 RID: 15605 RVA: 0x0013024C File Offset: 0x0012E44C
	public string GetTextButtonImageTag(string locTag, EControllerType controllerType, ControllerType rewiredControllerType, int controllerId)
	{
		string str = "<sprite=";
		if (controllerType == EControllerType.XBOX)
		{
			str += "\"XboxButtons\"";
		}
		else if (controllerType == EControllerType.KEYBOARD)
		{
			str += "\"KeyboardButtons\"";
		}
		else if (controllerType == EControllerType.PS)
		{
			str += "\"PlaystationButtons_PC\"";
		}
		else if (controllerType == EControllerType.SWITCH)
		{
			str += "\"SwitchButtons\"";
		}
		else
		{
			str += "\"XboxButtons\"";
		}
		str += " name=";
		switch (locTag)
		{
		case "jump":
			str = str + "\"" + this.GetButtonSpriteNameForAction("Jump", rewiredControllerType, controllerId) + "\"";
			break;
		case "attack":
			str = str + "\"" + this.GetButtonSpriteNameForAction("Attack", rewiredControllerType, controllerId) + "\"";
			break;
		case "rt":
			str = str + "\"" + this.GetButtonSpriteNameForAction("MagicBoots", rewiredControllerType, controllerId) + "\"";
			break;
		case "map":
			str = str + "\"" + this.GetButtonSpriteNameForAction("Map", rewiredControllerType, controllerId) + "\"";
			break;
		case "shuriken":
			str = str + "\"" + this.GetButtonSpriteNameForAction("Shuriken", rewiredControllerType, controllerId) + "\"";
			break;
		case "graplou":
			str = str + "\"" + this.GetButtonSpriteNameForAction("Graplou", rewiredControllerType, controllerId) + "\"";
			break;
		case "start":
			str = str + "\"" + this.GetButtonSpriteNameForAction("Start", rewiredControllerType, controllerId) + "\"";
			break;
		case "ls":
			str += "\"LS\"";
			break;
		case "lsUp":
			if (this.GetLastUsedControllerType() == EControllerType.KEYBOARD)
			{
				str = str + "\"" + this.GetButtonSpriteNameForAction("Dpad Up", rewiredControllerType, controllerId) + "\"";
			}
			else
			{
				str += "\"LS\"";
			}
			break;
		case "lsLeft":
			if (this.GetLastUsedControllerType() == EControllerType.KEYBOARD)
			{
				str = str + "\"" + this.GetButtonSpriteNameForAction("Dpad Left", rewiredControllerType, controllerId) + "\"";
			}
			else
			{
				str += "\"LS\"";
			}
			break;
		case "lsRight":
			if (this.GetLastUsedControllerType() == EControllerType.KEYBOARD)
			{
				str = str + "\"" + this.GetButtonSpriteNameForAction("Dpad Right", rewiredControllerType, controllerId) + "\"";
			}
			else
			{
				str += "\"LS\"";
			}
			break;
		case "lsDown":
			if (this.GetLastUsedControllerType() == EControllerType.KEYBOARD)
			{
				str = str + "\"" + this.GetButtonSpriteNameForAction("Dpad Down", rewiredControllerType, controllerId) + "\"";
			}
			else
			{
				str += "\"LS\"";
			}
			break;
		case "back":
			str = str + "\"" + this.GetButtonSpriteNameForAction("Back", rewiredControllerType, controllerId) + "\"";
			break;
		case "confirm":
			str = str + "\"" + this.GetButtonSpriteNameForAction("Confirm", rewiredControllerType, controllerId) + "\"";
			break;
		case "up":
			str = str + "\"" + this.GetButtonSpriteNameForAction("Dpad Up", rewiredControllerType, controllerId) + "\"";
			break;
		case "left":
			str = str + "\"" + this.GetButtonSpriteNameForAction("Dpad Left", rewiredControllerType, controllerId) + "\"";
			break;
		case "right":
			str = str + "\"" + this.GetButtonSpriteNameForAction("Dpad Right", rewiredControllerType, controllerId) + "\"";
			break;
		case "down":
			str = str + "\"" + this.GetButtonSpriteNameForAction("Dpad Down", rewiredControllerType, controllerId) + "\"";
			break;
		}
		return str + ">";
	}

	// Token: 0x06003CF6 RID: 15606 RVA: 0x00130730 File Offset: 0x0012E930
	public string GetTextButtonImageTagForAction(string action, EControllerType controllerType, ControllerType rewiredControllerType, int controllerId)
	{
		string str = "<sprite=";
		if (controllerType == EControllerType.XBOX)
		{
			str += "\"XboxButtons\"";
		}
		else if (controllerType == EControllerType.KEYBOARD)
		{
			str += "\"KeyboardButtons\"";
		}
		else if (controllerType == EControllerType.PS)
		{
			str += "\"PlaystationButtons_PC\"";
		}
		else if (controllerType == EControllerType.SWITCH)
		{
			str += "\"SwitchButtons\"";
		}
		else
		{
			str += "\"XboxButtons\"";
		}
		str += " name=";
		str = str + "\"" + this.GetButtonSpriteNameForAction(action, rewiredControllerType, controllerId) + "\"";
		return str + ">";
	}

	// Token: 0x06003CF7 RID: 15607 RVA: 0x001307E4 File Offset: 0x0012E9E4
	public string GetButtonSpriteNameForAction(string action, ControllerType controllerType, int controllerId)
	{
		Controller controller = this.player.controllers.GetController(controllerType, controllerId);
		ControllerMap map = this.player.controllers.maps.GetMap(controller, "default", "default");
		ActionElementMap firstElementMapWithAction = map.GetFirstElementMapWithAction(action);
		if (firstElementMapWithAction != null)
		{
			return firstElementMapWithAction.elementIdentifierName;
		}
		return "none";
	}

	// Token: 0x06003CF8 RID: 15608 RVA: 0x00022B39 File Offset: 0x00020D39
	public void SetDefaultMapping(ControllerType controllerType)
	{
		this.player.controllers.maps.LoadDefaultMaps(controllerType);
		this.CopyMapping(this.player.controllers.GetLastActiveController());
	}

	// Token: 0x06003CF9 RID: 15609 RVA: 0x00130840 File Offset: 0x0012EA40
	public void CopyMapping()
	{
		if (this.controller == null || this.controller.type != ControllerType.Joystick)
		{
			return;
		}
		ControllerMap controllerMap = ReInput.mapping.GetControllerMap(this.mappingId);
		this.CopyMapping(this.currentControllerMap);
	}

	// Token: 0x06003CFA RID: 15610 RVA: 0x00130888 File Offset: 0x0012EA88
	public void CopyMapping(Controller controller)
	{
		if (controller.type != ControllerType.Joystick)
		{
			return;
		}
		JoystickMap map = this.player.controllers.maps.GetMap(controller, "Default", "Default") as JoystickMap;
		this.CopyMapping(map);
	}

	// Token: 0x06003CFB RID: 15611 RVA: 0x001308D0 File Offset: 0x0012EAD0
	public void CopyMapping(JoystickMap map)
	{
		if (map == null)
		{
			return;
		}
		this.mappingId = map.id;
		this.currentControllerMap = new JoystickMap(map);
		for (int i = 0; i < this.player.controllers.Joysticks.Count; i++)
		{
			Controller controller = this.player.controllers.Joysticks[i];
			ControllerMap map2 = this.player.controllers.maps.GetMap(controller, "Default", "Default");
			List<ActionElementMap> list = new List<ActionElementMap>(map2.AllMaps);
			if (list != null)
			{
				for (int j = list.Count - 1; j >= 0; j--)
				{
					ActionElementMap actionElementMap = list[j];
					ActionElementMap firstElementMapWithAction = map.GetFirstElementMapWithAction(actionElementMap.actionId);
					if (firstElementMapWithAction != null)
					{
						map2.ReplaceOrCreateElementMap(new ElementAssignment(firstElementMapWithAction.elementIdentifierId, actionElementMap.actionId, false, actionElementMap.id)
						{
							type = ((firstElementMapWithAction.elementType != ControllerElementType.Button) ? ElementAssignmentType.FullAxis : ElementAssignmentType.Button)
						});
					}
				}
			}
		}
	}

	// Token: 0x06003CFC RID: 15612 RVA: 0x00022B67 File Offset: 0x00020D67
	public bool GetEndingSpamDown()
	{
		if(xNyuInstance == null){
			return !this.blockAllInputs && this.player.GetButtonDown("EndingSpam");
		}
		else
		{
			return !this.blockAllInputs && xNyuInstance.GetEndingSpamDown;
		}
	}

	// Token: 0x06003CFD RID: 15613 RVA: 0x00022B86 File Offset: 0x00020D86
	public EControllerType GetLastUsedControllerType()
	{
		return this.GetLastUsedControllerType(this.GetLastActiveController());
	}

	// Token: 0x06003CFE RID: 15614 RVA: 0x001309F0 File Offset: 0x0012EBF0
	public EControllerType GetLastUsedControllerType(Controller controller)
	{
		if (controller == null)
		{
			return EControllerType.XBOX;
		}
		if (controller.type == ControllerType.Keyboard || controller.type == ControllerType.Mouse)
		{
			return EControllerType.KEYBOARD;
		}
		if (controller.type == ControllerType.Joystick)
		{
			Joystick joystick = controller as Joystick;
			string text = joystick.hardwareTypeGuid.ToString();
			switch (text)
			{
			case "d74a350e-fe8b-4e9e-bbcd-efff16d34115":
			case "19002688-7406-4f4a-8340-8d25335406c8":
				return EControllerType.XBOX;
			case "c3ad3cad-c7cf-4ca8-8c2e-e3df8d9960bb":
			case "71dfe6c8-9e81-428f-a58e-c7e664b7fbed":
			case "cd9718bf-a87a-44bc-8716-60a0def28a9f":
				return EControllerType.PS;
			case "521b808c-0248-4526-bc10-f1d16ee76bf1":
			case "1fbdd13b-0795-4173-8a95-a2a75de9d204":
			case "3eb01142-da0e-4a86-8ae8-a15c2b1f2a04":
			case "7bf3154b-9db8-4d52-950f-cd0eed8a5819":
			case "605dc720-1b38-473d-a459-67d5857aa6ea":
				return EControllerType.SWITCH;
			}
		}
		return EControllerType.XBOX;
	}

	// Token: 0x06003CFF RID: 15615 RVA: 0x0000253E File Offset: 0x0000073E
	private void CallControllerApplet()
	{
	}

	// Token: 0x06003D00 RID: 15616 RVA: 0x00130B10 File Offset: 0x0012ED10
	private void OnControllerConnected(ControllerStatusChangedEventArgs args)
	{
		for (int i = 0; i < this.player.controllers.joystickCount; i++)
		{
			Controller controller = this.player.controllers.GetController(ControllerType.Joystick, i);
			if (controller != null)
			{
			}
		}
		this.lastConnectedController = this.player.controllers.GetController(args.controllerType, args.controllerId);
		this.CopyMapping();
	}

	// Token: 0x06003D01 RID: 15617 RVA: 0x00022B94 File Offset: 0x00020D94
	private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
	{
		base.StartCoroutine(this.DisconnectectTimerCoroutine());
	}

	// Token: 0x06003D02 RID: 15618 RVA: 0x00130B80 File Offset: 0x0012ED80
	private IEnumerator DisconnectectTimerCoroutine()
	{
		yield return CoroutineUtil.WaitForRealSeconds(1f);
		for (int i = 0; i < this.player.controllers.joystickCount; i++)
		{
			Controller controller = this.player.controllers.GetController(ControllerType.Joystick, i);
			if (controller != null)
			{
			}
		}
		if (this.player.controllers.joystickCount <= 0)
		{
		}
		yield break;
	}

	// Token: 0x06003D03 RID: 15619 RVA: 0x00022BA3 File Offset: 0x00020DA3
	protected override void OnDestroy()
	{
		ReInput.ControllerConnectedEvent -= this.OnControllerConnected;
		ReInput.ControllerDisconnectedEvent -= this.OnControllerDisconnected;
		base.OnDestroy();
	}

	// Token: 0x04003E87 RID: 16007
	public const string XBOX_360_ID = "d74a350e-fe8b-4e9e-bbcd-efff16d34115";

	// Token: 0x04003E88 RID: 16008
	public const string XBOX_ONE_ID = "19002688-7406-4f4a-8340-8d25335406c8";

	// Token: 0x04003E89 RID: 16009
	public const string PS2_ID = "c3ad3cad-c7cf-4ca8-8c2e-e3df8d9960bb";

	// Token: 0x04003E8A RID: 16010
	public const string PS3_ID = "71dfe6c8-9e81-428f-a58e-c7e664b7fbed";

	// Token: 0x04003E8B RID: 16011
	public const string PS4_ID = "cd9718bf-a87a-44bc-8716-60a0def28a9f";

	// Token: 0x04003E8C RID: 16012
	public const string SWITCH_LEFT_JOYCON = "3eb01142-da0e-4a86-8ae8-a15c2b1f2a04";

	// Token: 0x04003E8D RID: 16013
	public const string SWITCH_RIGHT_JOYCON = "605dc720-1b38-473d-a459-67d5857aa6ea";

	// Token: 0x04003E8E RID: 16014
	public const string SWITCH_DUAL_JOYCON = "521b808c-0248-4526-bc10-f1d16ee76bf1";

	// Token: 0x04003E8F RID: 16015
	public const string SWITCH_HANDHELD = "1fbdd13b-0795-4173-8a95-a2a75de9d204";

	// Token: 0x04003E90 RID: 16016
	public const string SWITCH_PRO_CONTROLLER = "7bf3154b-9db8-4d52-950f-cd0eed8a5819";

	// Token: 0x04003E91 RID: 16017
	public List<KeyCode> validKeyCodes;

	// Token: 0x04003E92 RID: 16018
	public float leftStickVerticalDeadzone = 0.3f;

	// Token: 0x04003E93 RID: 16019
	public float leftStickHorizontalDeadzone = 0.3f;

	// Token: 0x04003E94 RID: 16020
	private float jumpTimeDown;

	// Token: 0x04003E95 RID: 16021
	private float lastFrameHorizontalInput;

	// Token: 0x04003E96 RID: 16022
	private float lastFrameVerticalInput;

	// Token: 0x04003E97 RID: 16023
	private Player player;

	// Token: 0x04003E98 RID: 16024
	private bool mLastInputUp;

	// Token: 0x04003E99 RID: 16025
	private bool mInputUpPressed;

	// Token: 0x04003E9A RID: 16026
	public bool blockAllInputs;

	// Token: 0x04003E9B RID: 16027
	private bool rumble = true;

	// Token: 0x04003E9C RID: 16028
	private bool separateGraplouButton = true;

	// Token: 0x04003E9D RID: 16029
	private JoystickMap currentControllerMap;

	// Token: 0x04003E9E RID: 16030
	private bool horizontalInputForced;

	// Token: 0x04003E9F RID: 16031
	private float forcedHorizontalInput;

	// Token: 0x04003EA0 RID: 16032
	private int mappingId;

	// Token: 0x04003EA1 RID: 16033
	private Controller lastConnectedController;

	public xNyuTAS xNyuInstance = null;
	public bool xNyuInstanceFound = false;

}