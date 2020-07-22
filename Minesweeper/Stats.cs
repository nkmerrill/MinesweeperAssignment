using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Minesweeper
{
    /// <summary>
    /// Class to manage player stats.
    /// </summary>
    static class Stats
    {
        public static int Wins { get; private set; }
        public static int Losses { get; private set; }
        public static int AvgTime { get; private set; }

        public static void LoadStats()
        {
            try
            {
                using (StreamReader s = new StreamReader("stats.dat"))
                {

                    Wins = int.Parse(s.ReadLine());

                    Losses = int.Parse(s.ReadLine());

                    AvgTime = int.Parse(s.ReadLine());
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                Wins = 0;
                Losses = 0;
                AvgTime = 0;
            }
            catch ( Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void SaveStats(bool isWin, int timeElapsed)
        {
            if (isWin)
                Wins++;
            else
                Losses++;
            
            AvgTime = ((AvgTime * (Wins + Losses - 1)) + timeElapsed) / (Wins + Losses);

            try
            {
                using (StreamWriter s = new StreamWriter("stats.dat"))
                {
                    s.WriteLine(Wins.ToString());
                    s.WriteLine(Losses.ToString());
                    s.WriteLine(AvgTime.ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
    }
}
