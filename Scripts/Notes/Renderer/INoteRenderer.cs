namespace Plutono.Core.Note.Render;

public interface INoteRenderer
{
    public bool DisplayNoteId { get; protected set; }

    public void Render(double curTime);
    public void OnDispose();

}

public interface IRendererHoldable : INoteRenderer
{
    public void OnNoteLoaded(float chartPlaySpeed);
    public void UpdateComponentStates();
    public void UpdateComponentOpacity();
    public void UpdateTransformScale(double curTime);

    protected const float maximumNoteRange = 10f;
    protected static float NoteFallTime(float chartPlaySpeed)
    {
        const float maximumNoteRange = 10f;
        var falldownSpeedRevision = 3f;
        return maximumNoteRange / (chartPlaySpeed * falldownSpeedRevision);
    }
}

public interface IRendererTouchable : INoteRenderer
{
    public void OnTouch(NoteGrade grade);
    public void OnClear(NoteGrade grade);
}