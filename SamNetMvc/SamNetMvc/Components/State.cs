using System;

namespace SamNetMvc.Components
{
    internal class State
    {
        public View View { get; set; }

        // Derive the state representation as a function of the systen
        // control state
        public static void Representation(Model model, Action<string> next)
        {
            var representation = "oops... something went wrong, the system is in an invalid state";

            if (Ready(model))
            {
                representation = View.Ready(model);
            }

            View.Display(representation, next);
        }

        // Derive the current state of the system
        public static bool Ready(Model model)
        {
            return true;
        }


        /// <summary>
        /// Next action predicate, derives whether the system is in a (control) state where a new (next) action needs to be invoked
        /// </summary>
        public static void NextAction(Model model)
        {
            //nothing to do in this App
        }

        public void Render(Model model, Action<string> next)
        {
            Representation(model, next);
            NextAction(model);
        }
    }
}