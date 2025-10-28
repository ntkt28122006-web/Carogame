using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace caro_game
{
    [Serializable]
    public class GameResult
    {
        public string Winner { get; set; }
        public string Loser { get; set; }
        public DateTime Time { get; set; }
        public GameResult(string winner, string loser)
        {
            Winner = winner;
            Loser = loser;
            Time = DateTime.Now;
        }
        public override string ToString()
        {
            return $"{Time:dd/MM/yyyy HH:mm:ss} - Winner: {Winner}, Loser: {Loser}";
        }
    }
}
