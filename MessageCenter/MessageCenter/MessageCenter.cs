using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Noear.UWP.Data {
    public static class MessageCenter {

        static Dictionary<string, List<SubscribeItem>> center;

        static void tryInit() {
            if (center == null) {
                center = new Dictionary<string, List<SubscribeItem>>();
            }
        }

        public async static void SendMessage(string message, params object[] args) {
            tryInit();

            if (center.ContainsKey(message)) {
                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var list = center[message];
                    foreach (var item in list) {
                        item.action(args);
                    }
                });
            }
        }

        public static void UnSubscribe(string message, Object target) {
            tryInit();

            if (message == null || target == null)
                return;

            if (center.ContainsKey(message)) {
                var list = center[message];
                for (int i = 0, len = list.Count; i < len; i++) {
                    var item = list[i];
                    if (item.target.Equals(target)) {
                        list.RemoveAt(i);

                        item.target = null;
                        item.action = null;
                        return;
                    }
                }
            }
        }

        public static void Subscribe(string message, Object target, Action<object[]> action) {
            tryInit();

            if (message==null || target == null)
                return;

            List<SubscribeItem> list = null;
            if (center.ContainsKey(message)) {
                list = center[message];

                foreach (var item in list) {
                    if (item.target.Equals(target)) {
                        item.action = action;
                        return;
                    }
                }
            }
            else {
                list = new List<SubscribeItem>();
                center[message] = list;
            }

            list.Add(new SubscribeItem() { target = target, action = action });
        }

        class SubscribeItem {
            public Object target { get; set; }
            public Action<object[]> action { get; set; }
        }
    }
}
