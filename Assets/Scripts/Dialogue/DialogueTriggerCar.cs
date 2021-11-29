using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTriggerCar : DialogueTrigger
{
    //Method triggered from GameInteraction Event
    public override void OnInteract()
    {
        //Decide ink file to play
        _JSONDataContainer.SetJSON(_inkJSON);
        
        //Execute base class funcitonality
        base.OnInteract();
    }

    //Method triggered from DragAndDrop
    public override void OnInteract(GameObject obj){

        // ItemEnum type = obj.GetComponent<ItemType>().GetItemtype();

        // //Decide ink file to play
        // switch(type){
        //     case ItemEnum.GreenSquare: _JSONDataContainer.SetJSON(_inkObjectJSON[0]); break;  
        //     case ItemEnum.RedSquare:

        //         _JSONDataContainer.SetJSON(_inkObjectJSON[1]);

        //         // Can load other scenes like so (maybe another script attached that triggers minigame
        //         // would be a better approach):
                
        //         // SceneLoader.Instance.LoadScene(_minigameScene, LoadSceneMode.Additive);

        //         break;  
        // }
        
        //Execute base class funcitonality
        base.OnInteract(obj);

        if (_playerInRange) GameStateData.Instance.gameData.carPieceInPlace = true;
    }
}
