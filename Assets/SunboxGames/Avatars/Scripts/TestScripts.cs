
using UnityEngine;
using Sunbox.Avatars;

public class TestScripts : MonoBehaviour {
    
    public GameObject AvatarPrefab;
    public TextAsset AvatarPreset;

    void Start()  {
        for (int i = 0; i < 20; i++) {
            Vector2 positionXZ = UnityEngine.Random.insideUnitCircle * 8;
            Vector3 position = new Vector3(positionXZ.x, 0, positionXZ.y);

            SpawnAvatarWithRandomization(position, i);
        }

        SpawnAvatarIndividualSettings(Vector3.zero);

        SpawnAvatarFromPreset(AvatarPreset, new Vector3(0, 0, -3));

        SpawnAvatarClothingOptions(new Vector3(0, 0, 3));
    }

    public void SpawnAvatarWithRandomization(Vector3 position, int seed = 100) {
        GameObject avatarInstance = Instantiate(AvatarPrefab, position, Quaternion.identity);

        AvatarCustomization avatar = avatarInstance.GetComponent<AvatarCustomization>();
        avatar.RandomizeBodyParameters(
            seed: seed,
            ignoreHeight: true,
            unifiedHairColors: true,
            noRandomExpressions: true
        );
        avatar.RandomizeClothing(
            seed: seed
        );
    }

    public void SpawnAvatarIndividualSettings(Vector3 position) {
        GameObject avatarInstance = Instantiate(AvatarPrefab, position, Quaternion.identity);

        AvatarCustomization avatar = avatarInstance.GetComponent<AvatarCustomization>();
        avatar.BodyHeight = 1.6f;
        avatar.NoseBridge = 50f;
        avatar.BodyMuscle = 100f;
        avatar.UpdateCustomization();
        avatar.SetExpression(AvatarCustomization.AvatarExpression.Anger | AvatarCustomization.AvatarExpression.Disgust);
    }

    public void SpawnAvatarFromPreset(TextAsset asset, Vector3 position) {
        GameObject avatarInstance = Instantiate(AvatarPrefab, position, Quaternion.identity);

        AvatarCustomization avatar = avatarInstance.GetComponent<AvatarCustomization>();
        AvatarCustomization.ApplyConfigFile(asset, avatar);
    }

    public ClothingItem GlassesClothingItem;

    public void SpawnAvatarClothingOptions(Vector3 position) {
        GameObject avatarInstance = Instantiate(AvatarPrefab, position, Quaternion.identity);

        AvatarCustomization avatar = avatarInstance.GetComponent<AvatarCustomization>();
        avatar.AttachClothingItem(
            item: GlassesClothingItem,
            variationIndex: 1
        );
    }

}
