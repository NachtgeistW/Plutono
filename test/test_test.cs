using GdUnit4;
using Plutono.Core.Note;
using Plutono.Scripts.Utils;

namespace Plutono.test;

using static Assertions;

[TestSuite]
public class NoteTest
{
	[TestCase]
    public void TestSlideNoteCanBeClearDeemo()
    {
	    SlideNote slideNote = new(new SlideNoteData(4, -2f, 1.2, 1.5));
		slideNote.Initialize();
		AutoFree(slideNote);

		AssertBool(slideNote.CanBeClear(0f, GameMode.Arbo)).IsFalse();
		AssertBool(slideNote.CanBeClear(-2f, GameMode.Arbo)).IsTrue();
	}

	[TestCase]
	public void TestSlideNoteCanBeClearPlutono()
	{
		SlideNote slideNote = new(new SlideNoteData(4, -2f, 1.2, 1.5));
		slideNote.Initialize();
		AutoFree(slideNote);

		AssertBool(slideNote.CanBeClear(0f, GameMode.Stelo)).IsFalse();
		slideNote.OnSlideStart(-2.6f, 1.5);
		AssertBool(slideNote.CanBeClear(-2f, GameMode.Stelo)).IsFalse();
		slideNote.UpdateSlide(-2f);
		AssertBool(slideNote.CanBeClear(-2f, GameMode.Stelo)).IsTrue();
		AssertBool(slideNote.CanBeClear(-2.61f, GameMode.Stelo)).IsTrue();
	}

	[TestCase]
	public void TestBlankNoteShouldBeMiss()
	{
		BlankNote blankNote = new(new SlideNoteData(4, -2f, 1.2, 1.5));
		blankNote.Initialize();
		AutoFree(blankNote);

		AssertBool(blankNote.ShouldMiss(1.49, GameMode.Stelo)).IsFalse();
		AssertBool(blankNote.ShouldMiss(1.6, GameMode.Stelo)).IsFalse();
		AssertBool(blankNote.ShouldMiss(1.61, GameMode.Stelo)).IsTrue();
	}

	[TestCase]
	public void TestSlideNoteShouldBeMiss()
	{
		SlideNote slideNote = new(new SlideNoteData(4, -2f, 1.2, 1.5));
		slideNote.Initialize();
		AutoFree(slideNote);

		AssertBool(slideNote.ShouldMiss(1.49, GameMode.Stelo)).IsFalse();
		AssertBool(slideNote.ShouldMiss(1.6, GameMode.Stelo)).IsFalse();
		AssertBool(slideNote.ShouldMiss(1.61, GameMode.Stelo)).IsTrue();
	}
}
