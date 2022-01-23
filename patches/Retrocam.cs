using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class RetroCamera : MonoBehaviour
{
	// Token: 0x060000DB RID: 219 RVA: 0x00035EB0 File Offset: 0x000340B0
	public RetroCamera()
	{
	}

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x060000DC RID: 220 RVA: 0x00035F00 File Offset: 0x00034100
	// (remove) Token: 0x060000DD RID: 221 RVA: 0x00035F38 File Offset: 0x00034138
	public event Action<Vector2> onMove;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x060000DE RID: 222 RVA: 0x00035F70 File Offset: 0x00034170
	// (remove) Token: 0x060000DF RID: 223 RVA: 0x00035FA8 File Offset: 0x000341A8
	public event Action<Vector3> onScreenChanged;

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060000E0 RID: 224 RVA: 0x0000290C File Offset: 0x00000B0C
	// (set) Token: 0x060000E1 RID: 225 RVA: 0x00002914 File Offset: 0x00000B14
	public float MinCamX
	{
		get
		{
			return this.minCamX;
		}
		set
		{
			this.minCamX = value;
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000291D File Offset: 0x00000B1D
	public float MaxCamX
	{
		get
		{
			return this.maxCamX;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060000E3 RID: 227 RVA: 0x00002925 File Offset: 0x00000B25
	public float MinCamY
	{
		get
		{
			return this.minCamY;
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060000E4 RID: 228 RVA: 0x0000292D File Offset: 0x00000B2D
	// (set) Token: 0x060000E5 RID: 229 RVA: 0x00002935 File Offset: 0x00000B35
	public float MaxCamY
	{
		get
		{
			return this.maxCamY;
		}
		set
		{
			this.maxCamY = value;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000293E File Offset: 0x00000B3E
	public Camera Camera
	{
		get
		{
			return this.camera;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060000E7 RID: 231 RVA: 0x00002946 File Offset: 0x00000B46
	// (set) Token: 0x060000E8 RID: 232 RVA: 0x0000294E File Offset: 0x00000B4E
	public bool ForceScreenChange
	{
		get
		{
			return this.forceScreenChange;
		}
		set
		{
			this.forceScreenChange = value;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060000E9 RID: 233 RVA: 0x00002957 File Offset: 0x00000B57
	public CameraBehaviour CameraBehaviour
	{
		get
		{
			return this.cameraBehaviour;
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060000EA RID: 234 RVA: 0x0000295F File Offset: 0x00000B5F
	// (set) Token: 0x060000EB RID: 235 RVA: 0x00002967 File Offset: 0x00000B67
	public bool ForceUpdate
	{
		get
		{
			return this.forceUpdate;
		}
		set
		{
			this.forceUpdate = value;
		}
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00002970 File Offset: 0x00000B70
	private void Awake()
	{
		this.screenChangeData = new ScreenChangeData();
		this.cameraBehaviour = base.GetComponent<CameraBehaviour>();
		this.spawnZoneRect = this.spawnZone.GetRect();
		this.camera = base.GetComponent<Camera>();
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00035FE0 File Offset: 0x000341E0
	private void Start()
	{
		this.playerCtrl = Manager<PlayerManager>.Instance.Player;
		Vector3 v = this.playerCtrl.transform.position - base.transform.position;
		base.transform.position = this.playerCtrl.transform.position;
		if (this.updateParallax)
		{
			Manager<ParallaxManager>.Instance.AddCameraMovement(this.camera, v);
		}
		this.SetLastPosition(base.transform.position);
	}

	// Token: 0x060000EE RID: 238 RVA: 0x000029A6 File Offset: 0x00000BA6
	private void LateUpdate()
	{
		if (Manager<Level>.Instance.Initialized)
		{
			if (Manager<PlayerManager>.Instance.Player.IsDead() && !this.forceUpdate)
			{
				return;
			}
			this.UpdateCamera();
			this.UpdateSpawnZoneRect();
		}
	}

	// Token: 0x060000EF RID: 239 RVA: 0x0003606C File Offset: 0x0003426C
	private void UpdateSpawnZoneRect()
	{
		Vector2 a = this.spawnZone.transform.position;
		this.spawnZoneRect.center = a + this.spawnZone.offset;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x000360AC File Offset: 0x000342AC
	public void UpdateCamera()
	{
		CameraInfo cameraInfo = this.cameraBehaviour.GetCameraInfo();
		this.UpdateCamera(cameraInfo, this.cameraBehaviour.GetAdditiveCameraInfo());
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x000360D8 File Offset: 0x000342D8
	public Vector3 SnapPositionToCameraBounds(Vector3 pos)
	{
		pos.x = Mathf.Clamp(pos.x, this.minCamX, this.maxCamX);
		pos.y = Mathf.Clamp(pos.y, this.minCamY, this.maxCamY);
		return pos;
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x000029E3 File Offset: 0x00000BE3
	public Vector3 SnapPositionToHorizontalCameraBounds(Vector3 pos)
	{
		pos.x = Mathf.Clamp(pos.x, this.minCamX, this.maxCamX);
		return pos;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00002A05 File Offset: 0x00000C05
	public Vector3 SnapPositionToVerticalCameraBounds(Vector3 pos)
	{
		pos.y = Mathf.Clamp(pos.y, this.minCamY, this.maxCamY);
		return pos;
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00036124 File Offset: 0x00034324
	public void AddScreenBoundsOverride(ScreenBoundsOverride screenBoundsOverride, bool instantChange, float changeSpeed)
	{
		if (!this.screenBoundsOverrides.Contains(screenBoundsOverride))
		{
			this.screenBoundsOverrides.Add(screenBoundsOverride);
		}
		Bounds bounds = screenBoundsOverride.GetBounds();
		this.ChangeCameraBounds(bounds.min.x, bounds.max.x, bounds.min.y, bounds.max.y, instantChange, changeSpeed);
		base.transform.position = this.SnapPositionToCameraBounds(base.transform.position);
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x000361B8 File Offset: 0x000343B8
	public void RemoveScreenBoundsOverride(ScreenBoundsOverride screenBoundsOverride)
	{
		int num = this.screenBoundsOverrides.IndexOf(screenBoundsOverride);
		if (num != -1)
		{
			this.screenBoundsOverrides.RemoveAt(num);
			if (this.screenBoundsOverrides.Count == 0)
			{
				this.ChangeCameraBounds(this.basicCamBounds.min.x, this.basicCamBounds.max.x, this.basicCamBounds.min.y, this.basicCamBounds.max.y, screenBoundsOverride.deactivateInstantChange, screenBoundsOverride.deactivateTransitionSpeed);
			}
			else if (num == 0)
			{
				this.AddScreenBoundsOverride(this.screenBoundsOverrides[0], this.screenBoundsOverrides[0].activateInstantChange, this.screenBoundsOverrides[0].activateTransitionSpeed);
			}
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00036294 File Offset: 0x00034494
	public void SetCamMaxY(float edgePosition)
	{
		float orthographicSize = this.camera.orthographicSize;
		this.maxCamY = edgePosition - orthographicSize + (float)this.hudTileHeight;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x000362C0 File Offset: 0x000344C0
	public void ChangeCameraBounds(float minX, float maxX, float minY, float maxY, bool instant = true, float speed = 10f)
	{
		for (int i = this.screenBoundsTweenCoroutines.Count - 1; i >= 0; i--)
		{
			base.StopCoroutine(this.screenBoundsTweenCoroutines[i]);
		}
		this.screenBoundsTweenCoroutines.Clear();
		float num = this.camera.orthographicSize * this.camera.aspect;
		float orthographicSize = this.camera.orthographicSize;
		minX += num;
		maxX -= num;
		minY += orthographicSize;
		maxY = maxY - orthographicSize + (float)this.hudTileHeight;
		if (instant)
		{
			this.minCamX = minX;
			this.maxCamX = maxX;
			this.minCamY = minY;
			this.maxCamY = maxY;
		}
		else
		{
			this.screenBoundsTweenCoroutines.Add(base.StartCoroutine(this.ChangeCamMinX(minX, speed)));
			this.screenBoundsTweenCoroutines.Add(base.StartCoroutine(this.ChangeCamMaxX(maxX, speed)));
			this.screenBoundsTweenCoroutines.Add(base.StartCoroutine(this.ChangeCamMinY(minY, speed)));
			this.screenBoundsTweenCoroutines.Add(base.StartCoroutine(this.ChangeCamMaxY(maxY, speed)));
		}
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x000363DC File Offset: 0x000345DC
	private IEnumerator ChangeCamMinX(float minX, float speed)
	{
		float startMinX = this.minCamX;
		float duration = Mathf.Abs(startMinX - minX) / speed;
		float progress = 0f;
		while (progress < 1f)
		{
			progress += TimeVars.GetDeltaTime() / duration;
			float tweenedProgress = TweenFunctions.Quadratic.InOut(0f, 1f, progress);
			this.minCamX = Mathf.Lerp(startMinX, minX, tweenedProgress);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00036408 File Offset: 0x00034608
	private IEnumerator ChangeCamMaxX(float maxX, float speed)
	{
		float startMaxX = this.maxCamX;
		float duration = Mathf.Abs(startMaxX - maxX) / speed;
		float progress = 0f;
		while (progress < 1f)
		{
			progress += TimeVars.GetDeltaTime() / duration;
			float tweenedProgress = TweenFunctions.Quadratic.InOut(0f, 1f, progress);
			this.maxCamX = Mathf.Lerp(startMaxX, maxX, tweenedProgress);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00036434 File Offset: 0x00034634
	private IEnumerator ChangeCamMinY(float minY, float speed)
	{
		float startMinY = (base.transform.position.y <= this.minCamY) ? this.minCamY : base.transform.position.y;
		float duration = Mathf.Abs(startMinY - minY) / speed;
		float progress = 0f;
		while (progress < 1f)
		{
			progress += TimeVars.GetDeltaTime() / duration;
			float tweenedProgress = TweenFunctions.Quadratic.InOut(0f, 1f, progress);
			this.minCamY = Mathf.Lerp(startMinY, minY, tweenedProgress);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00036460 File Offset: 0x00034660
	private IEnumerator ChangeCamMaxY(float maxY, float speed)
	{
		float startMaxY = this.maxCamY;
		float duration = Mathf.Abs(startMaxY - maxY) / speed;
		float progress = 0f;
		while (progress < 1f)
		{
			progress += TimeVars.GetDeltaTime() / duration;
			float tweenedProgress = TweenFunctions.Quadratic.InOut(0f, 1f, progress);
			this.maxCamY = Mathf.Lerp(startMaxY, maxY, tweenedProgress);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000FC RID: 252 RVA: 0x0003648C File Offset: 0x0003468C
	public void UpdateCamera(CameraInfo cameraInfo, CameraInfo additiveContextInfo)
	{
		ScreenChangeData screenChangeData = this.TestScreenChange(cameraInfo);
		Vector3 vector = screenChangeData.newPosition;
		if (screenChangeData.shouldChangeScreenButCant)
		{
			this.KillPlayerIfInHole();
		}
		if (screenChangeData.changedScreen)
		{
			this.ChangeScreen(vector);
			vector = this.SnapPositionToCameraBounds(cameraInfo.cameraScreenLimitPosition);
		}
		if (additiveContextInfo != null)
		{
			vector += additiveContextInfo.cameraPosition;
		}
		vector.z = -40f;
		vector = MathUtil.FixFloatPrecision(vector, 1000f);
		Vector2 vector2 = vector - this.lastPosition;
		xNyuDebug xNyuDebugMenu = GameObject.FindObjectOfType<xNyuDebug>();

		if( xNyuDebugMenu != null){
			if(xNyuDebugMenu.CameraUnlocked){
				base.transform.position = xNyuDebugMenu.CameraPosition;
				vector2 = new Vector2(2f, 2f);
			}else{
				base.transform.position = vector;
			}
		}else{
			base.transform.position = vector;
		}

		if (this.updateParallax)
		{
			Manager<ParallaxManager>.Instance.AddCameraMovement(this.camera, vector2);
		}
		if (this.onMove != null)
		{
			this.onMove(vector2);
		}
		this.lastPosition = base.transform.position;

		if (screenChangeData.changedScreen || this.forceScreenChange)
		{
			this.forceScreenChange = false;
			if (Manager<Level>.Instance.IsActive())
			{
				Manager<Level>.Instance.ChangeRoom(this.screenLeftEdge, this.screenRightEdge, this.screenBottomEdge, this.screenTopEdge, false);
			}
			if (this.onScreenChanged != null)
			{
				this.onScreenChanged(vector2);
			}
		}
	}

	public void Update(){
		xNyuDebug xNyuDebugMenu = GameObject.FindObjectOfType<xNyuDebug>();

		if( xNyuDebugMenu != null){
			if(xNyuDebugMenu.CameraUnlocked){
				base.transform.position = xNyuDebugMenu.CameraPosition;
			}
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x000365D4 File Offset: 0x000347D4
	private ScreenChangeData TestScreenChange(CameraInfo cameraInfo)
	{
		Vector3 cameraScreenLimitPosition = cameraInfo.cameraScreenLimitPosition;
		this.screenChangeData.newPosition = cameraInfo.cameraPosition;
		this.screenChangeData.changedScreen = false;
		this.screenChangeData.shouldChangeScreenButCant = false;
		if (!cameraInfo.respectWorldCameraBounds)
		{
			return this.screenChangeData;
		}
		if (cameraScreenLimitPosition.x > this.maxScreenX)
		{
			if (this.CanSwitchScreenRight())
			{
				this.screenChangeData.changedScreen = true;
				ScreenChangeData screenChangeData = this.screenChangeData;
				screenChangeData.newPosition.x = screenChangeData.newPosition.x + 17f;
			}
			else
			{
				this.screenChangeData.shouldChangeScreenButCant = true;
			}
		}
		if (cameraScreenLimitPosition.x < this.minScreenX)
		{
			if (this.CanSwitchScreenLeft())
			{
				this.screenChangeData.changedScreen = true;
				ScreenChangeData screenChangeData2 = this.screenChangeData;
				screenChangeData2.newPosition.x = screenChangeData2.newPosition.x - 17f;
			}
			else
			{
				this.screenChangeData.shouldChangeScreenButCant = true;
			}
		}
		if (cameraScreenLimitPosition.y + 0.95f > this.maxScreenY)
		{
			if (this.CanSwitchScreenUp())
			{
				this.screenChangeData.changedScreen = true;
				ScreenChangeData screenChangeData3 = this.screenChangeData;
				screenChangeData3.newPosition.y = screenChangeData3.newPosition.y + 9f;
			}
			else
			{
				this.screenChangeData.shouldChangeScreenButCant = true;
			}
		}
		if (cameraScreenLimitPosition.y + 1f < this.minScreenY)
		{
			if (this.CanSwitchScreenDown())
			{
				this.screenChangeData.changedScreen = true;
				ScreenChangeData screenChangeData4 = this.screenChangeData;
				screenChangeData4.newPosition.y = screenChangeData4.newPosition.y - 10f;
			}
			else
			{
				this.screenChangeData.shouldChangeScreenButCant = true;
			}
		}
		return this.screenChangeData;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00036790 File Offset: 0x00034990
	public void SetCurrentScreenInfo(Vector3 position, bool instant = true, float speed = 10f)
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(position, Vector2.left, float.PositiveInfinity, LayerMaskConstants.SCROLL_BLOCK);
		this.screenLeftEdge = raycastHit2D.collider.GetComponent<ScreenEdge>();
		this.minScreenX = raycastHit2D.point.x;
		raycastHit2D = Physics2D.Raycast(position, Vector2.right, float.PositiveInfinity, LayerMaskConstants.SCROLL_BLOCK);
		this.screenRightEdge = raycastHit2D.collider.GetComponent<ScreenEdge>();
		this.maxScreenX = raycastHit2D.point.x;
		raycastHit2D = Physics2D.Raycast(position, Vector2.down, float.PositiveInfinity, LayerMaskConstants.SCROLL_BLOCK);
		this.screenBottomEdge = raycastHit2D.collider.GetComponent<ScreenEdge>();
		this.minScreenY = raycastHit2D.point.y;
		raycastHit2D = Physics2D.Raycast(position, Vector2.up, float.PositiveInfinity, LayerMaskConstants.SCROLL_BLOCK);
		this.screenTopEdge = raycastHit2D.collider.GetComponent<ScreenEdge>();
		this.maxScreenY = raycastHit2D.point.y;
		this.basicCamBounds.min = new Vector3(this.minScreenX, this.minScreenY);
		this.basicCamBounds.max = new Vector3(this.maxScreenX, this.maxScreenY);
		this.ChangeCameraBounds(this.minScreenX, this.maxScreenX, this.minScreenY, this.maxScreenY, instant, 10f);
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00036904 File Offset: 0x00034B04
	private bool CanSwitchScreenUp()
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(new Vector2(base.transform.position.x, base.transform.position.y), new Vector2(0f, 1f), float.MaxValue, LayerMaskConstants.SCROLL_BLOCK);
		return !(raycastHit2D.collider == null) && raycastHit2D.collider.gameObject.GetComponent<ScreenEdge>().triggerScreenChange;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00036988 File Offset: 0x00034B88
	private bool CanSwitchScreenDown()
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(new Vector2(base.transform.position.x, base.transform.position.y), new Vector2(0f, -1f), float.MaxValue, LayerMaskConstants.SCROLL_BLOCK);
		return !(raycastHit2D.collider == null) && raycastHit2D.collider.gameObject.GetComponent<ScreenEdge>().triggerScreenChange;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00036A0C File Offset: 0x00034C0C
	private bool CanSwitchScreenLeft()
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(new Vector2(base.transform.position.x, base.transform.position.y), new Vector2(-1f, 0f), float.MaxValue, LayerMaskConstants.SCROLL_BLOCK);
		return !(raycastHit2D.collider == null) && raycastHit2D.collider.gameObject.GetComponent<ScreenEdge>().triggerScreenChange;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00036A90 File Offset: 0x00034C90
	private bool CanSwitchScreenRight()
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(new Vector2(base.transform.position.x, base.transform.position.y), new Vector2(1f, 0f), float.MaxValue, LayerMaskConstants.SCROLL_BLOCK);
		return !(raycastHit2D.collider == null) && raycastHit2D.collider.gameObject.GetComponent<ScreenEdge>().triggerScreenChange;
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00036B14 File Offset: 0x00034D14
	private void ChangeScreen(Vector3 screenPosition)
	{
		if (!this.KillPlayerIfInHole() && this.allowScreenChange)
		{
			this.SetCurrentScreenInfo(screenPosition, true, 10f);
		}
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00036B48 File Offset: 0x00034D48
	private bool KillPlayerIfInHole()
	{
		if (Manager<PlayerManager>.Instance.Player.IsInHole() && !Manager<PlayerManager>.Instance.Player.IsDead() && !Manager<PlayerManager>.Instance.Player.StateMachine.IsState<PlayerCinematicState>())
		{
			Manager<PlayerManager>.Instance.Player.Kill(EDeathType.PITFALL, null);
			return true;
		}
		return false;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00002A27 File Offset: 0x00000C27
	public void SetLastPosition(Vector3 pos)
	{
		this.lastPosition = pos;
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00036BAC File Offset: 0x00034DAC
	public void RefreshBounds()
	{
		this.SetCurrentScreenInfo(base.transform.position, true, 10f);
		if (Manager<Level>.Instance.IsActive())
		{
			Manager<Level>.Instance.ChangeRoom(this.screenLeftEdge, this.screenRightEdge, this.screenBottomEdge, this.screenTopEdge, true);
		}
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00036C04 File Offset: 0x00034E04
	public bool CurrentRoomContainsPosition(Vector3 position)
	{
		return position.x > this.minScreenX && position.x < this.maxScreenX && position.y > this.minScreenY && position.y < this.maxScreenY;
	}

	// Token: 0x040000CC RID: 204
	public int hudTileHeight = 2;

	// Token: 0x040000CD RID: 205
	public BoxCollider2D spawnZone;

	// Token: 0x040000CE RID: 206
	public Rect spawnZoneRect;

	// Token: 0x040000CF RID: 207
	public bool allowScreenChange = true;

	// Token: 0x040000D0 RID: 208
	public bool updateParallax = true;

	// Token: 0x040000D1 RID: 209
	private float minScreenX;

	// Token: 0x040000D2 RID: 210
	private float maxScreenX;

	// Token: 0x040000D3 RID: 211
	private float minScreenY;

	// Token: 0x040000D4 RID: 212
	private float maxScreenY;

	// Token: 0x040000D5 RID: 213
	private Bounds basicCamBounds = default(Bounds);

	// Token: 0x040000D6 RID: 214
	private float minCamX;

	// Token: 0x040000D7 RID: 215
	private float maxCamX;

	// Token: 0x040000D8 RID: 216
	private float minCamY;

	// Token: 0x040000D9 RID: 217
	private float maxCamY;

	// Token: 0x040000DA RID: 218
	private ScreenEdge screenLeftEdge;

	// Token: 0x040000DB RID: 219
	private ScreenEdge screenRightEdge;

	// Token: 0x040000DC RID: 220
	private ScreenEdge screenBottomEdge;

	// Token: 0x040000DD RID: 221
	private ScreenEdge screenTopEdge;

	// Token: 0x040000DE RID: 222
	private List<Coroutine> screenBoundsTweenCoroutines = new List<Coroutine>();

	// Token: 0x040000DF RID: 223
	private Camera camera;

	// Token: 0x040000E0 RID: 224
	private Vector3 lastPosition;

	// Token: 0x040000E1 RID: 225
	private PlayerController playerCtrl;

	// Token: 0x040000E2 RID: 226
	private ScreenChangeData screenChangeData;

	// Token: 0x040000E3 RID: 227
	private bool forceScreenChange;

	// Token: 0x040000E4 RID: 228
	private CameraBehaviour cameraBehaviour;

	// Token: 0x040000E5 RID: 229
	public PanCameraContext panCameraContext;

	// Token: 0x040000E6 RID: 230
	public AutoscrollCameraContext autoscrollCameraContext;

	// Token: 0x040000E7 RID: 231
	public FixedPositionCameraContext fixedPositionContext;

	// Token: 0x040000E8 RID: 232
	public ShakeCameraAdditiveContext shakeCameraAdditiveContext;

	// Token: 0x040000E9 RID: 233
	public FollowTransformCameraContext followTransformCameraContext;

	// Token: 0x040000EA RID: 234
	private List<ScreenBoundsOverride> screenBoundsOverrides = new List<ScreenBoundsOverride>();

	// Token: 0x040000EB RID: 235
	private bool forceUpdate;
}





















































using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x0200002A RID: 42
public partial class RetroCamera : MonoBehaviour
{
	// Token: 0x060000FC RID: 252 RVA: 0x00036438 File Offset: 0x00034638
	public void UpdateCamera(CameraInfo cameraInfo, CameraInfo additiveContextInfo)
	{
		xNyuDebug xNyuDebug = UnityEngine.Object.FindObjectOfType<xNyuDebug>();

		if(xNyuDebug != null){
			if(DebugOffInit){
				this.camera.orthographicSize = xNyuDebug.UnlockedCameraSizeInit;
				DebugOffInit = false;
			}
		}

		ScreenChangeData screenChangeData = this.TestScreenChange(cameraInfo);
		Vector3 vector = screenChangeData.newPosition;
		if (screenChangeData.shouldChangeScreenButCant)
		{
			this.KillPlayerIfInHole();
		}
		if (screenChangeData.changedScreen)
		{
			this.ChangeScreen(vector);
			vector = this.SnapPositionToCameraBounds(cameraInfo.cameraScreenLimitPosition);
		}
		if (additiveContextInfo != null)
		{
			vector += additiveContextInfo.cameraPosition;
		}
		vector.z = -40f;
		vector = MathUtil.FixFloatPrecision(vector, 1000f);
		Vector2 vector2 = vector - this.lastPosition;
		if (xNyuDebug != null)
		{
			if (xNyuDebug.CameraUnlocked)
			{
				vector.x = xNyuDebug.UnlockedCameraPosition.x;
				vector.y = xNyuDebug.UnlockedCameraPosition.y;
				vector.z = xNyuDebug.UnlockedCameraPosition.z;
				vector2 = new Vector2(vector.x, vector.y);
				base.transform.position = vector;
			}
			else
			{
				this.CameraSize = this.camera.orthographicSize;
				base.transform.position = vector;
			}
		}
		else
		{
			this.CameraSize = this.camera.orthographicSize;
			base.transform.position = vector;
		}
		if (this.updateParallax)
		{
			Manager<ParallaxManager>.Instance.AddCameraMovement(this.camera, vector2);
		}
		if (this.onMove != null)
		{
			this.onMove(vector2);
		}
		this.lastPosition = base.transform.position;
		if (screenChangeData.changedScreen || this.forceScreenChange)
		{
			this.forceScreenChange = false;
			if (Manager<Level>.Instance.IsActive())
			{
				Manager<Level>.Instance.ChangeRoom(this.screenLeftEdge, this.screenRightEdge, this.screenBottomEdge, this.screenTopEdge, false);
			}
			if (this.onScreenChanged != null)
			{
				this.onScreenChanged(vector2);
			}
		}
	}
}
