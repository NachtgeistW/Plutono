using Godot;

namespace Plutono.Core.Note
{
    public interface INote
    {
        public void Initialize();
    }

    public interface IMovable : INote
    {
        public void Move(double elapsedTime, float chartPlaySpeed, float curTime);
        public bool ShouldMiss();
    }

    public interface IPianoSoundPlayable : INote
    {
        public void OnPlayPianoSounds();
    }

    /// <summary>
    /// control the tap action
    /// </summary>
    public interface ITapable : INote
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Is </returns>
        public bool OnTap(Vector2 worldPos, double hitTime, out double deltaTime, out float deltaXPos);
    }

    /// <summary>
    /// control the slide action
    /// </summary>
    // ReSharper disable once IdentifierTypo
    public interface ISlidable : INote
    {
        public void OnSlideStart(Vector2 worldPos, float curTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns>If the note can be cleared</returns>
        public bool UpdateSlide(Vector2 worldPos);
        public void OnSlideEnd(double curTime, out float deltaTime, out float deltaXPos);
    }

    public interface IHoldable : INote
    {
        public void OnHoldStart(Vector2 worldPos, float curTime);
        public void UpdateHold(Vector2 worldPos, float curTime);
        public void OnHoldEnd();
        public void OnHoldMiss();
    }

    // ReSharper disable once IdentifierTypo
    public interface IFlickable : INote
    {
        public void OnFlickStart(Vector2 worldPos, float curTime);
        public bool UpdateFlick(Vector2 worldPos);
        public void OnFlickEnd();
    }
}
