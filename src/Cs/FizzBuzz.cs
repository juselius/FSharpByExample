using System;

namespace CsExamples {

class FizzBuzz {
    int nIter;

    public FizzBuzz() {
        nIter = 1;
    }

    public FizzBuzz(int n) {
        nIter = n;
    }

    public int iterations {
        get {
            return nIter;
        }
        set {
            if (value > 0) 
                nIter = value;
            else 
                nIter = 1;
        }
    }

    public void fizzBuzz() {
        for (int i=0; i < nIter; i++) {
            if (i % 15 == 0) 
                Console.WriteLine("FIZZBUZZ {0}", i);
            else if (i % 5 == 0) 
                Console.WriteLine("buzz {0}", i);
            else if (i % 3 == 0) 
                Console.WriteLine("fizz {0}", i);
            else
                Console.WriteLine("{0}", i);
        }
    }
}
}
