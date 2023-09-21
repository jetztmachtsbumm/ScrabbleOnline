using System.Collections.Generic;

public class Letters
{

    public static readonly LetterData A = new LetterData('A', 5, 1);
    public static readonly LetterData B = new LetterData('B', 2, 3);
    public static readonly LetterData C = new LetterData('C', 2, 4);
    public static readonly LetterData D = new LetterData('D', 4, 1);
    public static readonly LetterData E = new LetterData('E', 15, 1);
    public static readonly LetterData F = new LetterData('F', 2, 4);
    public static readonly LetterData G = new LetterData('G', 3, 2);
    public static readonly LetterData H = new LetterData('H', 4, 2);
    public static readonly LetterData I = new LetterData('I', 6, 1);
    public static readonly LetterData J = new LetterData('J', 1, 6);
    public static readonly LetterData K = new LetterData('K', 2, 4);
    public static readonly LetterData L = new LetterData('L', 3, 2);
    public static readonly LetterData M = new LetterData('M', 4, 3);
    public static readonly LetterData N = new LetterData('N', 9, 1);
    public static readonly LetterData O = new LetterData('O', 3, 2);
    public static readonly LetterData P = new LetterData('P', 1, 4);
    public static readonly LetterData Q = new LetterData('Q', 1, 10);
    public static readonly LetterData R = new LetterData('R', 6, 1);
    public static readonly LetterData S = new LetterData('S', 7, 1);
    public static readonly LetterData T = new LetterData('T', 6, 1);
    public static readonly LetterData U = new LetterData('U', 6, 1);
    public static readonly LetterData V = new LetterData('V', 1, 6);
    public static readonly LetterData W = new LetterData('W', 1, 3);
    public static readonly LetterData X = new LetterData('X', 1, 8);
    public static readonly LetterData Y = new LetterData('Y', 1, 10);
    public static readonly LetterData Z = new LetterData('Z', 1, 3);
    public static readonly LetterData AE = new LetterData('Ä', 1, 6);
    public static readonly LetterData OE = new LetterData('Ö', 1, 8);
    public static readonly LetterData UE = new LetterData('Ü', 1, 6);

    public static readonly List<LetterData> ABC = new List<LetterData>()
    {
        A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,AE,OE,UE
    };

}
