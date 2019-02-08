namespace DeenGames.AbdullahTheWarrior.Prototypes.Prototype2
{
    // Two of these make a pair of a single teleporter. Or maybe three of them cycle ...   
    public class TeleporterPad : AbstractEntity
    {
        public TeleporterPad Destination { get; set; }  
        
        public TeleporterPad(int x, int y) : base(x, y, '?', Palette.Blue)
        {
        }        
    }
}