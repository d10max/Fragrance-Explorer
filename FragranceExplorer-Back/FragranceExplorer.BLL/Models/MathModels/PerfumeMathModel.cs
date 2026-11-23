using FragranceExplorer.BLL.Constants;
using FragranceExplorer.DAL.Entities.MathDataEntities;
using FragranceExplorer.DAL.Enums;

namespace FragranceExplorer.BLL.Models.MathModels;

public class PerfumeMathModel
{
    public int Id { get; }

    public int People { get; }
    public double Rating { get; }

    public bool IsFlatStructure { get; }

    public ScentVector<AccordMathModel> Accords { get; }
    public ScentVector<NoteMathModel> Notes { get; }

    public ScentVector<NoteMathModel> TopNotes { get; }
    public ScentVector<NoteMathModel> MiddleNotes { get; }
    public ScentVector<NoteMathModel> BaseNotes { get; }

    private PerfumeMathModel(
        int id,
        int? people,
        double rating,
        bool isFlatStructure,
        ScentVector<AccordMathModel> accords,
        ScentVector<NoteMathModel> notes,
        ScentVector<NoteMathModel> topNotes,
        ScentVector<NoteMathModel> middleNotes,
        ScentVector<NoteMathModel> baseNotes)
    {
        Id = id;
        People = people ?? 0;
        Rating = rating;
        IsFlatStructure = isFlatStructure;
        Accords = accords;
        Notes = notes;
        TopNotes = topNotes;
        MiddleNotes = middleNotes;
        BaseNotes = baseNotes;
    }

    public static PerfumeMathModel FromFormulaData(PerfumeFormulaData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var accordsDict = new Dictionary<int, double>(data.Accords.Count());
        foreach (var accord in data.Accords)
        {
            accordsDict[accord.AccordId] = accord.Weight;
        }

        var allNotesDict = new Dictionary<int, double>(data.Notes.Count());
        var topNotesDict = new Dictionary<int, double>();
        var heartNotesDict = new Dictionary<int, double>();
        var baseNotesDict = new Dictionary<int, double>();
        bool isFlat = false;

        foreach (var note in data.Notes)
        {
            if (note.Layer == NoteLayer.Flat)
            {
                isFlat = true;
            }

            double significance = note.Weight * ScentConstants.GetNoteLayerWeight(note.Layer);
            allNotesDict[note.NoteId] = significance;

            switch (note.Layer)
            {
                case NoteLayer.Top:
                    topNotesDict[note.NoteId] = significance;
                    break;
                case NoteLayer.Middle:
                    heartNotesDict[note.NoteId] = significance;
                    break;
                case NoteLayer.Base:
                    baseNotesDict[note.NoteId] = significance;
                    break;
            }
        }

        return new PerfumeMathModel(
            data.Id,
            data.People,
            data.Rating,
            isFlat,
            new ScentVector<AccordMathModel>(accordsDict),
            new ScentVector<NoteMathModel>(allNotesDict),
            new ScentVector<NoteMathModel>(topNotesDict),
            new ScentVector<NoteMathModel>(heartNotesDict),
            new ScentVector<NoteMathModel>(baseNotesDict));
    }
}