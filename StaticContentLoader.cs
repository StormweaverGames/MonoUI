using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MonoUI
{
    public static class StaticContentLoader
    {
        static Dictionary<string, object> _content;
        static bool _initialized = false;
        static GraphicsDevice _graphics;

        public static int TabSize = 8;

        public static int LoadedCount
        {
            get { return _content.Count; }
        }

        internal static void TryInitialize(GraphicsDevice graphics)
        {
            if (!_initialized)
            {
                Initialize(graphics);
                System.Diagnostics.Debug.WriteLine("Initializing Static Content!");
            }
        }

        static void Initialize(GraphicsDevice graphics)
        {
            _graphics = graphics;
            _content = new Dictionary<string, object>();

            SimpleServiceProvider service = new SimpleServiceProvider(graphics);
            ResourceContentManager manager = new ResourceContentManager(service, Resources.ResourceManager);
            ResourceSet resourceSet = Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

            //_content.Add("Font_Arial_8", manager.Load<SpriteFont>("Font_Arial_8"));
            //_content.Add("Font_Arial_10", manager.Load<SpriteFont>("Font_Arial_10"));
            //_content.Add("Font_Arial_12", manager.Load<SpriteFont>("Font_Arial_12"));

            _initialized = true;
        }

        public static T GetItem<T>(string path)
        {
            if (_content.ContainsKey(path))
            {
                object item = _content[path];
                return (T)item;
            }
            else
            {
                if (Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true).GetObject(path, true) != null)
                {
                    SimpleServiceProvider service = new SimpleServiceProvider(_graphics);
                    ResourceContentManager manager = new ResourceContentManager(service, Resources.ResourceManager);
                    T item = (T)manager.Load<T>(path);
                    _content.Add(path, item);
                    return (T)_content[path];
                }
                else
                    return default(T);
            }
        }
    }
}
