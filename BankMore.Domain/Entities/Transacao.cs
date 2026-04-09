namespace BankMore.Domain.Entities;

public class Transacao
{
    public int Id { get; set; }
    public int ContaOrigemId { get; set; }
    public int ContaDestinoId { get; set; }
    public decimal Valor { get; set; }
    public string Tipo { get; set; } = "Transferencia";
    public DateTime DataTransacao { get; set; } = DateTime.Now;
}