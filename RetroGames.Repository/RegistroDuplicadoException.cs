namespace RetroGames.Repository;

/// <summary>Lançada quando o MySQL recusa um valor repetido em coluna UNIQUE (erro 1062).</summary>
public class RegistroDuplicadoException : Exception
{
    public RegistroDuplicadoException(Exception? excecaoInterna = null)
        : base("Registro duplicado.", excecaoInterna) { }
}
