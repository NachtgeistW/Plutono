namespace Plutono.Core.Note.Render;

public interface INoteRenderer
{
    public bool DisplayNoteId { get; protected set; }

    public void Render();
    public void OnDispose();
}

public interface IRendererMovable : INoteRenderer
{
    public void Move(double elapsedTime, float chartPlaySpeed);
    public void OnClear(NoteGrade grade);
    public void OnTouch(NoteGrade grade);
}

public interface IRendererHold : INoteRenderer
{
    public void OnNoteLoaded();
    public void UpdateComponentStates();
    public void UpdateComponentOpacity();
    public void UpdateTransformScale();
}

public interface IRendererTouchable : IRendererMovable
{

}