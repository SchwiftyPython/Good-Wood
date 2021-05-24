using Assets.Scripts.World.Biomes;

namespace Assets.Scripts.World.Decorators
{
    /// <summary>
    /// Used to decorate chunks with "decorations" such as trees, flowers, ores, etc.
    /// </summary>
    public interface IChunkDecorator
    {
        void Decorate(Chunk chunk, IBiomeRepository biomeRepo);
    }
}
