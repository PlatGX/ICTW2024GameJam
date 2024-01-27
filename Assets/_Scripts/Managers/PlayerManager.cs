using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	public static event EventHandler<PlayerEventArgs> OnPlayerJoined;
	public static event EventHandler<PlayerEventArgs> OnPlayerLeave;

	public class PlayerEventArgs : EventArgs{
		public Player player;
		public PlayerEventArgs(Player _player){
			player = _player;
		}
	}

	[SerializeField] private GameObject characterSelectScreen;

	[SerializeField] private List<Player> currentPlayerList;

	private void Awake() {
		currentPlayerList = new List<Player>();
	}

	public void OnSceneLoad(){
		ShowCharacterScreen();
	}

    private void ShowCharacterScreen(){
		characterSelectScreen.SetActive(true);
    }

	private void HideCharacterScreen(){
		characterSelectScreen.SetActive(false);
	}

    public void AddNewPlayer(PlayerInput playerInput){
		int incomingPlayerIndex = currentPlayerList.Count;
		
		Player incomingPlayer = new Player((KnightColor)incomingPlayerIndex, incomingPlayerIndex, playerInput);
		currentPlayerList.Add(incomingPlayer);

		playerInput.GetComponent<PlayerVisualsController>().UpdateKnightVisual(incomingPlayer.knightColor);

		OnPlayerJoined?.Invoke(this, new PlayerEventArgs(incomingPlayer));
    }

    public void RemovePlayer(PlayerInput playerInput){
		Player leavingPlayer = currentPlayerList.FirstOrDefault(x => x.assignedPlayerInput == playerInput);
		currentPlayerList.Remove(leavingPlayer);

		OnPlayerLeave?.Invoke(this, new PlayerEventArgs(leavingPlayer));
	}
}
