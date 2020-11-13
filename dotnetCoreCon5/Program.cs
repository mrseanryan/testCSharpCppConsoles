// C# 9 top-level statement!
using System;

System.Console.WriteLine("ATM in C# 8/9!");

void DumpState(AtmState state)
{
    System.Console.WriteLine(state);
}

AtmState state = new();
while (state.State != EState.Wait)
{
    DumpState(state);
    state = state.Next();
}

enum EState
{
    Idle,
    EnterPin,
    ChooseAmount,
    DispenseCash,
    Wait,
    WrongPin,
    NotEnoughCash
}

record AtmState
{
    public EState State { get; init; } = EState.Idle;

    public AtmState Next()
    {
        return new() { State = GetNextState() };
    }

    private EState GetNextState() =>
    State switch
    {
        EState.Idle => EState.EnterPin,
        EState.EnterPin => EState.ChooseAmount,
        EState.ChooseAmount => EState.DispenseCash,
        EState.DispenseCash => EState.Wait,
        EState.Wait => EState.Idle,
        _ => throw new ArgumentException(message: "Invalid EState", paramName: nameof(State))
    };
}
