namespace Snowfall.Application.Dtos.Uploads;

public enum ErreurUpload
{
    TailleMax,
    FichierVide,
    Ecriture,
    Lecture
}

public class ResultatUpload
{
    public bool EstSucces { get; set; }
    public string? NomFichier { get; set; }
    public string? UrlFichier { get; set; }
    public ErreurUpload? Erreur { get; set; }
}