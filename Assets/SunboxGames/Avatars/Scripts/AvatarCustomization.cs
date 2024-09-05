
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;
using UnityEditor;
using System.Text;

namespace Sunbox.Avatars {

    public class AvatarCustomization : MonoBehaviour {

        private const string NAME = "[AvatarCustomization]";

        public enum AvatarGender {
            Male = 0,
            Female = 1
        }

        public enum OutfitType {
            Top,
            Bottom,
            WholeOutfit,
            Shoes,
            Hat,
            Other
        }

        public enum HideBlendShapeIndex {
            Nothing = -1, 

            OutfitHideShirt = 23,
            OutfitHidePants = 24,
            OutfitHideShoes = 25,

            OutfitHideHoodie = 73,
            OutfitHideShorts = 74,
            OutfitHideSleeveless = 75,
            OutfitHideBriefs = 76,
            OutfitHideDress = 77,
            OutfitHideSkirtMid=78,
            OutfitHideFlats=79
        }

        [Flags]
        public enum AvatarExpression {
            None = 0,
            Anger = 1,
            Disgust = 2,
            Fear = 4,
            Joy = 8,
            Sadness = 16
        }

        
        public const string SECTION_BODY_PARAMETERS = "Body Parameters";
        public const string SECTION_FACE_PARAMETERS = "Face Parameters";
        public const string SECTION_FEATURE_STYLES = "Features";

        public HideBlendShapeIndex[] HideBlendShapeIndices {
            get {
                if (_hideBlendShapeIndices == null) {
                    _hideBlendShapeIndices = (HideBlendShapeIndex[]) System.Enum.GetValues(typeof(HideBlendShapeIndex));
                }
                return _hideBlendShapeIndices;
            }
        }

        public SkinnedMeshRenderer CurrentGenderSkinnedRenderer {
            get {
                if (CurrentGender == AvatarGender.Male) {
                    _currentGenderSkinnedMeshRenderer = MaleBodyGEO;
                }
                if (CurrentGender == AvatarGender.Female) {
                    _currentGenderSkinnedMeshRenderer = FemaleBodyGEO;
                }

                _bodyBones = _currentGenderSkinnedMeshRenderer.bones;

                return _currentGenderSkinnedMeshRenderer;
            }
        }

        public GameObject CurrentBase {
            get {
                if (CurrentGender == AvatarGender.Male) {
                    return MaleBase;
                }
                return FemaleBase;
            }
        }

        public Animator Animator {
            get {
                return CurrentBase.GetComponent<Animator>();
            }
        }

        public RuntimeAnimatorController AnimatorController {
            get {
                return Animator.runtimeAnimatorController;
            }
            set {
                MaleBase.GetComponent<Animator>().runtimeAnimatorController = value;
                FemaleBase.GetComponent<Animator>().runtimeAnimatorController = value;
            }
        }

        public TextAsset Preset;
        
        public GameObject MaleBase;
        public GameObject FemaleBase;

        public SkinnedMeshRenderer MaleBodyGEO;
        public SkinnedMeshRenderer FemaleBodyGEO;

        public AvatarReferences AvatarReferences;

        [AvatarGenderFieldAttribute("Gender", "gender")]
        public AvatarGender CurrentGender = AvatarGender.Male;
        
        [AvatarFloatFieldAttribute("Body Fat", "bodyFat", -100, 100, Section = SECTION_BODY_PARAMETERS)]
        public float BodyFat = 0f;

        [AvatarFloatFieldAttribute("Body Muscle", "bodyMuscle", 0, 100, Section = SECTION_BODY_PARAMETERS)] 
        public float BodyMuscle = 0f;

        [AvatarFloatFieldAttribute("Body Height (m)", "bodyHeightMetres", 1.5f, 2.1f, 1.5f, 2.1f, IgnoreInPlayMode = true)] 
        public float BodyHeight = 1.8f;
        
        [AvatarFloatFieldAttribute("Breast Size (Female only)", "breastSize", -100, 100, Section = SECTION_BODY_PARAMETERS)]
        public float BreastSize = 0f;

        [AvatarFloatFieldAttribute("Nose Width", "noseWidth", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float NoseWidth = 0f;
        
        [AvatarFloatFieldAttribute("Nose Length", "noseLength", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float NoseLength = 0f;
        
        [AvatarFloatFieldAttribute("Nose Height", "noseHeight", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float NoseHeight = 0f;
        
        [AvatarFloatFieldAttribute("Nose Bridge", "noseBridge", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float NoseBridge = 0f;

        [AvatarFloatFieldAttribute("Nose Tilt", "noseTilt", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float NoseTilt = 0f;

        [AvatarFloatFieldAttribute("Mouth Width", "mouthWidth", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float MouthWidth = 0f;
        
        [AvatarFloatFieldAttribute("Lips Width", "lipsWidth", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float LipsWidth = 0f;

        [AvatarFloatFieldAttribute("Jaw Width", "jawWidth", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float JawWidth = 0f;

        [AvatarFloatFieldAttribute("Chin Width", "chinWidth", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float ChinWidth = 0f;

        [AvatarFloatFieldAttribute("Chin Protrusion", "chinProtrusion", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float ChinProtrusion = 0f;

        [AvatarFloatFieldAttribute("Chin Cleft", "chinCleft", 0, 100, Section = SECTION_FACE_PARAMETERS)]
        public float ChinCleft = 0f;

        [AvatarFloatFieldAttribute("Brow Width", "browWidth", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float BrowWidth = 0f;

        [AvatarFloatFieldAttribute("Brow Height", "browHeight", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float BrowHeight = 0f;

        [AvatarFloatFieldAttribute("Brow Protrusion", "browProtrusion", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float BrowProtrusion = 0f;

        [AvatarFloatFieldAttribute("Brow Thickness", "browThickness", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float BrowThickness = 0f;

        [AvatarFloatFieldAttribute("Brow Curve", "browCurve", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float BrowCurve = 0f;

        [AvatarFloatFieldAttribute("Eyes Size", "eyesSize", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float EyesSize = 0f;
        
        [AvatarFloatFieldAttribute("Eyes Closed Default", "eyesClosedDefault", 0, 100, Section = SECTION_FACE_PARAMETERS)]
        public float EyesClosedDefault = 0f;

        [AvatarFloatFieldAttribute("Cheeks", "cheeks", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float Cheeks = 0f;

        [AvatarFloatFieldAttribute("Ears Size", "earsSize", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float EarsSize = 0f;

        [AvatarFloatFieldAttribute("Ears Flare", "earsFlare", -100, 100, Section = SECTION_FACE_PARAMETERS)]
        public float EarsFlare = 0f;
        
        [AvatarFloatFieldAttribute("Ears Pointy", "earsPointy", 0, 100, Section = SECTION_FACE_PARAMETERS)]
        public float EarsPointy = 0f;

        [AvatarFloatFieldAttribute("Expression Joy", "expressionJoy", 0, 100, IgnoreInPlayMode = true)]
        public float ExpressionJoy = 0f;
        
        [AvatarFloatFieldAttribute("Expression Anger", "expressionAnger", 0, 100, IgnoreInPlayMode = true)]
        public float ExpressionAnger = 0f;
        
        [AvatarFloatFieldAttribute("Expression Sadness", "expressionSadness", 0, 100, IgnoreInPlayMode = true)]
        public float ExpressionSadness = 0f;
        
        [AvatarFloatFieldAttribute("Expression Fear", "expressionFear", 0, 100, IgnoreInPlayMode = true)]
        public float ExpressionFear = 0f;

        [AvatarFloatFieldAttribute("Expression Disgust", "expressionDisgust", 0, 100, IgnoreInPlayMode = true)]
        public float ExpressionDisgust = 0f;
        
        [AvatarIntegerField("Skin Material", "skinMaterialIndex", Section = SECTION_FEATURE_STYLES, ArrayDependancyField = "MaleSkinMaterials")]
        public int SkinMaterialIndex = 0;

        [AvatarIntegerField("Hair Style", "hairStyleIndex", Section = SECTION_FEATURE_STYLES, ArrayDependancyField = "HairItems")]
        public int HairStyleIndex = 0;

        [AvatarIntegerField("Hair Material", "hairMaterialIndex", Section = SECTION_FEATURE_STYLES, ArrayDependancyField = "HairItems", IsVariationField = true)]
        public int HairMaterialIndex = 0;

        [AvatarIntegerField("Facial Hair Style", "facialHairStyleIndex", Section = SECTION_FEATURE_STYLES, ArrayDependancyField = "FacialHairItems")]
        public int FacialHairStyleIndex = 0;

        [AvatarIntegerField("Facial Hair Material", "facialHairMaterialIndex", Section = SECTION_FEATURE_STYLES, ArrayDependancyField = "FacialHairItems", IsVariationField = true)]
        public int FacialHairMaterialIndex = 0;

        [AvatarIntegerField("Eye Material", "eyeMaterialIndex", Section = SECTION_FEATURE_STYLES, ArrayDependancyField = "EyeMaterials")]
        public int EyeMaterialIndex = 0;

        [AvatarIntegerField("Lashes Material", "lashesMaterialIndex", Section = SECTION_FEATURE_STYLES, ArrayDependancyField = "LashesMaterials")]
        public int LashesMaterialIndex = 0;

        [AvatarIntegerField("Brow Material", "browMaterialIndex", Section = SECTION_FEATURE_STYLES, ArrayDependancyField = "BrowMaterials")]
        public int BrowMaterialIndex = 0;

        [AvatarIntegerField("Nails Material", "nailsMaterialIndex", Section = SECTION_FEATURE_STYLES, ArrayDependancyField = "NailMaterials")]
        public int NailsMaterialIndex = 0;

        [AvatarFloatFieldAttribute("Nails Length", "nailsLength", 0, 100, Section = SECTION_BODY_PARAMETERS)]
        public float NailsLength = 0f;

        [AvatarFloatFieldAttribute("Nails Curve", "nailsCurve", 0, 100, Section = SECTION_BODY_PARAMETERS)]
        public float NailsCurve = 0f;

        public ClothingItem ClothingItemHat;
        public int ClothingItemHatVariationIndex;

        public ClothingItem ClothingItemTop;
        public int ClothingItemTopVariationIndex;

        public ClothingItem ClothingItemBottom;
        public int ClothingItemBottomVariationIndex;

        public ClothingItem ClothingItemGlasses;
        public int ClothingItemGlassesVariationIndex;

        public ClothingItem ClothingItemShoes;
        public int ClothingItemShoesVariationIndex;

        [SerializeField]
        public int[] ClothingItemVariationIndices = new int[16];

        [Range(0, 11.7f)] 
        public float BlinkingInterval = 5f;
        
        [Range(0, 0.99f)] 
        public float BlinkSpeed = 0.5f;

        public float ExpressionChangeSpeed = 1f;

        private HideBlendShapeIndex[] _hideBlendShapeIndices;
        private Slot[] _avatarSlots;
        private SkinnedMeshRenderer _currentGenderSkinnedMeshRenderer;
        private Transform[] _bodyBones;
        private Dictionary<SlotType, SkinnedMeshRenderer> _activeSkinnedClothingItems = new Dictionary<SlotType, SkinnedMeshRenderer>();
        private Dictionary<SlotType, UClothingItem> _currentInstantiatedClothingItems = new Dictionary<SlotType, UClothingItem>();
        private GameObject _facialHairObject;
        private GameObject _hairGameObject;
        private float _eyesClosed = 0;
        private float _eyesClosedExpression =0;
        private float _adjustedBodyFat = 0;
        private float _adjustedBodyMuscle = 0;
        private float _blinkTimer = 0;

        private float _targetExpressionJoy = 0f;
        private float _targetExpressionAnger = 0f;
        private float _targetExpressionSadness = 0f;
        private float _targetExpressionFear = 0f;
        private float _targetExpressionDisgust = 0f;
        private float _currentExpressionJoy = 0f;
        private float _currentExpressionAnger = 0f;
        private float _currentExpressionSadness = 0f;
        private float _currentExpressionFear = 0f;
        private float _currentExpressionDisgust = 0f;
        private AvatarExpression _targetExpression = AvatarExpression.None;

        void Start() {
            SetGender(CurrentGender, true);

            UpdateCustomization();
            UpdateClothing_Internal();
        }

        void Update() {
            UpdateBlinking_Internal();
            UpdateExpression_Internal();
        }

        /// <summary>
        /// Randomizes avatar (body and hair) parameters.
        /// </summary>
        /// <param name="seed">Seed used with randomizer. Use -1 if you don't want to use the seed.</param>
        /// <param name="ignoreHeight">Ignores the height randomization and defaults to 1.8m.</param>
        /// <param name="unifiedHairColors">Unifies all hair colors (brows, hair, facial hair).</param>
        /// <param name="noRandomExpressions">Ignores expression randomization.</param>
        /// <param name="randomizeGender">Randomizes gender if set to true.</param>
        public void RandomizeBodyParameters(int seed = -1, bool ignoreHeight = true, bool unifiedHairColors = true, bool noRandomExpressions = true, bool randomizeGender = true) {
            if (seed != -1) {
                UnityEngine.Random.InitState(seed);
            }
            
            FieldInfo[] fields = typeof(AvatarCustomization).GetFields();

            // Set float values
            foreach (FieldInfo field in fields) {
                AvatarFieldAttribute avatarFieldAttribute = field.GetCustomAttribute<AvatarFieldAttribute>();

                if (avatarFieldAttribute == null) {
                    continue;
                }

                if (avatarFieldAttribute is AvatarFloatFieldAttribute) {
                    AvatarFloatFieldAttribute floatFieldAttribute = (AvatarFloatFieldAttribute) avatarFieldAttribute;
                    field.SetValue(this, UnityEngine.Random.Range(floatFieldAttribute.SourceMinValue, floatFieldAttribute.SourceMaxValue));
                }
            }

            // Eyes closed default should be > 0.5
            EyesClosedDefault = 20;

            // Height
            if (ignoreHeight) {
                BodyHeight = 1.8f;
            }

            // Expressions
            if (noRandomExpressions) {
                ExpressionAnger = 0;
                ExpressionDisgust = 0;
                ExpressionFear = 0;
                ExpressionJoy = 0;
                ExpressionSadness = 0;
            }

            // Set gender
            if (randomizeGender) {
                AvatarGender gender = UnityEngine.Random.value < 0.5f ? AvatarGender.Male : AvatarGender.Female;;
                CurrentGender = gender;

                _currentGenderSkinnedMeshRenderer = null;
                
                // Update bones and mesh renderer
                var skinnedMeshRenderer = CurrentGenderSkinnedRenderer;

                MaleBase.SetActive(CurrentGender == AvatarGender.Male);
                FemaleBase.SetActive(CurrentGender == AvatarGender.Female);
            }

            // Set skin
            if (CurrentGender == AvatarGender.Male) {
                SkinMaterialIndex = UnityEngine.Random.Range(0, AvatarReferences.MaleSkinMaterials.Length);
            }
            if (CurrentGender == AvatarGender.Female) {
                SkinMaterialIndex = UnityEngine.Random.Range(0, AvatarReferences.FemaleSkinMaterials.Length);
            }

            // Set hair
            HairStyleIndex = UnityEngine.Random.Range(0, AvatarReferences.HairItems.Length);
            HairMaterialIndex = UnityEngine.Random.Range(0, AvatarReferences.HairItems[HairStyleIndex].Variations.Length);

            // Set facial hair
            if (CurrentGender == AvatarGender.Male) {
                FacialHairStyleIndex = UnityEngine.Random.Range(0, AvatarReferences.FacialHairItems.Length);
                FacialHairMaterialIndex = UnityEngine.Random.Range(0, AvatarReferences.FacialHairItems[FacialHairStyleIndex].Variations.Length);
            }

            if (unifiedHairColors) {
                FacialHairMaterialIndex = HairMaterialIndex;
                BrowMaterialIndex = HairMaterialIndex;
            }

            // Set eyes
            EyeMaterialIndex = UnityEngine.Random.Range(0, AvatarReferences.EyeMaterials.Length);

            // Set eyebrow
            if (!unifiedHairColors) {
                BrowMaterialIndex = UnityEngine.Random.Range(0, AvatarReferences.BrowMaterials.Length);
            }

            UpdateCustomization();
        }

        /// <summary>
        /// Randomizes all clothing items and their colors/materials.
        /// </summary>
        /// <param name="seed">Randomization seed to apply. Using -1 will randomize it every time you call this method.</param>
        /// <param name="nudity">Enabled nudity can produce empty top or bottom clothing item.</param>
        public void RandomizeClothing(int seed = -1, bool nudity = false) {
            if (seed != -1) {
                UnityEngine.Random.InitState(seed);
            }

            // Set clothing items
            ClothingItemHat = AvatarReferences.AvailableClothingItems.RandomFirst(item => item.SlotType == SlotType.Hat);

            if (nudity) {
                ClothingItemTop = AvatarReferences.AvailableClothingItems.RandomFirst(item => item.SlotType == SlotType.Top);
                ClothingItemBottom = AvatarReferences.AvailableClothingItems.RandomFirst(item => item.SlotType == SlotType.Bottom);
            }
            else {
                ClothingItemTop = AvatarReferences.AvailableClothingItems.RandomFirst(item => item.SlotType == SlotType.Top && !item.IsEmpty);
                ClothingItemBottom = AvatarReferences.AvailableClothingItems.RandomFirst(item => item.SlotType == SlotType.Bottom && !item.IsEmpty);
            }

            ClothingItemGlasses = AvatarReferences.AvailableClothingItems.RandomFirst(item => item.SlotType == SlotType.Glasses);
            ClothingItemShoes = AvatarReferences.AvailableClothingItems.RandomFirst(item => item.SlotType == SlotType.Shoes);

            ClothingItemHatVariationIndex = ClothingItemHat == null || ClothingItemHat.Variations.Length == 1 ? 0 : UnityEngine.Random.Range(0, ClothingItemHat.Variations.Length);
            ClothingItemTopVariationIndex = ClothingItemTop == null || ClothingItemTop.Variations.Length == 1 ? 0 : UnityEngine.Random.Range(0, ClothingItemTop.Variations.Length);
            ClothingItemBottomVariationIndex = ClothingItemBottom == null || ClothingItemBottom.Variations.Length == 1 ? 0 : UnityEngine.Random.Range(0, ClothingItemBottom.Variations.Length);
            ClothingItemGlassesVariationIndex = ClothingItemGlasses == null || ClothingItemGlasses.Variations.Length == 1 ? 0 : UnityEngine.Random.Range(0, ClothingItemGlasses.Variations.Length);
            ClothingItemShoesVariationIndex = ClothingItemShoes == null || ClothingItemShoes.Variations.Length == 1 ? 0 : UnityEngine.Random.Range(0, ClothingItemShoes.Variations.Length);

            UpdateClothing();
        }

        /// <summary>
        /// Sets the expression. You can combine expressions using enum flags.
        /// If AvatarExpression.None is selected then the expression returns to the one set
        /// in expression properties.
        /// </summary>
        /// <param name="expression"></param>
        public void SetExpression(AvatarExpression expression) {
            if (expression == AvatarExpression.None) {
                _targetExpressionAnger = ExpressionAnger;
                _targetExpressionDisgust = ExpressionDisgust;
                _targetExpressionFear = ExpressionFear;
                _targetExpressionJoy = ExpressionJoy;
                _targetExpressionSadness = ExpressionSadness;

                _targetExpression = AvatarExpression.None;

                return;
            }

            _targetExpressionAnger = expression.HasFlag(AvatarExpression.Anger) ? 100 : 0;
            _targetExpressionDisgust = expression.HasFlag(AvatarExpression.Disgust) ? 100 : 0;
            _targetExpressionFear = expression.HasFlag(AvatarExpression.Fear) ? 100 : 0;
            _targetExpressionJoy = expression.HasFlag(AvatarExpression.Joy) ? 100 : 0;
            _targetExpressionSadness = expression.HasFlag(AvatarExpression.Sadness) ? 100 : 0;
            _targetExpression = expression;
        }
        
        /// <summary>
        /// Attaches clothing item to appopriate slot.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="variationIndex"></param>
        public void AttachClothingItem(ClothingItem item, int variationIndex = 0) {
            if (item.SlotType == SlotType.Hat) {
                ClothingItemHat = item;
                ClothingItemHatVariationIndex = variationIndex;
            }
            if (item.SlotType == SlotType.Top) {
                ClothingItemTop = item;
                ClothingItemTopVariationIndex = variationIndex;
            }
            if (item.SlotType == SlotType.Bottom) {
                ClothingItemBottom = item;
                ClothingItemBottomVariationIndex = variationIndex;
            }
            if (item.SlotType == SlotType.Glasses) {
                ClothingItemGlasses = item;
                ClothingItemGlassesVariationIndex = variationIndex;
            }
            if (item.SlotType == SlotType.Shoes) {
                ClothingItemShoes = item;
                ClothingItemShoesVariationIndex = variationIndex;
            }

            UpdateClothing();
        }

        /// <summary>
        /// Sets the clothing item variation from slot
        /// </summary>
        /// <param name="slotType"></param>
        /// <param name="value"></param>
        public void SetClothingItemVariation(SlotType slotType, int value) {
            if (slotType == SlotType.Hat) {
                ClothingItemHatVariationIndex = value;
            }
            if (slotType == SlotType.Top) {
                ClothingItemTopVariationIndex = value;
            }
            if (slotType == SlotType.Bottom) {
                ClothingItemBottomVariationIndex = value;
            }
            if (slotType == SlotType.Glasses) {
                ClothingItemGlassesVariationIndex = value;
            }
            if (slotType == SlotType.Shoes) {
                ClothingItemShoesVariationIndex = value;
            }

            UpdateClothing();
        }

        public ClothingItem GetClothingItemFromSlot(SlotType slotType) {
            if (slotType == SlotType.Hat) {
                return ClothingItemHat;
            }
            if (slotType == SlotType.Top) {
                return ClothingItemTop;
            }
            if (slotType == SlotType.Bottom) {
                return ClothingItemBottom;
            }
            if (slotType == SlotType.Glasses) {
                return ClothingItemGlasses;
            }
            if (slotType == SlotType.Shoes) {
                return ClothingItemShoes;
            }
            return null;
        }

        public int GetClothingItemVariationIndex(SlotType slotType) {
            if (slotType == SlotType.Hat) {
                return ClothingItemHatVariationIndex;
            }
            if (slotType == SlotType.Top) {
                return ClothingItemTopVariationIndex;
            }
            if (slotType == SlotType.Bottom) {
                return ClothingItemBottomVariationIndex;
            }
            if (slotType == SlotType.Glasses) {
                return ClothingItemGlassesVariationIndex;
            }
            if (slotType == SlotType.Shoes) {
                return ClothingItemShoesVariationIndex;
            }
            return 0;
        }

        /// <summary>
        /// Set the current gender.
        /// </summary>
        /// <param name="gender">Gender to apply</param>
        /// <param name="force">Force even if the current gender is the one you want to change to</param>
        public void SetGender(AvatarGender gender, bool force = false) {
            if (CurrentGender != gender || force) {

                CurrentGender = gender;

                _currentGenderSkinnedMeshRenderer = null;
                
                //ResetCostumization_Internal();
                //UpdateClothing_Internal();

                MaleBase.SetActive(CurrentGender == AvatarGender.Male);
                FemaleBase.SetActive(CurrentGender == AvatarGender.Female);

                UpdateCustomization();
                UpdateClothing();
            }
        }

        /// <summary>
        /// Updates the customization. Call this if you make any changes to the avatar.
        /// Doesn't update avatar clothing, for that call UpdateClothing(). 
        /// </summary>
        public void UpdateCustomization() {
            UpdateHeight_Internal();
            UpdateBodyParameters_Internal();

            UpdateFace_Internal();
            UpdateFacialHair_Shape_Internal();
            
            UpdateHair_Internal();

            UpdateExpression_Internal();

            UpdateFacialHair_Internal();

            UpdateMaterials_Internal();
            UpdateBlinking_Internal();
        }

        /// <summary>
        /// Updates and invalidates clothing items on avatar.
        /// </summary>
        public void UpdateClothing() {
            UpdateClothing_Internal();
        }

        /// <summary>
        /// Prepares config string that you can store as a text asset (preset).
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToConfigString(AvatarCustomization instance) {
            const string CLOTHING_ITEM_KEY = "clothingItem";

            StringBuilder sb = new StringBuilder();
            
            FieldInfo[] fields = typeof(AvatarCustomization).GetFields();

            // Variables
            foreach (FieldInfo fieldInfo in fields) {
                AvatarFieldAttribute fieldAttribute = fieldInfo.GetCustomAttribute<AvatarFieldAttribute>();
                if (fieldAttribute != null) {
                    sb.AppendLine($"{fieldAttribute.Prefix}{fieldAttribute.VariableName}={fieldAttribute.GetValueString(fieldInfo, instance)}");
                }
            }

            // Clothing items
            if (instance.ClothingItemHat != null) {
                sb.AppendLine($"{CLOTHING_ITEM_KEY}={instance.ClothingItemHat.Name}-{instance.ClothingItemHatVariationIndex}");
            }
            
            if (instance.ClothingItemTop != null) {
                sb.AppendLine($"{CLOTHING_ITEM_KEY}={instance.ClothingItemTop.Name}-{instance.ClothingItemTopVariationIndex}");
            }
            
            if (instance.ClothingItemBottom != null) {
                sb.AppendLine($"{CLOTHING_ITEM_KEY}={instance.ClothingItemBottom.Name}-{instance.ClothingItemBottomVariationIndex}");
            }

            if (instance.ClothingItemGlasses != null) {
                sb.AppendLine($"{CLOTHING_ITEM_KEY}={instance.ClothingItemGlasses.Name}-{instance.ClothingItemGlassesVariationIndex}");
            }
            
            if (instance.ClothingItemShoes != null) {
                sb.AppendLine($"{CLOTHING_ITEM_KEY}={instance.ClothingItemShoes.Name}-{instance.ClothingItemShoesVariationIndex}");
            }
 
            return sb.ToString();
        }

        /// <summary>
        /// Applies all preset data from a config string to an Avatar instance.
        /// </summary>
        /// <param name="configFile"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool ApplyConfigFile(TextAsset configFile, AvatarCustomization instance) {
            return ApplyConfigString(configFile.text, instance);
        }

        /// <summary>
        /// Applies all preset data from a text asset to an Avatar instance.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool ApplyConfigString(string configString, AvatarCustomization instance) {
            const string CLOTHING_ITEM_KEY = "clothingItem";

            FieldInfo[] fields = typeof(AvatarCustomization).GetFields();
            
            // To dict
            Dictionary<string, string> configValues = new Dictionary<string, string>();
            string[] configLines = configString.Split('\n');
            
            List<ClothingItem> clothingItemsToAttach = new List<ClothingItem>();
            List<int> clothingItemVariationIndices = new List<int>();

            foreach (string line in configLines) {
                if (line.Contains("=")) {
                    string[] splitted = line.Split('=');
                    if (splitted.Length == 2) {
                        if (splitted[0] == CLOTHING_ITEM_KEY) {
                            string clothingItemName = splitted[1].Split('-')[0].Trim();
                            ClothingItem clothingItem = instance.AvatarReferences.AvailableClothingItems.FirstOrDefault(item => item.Name == clothingItemName);
                            int clothingItemVariationIndex = int.Parse(splitted[1].Split('-')[1].Trim());

                            if (clothingItem != null) {
                                clothingItemsToAttach.Add(clothingItem);
                                clothingItemVariationIndices.Add(clothingItemVariationIndex);
                            }
                            else {
                                Debug.Log($"[Avatars] Clothing item {clothingItemName} not found. Add reference to Available Clothing Items");
                            }
                        }
                        else {
                            string key = splitted[0].Split('_')[1].Trim();
                            configValues.Add(key.Trim(), splitted[1].Trim());
                        }
                    }
                }
            }

            foreach (FieldInfo fieldInfo in fields) {
                AvatarFieldAttribute fieldAttribute = fieldInfo.GetCustomAttribute<AvatarFieldAttribute>();
                if (fieldAttribute != null && fieldAttribute is AvatarFloatFieldAttribute) {
                    if (configValues.ContainsKey(fieldAttribute.VariableName)) {
                        fieldInfo.SetValue(instance, float.Parse(configValues[fieldAttribute.VariableName]));
                    }
                }

                if (fieldAttribute != null && fieldAttribute is AvatarIntegerFieldAttribute) {
                    if (configValues.ContainsKey(fieldAttribute.VariableName)) {
                        fieldInfo.SetValue(instance, int.Parse(configValues[fieldAttribute.VariableName]));
                    }
                }

                if (fieldAttribute != null && fieldAttribute is AvatarGenderFieldAttribute) {
                    if (configValues.ContainsKey(fieldAttribute.VariableName)) {
                        fieldInfo.SetValue(instance, (AvatarCustomization.AvatarGender) Enum.Parse(typeof(AvatarCustomization.AvatarGender), configValues[fieldAttribute.VariableName]));
                    }
                }

                if (fieldAttribute != null && fieldAttribute is AvatarColorFieldAttribute) {
                    // Undefined
                }
            }

            instance.ClothingItemTop = null;
            instance.ClothingItemBottom = null;
            instance.ClothingItemShoes = null;
            instance.ClothingItemHat = null;
            instance.ClothingItemGlasses = null;

            for (int i = 0; i < clothingItemsToAttach.Count; i++) {
                instance.AttachClothingItem(clothingItemsToAttach[i], clothingItemVariationIndices[i]);
            }

            instance.UpdateClothing();

            return true;
        }

        private void SetBlendShapes_Internal(float amount, int min, int max, SkinnedMeshRenderer body) {
            if (amount >= 0){
                body.SetBlendShapeWeight(max, amount);
                body.SetBlendShapeWeight(min, 0);
            }
            else {
                body.SetBlendShapeWeight(min, -amount);
                body.SetBlendShapeWeight(max, 0);
            }
        }

        private void UpdateHair_Internal() {

            UHair[] existingHairs = CurrentBase.GetComponentsInChildren<UHair>(includeInactive: true);

            // Remove hair if none is available in the hair items pool
            if (AvatarReferences.HairItems == null || AvatarReferences.HairItems.Length == 0) {
                foreach (UHair hair in existingHairs) {
                    SafeDestroyGameObject_Internal(hair.gameObject);
                }

                return;
            }

            int index = Mathf.Clamp(HairStyleIndex, 0, AvatarReferences.HairItems.Length - 1);
            int materialIndex = AvatarReferences.HairItems[index].HasVariations() ? Mathf.Clamp(HairMaterialIndex, 0, AvatarReferences.HairItems[index].Variations.Length - 1) : 0;
            
            // Remove excess hair
            _hairGameObject = null;
            UHair hairInstance = null;
            foreach (UHair hair in existingHairs) {
                if (hair.HairItem == AvatarReferences.HairItems[index] && _hairGameObject == null) {
                    _hairGameObject = hair.gameObject;
                    continue;
                }

                SafeDestroyGameObject_Internal(hair.gameObject);
            }

            if (_hairGameObject == null) {
                _hairGameObject = Instantiate(AvatarReferences.HairItems[index].HairMesh).gameObject;
                _hairGameObject.transform.parent = CurrentBase.transform;
                
                hairInstance = _hairGameObject.AddComponent<UHair>();
                hairInstance.HairItem = AvatarReferences.HairItems[index];

                _avatarSlots = CurrentGenderSkinnedRenderer.transform.parent.GetComponentsInChildren<Slot>();
                Slot slot = _avatarSlots.FirstOrDefault(slot => slot.SlotType == SlotType.Hair);
                _hairGameObject.transform.position = slot.BoneTransform.position;
                _hairGameObject.transform.rotation = slot.BoneTransform.rotation;
                _hairGameObject.transform.localScale = Vector3.one;
                _hairGameObject.transform.parent = slot.BoneTransform;
            }

            if (_hairGameObject != null) {
                hairInstance = _hairGameObject.GetComponent<UHair>();
                MeshRenderer renderer = _hairGameObject.GetComponent<MeshRenderer>();
                renderer.sharedMaterial = AvatarReferences.HairItems[index].HasVariations() ? AvatarReferences.HairItems[index].Variations[materialIndex] : null;
            }

            // Check if there is a hat and hair doesn't allow it
            UClothingItem hatClothingInstance = CurrentBase.GetComponentsInChildren<UClothingItem>().Where(item => item.ClothingItem.SlotType == SlotType.Hat).FirstOrDefault();
            if (hatClothingInstance != null && !hatClothingInstance.ClothingItem.IsEmpty && hairInstance.HairItem.HideHairWhenHatEquipped) {
                _hairGameObject.SetActive(hatClothingInstance == null);
            }
            if (hatClothingInstance != null) {
                Slot slot = _avatarSlots.FirstOrDefault(slot => slot.SlotType == SlotType.Hair);
                hatClothingInstance.transform.parent = CurrentBase.transform;
                hatClothingInstance.transform.localScale = Vector3.one;
                hatClothingInstance.transform.parent = slot.BoneTransform;
                hatClothingInstance.transform.localPosition = AvatarReferences.HairItems[index].HatOffset;
            }
        }

        private void UpdateFacialHair_Internal() {
            if (CurrentGender != AvatarGender.Male) {
                return;
            }

            UFacialHair[] existingFacialHair = CurrentBase.GetComponentsInChildren<UFacialHair>();

            if (AvatarReferences.FacialHairItems == null || AvatarReferences.FacialHairItems.Length == 0) {
                foreach (UFacialHair facialHair in existingFacialHair) {
                    SafeDestroyGameObject_Internal(facialHair.gameObject);
                }

                return;
            }

            int index = Mathf.Clamp(FacialHairStyleIndex, 0, AvatarReferences.FacialHairItems.Length - 1);
            int materialIndex = AvatarReferences.FacialHairItems[index].HasVariations() ? Mathf.Clamp(FacialHairMaterialIndex, 0, AvatarReferences.FacialHairItems[index].Variations.Length - 1) : 0;
            
            _facialHairObject = null;
            foreach (UFacialHair facialHair in existingFacialHair) {
                if (facialHair.FacialHairItem == AvatarReferences.FacialHairItems[index]) {
                    _facialHairObject = facialHair.gameObject;
                    continue;
                }

                SafeDestroyGameObject_Internal(facialHair.gameObject);
            }

            if (_facialHairObject == null) {
                _facialHairObject = Instantiate(AvatarReferences.FacialHairItems[index].FacialHairmesh).gameObject;
                _facialHairObject.transform.parent = CurrentBase.transform;
                
                _facialHairObject.AddComponent<UFacialHair>().FacialHairItem = AvatarReferences.FacialHairItems[index];
                SkinnedMeshRenderer renderer = _facialHairObject.GetComponent<SkinnedMeshRenderer>();
                renderer.bones = _bodyBones;
                renderer.sharedMaterial = AvatarReferences.FacialHairItems[index].HasVariations() ? AvatarReferences.FacialHairItems[index].Variations[materialIndex] : null;
            }

            if (_facialHairObject != null) {
                SkinnedMeshRenderer renderer = _facialHairObject.GetComponent<SkinnedMeshRenderer>();
                renderer.sharedMaterial = AvatarReferences.FacialHairItems[index].HasVariations() ? AvatarReferences.FacialHairItems[index].Variations[materialIndex] : null;
                renderer.updateWhenOffscreen = true;
            }

            UpdateFacialHair_Shape_Internal();
        }

        private void UpdateFacialHair_Shape_Internal() {
            if (_facialHairObject) {
                SkinnedMeshRenderer skinnedMeshRender = _facialHairObject.GetComponent<SkinnedMeshRenderer>();
                SetBlendShapes_Internal(NoseWidth, Constants.NOSEWIDTH_MIN, Constants.NOSEWIDTH_MAX, skinnedMeshRender);
                SetBlendShapes_Internal(NoseLength, Constants.NOSELENGTH_MIN, Constants.NOSELENGTH_MAX,skinnedMeshRender);
                SetBlendShapes_Internal(NoseHeight, Constants.NOSEHEIGHT_MIN, Constants.NOSEHEIGHT_MAX, skinnedMeshRender);
                SetBlendShapes_Internal(NoseBridge, Constants.NOSEBRIDGE_MIN, Constants.NOSEBRIDGE_MAX, skinnedMeshRender);
                SetBlendShapes_Internal(NoseTilt, Constants.NOSETILT_MIN, Constants.NOSETILT_MAX, skinnedMeshRender);

                SetBlendShapes_Internal(MouthWidth, Constants.MOUTHWIDTH_MIN, Constants.MOUTHWIDTH_MAX, skinnedMeshRender);
                SetBlendShapes_Internal(LipsWidth, Constants.LIPSWIDTH_MIN, Constants.LIPSWIDTH_MAX, skinnedMeshRender);
                SetBlendShapes_Internal(Cheeks, Constants.CHEEKS_MIN, Constants.CHEEKS_MAX, skinnedMeshRender);
       
                SetBlendShapes_Internal(JawWidth, Constants.JAWWIDTH_MIN, Constants.JAWWIDTH_MAX, skinnedMeshRender);
                SetBlendShapes_Internal(ChinWidth, Constants.CHINWIDTH_MIN, Constants.CHINWIDTH_MAX, skinnedMeshRender);
                SetBlendShapes_Internal(ChinProtrusion, Constants.CHINPROTRUSION_MIN, Constants.CHINPROTRUSION_MAX, skinnedMeshRender);

                SetBlendShapes_Internal(EyesSize, Constants.EYESIZE_MIN, Constants.EYESIZE_MAX, skinnedMeshRender);
                SetBlendShapes_Internal(EarsSize, Constants.EARSIZE_MIN, Constants.EARSIZE_MAX, skinnedMeshRender);
                SetBlendShapes_Internal(EarsFlare, Constants.EARSFLARE_MIN, Constants.EARSFLARE_MAX, skinnedMeshRender);

                if (BodyMuscle + Mathf.Abs(BodyFat) <= 100){
                    SetBlendShapes_Internal(BodyFat, Constants.BODY_SKINNY, Constants.BODY_CHUBBY, skinnedMeshRender);
                    skinnedMeshRender.SetBlendShapeWeight(Constants.BODY_MUSCLE, BodyMuscle);
                
                }
                else{
                    _adjustedBodyFat= BodyFat * 100/(Mathf.Abs(BodyFat)+BodyMuscle);
                    _adjustedBodyMuscle= BodyMuscle * 100/(Mathf.Abs(BodyFat)+BodyMuscle);

                    SetBlendShapes_Internal(_adjustedBodyFat, Constants.BODY_SKINNY, Constants.BODY_CHUBBY, skinnedMeshRender);
                    skinnedMeshRender.SetBlendShapeWeight(Constants.BODY_MUSCLE, _adjustedBodyMuscle);

                }
                
                skinnedMeshRender.SetBlendShapeWeight(Constants.CHINCLEFT_MAX, ChinCleft);
            }
        }

        private void UpdateBlinking_Internal(){
            if (_blinkTimer < 12 - BlinkingInterval + (1f - BlinkSpeed)){
                _blinkTimer += Time.deltaTime;
            }
            else{
                _blinkTimer = 0;
            }
            
            if (_blinkTimer > 12 - BlinkingInterval) {
                float closedAmount = 1 - 2 * Mathf.Abs((_blinkTimer - (12 - BlinkingInterval)) / ((1f - BlinkSpeed)) - 0.5f);
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EYES_BLINK, Mathf.Lerp(_eyesClosed, 100, closedAmount));
            }
            else {
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EYES_BLINK, _eyesClosed);
            }
        }

        private void UpdateExpression_Internal(bool force = false) {
            if  (_currentExpressionAnger == _targetExpressionAnger && 
                _currentExpressionDisgust == _targetExpressionDisgust && 
                _currentExpressionFear == _targetExpressionFear && 
                _currentExpressionJoy == _targetExpressionJoy &&
                _currentExpressionSadness == _targetExpressionSadness && 
                _targetExpression != AvatarExpression.None) {
                return;
            }

            if (_targetExpression == AvatarExpression.None) {
                _targetExpressionAnger = ExpressionAnger;
                _targetExpressionDisgust = ExpressionDisgust;
                _targetExpressionFear = ExpressionFear;
                _targetExpressionJoy = ExpressionJoy;
                _targetExpressionSadness = ExpressionSadness;
            }

            float dt = Time.deltaTime * 100 * (force ? 1000000 : ExpressionChangeSpeed);

            _currentExpressionJoy = Mathf.MoveTowards(_currentExpressionJoy, _targetExpressionJoy, dt);
            _currentExpressionAnger = Mathf.MoveTowards(_currentExpressionAnger, _targetExpressionAnger, dt);
            _currentExpressionSadness = Mathf.MoveTowards(_currentExpressionSadness, _targetExpressionSadness, dt);
            _currentExpressionFear = Mathf.MoveTowards(_currentExpressionFear, _targetExpressionFear, dt);
            _currentExpressionDisgust = Mathf.MoveTowards(_currentExpressionDisgust, _targetExpressionDisgust, dt);

            float total = _currentExpressionJoy + _currentExpressionAnger + _currentExpressionSadness + _currentExpressionFear + _currentExpressionDisgust;

            if (total <= 200){
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EXPRESSION_JOY, _currentExpressionJoy);
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EXPRESSION_ANGER, _currentExpressionAnger);
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EXPRESSION_SADNESS, _currentExpressionSadness);
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EXPRESSION_FEAR, _currentExpressionFear);
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EXPRESSION_DISGUST, _currentExpressionDisgust);

                _eyesClosedExpression = _currentExpressionJoy * Constants.EXPRESSION_EYELID_JOY / 100 +
                                       _currentExpressionAnger * Constants.EXPRESSION_EYELID_ANGER / 100 + 
                                       _currentExpressionSadness * Constants.EXPRESSION_EYELID_SADNESS / 100 +
                                       _currentExpressionFear * Constants.EXPRESISON_EYELID_FEAR / 100 + 
                                       _currentExpressionDisgust * Constants.EXPRESSION_EYELID_DISGUST / 100;
                
                if (_facialHairObject != null) {
                    SkinnedMeshRenderer skinnedMeshRenderer = _facialHairObject.GetComponent<SkinnedMeshRenderer>();
                    skinnedMeshRenderer.SetBlendShapeWeight(Constants.EXPRESSION_JOY, _currentExpressionJoy);
                    skinnedMeshRenderer.SetBlendShapeWeight(Constants.EXPRESSION_ANGER, _currentExpressionAnger);
                    skinnedMeshRenderer.SetBlendShapeWeight(Constants.EXPRESSION_SADNESS, _currentExpressionSadness);
                    skinnedMeshRenderer.SetBlendShapeWeight(Constants.EXPRESSION_FEAR, _currentExpressionFear);
                    skinnedMeshRenderer.SetBlendShapeWeight(Constants.EXPRESSION_DISGUST, _currentExpressionDisgust);
                }
                
            }
            else {
                float divider = 200f / total;

                float adjustedJoy = _currentExpressionJoy * divider;
                float adjustedAnger = _currentExpressionAnger * divider;
                float adjustedSadness = _currentExpressionSadness * divider;
                float adjustedFear = _currentExpressionFear * divider;
                float adjustedDisgust = _currentExpressionDisgust * divider;

                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EXPRESSION_JOY, adjustedJoy);
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EXPRESSION_ANGER, adjustedAnger);
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EXPRESSION_SADNESS, adjustedSadness);
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EXPRESSION_FEAR, adjustedFear);
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EXPRESSION_DISGUST, adjustedDisgust);

                _eyesClosedExpression = adjustedJoy * Constants.EXPRESSION_EYELID_JOY / 100 + 
                                       adjustedAnger * Constants.EXPRESSION_EYELID_ANGER / 100 + 
                                       adjustedSadness * Constants.EXPRESSION_EYELID_SADNESS / 100 + 
                                       adjustedFear * Constants.EXPRESISON_EYELID_FEAR / 100 + 
                                       adjustedDisgust * Constants.EXPRESSION_EYELID_DISGUST / 100;

                if (_facialHairObject != null) {
                    SkinnedMeshRenderer skinnedMeshRenderer = _facialHairObject.GetComponent<SkinnedMeshRenderer>();
                    skinnedMeshRenderer.SetBlendShapeWeight(Constants.EXPRESSION_JOY, adjustedJoy);
                    skinnedMeshRenderer.SetBlendShapeWeight(Constants.EXPRESSION_ANGER, adjustedAnger);
                    skinnedMeshRenderer.SetBlendShapeWeight(Constants.EXPRESSION_SADNESS, adjustedSadness);
                    skinnedMeshRenderer.SetBlendShapeWeight(Constants.EXPRESSION_FEAR, adjustedFear);
                    skinnedMeshRenderer.SetBlendShapeWeight(Constants.EXPRESSION_DISGUST, adjustedDisgust);
                }
            }

            _eyesClosed = EyesClosedDefault * 0.7f + 15 + _eyesClosedExpression;
        }

        private void UpdateFace_Internal() {
            SetBlendShapes_Internal(NoseWidth, Constants.NOSEWIDTH_MIN, Constants.NOSEWIDTH_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(NoseLength, Constants.NOSELENGTH_MIN, Constants.NOSELENGTH_MAX,CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(NoseHeight, Constants.NOSEHEIGHT_MIN, Constants.NOSEHEIGHT_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(NoseBridge, Constants.NOSEBRIDGE_MIN, Constants.NOSEBRIDGE_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(NoseTilt, Constants.NOSETILT_MIN, Constants.NOSETILT_MAX, CurrentGenderSkinnedRenderer);

            SetBlendShapes_Internal(MouthWidth, Constants.MOUTHWIDTH_MIN, Constants.MOUTHWIDTH_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(LipsWidth, Constants.LIPSWIDTH_MIN, Constants.LIPSWIDTH_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(Cheeks, Constants.CHEEKS_MIN, Constants.CHEEKS_MAX, CurrentGenderSkinnedRenderer);

            SetBlendShapes_Internal(BrowWidth, Constants.BROWWIDTH_MIN, Constants.BROWWIDTH_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(BrowHeight, Constants.BROWHEIGHT_MIN, Constants.BROWHEIGHT_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(BrowProtrusion, Constants.BROWPROTRUSION_MIN, Constants.BROWPROTRUSION_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(BrowThickness, Constants.BROWTHICKNESS_MIN, Constants.BROWTHICKNESS_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(BrowCurve, Constants.BROWCURVE_MIN, Constants.BROWCURVE_MAX, CurrentGenderSkinnedRenderer);

            SetBlendShapes_Internal(EyesSize, Constants.EYESIZE_MIN, Constants.EYESIZE_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(EarsSize, Constants.EARSIZE_MIN, Constants.EARSIZE_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(EarsFlare, Constants.EARSFLARE_MIN, Constants.EARSFLARE_MAX, CurrentGenderSkinnedRenderer);
            CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.EARSPOINTY_MAX, EarsPointy);

            SetBlendShapes_Internal(JawWidth, Constants.JAWWIDTH_MIN, Constants.JAWWIDTH_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(ChinWidth, Constants.CHINWIDTH_MIN, Constants.CHINWIDTH_MAX, CurrentGenderSkinnedRenderer);
            SetBlendShapes_Internal(ChinProtrusion, Constants.CHINPROTRUSION_MIN, Constants.CHINPROTRUSION_MAX, CurrentGenderSkinnedRenderer);
            CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.CHINCLEFT_MAX, ChinCleft);

            _eyesClosed = EyesClosedDefault * 0.7f + 15 + _eyesClosedExpression;

            UpdateFacialHair_Shape_Internal();
        }

        private void UpdateMaterials_Internal() {
            // Shared materials
            Material[] sharedMaterials = CurrentGenderSkinnedRenderer.sharedMaterials;
            sharedMaterials[Constants.SKIN_MATERIAL_SLOT] = CurrentGender == AvatarGender.Male ? AvatarReferences.MaleSkinMaterials[SkinMaterialIndex] : AvatarReferences.FemaleSkinMaterials[SkinMaterialIndex];
            sharedMaterials[Constants.LASHES_MATERIAL_SLOT] = AvatarReferences.LashesMaterials[LashesMaterialIndex];
            sharedMaterials[Constants.NAILS_MATERIAL_SLOT] = AvatarReferences.NailMaterials[NailsMaterialIndex];
            sharedMaterials[Constants.EYE_MATERIAL_SLOT] = AvatarReferences.EyeMaterials[EyeMaterialIndex];
            sharedMaterials[Constants.BROWS_MATERIAL_SLOT] = AvatarReferences.BrowMaterials[BrowMaterialIndex];
            CurrentGenderSkinnedRenderer.sharedMaterials = sharedMaterials;
        }

        private void UpdateHeight_Internal() {
            CurrentBase.transform.localScale = Vector3.one * GetBodyHeightScale_Internal();
        }

        private void UpdateBodyParameters_Internal(){

            SetBlendShapes_Internal(BreastSize, Constants.BREASTSIZE_MIN, Constants.BREASTSIZE_MAX, CurrentGenderSkinnedRenderer);

            if (BodyMuscle + Mathf.Abs(BodyFat) <= 100){
                SetBlendShapes_Internal(BodyFat, Constants.BODY_SKINNY, Constants.BODY_CHUBBY, CurrentGenderSkinnedRenderer);
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.BODY_MUSCLE, BodyMuscle);

                foreach (SkinnedMeshRenderer clothingPiece in _activeSkinnedClothingItems.Values) {
                    SetBlendShapes_Internal(BodyFat, Constants.BODY_SKINNY, Constants.BODY_CHUBBY, clothingPiece);
                    clothingPiece.SetBlendShapeWeight(Constants.BODY_MUSCLE, BodyMuscle);
                    SetBlendShapes_Internal(BreastSize, Constants.BREASTSIZE_MIN, Constants.BREASTSIZE_MAX, clothingPiece);
                }
            }
            else {
                _adjustedBodyFat = BodyFat * 100 / (Mathf.Abs(BodyFat) + BodyMuscle);
                _adjustedBodyMuscle = BodyMuscle * 100 / (Mathf.Abs(BodyFat) + BodyMuscle);

                SetBlendShapes_Internal(_adjustedBodyFat, Constants.BODY_SKINNY, Constants.BODY_CHUBBY, CurrentGenderSkinnedRenderer);
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.BODY_MUSCLE, _adjustedBodyMuscle);

                foreach (SkinnedMeshRenderer clothingPiece in _activeSkinnedClothingItems.Values){
                    SetBlendShapes_Internal(_adjustedBodyFat, Constants.BODY_SKINNY, Constants.BODY_CHUBBY, clothingPiece);
                    clothingPiece.SetBlendShapeWeight(Constants.BODY_MUSCLE, _adjustedBodyMuscle);
                    SetBlendShapes_Internal(BreastSize, Constants.BREASTSIZE_MIN, Constants.BREASTSIZE_MAX, clothingPiece);
                }
            }

            CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.BODY_NAILSLENGTH_MAX, NailsLength);
            CurrentGenderSkinnedRenderer.SetBlendShapeWeight(Constants.BODY_NAILSCURVE_MAX, NailsCurve);

        }

        private float GetBodyHeightScale_Internal() {
            const float DEFAULT_HEIGHT = 1.8f;
            return BodyHeight / DEFAULT_HEIGHT;
        }
        
        private void FindClothingItemInstances_Internal() {

            //_currentInstantiatedClothingItems.Clear();            
            _activeSkinnedClothingItems.Clear();

            UClothingItem[] instanciatedClothingItems = CurrentBase.GetComponentsInChildren<UClothingItem>();
            foreach (UClothingItem item in instanciatedClothingItems) {
                item.IsEquipped = false;
            }

            // Finds all clothing item instances for internal tracking, removes any duplicates.
            _avatarSlots = CurrentGenderSkinnedRenderer.transform.parent.GetComponentsInChildren<Slot>();

            ResolveClothingItemHideSlots_Internal(ClothingItemHat);
            ResolveClothingItemHideSlots_Internal(ClothingItemTop);
            ResolveClothingItemHideSlots_Internal(ClothingItemBottom);
            ResolveClothingItemHideSlots_Internal(ClothingItemShoes);
            ResolveClothingItemHideSlots_Internal(ClothingItemGlasses);

            ResolveClothingItemSlot_Internal(instanciatedClothingItems, ClothingItemHat, SlotType.Hat, ClothingItemHatVariationIndex);
            ResolveClothingItemSlot_Internal(instanciatedClothingItems, ClothingItemTop, SlotType.Top, ClothingItemTopVariationIndex);
            ResolveClothingItemSlot_Internal(instanciatedClothingItems, ClothingItemBottom, SlotType.Bottom, ClothingItemBottomVariationIndex);
            ResolveClothingItemSlot_Internal(instanciatedClothingItems, ClothingItemShoes, SlotType.Shoes, ClothingItemShoesVariationIndex);
            ResolveClothingItemSlot_Internal(instanciatedClothingItems, ClothingItemGlasses, SlotType.Glasses, ClothingItemGlassesVariationIndex);

            // Destroy others
            foreach (UClothingItem item in CurrentBase.GetComponentsInChildren<UClothingItem>().Where(item => !item.IsEquipped)) {
                SafeDestroyGameObject_Internal(item.gameObject);
            }

        }

        private void ResolveClothingItemHideSlots_Internal(ClothingItem clothingItem) {
            if (clothingItem == null) {
                return;
            }
            
            if (clothingItem.HideSlots == null || clothingItem.HideSlots.Length == 0) {
                return;
            }

            if (clothingItem.HideSlots.Contains(SlotType.Bottom)) {
                ClothingItemBottom = null;
            }
            if (clothingItem.HideSlots.Contains(SlotType.Glasses)) {
                ClothingItemGlasses = null;
            }
            if (clothingItem.HideSlots.Contains(SlotType.Top)) {
                ClothingItemTop = null;
            }
            if (clothingItem.HideSlots.Contains(SlotType.Shoes)) {
                ClothingItemShoes = null;
            }
            if (clothingItem.HideSlots.Contains(SlotType.Hat)) {
                ClothingItemHat = null;
            }
        }

        private void ResolveClothingItemSlot_Internal(UClothingItem[] clothingInstances, ClothingItem clothingItem, SlotType slotType, int clothingItemIndex) {
            // Check if instance exists
            UClothingItem instance = clothingInstances.FirstOrDefault(item => item.ClothingItem == clothingItem);

            // Remove clothing item or any previous instances
            if (clothingItem == null) {
                if (instance != null) {
                    SafeDestroyGameObject_Internal(instance.gameObject);

                    if (_activeSkinnedClothingItems.ContainsKey(slotType)) {
                        _activeSkinnedClothingItems.Remove(slotType);
                    }
                }

                return;
            }

            // Check if clothing item is in the right slot
            if (clothingItem.SlotType != slotType) {
                Debug.Log($"{NAME} Clothing item {clothingItem} is not meant for slot {slotType} but for {clothingItem.SlotType}");
                clothingItem = null;
                return;
            }

            Slot slot = _avatarSlots.FirstOrDefault(slot => slot.SlotType == slotType);
            
            // Spawn new instance
            if (instance == null) {
                Mesh mesh = CurrentGender == AvatarGender.Male ? clothingItem.MaleMesh : clothingItem.FemaleMesh;

                GameObject instantiatedClothingItem = new GameObject($"{clothingItem.name}");
                instance = instantiatedClothingItem.AddComponent<UClothingItem>();
                MeshFilter meshFilter = instantiatedClothingItem.AddComponent<MeshFilter>();
                
                instance.ClothingItem = clothingItem;
                instantiatedClothingItem.transform.parent = transform;
                meshFilter.mesh = mesh;
                
                if (slot.AttachmentType == AttachmentType.ParentToBone) {
                    instantiatedClothingItem.transform.parent = slot.BoneTransform;
                    instantiatedClothingItem.transform.position = slot.BoneTransform.position;
                    instantiatedClothingItem.transform.rotation = slot.BoneTransform.rotation;

                    instantiatedClothingItem.AddComponent<MeshRenderer>();
                }

                if (slot.AttachmentType == AttachmentType.SkinnedToArmature) {
                    instantiatedClothingItem.transform.parent = slot.BoneTransform;

                    SkinnedMeshRenderer skinnedMeshRenderer = instantiatedClothingItem.AddComponent<SkinnedMeshRenderer>();
                    skinnedMeshRenderer.sharedMesh = mesh;
                    skinnedMeshRenderer.bones = _bodyBones;

                    _activeSkinnedClothingItems.Add(slot.SlotType, skinnedMeshRenderer);
                }

            }

            if (slot.AttachmentType == AttachmentType.ParentToBone) {
                Vector3 offset = Vector3.zero;
                if (_hairGameObject != null && slotType == SlotType.Hat) {
                    UHair hairInstance = _hairGameObject.GetComponent<UHair>();

                    offset = hairInstance.HairItem.HatOffset;
                    instance.transform.parent = CurrentBase.transform;
                    instance.transform.localScale = Vector3.one;
                    instance.transform.parent = slot.BoneTransform;

                    // Hide only hair if the hat is not empty
                    if (clothingItem.IsEmpty) {
                        hairInstance.gameObject.SetActive(true);
                    }
                    else {
                        hairInstance.gameObject.SetActive(!hairInstance.HairItem.HideHairWhenHatEquipped);
                    }
                }

                if (slotType == SlotType.Glasses) {
                    offset = Vector3.zero;
                    instance.transform.parent = CurrentBase.transform;
                    instance.transform.localScale = Vector3.one;
                    instance.transform.parent = slot.BoneTransform;
                }

                instance.transform.localPosition = offset;
            }

            if (slot.AttachmentType == AttachmentType.SkinnedToArmature) {
                if (!_activeSkinnedClothingItems.ContainsKey(slotType)) {
                    _activeSkinnedClothingItems.Add(slotType, instance.GetComponent<SkinnedMeshRenderer>());
                }
                else {
                    _activeSkinnedClothingItems[slotType] = instance.GetComponent<SkinnedMeshRenderer>();
                }
            }

            instance.SetVariation(clothingItemIndex);

            if (clothingItem.HideShapeWeightIndex != HideBlendShapeIndex.Nothing) {
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight((int) clothingItem.HideShapeWeightIndex, 100);
            }

            if (clothingItem.HideShapeWeightIndexSecondary != HideBlendShapeIndex.Nothing) {
                CurrentGenderSkinnedRenderer.SetBlendShapeWeight((int) clothingItem.HideShapeWeightIndexSecondary, 100);
            }
            
            instance.IsEquipped = true;
        }

        private void UpdateClothing_Internal() {
            ResetCostumization_Internal();
            FindClothingItemInstances_Internal();
            UpdateBodyParameters_Internal();
        }

        private void ResetCostumization_Internal() {

            HideBlendShapeIndex[] indices = (HideBlendShapeIndex[]) System.Enum.GetValues(typeof(HideBlendShapeIndex));
            foreach (HideBlendShapeIndex index in indices) {
                if (index != HideBlendShapeIndex.Nothing) {
                    CurrentGenderSkinnedRenderer.SetBlendShapeWeight((int) index, 0);
                }
            }

        }

        private void SafeDestroyGameObject_Internal(GameObject gameObject) {
            #if UNITY_EDITOR
            if (PrefabUtility.IsPartOfAnyPrefab(this.gameObject)) {
                PrefabUtility.UnpackPrefabInstance(this.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }
            #endif

            DestroyImmediate(gameObject);
        }

        void OnApplicationQuit() {
            ResetCostumization_Internal();
            UpdateExpression_Internal(force: true);
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class AvatarFloatFieldAttribute : AvatarFieldAttribute {
        public float SourceMinValue;
        public float SourceMaxValue;
        public float DisplayMinValue;
        public float DisplayMaxValue;

        public AvatarFloatFieldAttribute(
            string displayName, 
            string variableName, 
            float sourceMinValue, 
            float sourceMaxValue,
            float displayMinValue = 0,
            float displayMaxValue = 1) : base(displayName, variableName, "f_") {
            
            SourceMinValue = sourceMinValue;
            SourceMaxValue = sourceMaxValue;
            DisplayMinValue = displayMinValue;
            DisplayMaxValue = displayMaxValue;

        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class AvatarIntegerFieldAttribute : AvatarFieldAttribute {
        
        public string ArrayDependancyField = null;
        public bool IsVariationField = false;

        public AvatarIntegerFieldAttribute(
            string displayName, 
            string variableName) : base(displayName, variableName, "i_") { }

        public bool HasArrayDependancy() => ArrayDependancyField != null;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class AvatarColorFieldAttribute : AvatarFieldAttribute {
        public AvatarColorFieldAttribute(
            string displayName, 
            string variableName) : base(displayName, variableName, "c_") { }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class AvatarGenderFieldAttribute : AvatarFieldAttribute {
        public AvatarGenderFieldAttribute(
            string displayName, 
            string variableName) : base(displayName, variableName, "g_") { }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class AvatarFieldAttribute : Attribute {
        public string Section = "None";
        public string DisplayName;
        public string VariableName;
        public string Prefix;
        public bool IgnoreInPlayMode;

        public AvatarFieldAttribute(string displayName, string variableName, string prefix) {
            DisplayName = displayName;
            VariableName = variableName;
            Prefix = prefix;
        }

        public virtual string GetValueString(FieldInfo fieldInfo, object instance) {
            return fieldInfo.GetValue(instance).ToString();
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class AvatarTitleAttribute : Attribute {
        public string Title { get; }

        public AvatarTitleAttribute(string title) {
            Title = title;
        }

    }
}
