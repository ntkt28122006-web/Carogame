using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace caro_game
{
    public static class GameHistory
    {
        private static string filepath = Path.Combine(Application.StartupPath, "history.txt");
        public static void LuuKetqua(GameResult Ketqua)
        {
            File.AppendAllText(filepath, Ketqua.ToString() + Environment.NewLine);
        }

        public static List<GameResult> LoadResults()
        {
            var list = new List<GameResult>();
            if (!File.Exists(filepath))
            {
                return list;
            }
            var lines = File.ReadAllLines(filepath);
            foreach (var line in lines)
            {
                list.Add(ParseResult(line));
            }
            return list;
        }
        private static GameResult ParseResult(string line)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(line))
                    return new GameResult("Unknown", "Unknown");
                int idx = line.IndexOf(" - ");
                if (idx < 0)
                    return new GameResult("Unknown", "Unknown");
                string timePart = line.Substring(0, idx).Trim();
                string infoPart = line.Substring(idx + 3).Trim();


                string winner = "Unknow";
                string loser = "Unknow";

                var parts = infoPart.Split(',');
                foreach (var p in parts)
                {
                    if (p.Contains("Winner:"))
                    {
                        winner = p.Replace("Winner:", "").Trim();
                    }
                    else if (p.Contains("Loser:"))
                    {
                        loser = p.Replace("Loser:", "").Trim();
                    }
                }
                DateTime time;
                bool parsed = DateTime.TryParse(timePart, out time);
                if (!parsed)
                {
                    Console.WriteLine($"Failed to parse datetime: {timePart}");
                    time = DateTime.Now;
                }
                return new GameResult(winner,loser) { Time = time };
            }
            catch
            {
                return new GameResult("Unknow", "Unknow");
            }
        }
        public static Dictionary<string, int> XepHang()
        {
            var results = LoadResults();
            return results.GroupBy(r => r.Winner)
                .ToDictionary(g => g.Key, g => g.Count())
                .OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x=> x.Value);
        }
    }
}
