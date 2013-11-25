using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanlin.Common.Extensions
{
    public static class EventHandlerExtensions
    {
        public static void Raise<T>(this EventHandler<T> handler, object sender, T args) where T: EventArgs
        {
            var handlerTemp = handler;
            if (handlerTemp != null)
            {
                handlerTemp(sender, args);
            }
        }
    }
}
