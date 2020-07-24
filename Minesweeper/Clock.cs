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
    /// Class to track and manage the number of seconds in the current game.
    /// </summary>
    public class Clock
    {
        Timer timer;
        int totalSeconds = 0;

        /// <summary>
        /// Constructor creates a timer that triggers in one second intervals.
        /// Registers the tick event to cause the second count to increase by 1.
        /// </summary>
        public Clock()
        {
            timer = new Timer(1000);
            timer.Elapsed += (sender, e) => { totalSeconds++; };
        }

        /// <summary>
        /// Pause the clock.
        /// </summary>
        public void StopClock()
        {
            timer.Stop();
        }

        /// <summary>
        /// Returns the current second count.
        /// </summary>
        /// <returns>integer representing the seconds elapsed for the clock.</returns>
        public int GetClock()
        {
            return totalSeconds;
        }

        /// <summary>
        /// Start the clock.
        /// </summary>
        public void Start()
        {
            timer.Start();
        }

        /// <summary>
        /// Exposes the clock tick event. internally, class should increment the second 
        /// counter. Also takes an event handler so other objects can execute actions 
        /// based on the clock ticks.
        /// </summary>
        /// <param name="timerEvent">Passed event handler that should execute on tick.</param>
        public void OnTick(ElapsedEventHandler timerEvent)
        {

            timer.Elapsed += timerEvent;
        }

    }
}
