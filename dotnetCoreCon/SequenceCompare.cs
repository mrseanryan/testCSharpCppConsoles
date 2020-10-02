using System;
using System.Linq;

static class SequenceCompare
{
    public static void Go()
    {
        var seq1 = new[] { "A", "B", "C" };
        var seq2 = new[] { "B", "C", "A" };

        var areEqualDespiteOrder = seq1.SequenceEqual(seq2);

        if (!areEqualDespiteOrder)
            "SequenceEqual DOES depend on order!".Dump();
        else
            "SequenceEqual does NOT depend on order".Dump();
    }
}