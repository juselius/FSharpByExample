using System;

namespace CsExamples {

class Hanoi {
    int m_numdiscs;

    public Hanoi() {
        numdiscs = 0;
    }

    public Hanoi(int newval) {
        numdiscs = newval;
    }

    public int numdiscs {
        get {
            return m_numdiscs;
        }
        set {
            if (value > 0) m_numdiscs = value;
        }
    }

    public void movetower(int n, int from, int to, int other) {
        if (n > 0) {
            movetower(n - 1, from, other, to);
            Console.WriteLine("Move disk {0} from tower {1} to tower {2}", 
                n, from, to);
            movetower(n - 1, other, to, from);
        }
    }
}
}