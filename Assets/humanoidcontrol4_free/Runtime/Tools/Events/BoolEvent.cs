﻿using UnityEngine.Events;

namespace Passer {

    /// <summary>
    /// A list of event handlers with a boolean parameter
    /// </summary>
    /// This is used to implement a list of functions 
    /// which should be called with a boolean as parameter
    [System.Serializable]
    public class BoolEventHandlers : EventHandlers<BoolEvent> {
        public bool value {
            get {
                if (events == null || events.Count == 0)
                    return false;
                return events[0].value;
            }
            set {
                foreach (BoolEvent boolEvent in events)
                    boolEvent.value = value;
            }
        }

        public void Update() {
            foreach (BoolEvent boolEvent in events)
                boolEvent.Update();
        }
    }

    /// <summary>
    /// An event handler calling a function with a boolean parameter
    /// </summary>
    [System.Serializable]
    public class BoolEvent : EventHandler {
        public BoolEvent(Type newEventType = Type.OnChange) {
            eventType = newEventType;
        }

        public void SetMethod(Type newEventType, UnityAction voidAction) {
            eventType = newEventType;
        }

        public void SetMethod(Type newEventType, UnityAction<bool> boolAction) {
            eventType = newEventType;
        }

        /// <summary>
        /// The GameObject value for this event
        /// </summary>
        public bool value {
            get { return boolValue; }
            set {
                bool newBoolValue = boolInverse ? !value : value;
                boolChanged = newBoolValue != boolValue;
                boolValue = newBoolValue;

                _intValue = boolValue ? 1 : 0;
                _floatValue = boolValue ? 1.0F : 0.0F;

                Update();
            }
        }

        protected override void UpdateBool() {
            if (CheckCondition(boolValue, boolChanged, boolChanged)) {
                if (functionCall.parameters[0].fromEvent)
                    functionCall.Execute(boolValue);
                else
                    functionCall.Execute(functionCall.parameters[0].boolConstant);
            }
        }

        override protected void UpdateInt() {
            if (CheckCondition(boolValue, boolChanged, intChanged)) {
                if (functionCall.parameters[0].fromEvent)
                    functionCall.Execute(_intValue);
                else
                    functionCall.Execute(functionCall.parameters[0].intConstant);
            }
        }

        override protected void UpdateFloat() {
            if (CheckCondition(boolValue, boolChanged, intChanged)) {
                if (functionCall.parameters[0].fromEvent)
                    functionCall.Execute(_floatValue);
                else
                    functionCall.Execute(functionCall.parameters[0].floatConstant);
            }
        }
    }

}