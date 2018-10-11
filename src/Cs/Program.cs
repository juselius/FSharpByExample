using System;

namespace CsExamples {
    
class Program {

    static void Main(string[] args) {
        Hanoi T = new Hanoi();
        string cnumdiscs;

        Console.Write("Enter the number of discs: ");

        cnumdiscs = Console.ReadLine();
        T.numdiscs = Convert.ToInt32(cnumdiscs);
        T.movetower(T.numdiscs, 1, 3, 2);

        FizzBuzz F = new FizzBuzz(50);
        F.fizzBuzz();
    }
}
}
