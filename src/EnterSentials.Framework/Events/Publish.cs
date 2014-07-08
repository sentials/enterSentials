using System;
using System.Linq;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2013/07/how-to-properly-publish-events.html
    public static class Publish
    {
        public const bool CatchesExceptionsThrownByEventHandlersByDefault = false;


        static void Event(EventHandler eventPublishingDelegate, object sender, EventArgs eventArgs, bool catchExceptionsThrownByEventHandlers)
        {
            var eventPublishingDelegate_referenceHolder = eventPublishingDelegate;

            if (eventPublishingDelegate_referenceHolder != null)
            {
                if (!catchExceptionsThrownByEventHandlers)
                    eventPublishingDelegate_referenceHolder(sender, eventArgs);
                else
                    foreach (EventHandler eventHandler in eventPublishingDelegate_referenceHolder.GetInvocationList().Cast<EventHandler>())
                    {
                        try
                        { eventHandler(sender, eventArgs); }
                        catch
                        { }
                    }
            }
        }


        static void Event<TEventArgs>(EventHandler<TEventArgs> eventPublishingDelegate, object sender, TEventArgs eventArgs, bool catchExceptionsThrownByEventHandlers) where TEventArgs : EventArgs
        {
            var eventPublishingDelegate_referenceHolder = eventPublishingDelegate;

            if (eventPublishingDelegate_referenceHolder != null)
            {
                if (!catchExceptionsThrownByEventHandlers)
                    eventPublishingDelegate_referenceHolder(sender, eventArgs);
                else
                    foreach (EventHandler<TEventArgs> eventHandler in eventPublishingDelegate_referenceHolder.GetInvocationList().Cast<EventHandler<TEventArgs>>())
                    {
                        try
                        { eventHandler(sender, eventArgs); }
                        catch
                        { }
                    }
            }
        }

        static void Event<TEventArgs>(Delegate eventPublishingDelegate, object sender, TEventArgs eventArgs, bool catchExceptionsThrownByEventHandlers)
            where TEventArgs : EventArgs
        {
            if (!(eventPublishingDelegate is Delegate))
                return;

            var eventPublishingDelegate_referenceHolder = eventPublishingDelegate;
            var delegateInvocationArguments = new object[] { sender, eventArgs };

            if (eventPublishingDelegate_referenceHolder != null)
            {
                foreach (Delegate eventHandler in eventPublishingDelegate_referenceHolder.GetInvocationList())
                {
                    if (!catchExceptionsThrownByEventHandlers)
                        eventHandler.DynamicInvoke(delegateInvocationArguments);
                    else
                    {
                        try
                        { eventHandler.DynamicInvoke(delegateInvocationArguments); }
                        catch
                        { }
                    }
                }
            }
        }



        public static void Event(EventHandler eventPublishingDelegate, object sender, EventArgs eventArgs)
        { Event(eventPublishingDelegate, sender, eventArgs, CatchesExceptionsThrownByEventHandlersByDefault); }

        public static void Event<TEventArgs>(EventHandler<TEventArgs> eventPublishingDelegate, object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
        { Event<TEventArgs>(eventPublishingDelegate, sender, eventArgs, CatchesExceptionsThrownByEventHandlersByDefault); }

        public static void Event<TEventArgs>(Delegate eventPublishingDelegate, object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
        { Event<TEventArgs>(eventPublishingDelegate, sender, eventArgs, CatchesExceptionsThrownByEventHandlersByDefault); }


        public static void EventWithExceptions(EventHandler eventPublishingDelegate, object sender, EventArgs eventArgs)
        { Event(eventPublishingDelegate, sender, eventArgs, false); }

        public static void EventWithExceptions<TEventArgs>(EventHandler<TEventArgs> eventPublishingDelegate, object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
        { Event<TEventArgs>(eventPublishingDelegate, sender, eventArgs, false); }

        public static void EventWithExceptions<TEventArgs>(Delegate eventPublishingDelegate, object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
        { Event<TEventArgs>(eventPublishingDelegate, sender, eventArgs, false); }


        public static void EventWithoutExceptions(EventHandler eventPublishingDelegate, object sender, EventArgs eventArgs)
        { Event(eventPublishingDelegate, sender, eventArgs, true); }

        public static void EventWithoutExceptions<TEventArgs>(EventHandler<TEventArgs> eventPublishingDelegate, object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
        { Event<TEventArgs>(eventPublishingDelegate, sender, eventArgs, true); }

        public static void EventWithoutExceptions<TEventArgs>(Delegate eventPublishingDelegate, object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
        { Event<TEventArgs>(eventPublishingDelegate, sender, eventArgs, true); }
    }
}
