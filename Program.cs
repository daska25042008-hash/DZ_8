using System;
using System.Collections;
using System.IO;
enum AccountType
{
    Current = 1,
    Savings = 2
}
class BankTransaction
{
    private readonly DateTime date;
    private readonly double amount;
    public BankTransaction(double sum)
    {
        date = DateTime.Now;
        amount = sum;
    }
    public DateTime GetDate()
    {
        return date;
    }
    public double GetAmount()
    {
        return amount;
    }
}
class Account : IDisposable
{
    private static int count = 0;
    private int num;
    private double money;
    private AccountType type;
    private Queue transactions = new Queue();
    private static int NextNum()
    {
        count++;
        return count;
    }
    public Account()
    {
        num = NextNum();
    }
    public Account(double m)
    {
        num = NextNum();
        money = m;
    }
    public Account(AccountType t)
    {
        num = NextNum();
        type = t;
    }
    public Account(double m, AccountType t)
    {
        num = NextNum();
        money = m;
        type = t;
    }
    public void Put(double sum)
    {
        money = money + sum;
        BankTransaction transaction = new BankTransaction(sum);
        transactions.Enqueue(transaction);
    }
    public bool Take(double sum)
    {
        if (sum > 0 && money >= sum)
        {
            money = money - sum;
            BankTransaction transaction = new BankTransaction(-sum);
            transactions.Enqueue(transaction);
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool Transfer(Account from, double sum)
    {
        if (from.Take(sum))
        {
            Put(sum);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ShowTransactions()
    {
        foreach (BankTransaction transaction in transactions)
        {
            Console.WriteLine(transaction.GetDate() + " " + transaction.GetAmount());
        }
    }
    public void Dispose()
    {
        using (StreamWriter writer = new StreamWriter("Account_" + num + "_transactions.txt"))
        {
            foreach (BankTransaction transaction in transactions)
            {
                writer.WriteLine(transaction.GetDate() + " " + transaction.GetAmount());
            }
        }
        GC.SuppressFinalize(this);
    }
    public int GetNum()
    {
        return num;
    }
    public double GetMoney()
    {
        return money;
    }
    public AccountType GetTypeAccount()
    {
        return type;
    }
}
class Program
{
    static void Main()
    {
        Account a = new Account(5000, AccountType.Savings);
        Account b = new Account(3000, AccountType.Current);
        bool result = b.Transfer(a, 2000);
        Console.WriteLine("Счет a: " + a.GetMoney());
        Console.WriteLine("Счет b: " + b.GetMoney());
        Console.WriteLine("Перевод успешен: " + result);
        Console.WriteLine("Операции счета a:");
        a.ShowTransactions();
        Console.WriteLine("Операции счета b:");
        b.ShowTransactions();
        a.Dispose();
        b.Dispose();
    }
}