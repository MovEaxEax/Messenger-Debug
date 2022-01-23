using System;
using UnityEngine;
public partial class IntroManager : MonoBehaviour
{
	private void Start()
	{
		if (UnityEngine.Object.FindObjectOfType<xNyuDebug>() == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<xNyuDebug>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		if (Manager<UIManager>.Instance.GetView<SavingScreen>() == null)
		{
			Manager<UIManager>.Instance.ShowView<SavingScreen>(EScreenLayers.CONSOLE, null, true, AnimatorUpdateMode.UnscaledTime);
		}
		DebugConsole.Log("Intro Manager Start");
		Manager<UIManager>.Instance.PreloadView<TitleScreen>(false);
		Manager<UIManager>.Instance.PreloadView<SaveGameSelectionScreen>(false);
		Manager<UIManager>.Instance.PreloadView<OptionScreen>(false);
		this.PlayIntroSequence();
		Manager<InputManager>.Instance.blockAllInputs = false;
	}
}



using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public partial class PauseScreen : View
{
	private void OnEnable()
	{
		this.justOpened = true;
		this.backgroundFrame.sizeDelta = new Vector2(this.backgroundFrame.sizeDelta.x, this.backgroundFrame.sizeDelta.y - this.heightPerButton);
		if(GameObject.FindObjectOfType<xNyuDebug>() != null){
			this.cheatButton.SetActive(GameObject.FindObjectOfType<xNyuDebug>().CheatButtonEnable);
		}else{
			this.cheatButton.SetActive(false);
		}
		if (Manager<SurfManager>.Instance != null && Manager<SurfManager>.Instance.allowRestartLevel)
		{
			this.restartSurfLevelButton.SetActive(true);
		}
		else
		{
			this.restartSurfLevelButton.SetActive(false);
			this.backgroundFrame.sizeDelta = new Vector2(this.backgroundFrame.sizeDelta.x, this.backgroundFrame.sizeDelta.y - this.heightPerButton);
		}
		this.versionText.text = string.Empty;
	}
}



using System;
using AdvancedInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
[AdvancedInspector(true, true)]
public partial class LevelSelectCheat : MonoBehaviour, IMoveHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler
{
	public void OnMove(AxisEventData eventData)
	{

			if (eventData.moveDir == MoveDirection.Left || eventData.moveDir == MoveDirection.Right)
			{
				ELevel elevel = (ELevel)this.selectedLevel;
				do
				{
					this.selectedLevel += ((eventData.moveDir != MoveDirection.Right) ? -1 : 1);
					this.selectedLevel = ((this.selectedLevel >= this.minLevel) ? this.selectedLevel : this.maxLevel);
					this.selectedLevel = ((this.selectedLevel <= this.maxLevel) ? this.selectedLevel : this.minLevel);
					elevel = (ELevel)this.selectedLevel;
				}
				while (!this.levelNameByLevel.ContainsKey(elevel));
				this.SetLevel(elevel);
			}
		
	}
}






