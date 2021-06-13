using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ActivateGameOverBtns : MonoBehaviour
    {

		private List<Button> btns = new List<Button>();

		public void Activate()
		{
			btns = GetComponentsInChildren<Button>().ToList();
			SetBtns(true);
		}

		public void Deactivate() => SetBtns(false);

		private void Awake()
		{
			btns = GetComponentsInChildren<Button>().ToList();
			SetBtns(false);
		}

		private void SetBtns(bool value) { foreach (Button btn in btns) btn.interactable = value; }
	}
}
