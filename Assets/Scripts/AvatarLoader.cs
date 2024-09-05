using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UMA;
using UMA.CharacterSystem;
using Sunbox.Avatars;

public class AvatarLoader : MonoBehaviour
{
    public enum AvatarType {Humanoid, Humanlike};
    public AvatarType currentType;
    public DynamicCharacterAvatar humanlikeAvatar;
    public AvatarCustomization humanoidAvatar;
    public string myRecipe;
    void Start()
    {
        if (currentType == AvatarType.Humanlike) {
            myRecipe = File.ReadAllText(Application.persistentDataPath + "/customizationDataHumanlike.txt");
            humanlikeAvatar.ClearSlots();
            humanlikeAvatar.LoadFromRecipeString(myRecipe);
        }
        else if (currentType == AvatarType.Humanoid) {
            myRecipe = File.ReadAllText(Application.persistentDataPath + "/customizationDataHumanoid.txt");
            AvatarCustomization.ApplyConfigString(myRecipe, humanoidAvatar);
        }
    }
}
