namespace Service.Common.Integration.DTO;

/// <summary>
/// Информация о документе движения товара для интеграционных событий
/// </summary>
public class ProductFlowInfoDto
{
	public Guid ProductFlowId { get; set; }
	public string Number { get; set; }
}