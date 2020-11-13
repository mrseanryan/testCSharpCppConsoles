// C# 9 top-level statement!

using System;

System.Console.WriteLine("ATM in C# 8/9!");

void DumpState(AtmState state)
{
    System.Console.WriteLine(state.StateName);
    Console.WriteLine();
}

var accounts = new[] {
            new Account(1234, accountNumber:"A1234", balance: 67),
            new Account(5678, accountNumber:"A5678", balance: 100),
            new Account(1234, accountNumber:"A2000", balance: 1000),
            };

Bank bank = new Bank("River Bank", accounts);

AtmState state = new IdleState(bank);
while (true)
{
    DumpState(state);
    state = state.Next();
}
