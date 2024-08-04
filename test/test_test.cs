namespace Plutono.test;

using GdUnit4;
using static GdUnit4.Assertions;
using Plutono.Core.Note;

[TestSuite]
public class GdUnitExampleTest
{
	[TestCase]
    public void TestSlideNoteCanBeClearDeemo()
    {
		SlideNote slideNote = new()
		{
			Data = new SlideNoteData(4, -2f, 1.2, 1.5)
		};
		AutoFree(slideNote);

		AssertBool(slideNote.CanBeClear(0f, Scripts.Utils.GameMode.Arbo) == false);
		AssertBool(slideNote.CanBeClear(-2f, Scripts.Utils.GameMode.Arbo) == true);
	}

	[TestCase]
	public void TestSlideNoteCanBeClearPlutono()
	{
		SlideNote slideNote = new()
		{
			Data = new SlideNoteData(4, -2f, 1.2, 1.5)
		};
		AutoFree(slideNote);

		AssertBool(slideNote.CanBeClear(0f, Scripts.Utils.GameMode.Stelo) == false);
		AssertBool(slideNote.CanBeClear(-2f, Scripts.Utils.GameMode.Stelo) == false);
		slideNote.OnSlideStart(-2.6f, 1.5);
		slideNote.UpdateSlide(-2f);
		AssertBool(slideNote.CanBeClear(-2f, Scripts.Utils.GameMode.Stelo) == true);
	}
}
