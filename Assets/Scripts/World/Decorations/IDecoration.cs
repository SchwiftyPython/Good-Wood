using UnityEngine;

namespace Assets.Scripts.World.Decorations
{
    public interface IDecoration
    {
        bool ValidLocation(Vector3 location);
        bool GenerateAt(Chunk chunk, Vector3 location);
    }
}
