// C# 9 top-level statement!

using System;

System.Console.WriteLine("ATM in C# 8/9!");

void DumpState(AtmState state)
{
    System.Console.WriteLine(state);
    Console.WriteLine();
}

var accounts = new[] {
            new Account(1234) {AccountNumber="A1234", Balance = 67},
            new Account(1234) {AccountNumber="A5678", Balance = 100},
            new Account(1234) {AccountNumber="A2000", Balance = 1000},
            };

Bank bank = new Bank(accounts);

AtmState state = new IdleState(bank);
while (true)
{
    DumpState(state);
    state = state.Next();
}
