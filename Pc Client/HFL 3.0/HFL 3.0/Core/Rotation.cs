using HFL_3._0.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HFL_3._0
{
    public class Rotation
    {
        private bool _done = false;
        private bool init = false;
        public bool done
        {
            get { return _done; }
            set
            {
                _done = value;
                if (!init)
                {
                    sessionManager.updateCurrent();
                    Rotator.rotationUpdate();
                }
            }
        }

        private List<RotationJob> tasks = new List<RotationJob>();

        public string type { get; set; }
        public double waitTime { get; set; }
        public string endType { get; set; }
        public bool pcSleep { get; set; }

        public void initRot()
        {
            init = true;
        }

        public void start()
        {
            if (tasks.Count > 0)
            {
                Console.WriteLine("Rotation starting");
                if (!Rotator.paused)
                {
                    foreach (RotationJob jobS in tasks)
                    {
                        jobS.done = false;
                    }
                }
                foreach (RotationJob jobS in tasks)
                {
                    if (!jobS.running)
                    {
                        jobS.run();
                    }
                }
            }
            else
            {
                complete();
            }
        }

        public void stop()
        {

        }

        public void complete()
        {
            Console.WriteLine("Rotation Completed.");

            /* End Rotation */
            done = true;
        }

        public void taskUpdate()
        {
            RotationJob job = tasks.FirstOrDefault(j => j.done == false);
            if (job == null)
            {
                complete();
            }
        }

        public void addTask(RotationJob _task)
        {
            tasks.Add(_task);
        }
    }
}
