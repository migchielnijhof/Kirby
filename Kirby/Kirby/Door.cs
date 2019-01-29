struct Door
{

    public byte X;
    public byte Y;
    public byte Destination;
    public byte DX;
    public byte DY;

    public Door(byte x, byte y, byte destination, byte dX, byte dY)
    {
        X = x;
        Y = y;
        Destination = destination;
        DX = dX;
        DY = dY;
    }

}