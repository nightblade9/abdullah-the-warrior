using Microsoft.Xna.Framework;

namespace DeenGames.AbdullahTheWarrior.Prototypes.Prototype1
{
    // Values from Fleja palette: https://lospec.com/palette-list/fleja-master-palette
    public static class Palette
    {
        public static Color DarkestBlue = FromHex("#1f1833");
        public static Color DarkestGrey = FromHex("#2b2e42");

        public static Color DarkGrey = FromHex("#414859");
        public static Color Grey = FromHex("#68717a");
        public static Color LightGrey = FromHex("#90a1a8");
        public static Color LightestGrey = FromHex("#b6cbcf");
        public static Color White = FromHex("#ffffff");
        public static Color LightSkin = FromHex("#fcbf8a");
        public static Color MediumSkin = FromHex("#b58057");
        public static Color DarkSkin = FromHex("#8a503e");
        public static Color DarkBrownPurple = FromHex("#5c3a41");
        public static Color Red = FromHex("#c93038");
        public static Color Orange = FromHex("#de6a38");

        public static Color Gold = FromHex("#ffad3b");
        public static Color Offwhite = FromHex("#ffe596");
        public static Color PaleYellow = FromHex("#fcf960");
        public static Color LimeGreen = FromHex("#b4d645");
        public static Color Green = FromHex("#51c43f");
        public static Color BlueGreen = FromHex("#309c63");
        public static Color Aquamarine = FromHex("#236d7a");
        public static Color DarkAquamarine = FromHex("#264f6e");
        public static Color DarkBlue = FromHex("#233663");

        public static Color FadedBlue = FromHex("#417291");
        public static Color LighterFadedBlue = FromHex("#4c93ad");

        public static Color LightestFadedBlue = FromHex("#63c2c9");
        public static Color WashedOutFadedBlue = FromHex("#94d2d4");
        public static Color LightCyan = FromHex("#b8fdff");
        public static Color DarkPurpleBrown = FromHex("#3c2940");
        public static Color DarkPurple = FromHex("#46275c");
        public static Color FadedDarkPurple = FromHex("#826481");
        public static Color Skin = FromHex("#f7a48b");
        public static Color FadedPurple = FromHex("#c27182");
        public static Color Purple = FromHex("#852d66");



        private static Color FromHex(string hexValue)
        {
            if (hexValue.StartsWith("#"))
            {
                hexValue = hexValue.Substring(1);
            }

            var red = HexToDec(hexValue.Substring(0, 2));
            var green = HexToDec(hexValue.Substring(2, 2));
            var blue = HexToDec(hexValue.Substring(4, 2));

            return Color.FromNonPremultiplied(red, green, blue, 255);
        }

        private static int HexToDec(string hexValue)
        {
            // https://stackoverflow.com/questions/1139957/c-sharp-convert-integer-to-hex-and-back-again
            return int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
        }
    }
}