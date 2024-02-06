using MudBlazor;

namespace CyclingApp.Resources
{
    public static class Themes
    {
        public static MudTheme Default => new()
        {
            PaletteDark = new PaletteDark
            {
                // Primary
                Primary = "#28EB67",
                PrimaryContrastText = "#000000",

                // Dark
                Dark = "#4B4D52",
                DarkLighten = "#393A40",
                DarkDarken = "#191A1B",
                DarkContrastText = "#FFFFFF",

                // Surface
                Surface = "#191A1B",

                // Background
                Background = "#000000",
                //BackgroundGrey = "",

                // Text
                TextPrimary = "#FFFFFF",
                //TextSecondary = ""
                //TextDisabled = "",
            },
            Typography = new Typography
            {
                Default = new()
                {
                    FontFamily = ["sui-generis", "sans-serif"],
                    FontWeight = 500
                }
            }
        };
    }
}
