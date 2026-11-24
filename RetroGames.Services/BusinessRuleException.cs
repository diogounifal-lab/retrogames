namespace RetroGames.Services;

/// <summary>Lançada pelo Service quando uma regra de negócio é violada.</summary>
public class BusinessRuleException(BusinessError erro) : Exception(erro.DefaultMessage)
{
    public BusinessError Erro { get; } = erro;
}
