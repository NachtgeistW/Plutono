using Godot;

namespace Plutono.Core.Note
{
    public interface INote
    {
    }

    public interface IMovable : INote
    {
        public void Move(double delta, float chartPlaySpeed);
        public bool ShouldMiss();

        /// <summary>
        /// note 是否被触摸（点击、按着或滑动）
        /// </summary>
        /// <returns>只要有一只手指按住就返回 true</returns>
        public bool IsTouch(float xPos, out float deltaXPos, double touchTime, out double deltaTime);

        public void OnClear(NoteGrade grade);
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
        public void OnSlideStart(Vector2 worldPos, double curTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns>If the note can be cleared</returns>
        public bool UpdateSlide(Vector2 worldPos);
        public void OnSlideEnd(double curTime, out double deltaTime, out float deltaXPos);
    }

    public interface IHoldable : INote
    {
        public void OnHoldStart(Vector3 worldPos, double curTime);
        public void UpdateHold(Vector3 worldPos, double curTime);
        public void OnHoldEnd();
        public void OnHoldMiss();
    }

    // ReSharper disable once IdentifierTypo
    public interface IFlickable : INote
    {
        public void OnFlickStart(Vector2 worldPos, double curTime);
        public bool UpdateFlick(Vector2 worldPos);
        public void OnFlickEnd();
    }
}
