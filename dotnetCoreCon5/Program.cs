// C# 9 top-level statement!

using System;

System.Console.WriteLine("ATM in C# 8/9!");

void DumpState(AtmState state)
{
    System.Console.WriteLine(state.StateName);
    Console.WriteLine();
}

var accounts = new[] {
            new Account(accountNumber:"A1234", balance: 67, pin: 1234),
            new Account(accountNumber:"A5678", balance: 2000, pin: 5678),
            new Account(accountNumber:"A2000", balance: 10000, pin: 0809),
            };

Bank bank = new Bank("River Bank", accounts);

AtmState state = new IdleState(bank);
while (true)
{
    DumpState(state);
    state = state.Next();
}
