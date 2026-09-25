using System.Collections;
//using Il2Cpp;
using VRC.Localization;
//using MelonLoader;
using UnityEngine;

namespace catclientv3.OtherUI.EdenToast;

public class ToastNotification //obviously, huge thanks to eden for making this open source, I likely wouldn't have had toast stuff otherwise
    {
        private static readonly Queue<ToastMessage> toastQueue = new Queue<ToastMessage>(); // queue is a golified list that just ensures first in first out
        private static bool isShowing = false;

        /// <summary>
        /// Add the toast message to the queue
        /// </summary>
        public static void QueueToast(string content, string description = null, Sprite icon = null, float duration = 5f)
        {
            toastQueue.Enqueue(new ToastMessage(content, description, icon, duration));
            if (!isShowing)
                CCEventSystem.StartCoroutinePls(ProcessQueue());
        }


        /// <summary>
        /// Try Queue the toast if it doesnt exceeed the maxQueueCount length already
        /// This will return True or False depending if it was successful
        /// </summary>
        public static bool TryQueueToast(int maxQueueCount,string content, string description = null, Sprite icon = null, float duration = 5f)
        {
            if (toastQueue.Count > maxQueueCount) return false;
            toastQueue.Enqueue(new ToastMessage(content, description, icon, duration));
            if (!isShowing)
                CCEventSystem.StartCoroutinePls(ProcessQueue());
            return true;
        }


        /// <summary>
        /// Bypasses the queue, will stop previous messages and show this one immediately.
        /// </summary>
        public static void ForceToast(string content, string description = "", Sprite icon = null, float duration = 5f)
        {
            ShowToast(content, description, icon, duration);
        }


        /// <summary>
        /// After a delay it will add the toast notification to the queue.
        /// </summary>
        public static void DelayQueueToast(float Delay, string content, string description = null, Sprite icon = null, float duration = 5f)
        {
            CCEventSystem.StartCoroutinePls(DelayQueueToastEnum(Delay, content, description , icon , duration));
        }


        /// <summary>
        /// After a delay it will force a toast notification regardless of the queue.
        /// </summary>
        public static void DelayForceToast(float Delay, string content, string description = null, Sprite icon = null, float duration = 5f)
        {
            CCEventSystem.StartCoroutinePls(DelayForceToastEnum(Delay, content, description, icon , duration));
        }


        private static IEnumerator DelayQueueToastEnum(float Delay, string content, string description = null, Sprite icon = null, float duration = 5f)
        {
            var startTime = Time.time;
            // Wait until the current time passes the start time + delay
            while (Time.time < (startTime + Delay))
                yield return null;

            startTime = Time.time;
            toastQueue.Enqueue(new ToastMessage(content, description, icon, duration));
            if (!isShowing)
                CCEventSystem.StartCoroutinePls(ProcessQueue());
        }

        private static IEnumerator DelayForceToastEnum(float Delay, string content, string description = null, Sprite icon = null, float duration = 5f)
        {
            var startTime = Time.time;
            while (Time.time < (startTime + Delay ))
                yield return null;

            startTime = Time.time;

            ShowToast(content, description, icon, duration);
        }

        private static IEnumerator ProcessQueue()
        {
            while (toastQueue.Count > 0)
            {
                isShowing = true;

                ToastMessage msg = toastQueue.Dequeue();
                ShowToast(msg.Content, msg.Description, msg.Icon, msg.Duration);

                var startTime = Time.time;
                // Wait for the duration + small buffer time
                while (Time.time < (startTime + msg.Duration + 0.2f))
                    yield return null;

                startTime = Time.time;
            }
            isShowing = false;
        }

        private static void ShowToast(string content, string description, Sprite icon, float duration)
        {
            if(description == null)
            {
                description = " ";
            }
            // Null-safe localization
            LocalizableString message = LocalizableStringExtensions.Localize(content);
            LocalizableString desc = description != null ? LocalizableStringExtensions.Localize(description) : default;

            // Call VRChat's notification UI method
            /*VRCUiManager.field_Private_Static_VRCUiManager_0.field_Private_HudController_0.notification
                .Method_Public_Void_Sprite_LocalizableString_LocalizableString_Single_Object1PublicTBoTUnique_1_Boolean_0(
                    icon, message, desc, duration); this doesn't work anymore </3*/
            //VRCUiManager.field_Private_Static_VRCUiManager_0.field_Private_HudController_0.notification.Method_Public_Void_Sprite_LocalizableString_LocalizableString_Single_Object1PublicObBoObUnique_1_Boolean_0(icon, message, desc, duration);
            //VRCUiManager.field_Private_Static_VRCUiManager_0.field_Private_HudController_0.notification.Method_Public_Void_Sprite_LocalizableString_LocalizableString_Single_Object1PublicIDisposableObAc1BoObObUnique_1_Boolean_0(icon, message, desc, duration);
            VRCUiManager.field_Private_Static_VRCUiManager_0.field_Private_HudController_0.notification.Method_Public_Void_Sprite_LocalizableString_LocalizableString_Single_ReactiveProperty_1_Boolean_0(icon, message, desc, duration);

            //MelonLogger.Msg($"[ToastNotification]\nContent: {content}\nDescription: {description}\nDuration: {duration}\n");
        }


        /// <summary>
        /// Clear the Queue
        /// </summary>
        public static void ClearQueue()
        {
            toastQueue.Clear();
        }


        /// <summary>
        /// Returns amount in queue currently
        /// </summary>
        public static int QueueCount()
        {
            return toastQueue.Count;
        }


        private class ToastMessage
        {
            public string Content { get; }
            public string Description { get; }
            public Sprite Icon { get; }
            public float Duration { get; }

            public ToastMessage(string content, string description, Sprite icon, float duration)
            {
                Content = content;
                Description = description;
                Icon = icon;
                Duration = duration;
            }
        }
    }