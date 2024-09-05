using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering;
using System.Diagnostics.Tracing;
using System.Linq;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class Customization : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;
    public Slider heightSlider;
    public Slider weightSlider;
    public Slider muscleSlider;
    public Slider armSlider;
    public Slider skinSlider;
    public Slider hairColourMSlider;
    public Slider hairColourFSlider;
    private Dictionary<string, DnaSetter> dna;

    public List<String> HairM = new List<string>();
    public List<String> HairF = new List<string>();
    public List<String> FacialHairM = new List<string>();
    public List<String> ShirtM = new List<string>();
    public List<String> PantsM = new List<string>();
    public List<String> ShoesM = new List<string>();
    public List<String> AccessoriesM = new List<string>();

    public List<String> ShirtF = new List<string>();
    public List<String> PantsF = new List<string>();

    public String myRecipe;


    void OnEnable () {
        avatar.CharacterUpdated.AddListener(updated);
        heightSlider.onValueChanged.AddListener(heightChange);
        weightSlider.onValueChanged.AddListener(weightChange);
        muscleSlider.onValueChanged.AddListener(muscleChange);
        armSlider.onValueChanged.AddListener(armChange);
        skinSlider.onValueChanged.AddListener(changeColour);
        hairColourMSlider.onValueChanged.AddListener(changeHairColour);
        hairColourFSlider.onValueChanged.AddListener(changeHairColour);
    }

    void OnDisable () {
        avatar.CharacterUpdated.RemoveListener(updated);
        heightSlider.onValueChanged.RemoveListener(heightChange);
        weightSlider.onValueChanged.RemoveListener(weightChange);
        muscleSlider.onValueChanged.RemoveListener(muscleChange);
        armSlider.onValueChanged.RemoveListener(armChange);
        skinSlider.onValueChanged.RemoveListener(changeColour);
        hairColourMSlider.onValueChanged.RemoveListener(changeHairColour);
        hairColourFSlider.onValueChanged.RemoveListener(changeHairColour);
    }

    public void switchGender (bool male) {
        if (male && avatar.activeRace.name != "HumanMale") {
            avatar.ChangeRace("HumanMale");
        }
        if (!male && avatar.activeRace.name != "HumanFemale") {
            avatar.ChangeRace("HumanFemale");
        }
    }

    void updated (UMAData data) {
        dna = avatar.GetDNA();
        heightSlider.value = dna["height"].Get();
        weightSlider.value = dna["lowerWeight"].Get();
        muscleSlider.value = dna["lowerMuscle"].Get();
        armSlider.value = dna["armWidth"].Get();
    }

    public void heightChange (float val) {
        dna["height"].Set(val);
        avatar.BuildCharacter();
    }

    public void weightChange (float val) {
        dna["lowerWeight"].Set(val);
        dna["upperWeight"].Set(val);
        dna["neckThickness"].Set(val);
        dna["belly"].Set(val);
        dna["waist"].Set(val);
        avatar.BuildCharacter();
    }

    public void muscleChange (float val) {
        dna["lowerMuscle"].Set(val);
        dna["upperMuscle"].Set(val);
        avatar.BuildCharacter();
    }

    public void armChange (float val) {
        dna["armWidth"].Set(val);
        dna["forearmWidth"].Set(val);
        avatar.BuildCharacter();
    }

    public void changeColour(float val) {
        Color col = new Color(1.1f - val, 0.9f - val, 0.7f - val);
        avatar.SetColor("Skin", col);
        avatar.UpdateColors(true);
    }

    public void changeHairM (Slider slider) {
        int val = (int)slider.value;
        if (HairM[val] == "None") {
            avatar.ClearSlot("Hair");
        }
        else {
            avatar.SetSlot("Hair", HairM[val]);
        }
        avatar.BuildCharacter();
    }

    public void changeHairF (Slider slider) {
        int val = (int)slider.value;
        if (HairF[val] == "None") {
            avatar.ClearSlot("Hair");
        }
        else {
            avatar.SetSlot("Hair", HairF[val]);
        }
        avatar.BuildCharacter();
    }

    public void changeFacialM (Slider slider) {
        int val = (int)slider.value;
        if (FacialHairM[val] == "None") {
            avatar.ClearSlot("Beard");
        }
        else {
            avatar.SetSlot("Beard", FacialHairM[val]);
        }
        avatar.BuildCharacter();
    }

    public void changeHairColour(float val) {
        Color col = new Color(1.1f - val, 0.9f - val, 0.7f - val);
        avatar.SetColor("Hair", col);
        avatar.UpdateColors(true);
    }

    public void changeShirtM (TMP_Dropdown dropdown) {
        int val = dropdown.value;
        if (ShirtM[val] == "None") {
            avatar.ClearSlot("Chest");
        }
        else {
            avatar.SetSlot("Chest", ShirtM[val]);
        }
        avatar.BuildCharacter();
    }

    public void changeShirtF (TMP_Dropdown dropdown) {
        int val = dropdown.value;
        if (ShirtF[val] == "None") {
            avatar.ClearSlot("Chest");
        }
        else {
            avatar.SetSlot("Chest", ShirtF[val]);
        }
        avatar.BuildCharacter();
    }

    public void changePantstM (TMP_Dropdown dropdown) {
        int val = dropdown.value;
        if (PantsM[val] == "None") {
            avatar.ClearSlot("Legs");
        }
        else {
            avatar.SetSlot("Legs", PantsM[val]);
        }
        avatar.BuildCharacter();
    }

    public void changePantstF (TMP_Dropdown dropdown) {
        int val = dropdown.value;
        if (PantsF[val] == "None") {
            avatar.ClearSlot("Legs");
        }
        else {
            avatar.SetSlot("Legs", PantsF[val]);
        }
        avatar.BuildCharacter();
    }

    public void changeShoestM (TMP_Dropdown dropdown) {
        int val = dropdown.value;
        if (ShoesM[val] == "None") {
            avatar.ClearSlot("Feet");
        }
        else {
            avatar.SetSlot("Feet", ShoesM[val]);
        }
        avatar.BuildCharacter();
    }

    public void changeAccessoriestM (TMP_Dropdown dropdown) {
        int val = dropdown.value;
        if (AccessoriesM[val] == "None") {
            avatar.ClearSlot("Helmet");
        }
        else {
            avatar.SetSlot("Helmet", AccessoriesM[val]);
        }
        avatar.BuildCharacter();
    }


    public void updateFace1 (Slider slider) {
        dna["cheekPosition"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace2 (Slider slider) {
        dna["cheekSize"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace3 (Slider slider) {
        dna["chinPosition"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace4 (Slider slider) {
        dna["chinPronounced"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace5 (Slider slider) {
        dna["chinSize"].Set(slider.value);
        avatar.BuildCharacter();
    }

        public void updateFace6 (Slider slider) {
        dna["earsPosition"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace7 (Slider slider) {
        dna["earsRotation"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace8 (Slider slider) {
        dna["earsSize"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace9 (Slider slider) {
        dna["eyeRotation"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace10 (Slider slider) {
        dna["eyeSize"].Set(slider.value);
        avatar.BuildCharacter();
    }

        public void updateFace11 (Slider slider) {
        dna["eyeSpacing"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace12 (Slider slider) {
        dna["foreheadPosition"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace13 (Slider slider) {
        dna["foreheadSize"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace14 (Slider slider) {
        dna["jawsPosition"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace15 (Slider slider) {
        dna["jawsSize"].Set(slider.value);
        avatar.BuildCharacter();
    }

        public void updateFace16 (Slider slider) {
        dna["lipsSize"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace17 (Slider slider) {
        dna["mandibleSize"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace18 (Slider slider) {
        dna["mouthSize"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace19 (Slider slider) {
        dna["noseCurve"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace20 (Slider slider) {
        dna["noseFlatten"].Set(slider.value);
        avatar.BuildCharacter();
    }

        public void updateFace21 (Slider slider) {
        dna["noseInclination"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace22 (Slider slider) {
        dna["nosePosition"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace23 (Slider slider) {
        dna["nosePronounced"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace24 (Slider slider) {
        dna["noseSize"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void updateFace25 (Slider slider) {
        dna["noseWidth"].Set(slider.value);
        avatar.BuildCharacter();
    }

    public void saveAvatar () {
        myRecipe = avatar.GetCurrentRecipe();
        File.WriteAllText(Application.persistentDataPath + "/customizationDataHumanlike.txt", myRecipe);
     }

    public void loadAvatar () {
        myRecipe = File.ReadAllText(Application.persistentDataPath + "/customizationDataHumanlike.txt");
        avatar.ClearSlots();
        avatar.LoadFromRecipeString(myRecipe);
    }

    public void loadNextScene (String sceneName) {
        saveAvatar();
        SceneManager.LoadScene(sceneName);
    }

}
