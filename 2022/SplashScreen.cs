namespace AdventOfCode.Y2022;

class SplashScreenImpl : SplashScreen
{
    public void Show()
    {
        var color = Console.ForegroundColor;
        Write(0xcc00, false, "           ▄█▄ ▄▄█ ▄ ▄ ▄▄▄ ▄▄ ▄█▄  ▄▄▄ ▄█  ▄▄ ▄▄▄ ▄▄█ ▄▄▄           █▄█ █ █ █ █ █▄█ █ █ █   █ █ █▄  ");
            Write(0xcc00, false, "█  █ █ █ █ █▄█           █ █ █▄█ ▀▄▀ █▄▄ █ █ █▄  █▄█ █   █▄ █▄█ █▄█ █▄▄  $year = 2022               ");
            Write(0xcc00, false, "        ");
            Write(0x333333, false, "##@@@@#@@#@@@@#####@#@@#@#|@@#@@@@@##@##@#@@@@#@@  ");
            Write(0x666666, false, "25           ");
            Write(0x333333, false, "#@@@##@@######@#@@@#@#@@@#@@#@@#@@@@@@#@@@@@#@@@@  ");
            Write(0x666666, false, "24           ");
            Write(0x333333, false, "####@#@#@@@@@#@@@@@@@@@##@@@#@@@@@#@##@@@@@@###@@  ");
            Write(0x666666, false, "23           ");
            Write(0x333333, false, "@@@##@@#@@@@#@@@@@#@@@@#@#@@@@@@#@@@@@#@###@@@#@@  ");
            Write(0x666666, false, "22           ");
            Write(0x333333, false, "@#@##@##@@@#@#@@@#@@@@@@##@@@@#@#@@@#@####@@@@#@@  ");
            Write(0x666666, false, "21           ");
            Write(0x333333, false, "@##@##@@@@@@##@@#@@#@#@##@@@@@#@@#@@#@#@##@@@@@#@  ");
            Write(0x666666, false, "20           ");
            Write(0x333333, false, "@@@@@#@@@@@@@@@#@@@@@##@@@@@#@@@@@@@@@@@@@@@@##@@  ");
            Write(0x666666, false, "19           ");
            Write(0x333333, false, "@@@#@@@@@@#@@@@@@@@#@@@@@#@@###@@@@#@@##@@@@@#@#@  ");
            Write(0x666666, false, "18           ");
            Write(0x333333, false, "#@#@@@@@@@@@@@@#@@@#@#@##@@@@@@@@@@#@@@###@#@#@@@  ");
            Write(0x666666, false, "17           ");
            Write(0x333333, false, "@@@#@@@#@#@@@@@#@@@@##@#@###@@@@@@@##@##@@#@@#@@@  ");
            Write(0x666666, false, "16           ");
            Write(0x333333, false, "#@@####@#@@@@###@@@@@@@@@@@@@@#@@@@@#@@@##@@@@#@@  ");
            Write(0x666666, false, "15           ");
            Write(0x333333, false, "@@@@@@#@#@#@@#@#@@###@@@@##@@@#@@##@@@##@##@@##@@  ");
            Write(0x666666, false, "14           ");
            Write(0x333333, false, "@@@@###@@###@@@#@#@@@@@#@@@#@@@@#@@@###@@@@#@@@@#  ");
            Write(0x666666, false, "13           ");
            Write(0x333333, false, "#@###@@@@@@@@@#@@@@#@#@@@@@#@@@@@####@@@@@@#@#@@@  ");
            Write(0x666666, false, "12           ");
            Write(0x333333, false, "@@@#@@@@@@@@@@##@@#@@@#@@@@@@@##@@#@#@##@@@@@##@@  ");
            Write(0x666666, false, "11           ");
            Write(0x333333, false, "@@@@@#@@#@@@#@@##@@@@@@@#@###@@@@@##@#@@@@#@@@##|  ");
            Write(0x666666, false, "10           ");
            Write(0x333333, false, "@#@@@@@@@#@@#@@#@@@###@@@@@@@#@@@@#@#@#@@@@@@####  ");
            Write(0x666666, false, " 9           ");
            Write(0x333333, false, "#@@@@#@@##@@@@@#@##@#@@@@@@@@#@#@#@@@@@@|#@@@@@@@  ");
            Write(0x666666, false, " 8           ");
            Write(0x333333, false, "#@##@##@@#@#@@#@@@#@@#@@@@@@@@@@#####@@@@@#@@@#@@  ");
            Write(0x666666, false, " 7           ");
            Write(0x333333, false, "@@@@@#@#@@@@@@@@#@@#@@@@@@@@#@@#@######@@@#@@#@#@  ");
            Write(0x666666, false, " 6           ");
            Write(0x333333, false, "@@@@#@@#@@@@@@@@@@@@@#@@@#@#@@@@@@###@@@@@@@#@@@@  ");
            Write(0x666666, false, " 5           @@@@##@@@@#.' ~  './\\'./\\' .@@@##@@@@@@@#@@##@@@@  ");
            Write(0xcccccc, false, " 4 ");
            Write(0xffff66, false, "*");
            Write(0x666666, false, "*           @@#@#@@");
            Write(0x4d8b03, false, "#");
            Write(0x427322, false, "@");
            Write(0xd0b376, false, "_/");
            Write(0x5eabb4, false, " ~   ~  ");
            Write(0xd0b376, false, "\\ ' '. '.'.");
            Write(0x1461f, false, "@");
            Write(0x4d8b03, false, "#");
            Write(0x666666, false, "#@@@@@@@@@#@@@##@  ");
            Write(0xcccccc, false, " 3 ");
            Write(0xffff66, false, "**           ");
            Write(0xd0b376, false, "-~------'");
            Write(0x5eabb4, false, "    ~    ~ ");
            Write(0xd0b376, false, "'--~-----~-~----___________--  ");
            Write(0xcccccc, false, " 2 ");
            Write(0xffff66, false, "**           ");
            Write(0x5eabb4, false, "  ~    ~  ~      ~     ~ ~   ~     ~  ~  ~   ~     ");
            Write(0xcccccc, false, " 1 ");
            Write(0xffff66, false, "**           ");
            
        Console.ForegroundColor = color;
        Console.WriteLine();
    }

    private static void Write(int rgb, bool bold, string text)
        => Console.Write($"\u001b[38;2;{(rgb>>16)&255};{(rgb>>8)&255};{rgb&255}{(bold ? ";1" : "")}m{text}");
}