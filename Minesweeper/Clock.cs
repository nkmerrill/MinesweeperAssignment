using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Minesweeper
{

    /// <summary>
    /// Class to track and manage the number of turns in the current game.
    /// </summary>
    public class Clock
    {
        Timer timer;
        int totalSeconds = 0;


        public Clock()
        {
            timer = new Timer(1000);
        }

        public void StopClock()
        {
            timer.Stop();
        }

        public int GetClock()
        {
            return totalSeconds;
        }

        public void Start()
        {
            timer.Start();
        }

        public void OnTick(ElapsedEventHandler timerEvent )
        {
            timer.Elapsed += (sender, e) => { totalSeconds++; };
            timer.Elapsed += timerEvent;
        }

    }
}
