using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sunbox.Avatars;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { SelectionSort, InsertionSort }
    public GameState currentGameState;
    //public AvatarCustomization humanoidAvatar;
    //public DynamicCharacterAvatar humanlikeAvatar;
    //public String myRecipe;

    public void Start () {
        //loadAvatar(avatar);
        //loadAvatar(humanlikeAvatar);
    }

    // public void loadAvatar (AvatarCustomization instance) {
    //         myRecipe = File.ReadAllText(Application.persistentDataPath + "/customizationDataHumanoid.txt");
    //         AvatarCustomization.ApplyConfigString(myRecipe, instance);
    // }

    // public void loadAvatar (DynamicCharacterAvatar instance) {
    //     myRecipe = File.ReadAllText(Application.persistentDataPath + "/customizationDataHumanlike.txt");
    //     humanlikeAvatar.ClearSlots();
    //     humanlikeAvatar.LoadColorsFromRecipeString(myRecipe);
    // }

    public void StartLevel(GameState state)
    {
        currentGameState = state;
        SetupLevel();
    }

    private void SetupLevel()
    {
        if (currentGameState == GameState.SelectionSort)
        {
            // Initialize selection sort specific components
            // You might spawn blocks, reset pointers, etc.
        }
        else if (currentGameState == GameState.InsertionSort)
        {
            // Initialize insertion sort specific components
        }
    }

    public void CompleteLevel()
    {
        if (currentGameState == GameState.SelectionSort)
        {
            // Load the insertion sort level (next scene)
            SceneManager.LoadScene("InsertionSortScene");
        }
        else if (currentGameState == GameState.InsertionSort)
        {
            // Handle game completion, or load the next scene/level if necessary
            // For example, you might want to return to a menu or show a victory screen
            SceneManager.LoadScene("GameCompleteScene"); // Placeholder for end game
        }
    }

    public void loadNextScene (String sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
