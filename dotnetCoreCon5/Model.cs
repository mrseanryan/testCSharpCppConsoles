using System;
using System.Collections.Generic;
using System.Linq;

public abstract record AtmState
{
    public AtmState(Bank bank)
    {
        ThisBank = bank;
    }

    public Bank ThisBank { get; }
    public abstract string StateName { get; }

    public abstract AtmState Next();
}

public record Account
{
    public Account(int pin)
    {
        Pin = pin;
    }

    public string AccountNumber { get; init; }
    public double Balance { get; init; }
    private int Pin { get; }

    public bool IsPinOk(int pin)
    {
        return pin == Pin;
    }
}

public record Card
{
    public Card(string account)
    {
        AccountNumber = account;
    }

    public string AccountNumber { get; }
}

public record Bank
{
    public Bank(IEnumerable<Account> accounts)
    {
        Accounts = accounts;
    }

    public IEnumerable<Account> Accounts { get; } // avoiding 'init' here
}

public abstract record AtmLoggedInState : AtmState
{
    protected AtmLoggedInState(AtmState original) : base(original)
    {
    }

    public Account ThisAccount { get; init; }
}

public record IdleState : AtmState
{
    public IdleState(AtmState original) : base(original)
    {
    }

    public IdleState(Bank bank) : base(bank)
    {
    }

    public override string StateName => "Idle";

    public override AtmState Next()
    {
        return new PickCardState(ThisBank);
    }
}

public record PickCardState : AtmState
{
    public PickCardState(Bank bank) : base(bank)
    {
    }

    public override string StateName => "Select CARD";

    public override AtmState Next()
    {
        var cards = new[] { new Card("A1234"), new Card("A5678"), new Card("A1000"), new Card("BAD1") };

        int i = 1;
        foreach (var card in cards)
        {
            Console.WriteLine($"[{i++}] {card}");
        }

        do
        {
            Console.WriteLine("Please insert a card");

            var j = Input.GetNumber();
            if (j > 0 && j - 1 < cards.Count())
            {
                var card = cards.ElementAt(j - 1);
                return new EnterPinState(ThisBank) { AccountNumber = card.AccountNumber };
            }

        } while (true);
    }
}

public record EnterPinState : AtmState
{
    public EnterPinState(Bank bank) : base(bank)
    {
    }

    public override string StateName => "Enter PIN";

    public string AccountNumber { get; init; }

    public override AtmState Next()
    {
        var account = ThisBank.Accounts.SingleOrDefault(a => a.AccountNumber == AccountNumber);
        if (account == null)
        {
            return new BadCardState(this);
        }

        System.Console.WriteLine("Please enter your PIN:");
        var pinInput = Input.GetNumberHidden();

        if (account.IsPinOk(pinInput))
            return new ChooseCashAmountState(this, account);

        return new BadPinState(this);
    }
}

public record BadCardState : AtmState
{
    public BadCardState(AtmState original) : base(original)
    {
    }

    public override string StateName => "BAD CARD";

    public override AtmState Next()
    {
        Console.WriteLine("Bad card or not for this bank!");
        return new IdleState(this);
    }
}

public record BadPinState : AtmState
{
    public BadPinState(AtmState original) : base(original)
    {
    }

    public override string StateName => "BAD PIN";

    public override AtmState Next()
    {
        Console.WriteLine("Bad pin!");
        return new IdleState(this);
    }
}

public record NotEnoughBalanceState : AtmLoggedInState
{
    public NotEnoughBalanceState(AtmLoggedInState original) : base(original)
    {
    }

    public override string StateName => "INSUFFICIENT BALANCE";

    public override AtmState Next()
    {
        Console.WriteLine("Not enough balance in your account!");
        Console.WriteLine("Please take your card");
        return new IdleState(this);
    }
}

public record ChooseCashAmountState : AtmLoggedInState
{
    public ChooseCashAmountState(AtmState original, Account account) : base(original)
    {
        ThisAccount = account;
    }

    IEnumerable<int> GetAmountsForBalance(IEnumerable<int> amounts)
    {
        return amounts.Where(a => a < ThisAccount.Balance);
    }

    public override string StateName => "CHOOSE DENOMINATION";

    public override AtmState Next()
    {
        Console.WriteLine($"Balance = {ThisAccount.Balance}");

        var denominations = new[] { 100, 50, 20, 10, 5 };

        var amountsAvailable = GetAmountsForBalance(denominations);

        if (!amountsAvailable.Any())
        {
            return new NotEnoughBalanceState(this);
        }

        int i = 1;
        foreach (var amount in amountsAvailable)
        {
            Console.WriteLine($"[{i++}] {amount}");
        }

        do
        {
            Console.WriteLine("Please select an amount");

            var j = Input.GetNumber();
            if (j > 0 && j - 1 < amountsAvailable.Count())
            {
                var amountSelected = amountsAvailable.ElementAt(j - 1);
                return new DispenseCashState(this) { AmountSelected = amountSelected };
            }

        } while (true);
    }
}

public record DispenseCashState : AtmLoggedInState
{
    public DispenseCashState(AtmLoggedInState original) : base(original)
    {
    }

    public int AmountSelected { get; init; }

    public override string StateName => "DISPENSE CASH";

    public override AtmState Next()
    {
        Console.WriteLine($"Please take your cash [{AmountSelected}]");

        var updatedAccount = ThisAccount with // using 'with' here, means we can avoid handling private state (pin)
        {
            Balance = ThisAccount.Balance - AmountSelected
        };

        var updatedAccounts = ThisBank.Accounts.Except(new[] { ThisAccount })
            .Append(updatedAccount);

        return new IdleState(new Bank(updatedAccounts));
    }
}
