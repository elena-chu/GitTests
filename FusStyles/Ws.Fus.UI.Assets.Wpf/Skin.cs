using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ws.Fus.UI.Assets.Wpf
{
    public enum Skin
    {
        Standard, // always first
    }

    public class SkinManager
    {
        private static readonly ILogger _logger = Log.ForContext<SkinManager>();

        /// <summary>
        /// We use static skins, i.e. skins that can't be changed at runtime.
        /// The property must be set by an application at startup.
        /// </summary>
        public static Skin AppSkin { get; set; } = Skin.Standard;

        /// <summary>
        /// In order to detect the calling assembly correctly, please decorate the caller function with:
        /// [MethodImpl(MethodImplOptions.NoInlining)] attribute.
        /// </summary>
        public static void LoadResources()
        {
            var rsrcName = Path.ChangeExtension(AppSkin.ToString(), "xaml");

            var callingAssemble = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            var thisAssembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            var rsrcPath = callingAssemble == thisAssembly ?
                $"/{callingAssemble};component/Skins/" : $"/{callingAssemble};component/Assets/Skins/";

            ResourceDictionary dict = null;

            try
            {
                dict = new ResourceDictionary
                {
                    Source = new Uri($"{rsrcPath}{rsrcName}", UriKind.RelativeOrAbsolute)
                };
            
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            catch (Exception ex)
            {
                _logger.Error(ex,
                    "Failed to load resources for path: {path}, name: {name}, thisAssembly: {thisAssembly}, callingAssembly: {callingAssembly}",
                    rsrcPath, rsrcName, thisAssembly, callingAssemble);
                throw;
            }
        }
    }
}
