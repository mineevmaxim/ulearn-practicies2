namespace Incapsulation.EnterpriseTask;

public class Enterprise
{
    public readonly Guid Guid;

    public string Name { get; set; }

    public string Inn
    {
        get => inn;
        set
        {
            if (value.Length != 10 || !value.All(z => char.IsDigit(z)))
                throw new ArgumentException();
            inn = value;
        }
    }
    
    public DateTime EstablishDate { get; set; }
    public TimeSpan ActiveTimeSpan => DateTime.Now - EstablishDate;

    private string inn;

    public Enterprise(Guid guid)
    {
        this.Guid = guid;
    }

    public double GetTotalTransactionsAmount()
    {
        DataBase.OpenConnection();
        return DataBase.Transactions()
            .Where(z => z.EnterpriseGuid == this.Guid)
            .Sum(t => t.Amount);
    }
}