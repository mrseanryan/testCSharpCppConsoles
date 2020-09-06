using System;
using System.Collections.Generic;

namespace dotnetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("dotnetCoreCon");

            var command = new GetBookReviewsCommandAsync();
            "Execute SYNC".Dump();
            var reviews = command.Execute();
            DumpReviews(reviews);

            "Execute ASYNC".Dump();
            var reviewsViaAsync = command.ExecuteAsync().GetAwaiter().GetResult(); // blocks this thread - OK only at top-level
            DumpReviews(reviewsViaAsync);
        }

        private static void DumpReviews(IEnumerable<string> reviews)
        {
            foreach (var review in reviews)
            {
                review.Dump();
            }
        }
    }
}
