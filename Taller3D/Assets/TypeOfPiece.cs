using UnityEngine;

public class TypeOfPiece : MonoBehaviour
{
    [SerializeField] private TypeOfBone bone;
    public GameObject prefab;

    public TypeOfBone GetTypeBone()
    {
        return bone;
    }
}
