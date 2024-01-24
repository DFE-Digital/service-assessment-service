namespace ServiceAssessmentService.Domain.Model;

public record ProjectPhase(string Name, string Description = "")
{
    public static readonly ProjectPhase Discovery = new("Discovery");
    public static readonly ProjectPhase Alpha = new("Alpha");
    public static readonly ProjectPhase PrivateBeta = new("Private Beta");
    public static readonly ProjectPhase PublicBeta = new("Public Beta");
    public static readonly ProjectPhase Live = new("Live");
    public static readonly ProjectPhase Retired = new("Retired");

    public static readonly ProjectPhase[] Sequence =
    {
        Discovery,
        Alpha,
        PrivateBeta,
        PublicBeta,
        Live,
        Retired,
    };

    public static ProjectPhase? FromName(string name)
    {
        return Sequence.SingleOrDefault(x => x.Name == name);
    }

    public static ProjectPhase? NextPhase(ProjectPhase phase)
    {
        var index = Array.IndexOf(Sequence, phase);
        if (index == -1)
        {
            return null;
        }

        return Sequence[index + 1];
    }

    public static ProjectPhase? PreviousPhase(ProjectPhase phase)
    {
        var index = Array.IndexOf(Sequence, phase);
        if (index == -1)
        {
            return null;
        }

        return Sequence[index - 1];
    }
}
