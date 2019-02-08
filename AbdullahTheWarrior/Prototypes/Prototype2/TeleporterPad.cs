namespace DeenGames.AbdullahTheWarrior.Prototypes.Prototype2
{
    // Two of these make a pair of a single teleporter. Or maybe three of them cycle ...   
    public class TeleporterPad
    {
        public int X {get; private set;}
        public int Y {get; private set;}
        public TeleporterPad Destination { get; set; }  
        
        public TeleporterPad(int x, int y) {
            this.X = x;
            this.Y = y;
        }        
    }
}